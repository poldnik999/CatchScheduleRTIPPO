using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab6
{
    public abstract class PM
    {
        public User user;

        public abstract bool CanWatchAll(object obj); // постоянно переопределяем, возвращает tru/false в зависимости от роли

        public abstract bool CanUpdate(object obj); // постоянно переопределяем, возвращает tru/false в зависимости от роли

        public PM(User user)
        {
            this.user = user; // конструктор
        }
    }
}
