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
    public partial class variantsForm : Form
    {
        public variantsForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (checkBox1.Checked && !checkBox2.Checked)
            {
                meta.result = 1;
                this.Close();
            }
            else if (!checkBox1.Checked && checkBox2.Checked)
            {
                meta.result = 2;
                this.Close();
            }
            else if (checkBox1.Checked && checkBox2.Checked)
            {
                meta.result = 3;
                this.Close();
            }
            else
            {
                meta.result = 0;
                this.Close();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            meta.result = 0;
            this.Close();
        }
    }
}
