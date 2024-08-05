using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace lab6
{
    public class AuthController
    {
        public User user;
        DataTable table = new DataTable();
        PMFactory PMFactory = new PMFactory();


        public User AythMetod(string loginUser, string passUser)
        {
            table = DB.AuthSelectInBD(loginUser, passUser);
            if (table.Rows.Count > 0)
            {
                user = new User(table);
                PM pm = PMFactory.GetUserPM(user);
                Session.SetContext(user, pm);
                //bool b = Session.GetCurrentPM().CanUpdate(new MunicipalContract());
                return user;
            }
            else
            {
                return null;
            }
        }
    }
}
