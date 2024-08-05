using Microsoft.Office.Interop.Excel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.ListBox;
using DataTable = System.Data.DataTable;

namespace lab6
{
    internal class DB
    {
        static SQLiteConnection connection = new SQLiteConnection("Data Source=C:\\Users\\poldn\\source\\repos\\Catch-schedule-RTIPPO\\lab6\\db.sqlite3");
        //static SQLiteConnection connection = new SQLiteConnection("Data Source=C:\\Users\\kwa\\Documents\\GitHub\\Catch-schedule-RTIPPO\\lab6\\db.sqlite3");


        static SQLiteCommand cmd = new SQLiteCommand();
        static string sql;
        static DataTable table;
        public static void openConnection()
        {
            connection.Open();
        }

        public static void closeConnection()
        {
            connection.Close();
        }

        public static SQLiteConnection getConnection()
        {
            return connection;
        }

        public static DataTable SelectFromDB(string Sring)
        {
            DataTable table = new DataTable();
            SQLiteDataAdapter adapter = new SQLiteDataAdapter();
            cmd = new SQLiteCommand(Sring, getConnection());

            adapter.SelectCommand = cmd;
            adapter.Fill(table);
            return table;
        }

        // Таня
        // ГОТОВО
        public static DataTable AuthSelectInBD(string loginUser, string passUser)
        {
            string sql = "SELECT *, Role.Name AS Rolename FROM User INNER JOIN Role ON User.id_Role = Role.id_Role WHERE Login = " + loginUser + " AND Password = " + passUser;
            return SelectFromDB(sql);
        }

        public static DataTable ListMunicipalContractsSelect(string filt, bool roleFilter)
        {
            string cellValue = "";
            User user = Session.GetCurrentUser();
            if (roleFilter)
            {
                sql = "SELECT id_MunicipalContract, Number, Date_of_conclusion, Date_of_execution, o1.Name AS Customer, o2.Name AS Executor FROM Municipal_contract INNER JOIN Organization o1 ON Municipal_contract.Customer = o1.id_Organization INNER JOIN Organization o2 ON Municipal_contract.Executor = o2.id_Organization ";
            }
            else
            {
                sql = "SELECT id_MunicipalContract, Number, Date_of_conclusion, Date_of_execution, o1.Name AS Customer, o2.Name AS Executor FROM Municipal_contract INNER JOIN Organization o1 ON Municipal_contract.Customer = o1.id_Organization INNER JOIN Organization o2 ON Municipal_contract.Executor = o2.id_Organization WHERE (Municipal_contract.Customer = '" + user.idOrganization.ToString() + "' OR Municipal_contract.Executor = '" + user.idOrganization.ToString() + "') ";
            }
            table = SelectFromDB(sql);
            if (filt != "")
            {
                for (int i = 0; i < table.Rows.Count; i++)
                {
                    bool there_is_match = false;
                    for (int j = 0; j < table.Columns.Count; j++)
                    {
                        cellValue = table.Rows[i][j].ToString();
                        if (cellValue.Contains(filt))
                        {
                            there_is_match = true;
                        }
                        else if (!there_is_match && j == (table.Columns.Count - 1))
                        {
                            table.Rows[i].Delete();
                            j = table.Columns.Count;
                        }
                    }
                }
                table.AcceptChanges();
            }
            return table;
        }

        public static DataTable ListOrganizationNameSelect()
        {
            string sql = "SELECT id_Organization, Name FROM Organization";
            return SelectFromDB(sql);
        }

        public static DataTable ListLocalitySelectid_Locality()
        {
            string sql = "SELECT id_Locality, Name FROM Locality";
            return SelectFromDB(sql);
        }

        public static DataTable ListLocalityAndPriceForMC(int id_MunicipalContract)
        {
            string sql = "SELECT id_RecordingContract, Name, Recording_Contract.Price, [Recording_Contract].id_Locality FROM Recording_Contract INNER JOIN Locality ON Recording_Contract.id_Locality = Locality.id_Locality WHERE id_MunicipalContract = " + id_MunicipalContract;
            return SelectFromDB(sql);
        }

