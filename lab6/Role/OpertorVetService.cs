using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab6
{
    public class OpertorVetService : PM
    {
        public OpertorVetService(User user) : base(user) { }
        public override bool CanWatchAll(object obj)
        {
            if (obj is MunicipalContract)
            {
                return true;
            }
            if (obj is CatchPlanSchedule)
            {
                return true;
            }
            return false;
        }
        public override bool CanUpdate(object obj)
        {
            if (obj is MunicipalContract)
            {
                return false;
            }
            if (obj is CatchPlanSchedule)
            {
                return false;
            }
            return false;
        }
    }
}
