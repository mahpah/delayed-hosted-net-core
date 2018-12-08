using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace WebApplication1.Services
{
    public class DataService
    {
        private readonly ILogger<DataService> _logger;

        public DataService(ILogger<DataService> logger)
        {
            _logger = logger;
        }

        public async Task SaveState(Guid buildingId, string state)
        {
            // should check state is conformed or not
            await Task.CompletedTask;
            _logger.LogInformation("{0} state is {1}", buildingId, state);
        }
    }
}
