using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Park
{
    internal class sqlManager
    {
        //Строка подключения
        public static string conn = @"Data Source=LAPTOP-OTQ2K1D1;Initial Catalog=CPKiO;Integrated Security=True";
        public static SqlConnection connect = null;
        //Меттод открытия соединения
        public static void openConnection(string connectionString)
        {
            connect = new SqlConnection(connectionString);
            connect.Open();
        }
        //Меод закрытия соединения
        public static void closeConnection()
        {
            connect.Close();
        }
        //Метод получения данных в виде <List>
        public static List<string> getTableDataAsList(string query)
        {
            List<string> Request = new List<string> { };
            SqlCommand command = new SqlCommand(query, sqlManager.connect);
            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                Request.Add(reader[0].ToString());
            }
            reader.Close();
            return Request;
        }
        //Метод для получения данных из таблицы в виде строк
        public static string getTableDataAsString(string query)
        {
            string Request = "";
            SqlCommand command = new SqlCommand(query, sqlManager.connect);
            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                //Если строка пустая
                if (string.IsNullOrEmpty(reader[0].ToString()))
                {
                    break;
                }
                else
                {
                    Request = (reader[0].ToString());
                }
            }
            reader.Close();
            return Request;
        }
        //Метод получения данных в виде <DataTable>
        public static DataTable geTableDataAsDataTable(string query)
        {
            DataTable Requset = new DataTable();
            using (SqlCommand cmd = new SqlCommand(query, sqlManager.connect))
            {
                SqlDataReader dataReader = cmd.ExecuteReader();
                Requset.Load(dataReader);
                dataReader.Close();
            }
            return Requset;
        }
        //Метод для Добавлени/Обновлени/Редактирования таблиц
        public static void CUDMethod(string query)
        {
            SqlDataAdapter adapter = new SqlDataAdapter(query, sqlManager.connect);
            DataSet dataSet = new DataSet();
            adapter.Fill(dataSet);
        }
    }
}

