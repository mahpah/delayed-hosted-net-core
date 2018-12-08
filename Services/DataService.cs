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

        public async Task SaveState(string buildingId, string state)
        {
            // should check state is conformed or not
            await Task.CompletedTask;
            _logger.LogWarning("{0} {1} state is {2}", DateTime.Now, buildingId, state);
        }
    }
}
