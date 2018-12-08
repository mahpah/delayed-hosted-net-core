using System;
using System.Threading;
using System.Threading.Tasks;

namespace WebApplication1.Models
{
    public class WorkItem
    {
        public WorkItem(Type handlerType, object data)
        {
            HandlerType = handlerType;
            Data = data;
        }

        public object Data { get; private set; }
        public Type HandlerType { get; private set; }
    }

    public interface IWorkItemHandler<T> where T : class
    {
        Task Handle(T itemData, CancellationToken cancellationToken = default(CancellationToken));
    }
}
