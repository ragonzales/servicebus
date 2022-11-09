using Microsoft.Azure.ServiceBus;

namespace PeruCompras.IntegracionMef.Transversal.Common.Resources
{
    public interface IServiceBus
    {
        QueueClient GetQueueClient(string conectionstring, string queue);
    }

    public class ServiceBus : IServiceBus
    {
        public QueueClient GetQueueClient(string conectionstring, string queue)
        {
            return new QueueClient(conectionstring, queue);
        }
    }
}