        public static DataTable ListLocalityFromMC(string nameLocality)
        {
            string sql = "SELECT id_Locality, Name, Price FROM Locality WHERE Name = '" + nameLocality + "'";
            return SelectFromDB(sql);
        }

        public static DataTable SelectViewMunicipalContractCard(int id_MunicipalContract)
        {
            string sql = "SELECT id_MunicipalContract, Number, Date_of_conclusion, Date_of_execution, o1.Name AS Customer, o2.Name AS Executor FROM Municipal_contract INNER JOIN Organization o1 ON Municipal_contract.Customer = o1.id_Organization INNER JOIN Organization o2 ON Municipal_contract.Executor = o2.id_Organization WHERE id_MunicipalContract = " + id_MunicipalContract;
            return SelectFromDB(sql);
        }

        public static void SelectDeleteMunicipalContract(int id_MunicipalContract)
        {
            ExecuteQueryWithAnswer("DELETE FROM Recording_Contract WHERE id_MunicipalContract = " + id_MunicipalContract);
            ExecuteQueryWithAnswer("DELETE FROM Municipal_contract WHERE id_MunicipalContract = " + id_MunicipalContract);
        }

        public static void SelectCreateMunicipalContract(MunicipalContract municipalContract)
        {
            ExecuteQueryWithAnswer("INSERT INTO Municipal_contract (Number, Date_of_conclusion, Date_of_execution, Customer, Executor) VALUES ('" + municipalContract.number + "', '" + municipalContract.dateOfConclusion + "', '" + municipalContract.dateOfExecotion + "', '" + municipalContract.customer + "', '" + municipalContract.executor + "')");
            DataTable idNewContract = SelectFromDB("SELECT id_MunicipalContract FROM Municipal_contract WHERE Number = " + Convert.ToInt32(municipalContract.number));
            for (int i = 0; i < municipalContract.tableLocalyty.Rows.Count; i++)
            {
                DataTable infoLocality = SelectFromDB("SELECT * FROM Locality WHERE Name = '" + municipalContract.tableLocalyty.Rows[i][1].ToString() + "'");
                ExecuteQueryWithAnswer("INSERT INTO Recording_Contract (id_Locality, id_MunicipalContract, Price) VALUES (" + Convert.ToInt32(infoLocality.Rows[0][0]) + ", " + Convert.ToInt32(idNewContract.Rows[0][0]) + ", " + Convert.ToInt32(infoLocality.Rows[0][2]) + ")");
            }
        }


        // ПРОСМОТРИ БЛИН

