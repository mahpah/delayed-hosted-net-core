using System;
using System.Threading;
using System.Threading.Tasks;
using WebApplication1.Models;
using WebApplication1.Services;

namespace WebApplication1.HostedService
{
    public class ConfirmHandler : IWorkItemHandler<Signal>
    {
        private readonly DataService _dataService;

        public ConfirmHandler(DataService dataService)
        {
            _dataService = dataService;
        }

        public async Task Handle(Signal itemData, CancellationToken token)
        {
            await Task.Delay(
                itemData.Timestamp.Add(TimeSpan.FromMinutes(2)).Subtract(DateTime.Now),
                token);
            await Confirm(itemData);
        }

        private async Task Confirm(Signal itemData)
        {
            await _dataService.SaveState(itemData.BuildingId, "in fire");
        }
    }

    public class Signal
    {
        public Signal(string buildingId)
        {
            BuildingId = buildingId;
            Timestamp = DateTime.Now;
        }

        public string BuildingId { get; private set; }
        public DateTime Timestamp { get; private set; }
    }
}
