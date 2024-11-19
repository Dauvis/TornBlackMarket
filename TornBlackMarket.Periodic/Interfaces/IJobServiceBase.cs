using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TornBlackMarket.Periodic.Interfaces
{
    public interface IJobServiceBase
    {
        public Task ExecuteAsync(JobSettings settings);
    }
}
