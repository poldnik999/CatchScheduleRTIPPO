using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Excel = Microsoft.Office.Interop.Excel;
using System.Windows.Forms;
using System.Data.SQLite;
using System.Data.Entity.Infrastructure;
using System.Threading;
using System.Collections;
using System.Text.RegularExpressions;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Reflection.Emit;
using System.Security.Cryptography.X509Certificates;
using static System.Windows.Forms.ListBox;

namespace lab6
{
    public partial class MainForm : Form
    {
        public User user;
        DataTable table = new DataTable();
        public string sort = "";
        public string filtr = "";


        PlanScheduleController planSchController = new PlanScheduleController();
        MunicipalContractController MunicipalContractController = new MunicipalContractController();
        private Thread th;

        public MainForm(User user)
        {
            this.user = user;
            InitializeComponent();
        }

        // Таня
        private void button1_Click(object sender, EventArgs e)
        {
            table = MunicipalContractController.getListMunicipalContract(sort, filtr);

            AddCustomerContract.DataSource = MunicipalContractController.getListOrganization();
            AddCustomerContract.DisplayMember = "Name";
            AddCustomerContract.ValueMember = "id_Organization";
            AddExecutinContract.DataSource = MunicipalContractController.getListOrganization();
            AddExecutinContract.DisplayMember = "Name";
            AddExecutinContract.ValueMember = "id_Organization";
            AddLocalityContract.DataSource = MunicipalContractController.getListLocaity();
            AddLocalityContract.DisplayMember = "Name";
            AddLocalityContract.ValueMember = "id_Locality";
            dataGridView3.DataSource = table;
            dataGridView3.Columns[0].Visible = false;
            dataGridView3.Update();
            tabControl1.SelectTab(tabPage3);
        }

        private void button5_Click(object sender, EventArgs e) //Експорт ексель
        {
            Excel.Application exApp = new Excel.Application();
            exApp.Workbooks.Add(); Excel.Worksheet wsh = (Excel.Worksheet)exApp.ActiveSheet;
            int i, j; for (i = 0; i < dataGridView3.RowCount - 1; i++)
            {
                for (j = 1; j < dataGridView3.ColumnCount; j++)
                {
                    wsh.Cells[i + 1, j + 1] = dataGridView3[j, i].Value.ToString();
                }
            }
            exApp.Visible = true;
        }

        private void ButtonCreateMunicipalContract_Click(object sender, EventArgs e)
        {
            ArrayList arrayLocalityContract = new ArrayList();
            var selectedObject = AddLocalityContract.SelectedItems;
            var val = "";
            foreach (DataRowView i in selectedObject)
            {
                val = i.Row.ItemArray[1].ToString();
                arrayLocalityContract.Add(val);
            }
            //MessageBox.Show(AddDateConContract.Text.ToString());
            ArrayList arrayNewContract = new ArrayList { AddNomerContract.Text, AddDateConContract.Text, AddDateExeContract.Text, AddDateConContract.Text, AddCustomerContract.SelectedValue.ToString(), AddExecutinContract.SelectedValue.ToString() };

            table = MunicipalContractController.CreateMunicipalContract(arrayNewContract, arrayLocalityContract);
            dataGridView3.DataSource = null;
            dataGridView3.DataSource = table;
            dataGridView3.Columns[0].Visible = false;
            dataGridView3.Update();
            //MessageBox.Show("У вас недостаточно прав для создания контракта!");

        }

        private void ButtonDeleteMunicipalContract_Click_1(object sender, EventArgs e)
        {
            int idmunisipalContract = Convert.ToInt32(dataGridView3.SelectedCells[0].Value.ToString());
            table = MunicipalContractController.DeleteMunicipalContract(idmunisipalContract);
            dataGridView3.DataSource = null;
            dataGridView3.DataSource = table;
            dataGridView3.Columns[0].Visible = false;
            dataGridView3.Update();
        }

        private void ButtonSortFiltrMunicipalContract_Click(object sender, EventArgs e)
        {
            dataGridView3.DataSource = null;            
            if (SortradioButtonExecute.Checked) sort = SortradioButtonExecute.Text.ToString();
            else if (SortradioButtonCalcute.Checked) sort = SortradioButtonCalcute.Text.ToString();
            else if (SortradioButtonNumber.Checked) sort = SortradioButtonNumber.Text.ToString();
            else if (SortradioButtonDateConclution.Checked) sort = SortradioButtonDateConclution.Text.ToString();
            else if (SortradioButtonDateExecute.Checked) sort = SortradioButtonDateExecute.Text.ToString();
            filtr = FilterTextBox.Text;
            table = MunicipalContractController.getListMunicipalContract(sort, filtr);
            dataGridView3.DataSource = table;
            dataGridView3.Columns[0].Visible = false;

        }

        private void button4_Click(object sender, EventArgs e) //открытие карточки
        {
            int idmunisipalContract = Convert.ToInt32(dataGridView3.SelectedCells[0].Value.ToString());
            MunicipalContractCard municipalContractCard = new MunicipalContractCard(idmunisipalContract);
            municipalContractCard.ShowDialog();
        }


