using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ZXing;
using ZXing.Common;
using ZXing.QrCode.Internal;
using static System.Net.Mime.MediaTypeNames;
using static System.Net.WebRequestMethods;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using Application = System.Windows.Forms.Application;
using Image = System.Drawing.Image;

namespace Park
{

    public partial class StarPoSmene_oformlenie_form : Form
    {
        //Строка подклюения
        public static string constr = Sotrudnik.sotrCONNECT;
        //public static string constr = @"Data Source=PC310-1;Initial Catalog=T-Shirts;Integrated Security=True";
        public static SqlConnection connect = null;
        //
        public static string code128;

        public static int[] times = new int[100];
        public static int[] timesunsort;
        public static int timescoll = 0;

        public static int[] prices = new int[100];
        public static int pricescoll = 0;

        public static int priceitog = 0;
        public static int accept = 0;
        public static int idz = 0;



        public StarPoSmene_oformlenie_form()
        {
            InitializeComponent();
            connect = new SqlConnection(constr);
            connect.Open();
        }








        private void StarPoSmene_oformlenie_form_Load(object sender, EventArgs e)
        {
            // TODO: данная строка кода позволяет загрузить данные в таблицу "cPKiODataSet3.Услуги". При необходимости она может быть перемещена или удалена.
            this.услугиTableAdapter1.Fill(this.cPKiODataSet3.Услуги);
            // TODO: данная строка кода позволяет загрузить данные в таблицу "cPKiODataSet2.Клиенты". При необходимости она может быть перемещена или удалена.
            this.клиентыTableAdapter1.Fill(this.cPKiODataSet2.Клиенты);
         
            //SELECT ID_Клиента, CONCAT(Фамилия, ' ', Имя, ' ', Отчество) AS [ФИО], Серия_паспорта, Номер_паспорта, Дата_рождения, Адрес, Email, Пароль FROM dbo.Клиенты

            string sqlRequest2 = $"SELECT ID_Клиента, CONCAT(Фамилия, ' ', Имя, ' ', Отчество) AS [ФИО], Серия_паспорта, Номер_паспорта, Дата_рождения, Адрес, Email, Пароль FROM dbo.Клиенты";

            SqlDataAdapter adapterx2 = new SqlDataAdapter(sqlRequest2, connect);
            DataSet ds = new DataSet();
            adapterx2.Fill(ds);


            string sqlRequest22 = $"SELECT [ID_Клиента] FROM [dbo].[Клиенты] WHERE ID_Клиента = {comboBox2.SelectedValue}";

            SqlDataAdapter adapterx22 = new SqlDataAdapter(sqlRequest22, connect);
            DataTable dataSet22 = new DataTable();

            adapterx2.Fill(dataSet22);
            int idckient = (Convert.ToInt32(dataSet22.Rows[0][0]));




            comboBox2.DataSource = ds.Tables[0];
            comboBox2.DisplayMember = "ФИО";
            comboBox2.ValueMember = "ID_Клиента";

            string date = DateTime.Now.Date.Day + "." + DateTime.Now.Date.Month + "." + DateTime.Now.Date.Year;
            textBox1.Text = $"{comboBox2.SelectedValue}/{date}";
            textBox1.ForeColor = Color.Gray;

        }










