using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using WebApplication1.Models;
using WebApplication1.Services;

namespace WebApplication1.HostedService
{
    public class TimerService : IHostedService
    {
        private Task _executingTask;
        private CancellationTokenSource _cts;
        private readonly InMemoryQueue _taskQueue;
        private ILogger<TimerService> _logger;
        private readonly IServiceProvider _serviceProvider;

        public TimerService(InMemoryQueue taskQueue, ILogger<TimerService> logger, IServiceProvider serviceProvider)
        {
            _taskQueue = taskQueue;
            _logger = logger;
            _serviceProvider = serviceProvider;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Ticking.....");
            _cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);

            // Store the task we're executing
            _executingTask = ExecuteAsync(_cts.Token);

            // If the task is completed then return it, otherwise it's running
            return _executingTask.IsCompleted ? _executingTask : Task.CompletedTask;
        }

        private async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Queued Hosted Service is starting.");

            while (!cancellationToken.IsCancellationRequested)
            {
                var workItem = await _taskQueue.DequeueAsync(cancellationToken);

                try
                {
                    await ProcessOnce(workItem, cancellationToken);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex,
                        $"Error occurred executing {nameof(workItem)}.");
                }
            }

            _logger.LogInformation("Queued Hosted Service is stopping.");
        }

        private async Task ProcessOnce(WorkItem item, CancellationToken cancellationToken)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var handlerType = scope.ServiceProvider.GetService(item.HandlerType);
                var dataType = item.Data.GetType();
                var concreteType = typeof(IWorkItemHandler<>).MakeGenericType(dataType);
                var task = (Task) concreteType.GetMethod("Handle").Invoke(handlerType, new[] { item.Data, cancellationToken });
            }
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            // Stop called without start
            if (_executingTask == null)
            {
                return;
            }

            // Signal cancellation to the executing method
            _cts.Cancel();

            // Wait until the task completes or the stop token triggers
            await Task.WhenAny(_executingTask, Task.Delay(-1, cancellationToken));

            // Throw if cancellation triggered
            cancellationToken.ThrowIfCancellationRequested();
        }
    }
}
