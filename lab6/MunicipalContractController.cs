using Microsoft.Office.Interop.Excel;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Threading;
using System.Windows.Forms;
using Application = System.Windows.Forms.Application;
using DataTable = System.Data.DataTable;

namespace lab6
{
    internal class MunicipalContractController
    {
        public User user = Session.GetCurrentUser();
        public PM pm = Session.GetCurrentPM();
        public bool canUpdate = Session.GetCurrentPM().CanUpdate(new MunicipalContract());


        public DataTable table = new DataTable();
        public string filtr = "";
        public string sort = "";
        Thread th;

        public DataTable getListMunicipalContract(string sort, string filtr)
        {
            bool roleFilter = Session.GetCurrentPM().CanWatchAll(new MunicipalContract());
            table = DB.ListMunicipalContractsSelect(filtr, roleFilter);
            table.Columns["Number"].ColumnName = "Номер";
            table.Columns["Date_of_conclusion"].ColumnName = "Дата Заключения";
            table.Columns["Date_of_execution"].ColumnName = "Дата действия";
            table.Columns["Customer"].ColumnName = "Заказчик";
            table.Columns["Executor"].ColumnName = "Исполнитель";
            table.DefaultView.Sort = sort;
            table = table.DefaultView.ToTable();
            return table;
        }
        public void UpdateMunicipalContract(int idmunisipalContract, ArrayList record, List<string> locality)
        {
            //if (canUpdate)
            DB.SelectUpdateMunicipalContract(idmunisipalContract, record, locality);
        }
        public DataTable getListOrganization()
        {
            return DB.ListOrganizationNameSelect();
        }

        public DataTable getListLocaity()
        {
            return DB.ListLocalitySelectid_Locality();
        }

        public MunicipalContract ViewMunicipalContractCard(int idmunisipalContract)
        {
            table = DB.SelectViewMunicipalContractCard(idmunisipalContract);
            MunicipalContract municipalContract = new MunicipalContract(table);
            return municipalContract;

        }

        public DataTable CreateMunicipalContract(ArrayList record, ArrayList arrayLocalityContract)
        {
            if (canUpdate)
            {
                MunicipalContract municipalContract = new MunicipalContract(record, arrayLocalityContract);

                DB.SelectCreateMunicipalContract(municipalContract);
            }
            else MessageBox.Show("У вас недостаточно прав для удаления записи!");
            return getListMunicipalContract(sort, filtr);
        }


        public DataTable DeleteMunicipalContract(int idmunisipalContract)
        {
            if (canUpdate)
            {
                DB.SelectDeleteMunicipalContract(idmunisipalContract);
            }
            else MessageBox.Show("У вас недостаточно прав для удаления записи!");
            return getListMunicipalContract(sort, filtr);

        }

    }
}