        private void button1_Click(object sender, EventArgs e)
        {
            if (accept != 0)
            {
                string sqlRequest2x5 = $"DELETE FROM [dbo].[Заказы] WHERE ID_Заказа = {idz}";
                SqlDataAdapter adapterx2x5 = new SqlDataAdapter(sqlRequest2x5, connect);
                DataSet dataSet2x5 = new DataSet();
                adapterx2x5.Fill(dataSet2x5);
                idz = 0;
                accept = 0;
                pictureBox3.Image = Image.FromFile("..\\Шаблоны\\" + "ШаблонШтрихКода.png");
                string datex = DateTime.Now.Date.Day + "." + DateTime.Now.Date.Month + "." + DateTime.Now.Date.Year;
                textBox1.Text = $"{comboBox2.SelectedValue}/{datex}";
                textBox1.ForeColor = Color.Gray;
            }

            timescoll = 0;
            pricescoll = 0;
            Array.Resize(ref times, 100);
            Array.Resize(ref prices, 100);
            listBox1.Items.Clear();
            listBox2.Items.Clear();
            listBox3.Items.Clear();
            textBox2.Text = "";
            string date = DateTime.Now.Date.Day + "." + DateTime.Now.Date.Month + "." + DateTime.Now.Date.Year;
            textBox1.Text = $"{comboBox2.SelectedValue}/{date}";
            textBox1.ForeColor = Color.Gray;
            priceitog = 0;
            pictureBox3.Image = Image.FromFile("..\\Шаблоны\\" + "ШаблонШтрихКода.png");



        }

        private void клиентыBindingSource_CurrentChanged(object sender, EventArgs e)
        {

        }



        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            //string lol = comboBox2.SelectedValue.ToString();

            if (accept != 0)
            {
                string sqlRequest2x5 = $"DELETE FROM [dbo].[Заказы] WHERE ID_Заказа = {idz}";
                SqlDataAdapter adapterx2x5 = new SqlDataAdapter(sqlRequest2x5, connect);
                DataSet dataSet2x5 = new DataSet();
                adapterx2x5.Fill(dataSet2x5);
                idz = 0;
                accept = 0;
                pictureBox3.Image = Image.FromFile("..\\Шаблоны\\" + "ШаблонШтрихКода.png");
                string date = DateTime.Now.Date.Day + "." + DateTime.Now.Date.Month + "." + DateTime.Now.Date.Year;
                textBox1.Text = $"{comboBox2.SelectedValue}/{date}";
                textBox1.ForeColor = Color.Gray;
            }
            else
            {
                string date = DateTime.Now.Date.Day + "." + DateTime.Now.Date.Month + "." + DateTime.Now.Date.Year;
                textBox1.Text = $"{comboBox2.SelectedValue}/{date}";
                textBox1.ForeColor = Color.Gray;
            }
        }


        private void pictureBox1_Click(object sender, EventArgs e)
        {
            if (accept != 0)
            {
                string sqlRequest2x5 = $"DELETE FROM [dbo].[Заказы] WHERE ID_Заказа = {idz}";
                SqlDataAdapter adapterx2x5 = new SqlDataAdapter(sqlRequest2x5, connect);
                DataSet dataSet2x5 = new DataSet();
                adapterx2x5.Fill(dataSet2x5);
                idz = 0;
                accept = 0;
                pictureBox3.Image = Image.FromFile("..\\Шаблоны\\" + "ШаблонШтрихКода.png");
                string date = DateTime.Now.Date.Day + "." + DateTime.Now.Date.Month + "." + DateTime.Now.Date.Year;
                textBox1.Text = $"{comboBox2.SelectedValue}/{date}";
                textBox1.ForeColor = Color.Gray;
            }
            try
            {
                // string selectedState = comboBox1.SelectedItem.ToString();
                //listBox1.Items.Add(selectedState);


                //listBox1.Items.Add(comboBox1.SelectedValue);
                string lol = comboBox1.Text;
                listBox1.Items.Add(comboBox1.SelectedValue);
                listBox2.Items.Add(lol);
                string timetext = "";
                if (numericUpDown1.Value == 1)
                {
                    timetext = "час.";
                }
                else if (numericUpDown1.Value > 1 && numericUpDown1.Value < 5)
                {
                    timetext = "часa.";
                }
                else
                {
                    timetext = "часов.";
                }
                listBox3.Items.Add(numericUpDown1.Value.ToString() + $" {timetext}");
                times[timescoll] = Convert.ToInt32(numericUpDown1.Value);
                timescoll++;

                string sqlRequest2 = $"SELECT [Стоимость_руб_за_час] FROM [dbo].[Услуги] WHERE ID_Услуги = {comboBox1.SelectedValue}";

                SqlDataAdapter adapterx2 = new SqlDataAdapter(sqlRequest2, connect);
                DataTable dataSet2 = new DataTable();

                adapterx2.Fill(dataSet2);

                int price = (Convert.ToInt32(dataSet2.Rows[0][0])) * Convert.ToInt32(numericUpDown1.Value);
                prices[pricescoll] = price;
                pricescoll++;
                priceitog = priceitog + price;
                textBox2.Text = priceitog + "руб.";





            }
            catch
            {

            }
        }

