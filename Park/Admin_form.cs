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
    public partial class Admin_form : Form
    {
        public Admin_form()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            History_admin_form adm = new History_admin_form();
            this.Hide();
            adm.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Otchet_admin_form adm = new Otchet_admin_form();
            this.Hide();
            adm.Show();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Sklad_admin_form adm = new Sklad_admin_form();
            this.Hide();
            adm.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Vhod_form adm = new Vhod_form();
            this.Hide();
            adm.Show();
        }

        private void Admin_form_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

        private void Admin_form_Load(object sender, EventArgs e)
        {
            pictureBox2.Image = Image.FromFile("..\\Фото_сотрудников\\" + Sotrudnik.foto);
            label1.Text = "Пользователь: " + Sotrudnik.fam + " " + Sotrudnik.name;
            label2.Text = "Роль: " + Sotrudnik.role;
        }
    }
}
