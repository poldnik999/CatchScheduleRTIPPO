using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SQLite;
using System.Data.Entity.Infrastructure;
using System.Threading;

namespace lab6
{
    public partial class AuthForm : Form
    {
        String loginUser;
        String passUser;
        User user;
        AuthController authController = new AuthController();
        private Thread th;

        public AuthForm()
        {
            InitializeComponent();
        }

        private void buttonLogin_Click(object sender, EventArgs e)
        {
            loginUser = loginField.Text;
            passUser = passField.Text;
            user = authController.AythMetod(loginUser, passUser);
            if(user != null) 
            {
                //MessageBox.Show(user.name);
                this.Close();
                th = new Thread(open);
                th.SetApartmentState(ApartmentState.STA);
                th.Start();
            }
            else MessageBox.Show("Такого пользователя нет в системе.");
        }

        void open(object obj)
        {
            Application.Run(new MainForm(user));
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
