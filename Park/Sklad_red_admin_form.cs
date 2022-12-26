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
    public partial class Sklad_red_admin_form : Form
    {
        //Строка подклюения
        public static string constr = Sotrudnik.sotrCONNECT;
       
        public static SqlConnection connect = null;
        //
        public Sklad_red_admin_form()
        {
            InitializeComponent();
            connect = new SqlConnection(constr);
            connect.Open();
        }

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.KeyChar = char.ToUpper(e.KeyChar);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Sklad_admin_form adm = new Sklad_admin_form();
            this.Hide();
            adm.Show();
        }

        private void Sklad_red_admin_form_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

        private void Sklad_red_admin_form_Load(object sender, EventArgs e)
        {
            textBox1.Text = Usluga.nameu;
            textBox2.Text = Usluga.codeu;
            numericUpDown1.Value = Convert.ToDecimal(Usluga.piceu);
            numericUpDown2.Value = Convert.ToDecimal(Usluga.kollu);

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(textBox1.Text == "" || textBox2.Text == "")
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
                string sqlRequest2 = $"UPDATE [dbo].[Услуги]\r\n   SET [Наименование_услуги] = '{textBox1.Text}'\r\n      ,[Код_услуги] = '{textBox2.Text}'\r\n      ,[Стоимость_руб_за_час] = {numericUpDown1.Value}\r\n      ,[Количество] = {numericUpDown2.Value}\r\n WHERE ID_Услуги =  {Usluga.idu}";

                SqlDataAdapter adapterx2 = new SqlDataAdapter(sqlRequest2, connect);
                DataSet dataSet2 = new DataSet();

                adapterx2.Fill(dataSet2);

                MessageBox.Show(
"Услуга успешно обновлена!",
"Успех",
MessageBoxButtons.OK,
MessageBoxIcon.Information,
MessageBoxDefaultButton.Button1,
MessageBoxOptions.DefaultDesktopOnly);
            }
        }
    }
}