        public static void SelectUpdateMunicipalContract(int id, ArrayList record, List<string> locality)
        {
            //Получение id Customer
            DataTable Customer = SelectFromDB(
                "SELECT [Organization].id_Organization " +
                "FROM Organization " +
                "WHERE [Organization].Name = '" + record[3] + "'"
                );
            //Получение id Executor
            DataTable Executor = SelectFromDB(
                "SELECT [Organization].id_Organization " +
                "FROM Organization " +
                "WHERE [Organization].Name = '" + record[4] + "'"
                );
            // Запрос на изменение таблицы муниципального контракта
            ExecuteQueryWithAnswer("UPDATE Municipal_contract SET " +
                "Number = '"+ record[0] + "'," +
                "Date_of_conclusion = '"+ record[1] + "'," +
                "Date_of_execution = '" + record[2] + "'," +
                "Customer = '" + Customer.Rows[0][0] + "'," +
                "Executor = '" + Executor.Rows[0][0] + "' " +
                "WHERE id_MunicipalContract = '"+id+"' ");
            // Получение таблицы Recording_Contract c неизменёнными данными, нужен для получения кол-ва записей
            DataTable infoRec = SelectFromDB(
                    "SELECT [Recording_Contract].id_RecordingContract, [Recording_Contract].id_MunicipalContract, [Locality].Name " +
                    "FROM Recording_Contract,Locality WHERE id_MunicipalContract = '" + record[5] + "' AND [Recording_Contract].id_Locality = [Locality].id_Locality "
                );

            //Изменение при уменьшенном числе выбранных городов
            DataTable excessLocalityTable = ListIdLocalitySelect(locality); // Получение таблицы со списком городов на удаление
            for (int j = 0;j< excessLocalityTable.Rows.Count; j++)
            {
                ExecuteQueryWithAnswer("DELETE FROM Recording_Contract WHERE id_Locality = '" + excessLocalityTable.Rows[j][0].ToString() + "' AND id_MunicipalContract = '" + record[5] + "' ");
            }
            int i = 0;
            List<string> list = new List<string>();
            foreach (var nameLocality in locality)
            {
                DataTable infoLocality = SelectFromDB("SELECT * FROM Locality WHERE Name = '" + nameLocality + "'");
                DataTable infoRecordingContract = SelectFromDB("SELECT * FROM Recording_Contract WHERE id_MunicipalContract = '" + record[5] + "'");
                if(i < infoRec.Rows.Count)
                {
                    ExecuteQueryWithAnswer(
                    "UPDATE Recording_Contract SET " +
                    "id_Locality = '" + infoLocality.Rows[0][0] + "'," +
                    "id_MunicipalContract = '" + record[5] + "'," +
                    "Price = '" + infoLocality.Rows[0][2] + "' " +
                    "WHERE id_RecordingContract = '" + infoRecordingContract.Rows[i][0] + "' ");
                }
                if (i >= infoRec.Rows.Count)
                {
                    ExecuteQueryWithAnswer(
                        "INSERT INTO Recording_Contract (id_Locality, id_MunicipalContract, Price) " +
                        "VALUES (" + Convert.ToInt32(infoLocality.Rows[0][0]) + ", " + record[5] + ", " + Convert.ToInt32(infoLocality.Rows[0][2]) + ")");
                }
                i++;
            }
        }
        //Метод на получение таблицы с городами на удаление
        public static DataTable ListIdLocalitySelect(List<string> names)
        {
            string sql = "SELECT [Locality].id_Locality, [Locality].Name FROM Locality WHERE [Locality].Name != '" + names[0] + "' ";
            for (int i = 1; i < names.Count; i++)
            {
                sql += "AND '" + names[i] + "' ";
            }
            DataTable table = SelectFromDB(sql);
            return table;
        }



        // Никита ///////////////////////////////////////////////////////////////////////////////////////////////
        // Список населенных пунктов
        public static DataTable ListLocaitySelect()
        {
            string sql = "SELECT [Locality].Name FROM Locality";
            DataTable table = SelectFromDB(sql);
            return table;
        }
        
        // Запрос к бд на изменение
        public static string ExecuteQueryWithAnswer(string query)
        {
            openConnection();
            cmd.CommandText = query;
            var answer = cmd.ExecuteScalar();
            closeConnection();

            if (answer != null) return answer.ToString();
            else return null;
        }

