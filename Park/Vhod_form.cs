using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Application = System.Windows.Forms.Application;

namespace Park
{
    public partial class Vhod_form : Form
    {

        //Строка подклюения
        public static string constr = Sotrudnik.sotrCONNECT;
        //public static string constr = @"Data Source=PC310-1;Initial Catalog=T-Shirts;Integrated Security=True";
        public static SqlConnection connect = null;
        //

        public Vhod_form()
        {
            InitializeComponent();
            connect = new SqlConnection(constr);
            connect.Open();
        }

        public static int passGUARD = 1;
        public static int authGUARD = 0;
        public static int capchaGUARD = 0;
        public static string text;

        private void button1_Click(object sender, EventArgs e)
        {
            if (authGUARD != 3)
            {
                if (textBox1.Text == "" || textBox2.Text == "")
                {
                    MessageBox.Show(
    "Вы ввели не все данные для входа!",
    "Ошибка",
    MessageBoxButtons.OK,
    MessageBoxIcon.Error,
    MessageBoxDefaultButton.Button1,
    MessageBoxOptions.DefaultDesktopOnly);
                }
                else
                {
                    try
                    {
                        string requestfoot = ($"SELECT [ID_Сотрудника]\r\n      ,Должности.Наименование_должности\r\n      ,[Фамилия]\r\n      ,[Имя]\r\n      ,[Отчество]\r\n      ,[Логин]\r\n      ,[Пароль]\r\n      ,[Последний_вход]\r\n      ,[Тип_входа]\r\n      ,[Фото]\r\n  FROM [dbo].[Сотрудники] INNER JOIN Должности ON Должности.ID_Должности = Сотрудники.ID_Должности  WHERE Логин = '{textBox1.Text}'\r\n  AND\r\n  Пароль = '{textBox2.Text}'");
                        SqlDataAdapter adapter = new SqlDataAdapter(requestfoot, connect);
                        DataTable ds = new DataTable();
                        adapter.Fill(ds);

                        if (ds.Rows.Count == 0)
                        {



                                authGUARD = authGUARD + 1;
                            if(authGUARD == 3)
                            {
                                MessageBox.Show(
$"Вы исчерпали попытки авторизации!\nПройтиде CAPTCHA для разблокироваки авторизации.",
"Ошибка",
MessageBoxButtons.OK,
MessageBoxIcon.Error,
MessageBoxDefaultButton.Button1,
MessageBoxOptions.DefaultDesktopOnly);
                                pictureBox1.Image = this.CreateImage(pictureBox1.Width, pictureBox1.Height);
                                tabControl1.SelectedTab = tabPage2;
                            }
                            else
                            {
                                MessageBox.Show(
$"Неверные данные авторизации!\nОсталось попыток входа: {3 - authGUARD}",
"Ошибка",
MessageBoxButtons.OK,
MessageBoxIcon.Error,
MessageBoxDefaultButton.Button1,
MessageBoxOptions.DefaultDesktopOnly);
                            }

                            



                        }
                        else
                        {
                            //string kek = ds.Rows[0].ToString();
                            Sotrudnik.idsotr = ds.Rows[0][0].ToString();
                            Sotrudnik.role = ds.Rows[0][1].ToString();
                            Sotrudnik.fam = ds.Rows[0][2].ToString();
                            Sotrudnik.name = ds.Rows[0][3].ToString();
                            Sotrudnik.otch = ds.Rows[0][4].ToString();
                            Sotrudnik.login = ds.Rows[0][5].ToString();
                            Sotrudnik.password = ds.Rows[0][6].ToString();
                            Sotrudnik.foto = ds.Rows[0][9].ToString();

                            MessageBox.Show(
    $"Добро пожаловать {Sotrudnik.fam} {Sotrudnik.name}.",
    "Успех",
    MessageBoxButtons.OK,
    MessageBoxIcon.Asterisk,
    MessageBoxDefaultButton.Button1,
    MessageBoxOptions.DefaultDesktopOnly);
                            DateTime n = DateTime.Now;

                            string sqlRequest = $"INSERT INTO [dbo].[История входа]\r\n           ([Логин]\r\n           ,[Время]\r\n           ,[Тип_входа]) VALUES('{Sotrudnik.login}','{n}','Успешно')";

                            SqlDataAdapter adapterx = new SqlDataAdapter(sqlRequest, connect);
                            DataSet dataSet = new DataSet();

                            adapterx.Fill(dataSet);

                            string sqlRequest2 = $"UPDATE [dbo].[Сотрудники]\r\n   SET \r\n       [Последний_вход] = '{n}'\r\n      ,[Тип_входа] = 'Успешно'\r\n\r\n WHERE ID_Сотрудника = {Sotrudnik.idsotr}";
                            string lol = "INSERT INTO [dbo].[Заказы]\r\n           ([Код_заказа]\r\n           ,[Дата_создания]\r\n           ,[Время_заказа]\r\n           ,[ID_Клиента]\r\n           ,[ID_Списка_услуг]\r\n           ,[ID_Статуса]\r\n           ,[Дата_закрытия]\r\n           ,[Время_проката_часов]\r\n           ,[Штрих_код])\r\n     VALUES\r\n           (<Код_заказа, nvarchar(255),>\r\n           ,<Дата_создания, date,>\r\n           ,<Время_заказа, time(4),>\r\n           ,<ID_Клиента, int,>\r\n           ,<ID_Списка_услуг, int,>\r\n           ,<ID_Статуса, int,>\r\n           ,<Дата_закрытия, date,>\r\n           ,<Время_проката_часов, int,>\r\n           ,<Штрих_код, nvarchar(255),>)";
                            SqlDataAdapter adapterx2 = new SqlDataAdapter(sqlRequest2, connect);
                            DataSet dataSet2 = new DataSet();

                            adapterx2.Fill(dataSet2);


                            if (Sotrudnik.role == "Администратор")
                            {
                                Admin_form adm = new Admin_form();
                                this.Hide();
                                adm.Show();
                            }
                            else if (Sotrudnik.role == "Продавец")
                            {
                                Prodavec_form adm = new Prodavec_form();
                                this.Hide();
                                adm.Show();
                            }
                            else if (Sotrudnik.role == "Старший смены")
                            {
                                StarPoSmene_form adm = new StarPoSmene_form();
                                this.Hide();
                                adm.Show();
                            }



                        }

                        //pictureBox63.Image = Image.FromFile("..\\Фото Сотрудников\\" + FOTO);

                    }
                    catch
                    {
                        MessageBox.Show(
        "Произошел сбой в работе Базы данных - обратитесь к системному администратору!",
        "Ошибка",
        MessageBoxButtons.OK,
        MessageBoxIcon.Error,
        MessageBoxDefaultButton.Button1,
        MessageBoxOptions.DefaultDesktopOnly);




                    }
                }
            }
            else
            {
                MessageBox.Show(
$"Вы исчерпали попытки авторизации!\nПройтиде CAPTCHA для разблокироваки авторизации.",
"Ошибка",
MessageBoxButtons.OK,
MessageBoxIcon.Error,
MessageBoxDefaultButton.Button1,
MessageBoxOptions.DefaultDesktopOnly);
                pictureBox1.Image = this.CreateImage(pictureBox1.Width, pictureBox1.Height);
                tabControl1.SelectedTab = tabPage2;
            }
            

        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (passGUARD == 1)
            {
                passGUARD = 0;
                textBox2.UseSystemPasswordChar= false;
                button3.Text = "👁";
            }
            else
            {
                passGUARD = 1;
                textBox2.UseSystemPasswordChar = true;
                button3.Text = "●";

            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }



        private Bitmap CreateImage(int Width, int Height)
        {
            Random rnd = new Random();

            //Создадим изображение
            Bitmap result = new Bitmap(Width, Height);

            //Вычислим позицию текста
            int Xpos = 80;
            int Ypos = 70;

            //Добавим различные цвета ддя текста
            Brush[] colors = {
Brushes.Green
};

            Pen[] colorpens = {
Pens.Black, };

            //Делаем случайный стиль текста
            FontStyle[] fontstyle = {
FontStyle.Italic,
FontStyle.Regular,
FontStyle.Strikeout,
FontStyle.Underline};

            //Добавим различные углы поворота текста
            Int16[] rotate = { 1, -1, 2, -2, 3, -3, 4, -4, 5, -5, 6, -10 };

            //Укажем где рисовать
            Graphics g = Graphics.FromImage((Image)result);

            //Пусть фон картинки будет белым
            g.Clear(Color.White);

            //Делаем случайный угол поворота текста
            g.RotateTransform(rnd.Next(rotate.Length));

            //Генерируем текст
            text = String.Empty;
            string ALF = "7890QWERTYUIOPASDFGHJKLZXCVBNM";
for (int i = 0; i < 3; ++i)
                text += ALF[rnd.Next(ALF.Length)];

            //Нарисуем сгенирируемый текст
            g.DrawString(text,
            new Font("Comic Sans MS", 25, fontstyle[rnd.Next(fontstyle.Length)]),
            colors[rnd.Next(colors.Length)],
            new PointF(Xpos, Ypos));

            //Добавим немного помех
            //Линии из углов
            g.DrawLine(colorpens[rnd.Next(colorpens.Length)],
            new Point(0, 0),
            new Point(Width - 1, Height - 1));
            g.DrawLine(colorpens[rnd.Next(colorpens.Length)],
            new Point(0, Height - 1),
            new Point(Width - 1, 0));

            //Черные точки
            for (int i = 0; i < Width; ++i)
                for (int j = 0; j < Height; ++j)
                    if (rnd.Next() % 8 == 0)
                        result.SetPixel(i, j, Color.Black);
            for (int i = 0; i < Width; ++i)
                for (int j = 0; j < Height; ++j)
                    if (rnd.Next() % 8 == 0)
                        result.SetPixel(i, j, Color.Green);

            return result;
        }



        private void button5_Click(object sender, EventArgs e)
        {
            pictureBox1.Image = this.CreateImage(pictureBox1.Width, pictureBox1.Height);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedTab = tabPage1;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (textBox3.Text == text)
            {
                capchaGUARD = 0;
                authGUARD = 0;
                MessageBox.Show(
$"CAPTCHA введена верно!",
"Успех",
MessageBoxButtons.OK,
MessageBoxIcon.Information,
MessageBoxDefaultButton.Button1,
MessageBoxOptions.DefaultDesktopOnly);

                tabControl1.SelectedTab = tabPage1;

                
            }
            else
            {
                if(capchaGUARD != 3)
                {
                    capchaGUARD = capchaGUARD + 1;
                    if(capchaGUARD == 3)
                    {
                        MessageBox.Show(
$"Вы исчерпали свои попытки для ввода CAPTCHA!\nВы будете заблокированы на 1 минуту.",
"Ошибка",
MessageBoxButtons.OK,
MessageBoxIcon.Error,
MessageBoxDefaultButton.Button1,
MessageBoxOptions.DefaultDesktopOnly);
                        Application.Exit();
                    }
                    else
                    {
                        MessageBox.Show(
$"Неверно введена CAPTCHA!\nОсталось попыток: {3 - capchaGUARD}",
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
$"Вы исчерпали свои попытки для ввода CAPTCHA!\nВы будете заблокированы на 30 секунд.",
"Ошибка",
MessageBoxButtons.OK,
MessageBoxIcon.Error,
MessageBoxDefaultButton.Button1,
MessageBoxOptions.DefaultDesktopOnly);
                    Application.Exit();
                }
               
                
            }
               
        }


        private void textBox3_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.KeyChar = char.ToUpper(e.KeyChar);
        }

        private void Vhod_form_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

        private void tabPage1_Click(object sender, EventArgs e)
        {

        }

        private void Vhod_form_Load(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
