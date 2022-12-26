using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Park
{
    public partial class ClientMenu : Form
    {
        //Строка подклюения
        public static string constr = Sotrudnik.sotrCONNECT;
       
        public static SqlConnection connect = null;
        //
        public ClientMenu()
        {
            InitializeComponent();
            connect = new SqlConnection(constr);
            connect.Open();
        }

        private void ClientMenu_Load(object sender, EventArgs e)
        {
            string requestfoot = ($"\r\nSELECT * FROM [dbo].[Клиенты]");
            SqlDataAdapter adapter = new SqlDataAdapter(requestfoot, connect);
            DataTable ds = new DataTable();
            adapter.Fill(ds);
            dataGridView1.DataSource = ds;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            StarPoSmene_oformlenie_form adm = new StarPoSmene_oformlenie_form();
            this.Hide();
            adm.Show();
        }

        private void ClientMenu_FormClosing(object sender, FormClosingEventArgs e)
        {
            System.Windows.Forms.Application.Exit();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            string requestfoot = ($"SELECT * FROM [dbo].[Клиенты] WHERE ID_Клиента LIKE '{textBox1.Text}%' AND Фамилия LIKE '{textBox2.Text}%' AND Имя LIKE '{textBox3.Text}%' AND Отчество LIKE '{textBox4.Text}%' AND Серия_паспорта LIKE '{textBox5.Text}%' AND Номер_паспорта LIKE '{textBox6.Text}%' AND Дата_рождения LIKE '{textBox8.Text}%' AND Адрес LIKE '{textBox7.Text}%'");
            SqlDataAdapter adapter = new SqlDataAdapter(requestfoot, connect);
            DataTable ds = new DataTable();
            adapter.Fill(ds);
            dataGridView1.DataSource = ds;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Client_add adm = new Client_add();
            this.Hide();
            adm.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                string idpos = (dataGridView1.SelectedRows[0].Cells[0].Value).ToString();

                string requestfoot = ($"DELETE FROM[dbo].[Клиенты] WHERE ID_Клиента = {idpos}");
                SqlDataAdapter adapter = new SqlDataAdapter(requestfoot, connect);
                DataTable ds = new DataTable();
                adapter.Fill(ds);

                MessageBox.Show(
 "Клиент успешно удален!",
 "Внимание",
 MessageBoxButtons.OK,
 MessageBoxIcon.Information,
 MessageBoxDefaultButton.Button1);
                string requestfoot2 = ($"Select * From Клиенты");
                SqlDataAdapter adapter2 = new SqlDataAdapter(requestfoot2, connect);
                DataTable ds2 = new DataTable();
                adapter2.Fill(ds2);
                dataGridView1.DataSource = ds2;
            }
            catch
            {
                MessageBox.Show(
"Вы невыбрали клиента!",
"Ошибка",
MessageBoxButtons.OK,
MessageBoxIcon.Error,
MessageBoxDefaultButton.Button1);
            }
        }
    }
}
