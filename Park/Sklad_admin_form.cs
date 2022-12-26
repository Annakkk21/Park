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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Park
{
    public partial class Sklad_admin_form : Form
    {
        //Строка подклюения
        public static string constr = Sotrudnik.sotrCONNECT;
       
        public static SqlConnection connect = null;
        //
        public Sklad_admin_form()
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

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                Usluga.idu = (dataGridView1.SelectedRows[0].Cells[0].Value).ToString();
                Usluga.nameu = (dataGridView1.SelectedRows[0].Cells[1].Value).ToString();
                Usluga.codeu = (dataGridView1.SelectedRows[0].Cells[2].Value).ToString();
                Usluga.piceu = (dataGridView1.SelectedRows[0].Cells[3].Value).ToString();
                Usluga.kollu = (dataGridView1.SelectedRows[0].Cells[4].Value).ToString();

                Sklad_red_admin_form adm = new Sklad_red_admin_form();
                this.Hide();
                adm.Show();
            }
            catch
            {
                MessageBox.Show(
"Вы невыбрали услугу!",
"Ошибка",
MessageBoxButtons.OK,
MessageBoxIcon.Error,
MessageBoxDefaultButton.Button1);
            }
           




        }

        private void button1_Click(object sender, EventArgs e)
        {
            Sklad_add_admin_form adm = new Sklad_add_admin_form();
            this.Hide();
            adm.Show();
        }

        private void Sklad_admin_form_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

        private void Sklad_admin_form_Load(object sender, EventArgs e)
        {
            string requestfoot = ($"\r\nSELECT [ID_Услуги]\r\n      ,[Наименование_услуги]\r\n      ,[Код_услуги]\r\n      ,[Стоимость_руб_за_час]\r\n      ,[Количество]\r\n  FROM [dbo].[Услуги]");
            SqlDataAdapter adapter = new SqlDataAdapter(requestfoot, connect);
            DataTable ds = new DataTable();
            adapter.Fill(ds);
            dataGridView1.DataSource = ds;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                string idpos = (dataGridView1.SelectedRows[0].Cells[0].Value).ToString();

                string requestfoot = ($"DELETE FROM[dbo].[Услуги]\r\n                WHERE ID_Услуги = {idpos}");
                SqlDataAdapter adapter = new SqlDataAdapter(requestfoot, connect);
                DataTable ds = new DataTable();
                adapter.Fill(ds);
                
                MessageBox.Show(
 "Услуга успешно удалена со склада!",
 "Внимание",
 MessageBoxButtons.OK,
 MessageBoxIcon.Information,
 MessageBoxDefaultButton.Button1);
                string requestfoot2 = ($"\r\nSELECT [ID_Услуги]\r\n      ,[Наименование_услуги]\r\n      ,[Код_услуги]\r\n      ,[Стоимость_руб_за_час]\r\n      ,[Количество]\r\n  FROM [dbo].[Услуги]");
                SqlDataAdapter adapter2 = new SqlDataAdapter(requestfoot2, connect);
                DataTable ds2 = new DataTable();
                adapter2.Fill(ds2);
                dataGridView1.DataSource = ds2;
            }
            catch
            {
                MessageBox.Show(
"Вы невыбрали услугу!",
"Ошибка",
MessageBoxButtons.OK,
MessageBoxIcon.Error,
MessageBoxDefaultButton.Button1);
            }
        }
    }
}
