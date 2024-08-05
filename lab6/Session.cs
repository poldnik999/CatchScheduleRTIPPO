using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace lab6
{
    public class Session // класс который мы можем вызвать везде, чтоб взять pm или user
    {
        private static User user;
        private static PM pm;
        public static void SetContext(User userSet, PM pmSet)
        {
            if(user == null && pm == null) 
            {
                user = userSet;
                pm = pmSet;
            }
        }
        public static PM GetCurrentPM()
        {          
            return pm;
        }

        public static User GetCurrentUser()
        {
            return user;
        }

    }
}
