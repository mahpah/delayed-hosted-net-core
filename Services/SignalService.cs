using System;
using WebApplication1.HostedService;
using WebApplication1.Models;

namespace WebApplication1.Services
{
    public class SignalService
    {
        private readonly InMemoryQueue _tasks;

        public SignalService(InMemoryQueue tasks)
        {
            _tasks = tasks;
        }

        public void AcceptSignal(string objectId)
        {
            _tasks.QueueBackgroundWorkItem(new WorkItem(typeof(ConfirmHandler), new Signal(objectId)));
        }
    }
}
