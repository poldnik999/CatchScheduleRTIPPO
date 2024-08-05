using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace lab6
{
    public partial class MunicipalContractCard : Form
    {
        public bool canUpdate = Session.GetCurrentPM().CanUpdate(new MunicipalContract());
        public int id;
        MunicipalContractController MunicipalContractController = new MunicipalContractController();
        MunicipalContract munnicipalContract;
        public MunicipalContractCard(int idmunisipalContract)
        {
            this.id = idmunisipalContract;
            InitializeComponent();
            munnicipalContract = MunicipalContractController.ViewMunicipalContractCard(idmunisipalContract);
            if (!canUpdate)
            {
                AddNomerContract.ReadOnly = true;
                AddDateConContract.ReadOnly = true;
                AddDateExeContract.ReadOnly = true;
                AddCustomerContract.ReadOnly = true;
                AddExecutinContract.ReadOnly = true;
                AddLocalityContract.Enabled = true;
                ButtonCreateMunicipalContract.Enabled = false;
            }
            AddLocalityContract.SelectedIndex= -1;
            textBox1.ReadOnly = true;
            AddNomerContract.Text = munnicipalContract.number.ToString();
            AddDateConContract.Text = munnicipalContract.dateOfConclusion.ToString();
            AddDateExeContract.Text = munnicipalContract.dateOfExecotion.ToString();
            AddCustomerContract.Text = munnicipalContract.customer.ToString();
            AddExecutinContract.Text = munnicipalContract.executor.ToString();
            //AddLocalityContract.DataSource = MunicipalContractController.getListLocaity();
            for(int i = 0;i < MunicipalContractController.getListLocaity().Rows.Count; i++)
            {
                AddLocalityContract.Items.Add(MunicipalContractController.getListLocaity().Rows[i][1]);
            }
            for (int i = 0; i < munnicipalContract.tableLocalyty.Rows.Count; i++)
            {
                AddLocalityContract.SetSelected(Convert.ToInt32(munnicipalContract.tableLocalyty.Rows[i][3].ToString())-1, true);
            }
            AddLocalityContract.DisplayMember = "Name";
            textBox1.Text = munnicipalContract.price.ToString();
        }

        private void ButtonCreateMunicipalContract_Click(object sender, EventArgs e)
        {
            
            List<string> lst = new List<string>();
            foreach(string i in AddLocalityContract.SelectedItems)
                lst.Add(i);
            MunicipalContractController.UpdateMunicipalContract(id, new System.Collections.ArrayList 
            { AddNomerContract.Text, AddDateConContract.Text, AddDateExeContract.Text, AddCustomerContract.Text, AddExecutinContract.Text,id }, lst);

        }

        private void groupBox3_Enter(object sender, EventArgs e)
        {

        }
    }
}
