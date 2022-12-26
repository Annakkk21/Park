using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Park
{
    public partial class StarPoSmene_form : Form
    {
        public StarPoSmene_form()
        {
            InitializeComponent();
        }

        private void StarPoSmene_form_Load(object sender, EventArgs e)
        {
            pictureBox2.Image = Image.FromFile("..\\Фото_сотрудников\\" + Sotrudnik.foto);
            label1.Text = "Пользователь: " + Sotrudnik.fam + " " + Sotrudnik.name;
            label2.Text = "Роль: " + Sotrudnik.role;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Vhod_form adm = new Vhod_form();
            this.Hide();
            adm.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            StarPoSmene_oformlenie_form adm = new StarPoSmene_oformlenie_form();
            this.Hide();
            adm.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Zakrit_zakaz adm = new Zakrit_zakaz();
            this.Hide();
            adm.Show(); 
        }

        private void StarPoSmene_form_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }
    }
}