        private void textBox1_Enter(object sender, EventArgs e)
        {
            if (textBox1.Text != "" && textBox1.ForeColor != Color.Gray)
            {

            }
            else
            {

                textBox1.Text = null;
                textBox1.ForeColor = Color.Black;
                var sourse = new AutoCompleteStringCollection();

                string date = DateTime.Now.Date.Day + "." + DateTime.Now.Date.Month + "." + DateTime.Now.Date.Year;

                string[] numbers = new string[1];
                numbers[0] = $"{comboBox2.SelectedValue}/{date}";
                sourse.AddRange(numbers);
                textBox1.AutoCompleteCustomSource = sourse;
                textBox1.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                textBox1.AutoCompleteSource = AutoCompleteSource.CustomSource;




            }

        }

        private void textBox1_Leave(object sender, EventArgs e)
        {
            if (textBox1.Text != "")
            {

            }
            else
            {
                textBox1.Text = "";
                string date = DateTime.Now.Date.Day + "." + DateTime.Now.Date.Month + "." + DateTime.Now.Date.Year;
                //label3.Text = "Примечание номер заказа должен быть: " + $"{comboBox2.SelectedValue}/{date}";
                textBox1.Text = $"{comboBox2.SelectedValue}/{date}";
                textBox1.ForeColor = Color.Gray;
            }
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                string date = DateTime.Now.Date.Day + "." + DateTime.Now.Date.Month + "." + DateTime.Now.Date.Year;
                if (textBox1.Text == $"{comboBox2.SelectedValue}/{date}")
                {
                    if (textBox1.ForeColor == Color.Black) {


                        if (accept != 0)
                        {
                            string sqlRequest2x5 = $"DELETE FROM [dbo].[Заказы] WHERE ID_Заказа = {idz}";
                            SqlDataAdapter adapterx2x5 = new SqlDataAdapter(sqlRequest2x5, connect);
                            DataSet dataSet2x5 = new DataSet();
                            adapterx2x5.Fill(dataSet2x5);
                            idz = 0;
                            accept = 0;
                            pictureBox3.Image = Image.FromFile("..\\Шаблоны\\" + "ШаблонШтрихКода.png");
                            string datex = DateTime.Now.Date.Day + "." + DateTime.Now.Date.Month + "." + DateTime.Now.Date.Year;
                            textBox1.Text = $"{comboBox2.SelectedValue}/{datex}";
                            textBox1.ForeColor = Color.Gray;
                        }
                        else
                        {

                        }

                        if (timescoll != 0)
                        {
                            int kek = 0;

                            kek = Convert.ToInt32(listBox1.Items.Count);
                            string[] kek2 = new string[kek];
                            int razmer = 0;
                            for (int i = 0; i < kek; i++)
                            {
                                razmer++;
                                kek2[i] = listBox1.Items[i].ToString();
                            }
                            Array.Resize(ref prices, razmer);
                            Array.Resize(ref times, razmer);
                            timesunsort = times;
                            Array.Sort(times);
                            int timmax = times[razmer - 1];


                            string timesnow = DateTime.Now.Hour + ":" + DateTime.Now.Minute;

                            string sqlRequest2x = $"INSERT INTO [dbo].[Заказы]\r\n           ([Код_заказа]\r\n           ,[Дата_создания]\r\n           ,[Время_заказа]\r\n           ,[ID_Клиента]\r\n           ,[ID_Списка_услуг]\r\n           ,[ID_Статуса]\r\n           ,[Дата_закрытия]\r\n           ,[Время_проката_часов]\r\n           ,[Штрих_код])\r\n     VALUES\r\n           ('{textBox1.Text}','{date}','{timesnow}',{comboBox2.SelectedValue},'','1',NULL,{timmax},'')";
                            SqlDataAdapter adapterx2x = new SqlDataAdapter(sqlRequest2x, connect);
                            DataSet dataSet2x = new DataSet();
                            adapterx2x.Fill(dataSet2x);

                            string sqlRequest2xlma = $"select * from Заказы";
                            SqlDataAdapter adapterx2xlma = new SqlDataAdapter(sqlRequest2xlma, connect);
                            DataTable dataSet2xlma = new DataTable();
                            adapterx2xlma.Fill(dataSet2xlma);

                            int countes = dataSet2xlma.Rows.Count;
                            string idzak = dataSet2xlma.Rows[countes - 1][0].ToString();
                            idz = Convert.ToInt32(idzak);
                            Random rand = new Random();
                            int randx = rand.Next(100000, 999999);
                            string shcode = $"{idzak}{DateTime.Now.Date.Day}{DateTime.Now.Date.Month}{DateTime.Now.Date.Year}{DateTime.Now.Hour}{DateTime.Now.Minute}{timmax}{randx}";
                            code128 = shcode;
                            string sqlRequest2xl = $"UPDATE [dbo].[Заказы] SET [ID_Списка_услуг] = '{idzak}' ,[ID_Статуса] = '2' ,[Штрих_код] = '{shcode}' WHERE ID_Заказа = '{idzak}'";
                            SqlDataAdapter adapterx2xl = new SqlDataAdapter(sqlRequest2xl, connect);
                            DataSet dataSet2xl = new DataSet();
                            adapterx2xl.Fill(dataSet2xl);
                            accept = 1;

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
                            pictureBox3.Image = writer.Write(shcode);
                        }
                        else
                        {
                            MessageBox.Show(
    "Вы не указали ни одной услуги!",
    "Ошибка",
    MessageBoxButtons.OK,
    MessageBoxIcon.Error,
    MessageBoxDefaultButton.Button1,
    MessageBoxOptions.DefaultDesktopOnly);

                            textBox1.Text = $"{comboBox2.SelectedValue}/{date}";
                            textBox1.ForeColor = Color.Gray;
                        }



                    }



                }
                else
                {
                    MessageBox.Show(
"Введен неверный код заказа!",
"Ошибка",
MessageBoxButtons.OK,
MessageBoxIcon.Error,
MessageBoxDefaultButton.Button1,
MessageBoxOptions.DefaultDesktopOnly);
                    textBox1.Text = $"{comboBox2.SelectedValue}/{date}";
                    textBox1.ForeColor = Color.Gray;
                }


                this.ActiveControl = null;
            }
        }

        private void StarPoSmene_oformlenie_form_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (accept == 1)
            {
                string sqlRequest2x5 = $"DELETE FROM [dbo].[Заказы] WHERE ID_Заказа = {idz}";
                SqlDataAdapter adapterx2x5 = new SqlDataAdapter(sqlRequest2x5, connect);
                DataSet dataSet2x5 = new DataSet();
                adapterx2x5.Fill(dataSet2x5);
            }
            Application.Exit();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (accept != 0)
            {
                string sqlRequest2x5 = $"DELETE FROM [dbo].[Заказы] WHERE ID_Заказа = {idz}";
                SqlDataAdapter adapterx2x5 = new SqlDataAdapter(sqlRequest2x5, connect);
                DataSet dataSet2x5 = new DataSet();
                adapterx2x5.Fill(dataSet2x5);
                idz = 0;
                accept = 0;
                pictureBox3.Image = Image.FromFile("..\\Шаблоны\\" + "ШаблонШтрихКода.png");
                string datex = DateTime.Now.Date.Day + "." + DateTime.Now.Date.Month + "." + DateTime.Now.Date.Year;
                textBox1.Text = $"{comboBox2.SelectedValue}/{datex}";
                textBox1.ForeColor = Color.Gray;
            }
            ClientMenu adm = new ClientMenu();
            this.Hide();
            adm.Show();

        }


        private void button4_Click(object sender, EventArgs e)
        {
            if (accept != 0)
            {
                string sqlRequest2x5 = $"DELETE FROM [dbo].[Заказы] WHERE ID_Заказа = {idz}";
                SqlDataAdapter adapterx2x5 = new SqlDataAdapter(sqlRequest2x5, connect);
                DataSet dataSet2x5 = new DataSet();
                adapterx2x5.Fill(dataSet2x5);
                idz = 0;
                accept = 0;
                pictureBox3.Image = Image.FromFile("..\\Шаблоны\\" + "ШаблонШтрихКода.png");
                string datex = DateTime.Now.Date.Day + "." + DateTime.Now.Date.Month + "." + DateTime.Now.Date.Year;
                textBox1.Text = $"{comboBox2.SelectedValue}/{datex}";
                textBox1.ForeColor = Color.Gray;
            }

            if(Sotrudnik.role == "Продавец")
            {
                Prodavec_form adm = new Prodavec_form();
                this.Hide();
                adm.Show();
            }
            else
            {
                StarPoSmene_form adm = new StarPoSmene_form();
                this.Hide();
                adm.Show();
            }

        }


        private async void button3_Click(object sender, EventArgs e)
        {
            string date = DateTime.Now.Date.Day + "." + DateTime.Now.Date.Month + "." + DateTime.Now.Date.Year;
            string timesnow = DateTime.Now.Hour + ":" + DateTime.Now.Minute;
            if (accept == 1)
            {
                if (textBox1.Text == $"{comboBox2.SelectedValue}/{date}")
                {
                    if (textBox1.ForeColor == Color.Black)
                    {
                        if (timescoll != 0)
                        {
                            try
                            {
                                int kek = 0;

                                kek = Convert.ToInt32(listBox1.Items.Count);
                                string[] kek2 = new string[kek];
                                int razmer = 0;
                                for (int i = 0; i < kek; i++)
                                {
                                    razmer++;
                                    kek2[i] = listBox1.Items[i].ToString();
                                }

                                string[] kekname = new string[kek];
                                for (int i = 0; i < kek; i++)
                                {

                                    kekname[i] = listBox2.Items[i].ToString();
                                }


                                Array.Resize(ref times, razmer);
                                Array.Resize(ref prices, razmer);

                                Array.Sort(times);
                                int timmax = times[razmer - 1];

                                string sqlRequest2xlma = $"select * from Заказы";
                                SqlDataAdapter adapterx2xlma = new SqlDataAdapter(sqlRequest2xlma, connect);
                                DataTable dataSet2xlma = new DataTable();
                                adapterx2xlma.Fill(dataSet2xlma);

                                int countes = dataSet2xlma.Rows.Count;
                                string idzak = dataSet2xlma.Rows[countes - 1][0].ToString();
                                idz = Convert.ToInt32(idzak);



                                for (int i = 0; i < kek; i++)
                                {
                                    string sqlRequest2 = $"INSERT INTO [dbo].[Список услуг]\r\n           ([ID_Списка_услуг]\r\n           ,[ID_Заказа]\r\n           ,[ID_Услуги])\r\n     VALUES\r\n           ({idzak},{idzak},{kek2[i]})";

                                    SqlDataAdapter adapterx2 = new SqlDataAdapter(sqlRequest2, connect);
                                    DataSet dataSet2 = new DataSet();

                                    adapterx2.Fill(dataSet2);

                                    string sqlRequest2X = $"UPDATE [dbo].[Услуги] SET [Количество] = Количество-1 WHERE ID_Услуги = {kek2[i]}";

                                    SqlDataAdapter adapterx2X = new SqlDataAdapter(sqlRequest2X, connect);
                                    DataSet dataSet2X = new DataSet();

                                    adapterx2X.Fill(dataSet2X);
                                }

                                string sqlRequest2x = $"Select * From Клиенты Where ID_Клиента = {comboBox2.SelectedValue}";

                                SqlDataAdapter adapterx2x = new SqlDataAdapter(sqlRequest2x, connect);
                                DataTable dataSet2x = new DataTable();

                                adapterx2x.Fill(dataSet2x);

                                

                                Zakaz.idzak = ""+idz;
                                Zakaz.datezak = ""+ date;
                                Zakaz.timezak = ""+ timesnow;
                                Zakaz.idklient = $"{dataSet2x.Rows[0][0].ToString()}";
                                Zakaz.fiolient = $"{dataSet2x.Rows[0][1].ToString()}"+ $" {dataSet2x.Rows[0][2].ToString()}" + $" {dataSet2x.Rows[0][3].ToString()}";
                                Zakaz.adreslient = $"{dataSet2x.Rows[0][7].ToString()}";
                                Zakaz.pos = kekname;
                                Zakaz.pricepos = prices;
                                Zakaz.itog = ""+ priceitog;
                                Zakaz.scode = "" + code128;





                                timescoll = 0;
                                pricescoll = 0;
                                Array.Resize(ref times, 100);
                                Array.Resize(ref prices, 100);
                                listBox1.Items.Clear();
                                listBox2.Items.Clear();
                                listBox3.Items.Clear();
                                textBox2.Text = "";
                                textBox1.Text = $"{comboBox2.SelectedValue}/{date}";
                                textBox1.ForeColor = Color.Gray;

                                priceitog = 0;

                                pictureBox3.Image = Image.FromFile("..\\Шаблоны\\" + "ШаблонШтрихКода.png");
                                accept = 0;





                                

                                idz = 0;

                                MessageBox.Show(
"Заказ успешно оформлен осталось\nраспечатать Штрих коды и электронные формы заказа!",
"Успех",
MessageBoxButtons.OK,
MessageBoxIcon.Information,
MessageBoxDefaultButton.Button1,
MessageBoxOptions.DefaultDesktopOnly);
                                textBox1.Text = $"{comboBox2.SelectedValue}/{date}";
                                textBox1.ForeColor = Color.Gray;

                                StarPoSmene_oformlenie_zavershenie_form adm = new StarPoSmene_oformlenie_zavershenie_form();
                                this.Hide();
                                adm.Show();

                            }
                            catch
                            {
                                MessageBox.Show(
    "Произошла непредвиденная ошибка Базы данных\nПожалуста свяжитесь с исстемным Администратором!",
    "Ошибка",
    MessageBoxButtons.OK,
    MessageBoxIcon.Error,
    MessageBoxDefaultButton.Button1,
    MessageBoxOptions.DefaultDesktopOnly);
                                textBox1.Text = $"{comboBox2.SelectedValue}/{date}";
                                textBox1.ForeColor = Color.Gray;
                            }

                        }
                        else
                        {
                            MessageBox.Show(
     "Вы не указали ни одной услуги!",
     "Ошибка",
     MessageBoxButtons.OK,
     MessageBoxIcon.Error,
     MessageBoxDefaultButton.Button1,
     MessageBoxOptions.DefaultDesktopOnly);
                            textBox1.Text = $"{comboBox2.SelectedValue}/{date}";
                            textBox1.ForeColor = Color.Gray;
                        }
                    }
                    else
                    {
                        MessageBox.Show(
      "Вы не ввели код заказа!",
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
       "Введен неверный код заказа!",
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
"Вы не сформировали штрих код!",
"Ошибка",
MessageBoxButtons.OK,
MessageBoxIcon.Error,
MessageBoxDefaultButton.Button1,
MessageBoxOptions.DefaultDesktopOnly);
            }
        }
    }
}
