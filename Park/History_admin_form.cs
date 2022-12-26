using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Park
{
    public partial class History_admin_form : Form
    {
        //Строка подклюения
        public static string constr = Sotrudnik.sotrCONNECT;
       
        public static SqlConnection connect = null;
        //
        public History_admin_form()
        {
            InitializeComponent();
            connect = new SqlConnection(constr);
            connect.Open();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Admin_form adm = new Admin_form();
            this.Hide();
            adm.Show();
        }

        private void History_admin_form_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

        private void History_admin_form_Load(object sender, EventArgs e)
        {
            string requestfoot = ($"SELECT [ID_Входа]\r\n      ,[Логин]\r\n      ,[Время]\r\n      ,[Тип_входа]\r\n  FROM [dbo].[История входа]");
            SqlDataAdapter adapter = new SqlDataAdapter(requestfoot, connect);
            DataTable ds = new DataTable();
            adapter.Fill(ds);
            dataGridView1.DataSource = ds;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            
                string requestfoot = ($"SELECT [ID_Входа]\r\n      ,[Логин]\r\n      ,[Время]\r\n      ,[Тип_входа]\r\n  FROM [dbo].[История входа]");
                SqlDataAdapter adapter = new SqlDataAdapter(requestfoot, connect);
                DataTable ds = new DataTable();
                adapter.Fill(ds);
                dataGridView1.DataSource = ds;
            
        }
    }
}
