using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab6
{
    public class PMFactory
    {

        public PM GetUserPM(User user)
        {
            if(user.role.name == "Куратор ВетСлужбы")
            {
                return new CuratorVebService(user);
            }
            else if (user.role.name == "Подписант ВетСлужбы")
            {
                return new SignatorVetService(user);
            }
            else if (user.role.name == "Оператор ВетСлужбы")
            {
                return new OpertorVetService(user);
            }
            else if (user.role.name == "Оператор по отлову")
            {
                return new OperatorTrapping(user);
            }
            else if (user.role.name == "Оператор ОМСУ")
            {
                return new OperatorOMSY(user);
            }
            return new Other(user);
        }
    }
}
