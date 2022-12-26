using iTextSharp.text.pdf;
using iTextSharp.text;
using Microsoft.Office.Interop.Word;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using Document = iTextSharp.text.Document;
//using Microsoft.Office.Interop.Access.Dao;
using static ZXing.Rendering.SvgRenderer;
using Application = System.Windows.Forms.Application;

namespace Park
{
    public partial class Otchet_admin_form : Form
    {
        public Otchet_admin_form()
        {
            InitializeComponent();
        }

       

        private void button2_Click(object sender, EventArgs e)
        {
            Admin_form adm = new Admin_form();
            this.Hide();
            adm.Show();
        }

        private void Otchet_admin_form_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

        private void Otchet_admin_form_Load(object sender, EventArgs e)
        {
            //TODO: данная строка кода позволяет загрузить данные в таблицу "cPKiODataSet.Услуги".При необходимости она может быть перемещена или удалена.
            this.услугиTableAdapter.Fill(this.cPKiODataSet.Услуги);

        }

        private void button1_Click(object sender, EventArgs e)
        {
            MessageBox.Show(
"Отчеты временно недоступны!",
"Внимание",
MessageBoxButtons.OK,
MessageBoxIcon.Warning,
MessageBoxDefaultButton.Button1,
MessageBoxOptions.DefaultDesktopOnly);

        }

