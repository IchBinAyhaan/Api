using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Servicer.Producer
{
    public interface IProducerService
    {
        Task ProduceAsync(string action, object data);
    }
}
