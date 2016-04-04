using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;

namespace GamePair
{
    public partial class Form3 : Form
    {
        public int CmbBox2
        {
            get { return comboBox2.SelectedIndex; }
            set { comboBox2.SelectedIndex = value; }
        }

        public string getTextLabel3
        {
            get { return label3.Text; }
        }

        public Form3()
        {
            ControlBox = false;
            InitializeComponent();
            comboBox2.SelectedIndex = 0;
        }

        public void ChooseFolder()
        {
            FolderBrowserDialog folderBrowserDialog1 = new FolderBrowserDialog();
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK) 
            {
                 label3.Text = folderBrowserDialog1.SelectedPath;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ChooseFolder();
            string[] dirs = Directory.GetFiles(label3.Text + "\\", "*.jpg");
            if (dirs.Length < 18)
            {
                MessageBox.Show("Количества картинок в формате *.jpg недостаточно. Выберите другую папку или стандартный набор картинок");
            }
            else
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            label3.Text = "";
            this.Close();
        }
    }
}
