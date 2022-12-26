using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ZXing.Common;
using ZXing;
using System.Diagnostics;
using System.IO;
//using EO.WebBrowser.DOM;
//using Microsoft.Office.Interop;
using Microsoft.Office.Interop.Word;
using Application = Microsoft.Office.Interop.Word.Application;


namespace Park
{
    public partial class StarPoSmene_oformlenie_zavershenie_form : Form
    {
        public StarPoSmene_oformlenie_zavershenie_form()
        {
            InitializeComponent();
        }

        private void StarPoSmene_oformlenie_zavershenie_form_Load(object sender, EventArgs e)
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
            pictureBox3.Image = writer.Write(Zakaz.scode);
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            //Электронный вид с выгрузкой в pdf


            string uslugi = "";
            for (int i = 0; i < Zakaz.pos.Length; i++)
            {
                uslugi = uslugi + $"<h1>{Zakaz.pos[i]}: {Zakaz.pricepos[i]}руб.</h1> ";
            }
            string text = $"<html><head><title>Заказ№{Zakaz.idzak}</title></head><body>\r\n  <style type=\"text/css\">h3" + "{ font-size: 300%; \r\n     font-family: Comic Sans MS, Arial, Helvetica, sans-serif; \r\n     color: #000000;margin-bottom: -10px;margin-top: 40px;}\r\n    h1 { font-size: 200%; \r\n     font-family: Comic Sans MS, Arial, Helvetica, sans-serif; \r\n     color: #000000;}h2 { font-size: 150%; \r\n     font-family: Comic Sans MS, Arial, Helvetica, sans-serif;  \r\n     color: #000000;}h4 { font-size: 200%; \r\n     font-family: Comic Sans MS, Arial, Helvetica, sans-serif; \r\n     color: #000000;margin-top: -40px;margin-bottom: -20px;}</style>" + $"\r\n  <header><h4>_______________________________________</h4>\r\n    <h3>Заказ№{Zakaz.idzak}</h3></header><br><br>\r\n  <h4>_______________________________________</h4><br>\r\n<h1>Дата: {Zakaz.datezak} Время: {Zakaz.timezak}</h1>\r\n<h4>_______________________________________</h4><br>\r\n<h2>Код клиента: {Zakaz.idklient}</h1>\r\n<h2>ФИО Клиента: {Zakaz.fiolient}</h1>\r\n<h2>Адрес: {Zakaz.adreslient}</h1><br>\r\n<h4>_______________________________________</h4><h1>Перечень услуг:</h1>\r\n<h4>_______________________________________</h4><br>\r\n{uslugi}\r\n<h4>_______________________________________</h4><br><h1>Итог: {Zakaz.itog}руб.</h1>\r\n<h4>_______________________________________</h4></body></html>";

            using (FileStream fstream = new FileStream("..\\Электронный_вид\\" + $"Заказ№{Zakaz.idzak}.html", FileMode.OpenOrCreate))
            {
                // преобразуем строку в байты
                byte[] buffer = Encoding.Default.GetBytes(text);
                // запись массива байтов в файл
                await fstream.WriteAsync(buffer, 0, buffer.Length);
                fstream?.Close();
            }



            using (FileStream fstream = new FileStream("..\\Электронный_вид\\" + $"Заказ№{Zakaz.idzak}.pdf", FileMode.OpenOrCreate))
            {
                // преобразуем строку в байты
                byte[] buffer = Encoding.Default.GetBytes(text);
                // запись массива байтов в файл
                await fstream.WriteAsync(buffer, 0, buffer.Length);
                fstream?.Close();
            }


            DirectoryInfo directory = new DirectoryInfo("..\\Электронный_вид\\" + $"Заказ№{Zakaz.idzak}.html");
            string path = directory.FullName;



            DirectoryInfo directoryx = new DirectoryInfo("..\\Электронный_вид\\" + $"Заказ№{Zakaz.idzak}.html");
            string pathx = directoryx.FullName;

            DirectoryInfo directoryxl = new DirectoryInfo("..\\Электронный_вид\\" + $"Заказ№{Zakaz.idzak}.pdf");
            string pathxl = directoryxl.FullName;
            EO.Pdf.HtmlToPdf.ConvertUrl($"{pathx}", $"{pathxl}");

            Process.Start(path);
            Process.Start(pathxl);
            /////////////////////////////////////////////////////////////////////////////////////////////////
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Application wordApp = new Application();
            Document wordDoc = wordApp.Documents.Add();
            Range docRange = wordDoc.Range();

            Bitmap bitmap = new Bitmap(pictureBox3.Width-2, pictureBox3.Height-2);
            using (Graphics gr = Graphics.FromImage(bitmap))
            {
                gr.CopyFromScreen(pictureBox3.PointToScreen(System.Drawing.Point.Empty), System.Drawing.Point.Empty, pictureBox3.Size);
            }
            bitmap.Save("..\\Штрих_коды\\" + $"ШтрихКод{Zakaz.scode}.png", System.Drawing.Imaging.ImageFormat.Png);


            DirectoryInfo directoryx = new DirectoryInfo("..\\Штрих_коды\\" + $"ШтрихКод{Zakaz.scode}.png");
            string pathx = directoryx.FullName;


            string imageName = $"{pathx}";
            for(int i = 0; i < Zakaz.pos.Length; i++)
            {
                InlineShape pictureShape = docRange.InlineShapes.AddPicture(imageName);
            }
            


            DirectoryInfo directoryx2 = new DirectoryInfo("..\\Штрих_коды_печать\\" + $"ПЕЧАТЬ_ШтрихКод{Zakaz.scode}.docx");
            string pathx2 = directoryx2.FullName;


            string imageName2 = $"{pathx2}";



            wordDoc.SaveAs2(imageName2);
            wordApp.Quit();

            Process.Start(imageName2);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            StarPoSmene_oformlenie_form adm = new StarPoSmene_oformlenie_form();
            this.Hide();
            adm.Show();
        }

        private void StarPoSmene_oformlenie_zavershenie_form_FormClosing(object sender, FormClosingEventArgs e)
        {
            System.Windows.Forms.Application.Exit();
        }
    }
}
