using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;


namespace lab6
{
    public class User
    {
        public int id;
        public string surname;
        public string name;
        public string patronymic;
        public string phone;
        public string email;
        public string login;
        public string password;
        public int idOrganization;
        public Role role;

        public User(DataTable table)
        {
            foreach (DataRow row in table.Rows)
            {
                id = Convert.ToInt32(row[0].ToString());
                surname = row[1].ToString();
                name = row[2].ToString();
                patronymic = row[3].ToString();
                phone = row[4].ToString();
                email = row[5].ToString();
                login = row[6].ToString();
                password = row[7].ToString();
                idOrganization = Convert.ToInt32(row[8].ToString());
                role = new Role(int.Parse(row[10].ToString()), row[12].ToString());
            }

        }

        public User()
        {

        }
    }
}