        private void button6_Click(object sender, EventArgs e)
        {
            chart1.Visible = true;
            dataGridView1.Visible = false;

            if (checkBox1.Checked == true)
            {
                string periodStartDate = maskedTextBox1.Text;
                string periodEndDate = maskedTextBox2.Text;

                sqlManager.openConnection(sqlManager.conn);
                dataGridView1.DataSource = sqlManager.geTableDataAsDataTable("select Заказы.Дата_создания, count([Список услуг].ID_Услуги) from Заказы, [Список услуг] where [Список услуг].ID_Заказа = Заказы.ID_Заказа and Заказы.Дата_создания >= '" + periodStartDate + "' and Заказы.Дата_создания <= '" + periodEndDate + "' group by Заказы.Дата_создания");
                sqlManager.closeConnection();

                sqlManager.openConnection(sqlManager.conn);

                List<string> Dates = sqlManager.getTableDataAsList("select Заказы.Дата_создания from Заказы, [Список услуг] where[Список услуг].ID_Заказа = Заказы.ID_Заказа and Заказы.Дата_создания >= '" + periodStartDate + "' and Заказы.Дата_создания <= '" + periodEndDate + "' group by Заказы.Дата_создания");
                List<string> values = sqlManager.getTableDataAsList("select count([Список услуг].ID_Услуги) from Заказы, [Список услуг] where[Список услуг].ID_Заказа = Заказы.ID_Заказа and Заказы.Дата_создания >= '" + periodStartDate + "' and Заказы.Дата_создания <= '" + periodEndDate + "' group by Заказы.Дата_создания");

                sqlManager.closeConnection();

                List<DateTime> trueDates = new List<DateTime>();
                List<int> trueValue = new List<int>();

                foreach (var date in Dates)
                {
                    trueDates.Add(DateTime.Parse(date));
                }
                foreach (var value in values)
                {
                    trueValue.Add(int.Parse(value));
                }
                chart1.Series[0].Points.Clear();

                chart1.Series[0].XValueType = ChartValueType.Date;

                chart1.ChartAreas[0].AxisY.Minimum = trueValue.Min();
                chart1.ChartAreas[0].AxisY.Maximum = trueValue.Max();

                chart1.ChartAreas[0].AxisX.Minimum = trueDates.Min().ToOADate();
                chart1.ChartAreas[0].AxisX.Maximum = trueDates.Max().ToOADate();

                chart1.ChartAreas[0].AxisX.Interval = 1;
                chart1.ChartAreas[0].AxisY.Interval = 1;

                for (int i = 0; i < trueDates.Count; i++)
                {
                    chart1.Series[0].Points.AddXY(trueDates[i].ToOADate(), trueValue[i]);
                }
            }
            else if (checkBox2.Checked == true)
            {
                string periodStartDate = maskedTextBox1.Text;
                string periodEndDate = maskedTextBox2.Text;

                sqlManager.openConnection(sqlManager.conn);
                dataGridView1.DataSource = sqlManager.geTableDataAsDataTable("select Заказы.Дата_создания,Услуги.Наименование_услуги,count([Список услуг].ID_Заказа) from [Список услуг],Заказы,Услуги where [Список услуг].ID_Услуги = Услуги.ID_Услуги and [Список услуг].ID_Заказа = Заказы.ID_Заказа and Заказы.Дата_создания >= '" + periodStartDate + "' and Заказы.Дата_создания <= '" + periodEndDate + "' group by Заказы.Дата_создания, Услуги.Наименование_услуги");
                sqlManager.closeConnection();

                sqlManager.openConnection(sqlManager.conn);

                List<string> stringFormatDates = sqlManager.getTableDataAsList("select Заказы.Дата_создания from [Список услуг],Заказы,Услуги where [Список услуг].ID_Услуги = Услуги.ID_Услуги and [Список услуг].ID_Заказа = Заказы.ID_Заказа and Заказы.Дата_создания >= '" + periodStartDate + "' and Заказы.Дата_создания <= '" + periodEndDate + "' group by Заказы.Дата_создания, Услуги.Наименование_услуги");
                List<string> servicesList = sqlManager.getTableDataAsList("select DISTINCT Услуги.Наименование_услуги from [Список услуг],Заказы,Услуги where [Список услуг].ID_Услуги = Услуги.ID_Услуги and [Список услуг].ID_Заказа = Заказы.ID_Заказа and Заказы.Дата_создания >= '" + periodStartDate + "' and Заказы.Дата_создания <= '" + periodEndDate + "' group by Заказы.Дата_создания, Услуги.Наименование_услуги");
                List<string> stringFormateValue = sqlManager.getTableDataAsList("select count([Список услуг].ID_Заказа) from [Список услуг],Заказы,Услуги where [Список услуг].ID_Услуги = Услуги.ID_Услуги and [Список услуг].ID_Заказа = Заказы.ID_Заказа and Заказы.Дата_создания >= '" + periodStartDate + "' and Заказы.Дата_создания <= '" + periodEndDate + "' group by Заказы.Дата_создания, Услуги.Наименование_услуги");

                sqlManager.closeConnection();

                List<DateTime> trueDates = new List<DateTime>();
                List<int> trueValues = new List<int>();

                foreach (var date in stringFormatDates)
                {
                    trueDates.Add(DateTime.Parse(date));
                }
                foreach (var value in stringFormateValue)
                {
                    trueValues.Add(int.Parse(value));
                }

                chart1.ChartAreas[0].AxisY.Minimum = 0;
                chart1.ChartAreas[0].AxisY.Maximum = trueValues.Max();

                chart1.ChartAreas[0].AxisX.Minimum = trueDates.Min().ToOADate();
                chart1.ChartAreas[0].AxisX.Maximum = trueDates.Max().ToOADate();

                chart1.ChartAreas[0].AxisX.Interval = 1;
                chart1.ChartAreas[0].AxisY.Interval = 1;


                for (int i = 0; i < servicesList.Count; i++)
                {
                    chart1.Series.Add(servicesList[i]);
                    chart1.Series[i].ChartType = SeriesChartType.Line;
                    chart1.Series[i].XValueType = ChartValueType.Date;
                }
                for (int i = 0; i < servicesList.Count; i++)
                {
                    chart1.Series[i].Points.AddXY(trueDates[i].ToOADate(), trueValues[i]);

                }

            }
            else if (checkBox3.Checked == true)
            {
                string periodStartDate = maskedTextBox1.Text;
                string periodEndDate = maskedTextBox2.Text;

                sqlManager.openConnection(sqlManager.conn);
                dataGridView1.DataSource = sqlManager.geTableDataAsDataTable("select Дата_создания,count(ID_Заказа) from Заказы where Дата_Создания >= '" + periodStartDate + "' and Дата_Создания <= '" + periodEndDate + "' group by Дата_создания");
                sqlManager.closeConnection();

                sqlManager.openConnection(sqlManager.conn);

                List<string> Dates = sqlManager.getTableDataAsList("select Дата_создания from Заказы where Дата_Создания >= '" + periodStartDate + "' and Дата_Создания <= '" + periodEndDate + "' group by Дата_создания");
                List<string> values = sqlManager.getTableDataAsList("select count(ID_Заказа) from Заказы where Дата_Создания >= '" + periodStartDate + "' and Дата_Создания <= '" + periodEndDate + "' group by Дата_создания");


                sqlManager.closeConnection();

                List<DateTime> trueDates = new List<DateTime>();
                List<int> trueValue = new List<int>();

                foreach (var date in Dates)
                {
                    trueDates.Add(DateTime.Parse(date));
                }
                foreach (var value in values)
                {
                    trueValue.Add(int.Parse(value));
                }


                chart1.Series[0].Points.Clear();
                chart1.Series[0].ChartType = SeriesChartType.Bar;
                chart1.Series[0].XValueType = ChartValueType.Date;

                chart1.ChartAreas[0].AxisY.Minimum = 0;
                chart1.ChartAreas[0].AxisY.Maximum = trueValue.Max();

                chart1.ChartAreas[0].AxisX.Minimum = trueDates.Min().ToOADate();
                chart1.ChartAreas[0].AxisX.Maximum = trueDates.Max().ToOADate();

                chart1.ChartAreas[0].AxisX.Interval = 1;
                chart1.ChartAreas[0].AxisY.Interval = 1;

                for (int i = 0; i < trueDates.Count; i++)
                {
                    chart1.Series[0].Points.AddXY(trueDates[i].ToOADate(), trueValue[i]);
                }
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            chart1.Visible = false;
            dataGridView1.Visible = true;

            if (checkBox1.Checked == true)
            {
                string periodStartDate = maskedTextBox1.Text;
                string periodEndDate = maskedTextBox2.Text;

                sqlManager.openConnection(sqlManager.conn);
                dataGridView1.DataSource = sqlManager.geTableDataAsDataTable("select Заказы.Дата_создания, count([Список услуг].ID_Услуги) from Заказы, [Список услуг] where [Список услуг].ID_Заказа = Заказы.ID_Заказа and Заказы.Дата_создания >= '" + periodStartDate + "' and Заказы.Дата_создания <= '" + periodEndDate + "' group by Заказы.Дата_создания");
                sqlManager.closeConnection();

                sqlManager.openConnection(sqlManager.conn);

                List<string> Dates = sqlManager.getTableDataAsList("select Заказы.Дата_создания from Заказы, [Список услуг] where[Список услуг].ID_Заказа = Заказы.ID_Заказа and Заказы.Дата_создания >= '" + periodStartDate + "' and Заказы.Дата_создания <= '" + periodEndDate + "' group by Заказы.Дата_создания");
                List<string> values = sqlManager.getTableDataAsList("select count([Список услуг].ID_Услуги) from Заказы, [Список услуг] where[Список услуг].ID_Заказа = Заказы.ID_Заказа and Заказы.Дата_создания >= '" + periodStartDate + "' and Заказы.Дата_создания <= '" + periodEndDate + "' group by Заказы.Дата_создания");

                sqlManager.closeConnection();

                List<DateTime> trueDates = new List<DateTime>();
                List<int> trueValue = new List<int>();

                foreach (var date in Dates)
                {
                    trueDates.Add(DateTime.Parse(date));
                }
                foreach (var value in values)
                {
                    trueValue.Add(int.Parse(value));
                }


                chart1.Series[0].Points.Clear();

                chart1.Series[0].XValueType = ChartValueType.Date;

                chart1.ChartAreas[0].AxisY.Minimum = trueValue.Min();
                chart1.ChartAreas[0].AxisY.Maximum = trueValue.Max();

                chart1.ChartAreas[0].AxisX.Minimum = trueDates.Min().ToOADate();
                chart1.ChartAreas[0].AxisX.Maximum = trueDates.Max().ToOADate();

                chart1.ChartAreas[0].AxisX.Interval = 1;
                chart1.ChartAreas[0].AxisY.Interval = 1;

                for (int i = 0; i < trueDates.Count; i++)
                {
                    chart1.Series[0].Points.AddXY(trueDates[i].ToOADate(), trueValue[i]);
                }
            }
            else if (checkBox2.Checked == true)
            {
                string periodStartDate = maskedTextBox1.Text;
                string periodEndDate = maskedTextBox2.Text;

                sqlManager.openConnection(sqlManager.conn);
                dataGridView1.DataSource = sqlManager.geTableDataAsDataTable("select Заказы.Дата_создания,Услуги.Наименование_услуги,count([Список услуг].ID_Заказа) from [Список услуг],Заказы,Услуги where [Список услуг].ID_Услуги = Услуги.ID_Услуги and [Список услуг].ID_Заказа = Заказы.ID_Заказа and Заказы.Дата_создания >= '" + periodStartDate + "' and Заказы.Дата_создания <= '" + periodEndDate + "' group by Заказы.Дата_создания, Услуги.Наименование_услуги");
                sqlManager.closeConnection();

                sqlManager.openConnection(sqlManager.conn);

                List<string> stringFormatDates = sqlManager.getTableDataAsList("select Заказы.Дата_создания from [Список услуг],Заказы,Услуги where [Список услуг].ID_Услуги = Услуги.ID_Услуги and [Список услуг].ID_Заказа = Заказы.ID_Заказа and Заказы.Дата_создания >= '" + periodStartDate + "' and Заказы.Дата_создания <= '" + periodEndDate + "' group by Заказы.Дата_создания, Услуги.Наименование_услуги");
                List<string> servicesList = sqlManager.getTableDataAsList("select DISTINCT Услуги.Наименование_услуги from [Список услуг],Заказы,Услуги where [Список услуг].ID_Услуги = Услуги.ID_Услуги and [Список услуг].ID_Заказа = Заказы.ID_Заказа and Заказы.Дата_создания >= '" + periodStartDate + "' and Заказы.Дата_создания <= '" + periodEndDate + "' group by Заказы.Дата_создания, Услуги.Наименование_услуги");
                List<string> stringFormateValue = sqlManager.getTableDataAsList("select count([Список услуг].ID_Заказа) from [Список услуг],Заказы,Услуги where [Список услуг].ID_Услуги = Услуги.ID_Услуги and [Список услуг].ID_Заказа = Заказы.ID_Заказа and Заказы.Дата_создания >= '" + periodStartDate + "' and Заказы.Дата_создания <= '" + periodEndDate + "' group by Заказы.Дата_создания, Услуги.Наименование_услуги");

                sqlManager.closeConnection();

                List<DateTime> trueDates = new List<DateTime>();
                List<int> trueValues = new List<int>();

                foreach (var date in stringFormatDates)
                {
                    trueDates.Add(DateTime.Parse(date));
                }
                foreach (var value in stringFormateValue)
                {
                    trueValues.Add(int.Parse(value));
                }

                chart1.ChartAreas[0].AxisY.Minimum = 0;
                chart1.ChartAreas[0].AxisY.Maximum = trueValues.Max();

                chart1.ChartAreas[0].AxisX.Minimum = trueDates.Min().ToOADate();
                chart1.ChartAreas[0].AxisX.Maximum = trueDates.Max().ToOADate();

                chart1.ChartAreas[0].AxisX.Interval = 1;
                chart1.ChartAreas[0].AxisY.Interval = 1;


                for (int i = 0; i < servicesList.Count; i++)
                {
                    chart1.Series.Add(servicesList[i]);
                    chart1.Series[i].ChartType = SeriesChartType.Line;
                    chart1.Series[i].XValueType = ChartValueType.Date;
                }
                for (int i = 0; i < servicesList.Count; i++)
                {
                    chart1.Series[i].Points.AddXY(trueDates[i].ToOADate(), trueValues[i]);

                }
            }
            else if (checkBox3.Checked == true)
            {
                string periodStartDate = maskedTextBox1.Text;
                string periodEndDate = maskedTextBox2.Text;

                sqlManager.openConnection(sqlManager.conn);
                dataGridView1.DataSource = sqlManager.geTableDataAsDataTable("select Дата_создания,count(ID_Заказа) from Заказы where Дата_Создания >= '" + periodStartDate + "' and Дата_Создания <= '" + periodEndDate + "' group by Дата_создания");
                sqlManager.closeConnection();

                sqlManager.openConnection(sqlManager.conn);

                List<string> Dates = sqlManager.getTableDataAsList("select Дата_создания from Заказы where Дата_Создания >= '" + periodStartDate + "' and Дата_Создания <= '" + periodEndDate + "' group by Дата_создания");
                List<string> values = sqlManager.getTableDataAsList("select count(ID_Заказа) from Заказы where Дата_Создания >= '" + periodStartDate + "' and Дата_Создания <= '" + periodEndDate + "' group by Дата_создания");


                sqlManager.closeConnection();

                List<DateTime> trueDates = new List<DateTime>();
                List<int> trueValue = new List<int>();

                foreach (var date in Dates)
                {
                    trueDates.Add(DateTime.Parse(date));
                }
                foreach (var value in values)
                {
                    trueValue.Add(int.Parse(value));
                }


                chart1.Series[0].Points.Clear();
                chart1.Series[0].ChartType = SeriesChartType.Bar;
                chart1.Series[0].XValueType = ChartValueType.Date;

                chart1.ChartAreas[0].AxisY.Minimum = 0;
                chart1.ChartAreas[0].AxisY.Maximum = trueValue.Max();

                chart1.ChartAreas[0].AxisX.Minimum = trueDates.Min().ToOADate();
                chart1.ChartAreas[0].AxisX.Maximum = trueDates.Max().ToOADate();

                chart1.ChartAreas[0].AxisX.Interval = 1;
                chart1.ChartAreas[0].AxisY.Interval = 1;

                for (int i = 0; i < trueDates.Count; i++)
                {
                    chart1.Series[0].Points.AddXY(trueDates[i].ToOADate(), trueValue[i]);
                }
            }
        }


        public Bitmap controlScreenshot(Control control)
        {
            Graphics graphics = control.CreateGraphics();
            Bitmap bm = new Bitmap(control.Width, control.Height);
            control.DrawToBitmap(bm, new System.Drawing.Rectangle(0, 0, control.Width, control.Height));
            return bm;

        }

        public void exportarPDF(Bitmap img, string savePath)
        {
            System.Drawing.Image image = img;
            Document doc = new Document(PageSize.A4);
            PdfWriter.GetInstance(doc, new FileStream(savePath, FileMode.Create));
            doc.Open();
            iTextSharp.text.Image pdfImage = iTextSharp.text.Image.GetInstance(image,
                    System.Drawing.Imaging.ImageFormat.Jpeg);
            doc.Add(pdfImage);
            doc.Close();
        }

        public void exportarPDF(Bitmap img, Bitmap img2, string savePath)
        {
            var pageSize = new iTextSharp.text.Rectangle(0, 0, img.Width, img.Height);

            System.Drawing.Image image = img;
            System.Drawing.Image image2 = img2;
            Document doc = new Document(pageSize);
            PdfWriter.GetInstance(doc, new FileStream(savePath, FileMode.Create));
            doc.Open();
            iTextSharp.text.Image pdfImage = iTextSharp.text.Image.GetInstance(image,
                    System.Drawing.Imaging.ImageFormat.Jpeg);
            iTextSharp.text.Image pdfImage2 = iTextSharp.text.Image.GetInstance(image2,
                    System.Drawing.Imaging.ImageFormat.Jpeg);
            doc.Add(pdfImage);
            doc.Add(pdfImage2);
            doc.Close();
        }

        public void showVariants()
        {
            variantsForm vrf = new variantsForm();
            vrf.ShowDialog();

            if (meta.result == 1)
            {
                //онли таблица
                int height = dataGridView1.Height;
                dataGridView1.Height = dataGridView1.RowCount * dataGridView1.RowTemplate.Height;

                Bitmap bitmap = controlScreenshot(dataGridView1);

                dataGridView1.Height = height;

                //В PDF
                Random rnd = new Random();
                string savePath = Path.GetFullPath(@"../../Statements/" + rnd.Next().ToString() + ".pdf");
                exportarPDF(bitmap, savePath);
            }
            else if (meta.result == 2)
            {
                //онли график
                Bitmap bitmap = controlScreenshot(chart1);

                Random rnd = new Random();
                string savePath = Path.GetFullPath(@"../../Statements/" + rnd.Next().ToString() + ".pdf");

                exportarPDF(bitmap, savePath);
            }
            else if (meta.result == 3)
            {
                int height = dataGridView1.Height;
                dataGridView1.Height = dataGridView1.RowCount * dataGridView1.RowTemplate.Height;

                //И то и то
                dataGridView1.Visible = true;
                chart1.Visible = true;
                Bitmap bitmap1 = controlScreenshot(dataGridView1);
                dataGridView1.Height = height;
                Bitmap bitmap2 = controlScreenshot(chart1);

                Random rnd = new Random();
                string savePath = Path.GetFullPath(@"../../Statements/" + rnd.Next().ToString() + ".pdf");

                exportarPDF(bitmap1, bitmap2, savePath);
            }



        }


        private void button3_Click(object sender, EventArgs e)
        {
            showVariants();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Admin_form adm = new Admin_form();
            this.Hide();
            adm.Show();
        }

      
    }
}

