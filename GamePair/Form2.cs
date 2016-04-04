using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GamePair
{
    public partial class Form2 : Form
    {

        public int CmbBox
        {
            get { return comboBox1.SelectedIndex; }
            set { comboBox1.SelectedIndex = value; }
        }


        public Form2()
        {
            ControlBox = false;
            InitializeComponent();
            comboBox1.SelectedIndex = 0;
            

        }

        private void button1_Click(object sender, EventArgs e)
        {
                this.Close();
        }
    }
}
