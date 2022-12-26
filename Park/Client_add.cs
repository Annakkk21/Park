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
    public partial class Client_add : Form
    {
        //Строка подклюения
        public static string constr = Sotrudnik.sotrCONNECT;
        
        public static SqlConnection connect = null;
        //
        public Client_add()
        {
            InitializeComponent();
            connect = new SqlConnection(constr);
            connect.Open();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            ClientMenu adm = new ClientMenu();
            this.Hide();
            adm.Show();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (textBox2.Text == "" || textBox3.Text == "" || textBox4.Text == "" || textBox7.Text == "")
            {
                MessageBox.Show(
    "Не все поля заполнены!",
    "Ошибка",
    MessageBoxButtons.OK,
    MessageBoxIcon.Error,
                MessageBoxDefaultButton.Button1,
    MessageBoxOptions.DefaultDesktopOnly);
            }
            else
            {
                string sqlRequest2 = $"INSERT INTO [dbo].[Клиенты] ([Фамилия],[Имя],[Отчество],[Серия_паспорта],[Номер_паспорта],[Дата_рождения],[Адрес],[Email],[Пароль]) VALUES('{textBox2.Text}','{textBox3.Text}','{textBox4.Text}','{numericUpDown1.Value}','{numericUpDown2.Value}','{dateTimePicker1.Value}','{textBox7.Text}',NULL,NULL)";

                SqlDataAdapter adapterx2 = new SqlDataAdapter(sqlRequest2, connect);
                DataSet dataSet2 = new DataSet();

                adapterx2.Fill(dataSet2);

                MessageBox.Show(
"Клиент успешно добавлен!",
"Успех",
MessageBoxButtons.OK,
MessageBoxIcon.Information,
MessageBoxDefaultButton.Button1,
MessageBoxOptions.DefaultDesktopOnly);
            }
        }

        private void Client_add_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }
    }
    
}
