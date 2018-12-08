using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using WebApplication1.Models;

namespace WebApplication1.Services
{
    // singleton
    public class InMemoryQueue
    {
        private ConcurrentQueue<WorkItem> _workItems =
            new ConcurrentQueue<WorkItem>();
        private readonly SemaphoreSlim _signal = new SemaphoreSlim(0);

        public void QueueBackgroundWorkItem(WorkItem workItem)
        {
            if (workItem == null)
            {
                throw new ArgumentNullException(nameof(workItem));
            }

            _workItems.Enqueue(workItem);
            _signal.Release();
        }

        public async Task<WorkItem> DequeueAsync(
            CancellationToken cancellationToken)
        {
            await _signal.WaitAsync(cancellationToken);
            _workItems.TryDequeue(out var workItem);

            return workItem;
        }
    }
}
