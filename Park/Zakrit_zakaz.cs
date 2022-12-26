using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using ZXing.Common;
using ZXing;

namespace Park
{
    public partial class Zakrit_zakaz : Form
    {
        //Строка подклюения
        public static string constr = Sotrudnik.sotrCONNECT;
        //public static string constr = @"Data Source=PC310-1;Initial Catalog=T-Shirts;Integrated Security=True";
        public static SqlConnection connect = null;
        //
        public Zakrit_zakaz()
        {
            InitializeComponent();
            connect = new SqlConnection(constr);
            connect.Open();
        }
        public static int acceptz = 0;
        public static string scode = "";
        private void textBox3_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {

                string sqlRequest2x5 = $"SELECT [Штрих_код] FROM [dbo].[Заказы] where Штрих_код = '{textBox3.Text}' AND ( ID_Статуса = 1 OR ID_Статуса = 2 )";
                SqlDataAdapter adapterx2x5 = new SqlDataAdapter(sqlRequest2x5, connect);
                DataTable dataSet2x5 = new DataTable();
                adapterx2x5.Fill(dataSet2x5);


                if (dataSet2x5.Rows.Count==1)
                {
                    BarcodeWriter writer = new BarcodeWriter()
                    {
                        Format = BarcodeFormat.CODE_128,
                        Options = new EncodingOptions
                        {
                            Height = 35,
                            Width = 200,
                            PureBarcode = false,
                            Margin = 20,

                        },
                    };
                    pictureBox3.Image = writer.Write(textBox3.Text);
                    acceptz = 1;
                    scode = "" + textBox3.Text;
                    MessageBox.Show(
                                            "Данный заказ готов к закрытию!",
                                            "Внимание",
                                            MessageBoxButtons.OK,
                                            MessageBoxIcon.Information,
                                            MessageBoxDefaultButton.Button1,
                    MessageBoxOptions.DefaultDesktopOnly);

                }
                else
                {
                    MessageBox.Show(
                        "Данный заказ не существует\nили уже закрыт!",
                        "Ошибка",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error,
                        MessageBoxDefaultButton.Button1,
MessageBoxOptions.DefaultDesktopOnly);
                    acceptz = 0;
                    textBox3.Text = "";
                    scode = "";
                    pictureBox3.Image = Image.FromFile("..\\Шаблоны\\" + "ШаблонШтрихКода.png");
                }
                this.ActiveControl = null;
            }
        }

        private void textBox3_Enter(object sender, EventArgs e)
        {
            string sqlRequest2x5 = $"SELECT [Штрих_код] FROM [dbo].[Заказы] where ID_Статуса = 1 OR ID_Статуса = 2";
            SqlDataAdapter adapterx2x5 = new SqlDataAdapter(sqlRequest2x5, connect);
            DataTable dataSet2x5 = new DataTable();
            adapterx2x5.Fill(dataSet2x5);

            string[] numbers = new string[dataSet2x5.Rows.Count];

            for (int i = 0; i < dataSet2x5.Rows.Count; i++)
            {
                numbers[i]= dataSet2x5.Rows[i][0].ToString();
            }

            var sourse = new AutoCompleteStringCollection();
            sourse.AddRange(numbers);
            textBox3.AutoCompleteCustomSource = sourse;
            textBox3.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            textBox3.AutoCompleteSource = AutoCompleteSource.CustomSource;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (acceptz == 1)
            {
                try
                {
                    string date = DateTime.Now.Date.Day + "." + DateTime.Now.Date.Month + "." + DateTime.Now.Date.Year;
                    string sqlRequest2x5 = $"UPDATE [dbo].[Заказы] SET ID_Статуса = 3, Дата_закрытия='{date}' where Штрих_код = '{scode}' AND ( ID_Статуса = 1 OR ID_Статуса = 2 )";
                    SqlDataAdapter adapterx2x5 = new SqlDataAdapter(sqlRequest2x5, connect);
                    DataTable dataSet2x5 = new DataTable();
                    adapterx2x5.Fill(dataSet2x5);



                    acceptz = 0;
                    textBox3.Text = "";

                    pictureBox3.Image = Image.FromFile("..\\Шаблоны\\" + "ШаблонШтрихКода.png");

                    MessageBox.Show(
    $"Заказ под штрих кодом: {scode}\nЗакрыт!",
    "Внимание",
    MessageBoxButtons.OK,
    MessageBoxIcon.Information,
    MessageBoxDefaultButton.Button1,
    MessageBoxOptions.DefaultDesktopOnly);

                    scode = "";
                }
                catch
                {
                    MessageBox.Show(
               "Штрих код не проведен!",
               "Ошибка",
               MessageBoxButtons.OK,
               MessageBoxIcon.Error,
               MessageBoxDefaultButton.Button1,
               MessageBoxOptions.DefaultDesktopOnly);
                }
               
            }
            else
            {
                MessageBox.Show(
                "Штрих код не проведен!",
                "Ошибка",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error,
                MessageBoxDefaultButton.Button1,
                MessageBoxOptions.DefaultDesktopOnly);


                acceptz = 0;
                textBox3.Text = "";
                pictureBox3.Image = Image.FromFile("..\\Шаблоны\\" + "ШаблонШтрихКода.png");
                scode = "";
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            StarPoSmene_form adm = new StarPoSmene_form();
            this.Hide();
            adm.Show();
        }

        private void Zakrit_zakaz_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }
    }
}
