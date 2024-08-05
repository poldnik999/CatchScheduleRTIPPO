using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace lab6
{
    internal class PlanScheduleController
    {
        public DataTable table = new DataTable();

        // получение таблицы из бд
        public DataTable getListPlanSchedule(string filter, string sort) 
        {
            bool roleFilter = Session.GetCurrentPM().CanWatchAll(new CatchPlanSchedule());
            table = DB.ListPlanScheduleSelect(filter, roleFilter);
            table.Columns[0].ColumnName = "id";
            table.Columns[1].ColumnName = "Name";
            table.Columns[2].ColumnName = "Month";
            table.Columns[3].ColumnName = "Year";

            table.DefaultView.Sort = sort;
            table = table.DefaultView.ToTable();

            return table; 
        }

        //Получение данных выбранной учетной карточки
        public DataTable getDataPlanScheduleCard(string idSelectedPlanSchedule){    return DB.ListDataPlanScheduleCard(idSelectedPlanSchedule); }

        //Удаление записи из бд
        public void getListPlanScheduleDeleted(int idSelectedPlanSchedule)
        {
            if(Session.GetCurrentPM().CanUpdate(new CatchPlanSchedule()))
                DB.ListPlanScheduleDelete(idSelectedPlanSchedule);
            else
                MessageBox.Show("Нет прав на Удаление");
        }

        //Добавление записи в бд
        public void getListPlanScheduleInserted(ArrayList record)
        {
            if (Session.GetCurrentPM().CanUpdate(new CatchPlanSchedule()))
                DB.ListPlanScheduleInsert(record);
            else
                MessageBox.Show("Нет прав на Добавление");
        }

        //Изменение записи в бд
        public void getListPlanScheduleUpdated(int idSelectedPlanSchedule, ArrayList record)
        {
            if (Session.GetCurrentPM().CanUpdate(new CatchPlanSchedule()))
                DB.ListPlanScheduleUpdate(idSelectedPlanSchedule, record);
            else
                MessageBox.Show("Нет прав на Изменение");
        }

        //Получение списка Нас. пунктов из бд
        public List<string> getListlocality()
        {
            List<string> lst = new List<string>();
            table = DB.ListLocaitySelect();
            for (int i = 0; i < table.Rows.Count; i++)
                lst.Add(table.Rows[i][0].ToString());

            return lst;
        }
        public void openInExcel(DataTable table)
        {
            ExportMaster.exportExcel(table);
        }
        public string attachPDF()
        {
            return ImportMaster.AttachPDF();
        }
    }
}