        // Илья
        private void button2_Click(object sender, EventArgs e)
        {
        }

        //Никита
        private void button3_Click(object sender, EventArgs e)
        {
            List<string> localityList = planSchController.getListlocality();
            catchScheduleComboBox1.DataSource = localityList;
            ExceptionLabel3.Text = "Есть права на удаление";
            table = planSchController.getListPlanSchedule("", "");
            dataGridView1.DataSource = table;
            dataGridView1.Columns[0].Visible = false;
            dataGridView1.Columns[4].Visible = false;
            dataGridView1.Update();
            if (Session.GetCurrentPM().CanUpdate(new CatchPlanSchedule()))
            {
                ExceptionLabel1.Text = "Есть права на добавление";
                ExceptionLabel3.Text = "Есть права на удаление";
                catchPlanScheduleButton1.Enabled = true;
                catchPlanScheduleButton5.Enabled = true;
            }
            tabControl1.SelectTab(tabPage1);
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            catchScheduleComboBox2.SelectedIndex = 0;
            ExceptionLabel1.Text = "Нет прав на добавление";
            ExceptionLabel3.Text = "Нет прав на удаление";
            catchPlanScheduleButton1.Enabled = false;
            catchPlanScheduleButton5.Enabled = false;
            //dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        }

        private void catchPlanScheduleButton1_Click(object sender, EventArgs e)
        {
            // Добавление
            Regex regex = new Regex("^\\d{4}$");
            if (regex.IsMatch(catchSheduleTextBox1.Text))
            {
                planSchController.getListPlanScheduleInserted(new ArrayList { catchScheduleComboBox1.Text, catchScheduleComboBox2.Text, catchSheduleTextBox1.Text });
                ExceptionLabel1.Text = "Успех!";
            }
            else
                ExceptionLabel1.Text = "Некорректный формат года";
            table = planSchController.getListPlanSchedule("","");
            dataGridView1.DataSource = table;
            dataGridView1.Columns[0].Visible = false;
            dataGridView1.Update();
        }

        private void button6_Click(object sender, EventArgs e)
        {

        }

        private void label14_Click(object sender, EventArgs e)
        {

        }


        private void tabPage1_Click(object sender, EventArgs e)
        {

        }
        private void catchPlanScheduleButton4_Click(object sender, EventArgs e)
        {
            
            table = planSchController.getDataPlanScheduleCard(dataGridView1.SelectedCells[0].Value.ToString());
            PlanScheduleCardForm planScheduleCardForm = new PlanScheduleCardForm(table);
            planScheduleCardForm.Owner= this;
            planScheduleCardForm.ShowDialog();

        }
        private void catchPlanScheduleButton2_Click(object sender, EventArgs e)
        {
            dataGridView1.DataSource = null;
            string sort = "";
            if (catchPlanScheduleRadioButton1.Checked) sort = catchPlanScheduleRadioButton1.Tag.ToString();
            if (catchPlanScheduleRadioButton2.Checked) sort = catchPlanScheduleRadioButton2.Tag.ToString();
            if (catchPlanScheduleRadioButton3.Checked) sort = catchPlanScheduleRadioButton3.Tag.ToString();
            table = planSchController.getListPlanSchedule(catchSheduleTextBox2.Text, sort);
            dataGridView1.DataSource = table;
            dataGridView1.Columns[0].Visible = false;
            dataGridView1.Columns[4].Visible = false;
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void groupBox4_Enter(object sender, EventArgs e)
        {

        }

        private void selectedIdLabel_Click(object sender, EventArgs e)
        {

        }

        private void catchPlanScheduleButton5_Click(object sender, EventArgs e)
        {
            planSchController.getListPlanScheduleDeleted(Convert.ToInt32(dataGridView1.SelectedCells[0].Value.ToString()));
            table = planSchController.getListPlanSchedule("","");
            dataGridView1.DataSource = table;
            dataGridView1.Columns[0].Visible = false;
        }
        private void dataGridView1_CellClick(object sender,DataGridViewCellEventArgs e)
        {
            selectedIdLabel.Text = dataGridView1.SelectedCells[0].Value.ToString();
        }

        private void AddNomerContract_TextChanged(object sender, EventArgs e)
        {

        }

        private void catchPlanScheduleButton3_Click(object sender, EventArgs e)
        {
            string sort = "";
            if (catchPlanScheduleRadioButton1.Checked) sort = catchPlanScheduleRadioButton1.Tag.ToString();
            if (catchPlanScheduleRadioButton2.Checked) sort = catchPlanScheduleRadioButton2.Tag.ToString();
            if (catchPlanScheduleRadioButton3.Checked) sort = catchPlanScheduleRadioButton3.Tag.ToString();
            table = planSchController.getListPlanSchedule(catchSheduleTextBox2.Text, sort);
            planSchController.openInExcel(table);
        }

        
    }
}