        // Возвращение таблицы с учетом ограничения роли пользователя и фильтрации
        public static DataTable ListPlanScheduleSelect(string filter, bool roleFilter)
        {
            string sql = "";
            DataTable table = new DataTable();  
            if (roleFilter)
            {
                sql = "SELECT [Plan_Schedule].id, [Locality].Name, [Plan_Schedule].Month, [Plan_Schedule].Year, [Plan_Schedule].PDF_path " +
                         "FROM Plan_Schedule, Locality " +
                         "WHERE [Plan_Schedule].id_Locality = [Locality].id_Locality ";
                table = SelectFromDB(sql);
            }
            else
            {
                sql = "SELECT [Plan_Schedule].id, [Locality].Name, [Plan_Schedule].Month, [Plan_Schedule].Year, [Plan_Schedule].PDF_path " +
                         "FROM Plan_Schedule, Locality " +
                         "WHERE [Plan_Schedule].id_Locality = [Locality].id_Locality " +
                         "AND [Plan_Schedule].id_Organization = '"+Session.GetCurrentUser().idOrganization+"' ";

                table = SelectFromDB(sql);
            }
            string cellValue = ""; 
            table.Columns[0].ColumnName = "id"; table.Columns[1].ColumnName = "Name"; table.Columns[2].ColumnName = "Month"; table.Columns[3].ColumnName = "Year";

            if (filter != "")
                for (int i = 0; i < table.Rows.Count; i++)
                    for (int j = 0; j < table.Columns.Count; j++)
                    {
                        cellValue = table.Rows[i][j].ToString();
                        if (cellValue.Contains(filter))
                        {
                            if (table.Columns[j].ColumnName.ToString() != "Name")
                                sql += "AND [Plan_schedule]." + table.Columns[j].ColumnName.ToString() + " = '" + cellValue + "';";
                            else
                                sql += "AND [Locality]." + table.Columns[j].ColumnName.ToString() + " = '" + cellValue + "';";
                        }

                    }
            table = SelectFromDB(sql);
            return table;
            //if(user.role.name == "Оператор ОМСУ")
            //{
            //    string sql = "SELECT [Plan_Schedule].id, [Locality].Name, [Plan_Schedule].Month, [Plan_Schedule].Year " +
            //             "FROM Plan_Schedule, Locality " +
            //             "WHERE [Plan_Schedule].id_Locality = [Locality].id_Locality ";
            //    DataTable table = SelectFromDB(sql);
            //    return table;
            //}

        }

        //Добавление записи в бд по списку значений
        public static void ListPlanScheduleInsert(ArrayList record) 
        {
            DataTable table = SelectFromDB(
                "SELECT [Locality].id_Locality " +
                "FROM Locality " +
                "WHERE [Locality].Name = '" + record[0]+"' "
                );
            string sql = ExecuteQueryWithAnswer(
                "INSERT INTO Plan_Schedule (id_Locality, Month, Year, PDF_path, id_Organization) " +
                "VALUES ('" + table.Rows[0][0] + "', '" + record[1] + "', '" + record[2] + "', '"+ "" + "', '"+ Session.GetCurrentUser().idOrganization +"'); ");
        }
        //Удаление записи из бд по id
        public static void ListPlanScheduleDelete(int idSelectedPlanSchedule) 
        {
            string sql = ExecuteQueryWithAnswer(
                "DELETE FROM Plan_Schedule " +
                "WHERE id = '" + idSelectedPlanSchedule + "';");
        }
        // Редактирование таблицы по id и списку значений
        public static void ListPlanScheduleUpdate(int idSelectedPlanSchedule, ArrayList record)
        {
            DataTable table = SelectFromDB(
                "SELECT [Locality].id_Locality " +
                "FROM Locality " +
                "WHERE [Locality].Name = '" + record[0] + "'"
                );
            string sql = ExecuteQueryWithAnswer(
                "UPDATE Plan_Schedule SET " +
                "id_Locality = '" + table.Rows[0][0] +"'," +
                "Month = '" + record[1] +"'," +
                "Year = '" + record[2] +"'," +
                "PDF_path = '" + record[3] +"' " +
                "WHERE id = " + idSelectedPlanSchedule + ";");
        }
        
        //Получение данных учетной карточки
        public static DataTable ListDataPlanScheduleCard(string idSelectedPlanSchedule)
        {
            string sql = "SELECT [Plan_Schedule].id, [Locality].Name, [Plan_Schedule].Month, [Plan_Schedule].Year, [Plan_Schedule].PDF_path " +
                        "FROM Plan_Schedule, Locality " +
                        "WHERE [Plan_Schedule].id_Locality = [Locality].id_Locality AND [Plan_Schedule].id = '" + idSelectedPlanSchedule + "'";
            DataTable table = SelectFromDB(sql);
            return table;
        }
    }
}
