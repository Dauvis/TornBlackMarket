using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TornBlackMarket.Periodic.Services
{
    public abstract class JobServiceBase
    {
        protected JobServiceBase() { }

        public abstract Task ExecuteAsync(JobSettings settings);
    }
}
