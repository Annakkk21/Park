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
    public partial class Sklad_add_admin_form : Form
    {
        //Строка подклюения
        public static string constr = Sotrudnik.sotrCONNECT;
       
        public static SqlConnection connect = null;
        //
        public Sklad_add_admin_form()
        {
            InitializeComponent();
            connect = new SqlConnection(constr);
            connect.Open();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Sklad_admin_form adm = new Sklad_admin_form();
            this.Hide();
            adm.Show();
        }

        private void Sklad_add_admin_form_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "" || textBox2.Text == "")
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
                string sqlRequest2 = $"INSERT INTO [dbo].[Услуги]\r\n           ([Наименование_услуги]\r\n           ,[Код_услуги]\r\n           ,[Стоимость_руб_за_час]\r\n           ,[Количество])\r\n     VALUES('{textBox1.Text}','{textBox2.Text}',{numericUpDown1.Value},{numericUpDown2.Value})";

                SqlDataAdapter adapterx2 = new SqlDataAdapter(sqlRequest2, connect);
                DataSet dataSet2 = new DataSet();

                adapterx2.Fill(dataSet2);

                MessageBox.Show(
"Услуга успешно добавлена!",
"Успех",
MessageBoxButtons.OK,
MessageBoxIcon.Information,
MessageBoxDefaultButton.Button1,
MessageBoxOptions.DefaultDesktopOnly);
            }
        }

        private void Sklad_add_admin_form_Load(object sender, EventArgs e)
        {

        }
    }
}
