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
    public partial class Form1 : Form
    {
        List<PictureBox> backCard = new List<PictureBox>(1000);
        List<PictureBox> images = new List<PictureBox>();
        long score = 0, maxScore;
        DirectoryInfo di1;
        string[] dirs;
        PictureBox png;
        Random random = new Random();
        // ссылки на картинки
        PictureBox firstClicked = null;
        PictureBox secondClicked = null;
        public Form1()
        {
            InitializeComponent();
            ControlBox = false;
        }

        private void AssignIconsToSquares()
        {
            var random = new Random(DateTime.Now.Millisecond);
            images = images.OrderBy(x => random.Next()).ToList();
        }

        private void refreshFields(int column, int row)
        {
            tableLayoutPanel2.Hide();
            tableLayoutPanel2.Controls.Clear();
            foreach (Control control in tableLayoutPanel2.Controls)
                control.Dispose();
            tableLayoutPanel2.ColumnCount = column;
            tableLayoutPanel2.RowCount = row;
            for (int i = 0; i < column * row; i++)
                tableLayoutPanel2.Controls.Add(backCard[i]);
            tableLayoutPanel2.Visible = true;
            for (int i = 0; i < column * row; i++)
                backCard[i].ImageLocation = null;
            images.Clear();
            for (int i = 0; i < tableLayoutPanel2.RowCount * tableLayoutPanel2.ColumnCount / 2; i++)
            {
                png = new PictureBox();
                png.Dock = System.Windows.Forms.DockStyle.Fill;
                png.ImageLocation = dirs[i];
                png.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
                images.Add(png);
                images.Add(png);
            }
            score = 0;
            label2.Text = score.ToString();
            firstClicked = null;
            secondClicked = null;
            AssignIconsToSquares();
        }

        private void выбратьРазмерToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // MessageBox.Show("Уважаемый игрок. Прошу набраться терпения! Раздел находится в разработке");
            Form2 setting = new Form2();
            setting.ShowDialog();
            int number = setting.CmbBox;
            switch (number)
            {
                // выбор поля 6x6
                case 0: refreshFields(6, 6); AssignIconsToSquares(); break;
                // Выбор поля 4x4
                case 1: refreshFields(4, 4); AssignIconsToSquares(); break;
                // Выбор поля 2x2
                case 2: refreshFields(2, 2); AssignIconsToSquares(); break;
            }
        }

        private void выходИзИгрыToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void addScore(int n)
        {
            score += n;
            label2.Text = score.ToString();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            tableLayoutPanel1.Hide();
            tableLayoutPanel2.ColumnCount = 4;
            tableLayoutPanel2.RowCount = 4;
            // Создаем массив рубашек
            for (int i = 0; i < 36; i++)
            {
                png = new PictureBox();
                png.Dock = System.Windows.Forms.DockStyle.Fill;
                di1 = new DirectoryInfo(@"..\..");
                png.Name = "" + i;
                png.BackgroundImage = Image.FromFile(di1.FullName + "\\card.png");
                png.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
                png.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
                backCard.Add(png);
            }
            // Загружаем картинки
            dirs = Directory.GetFiles(di1.FullName + "\\images\\", "*.jpg");
            refreshFields(4, 4);
            tableLayoutPanel1.Visible = true;
            // Прикрепляем к каждой картинке метод обаботки нажатия на нее
            for (int i = 0; i < 36; i++)
            {
                backCard[i].Click += new System.EventHandler(this.pictureClick);
            }
        }

        private void pictureClick(object sender, EventArgs e)
        {
            if (timer1.Enabled == true)
                return;
            PictureBox clickedPicture = sender as PictureBox;
            if (clickedPicture != null)
            {
                if (clickedPicture.ImageLocation == images[tableLayoutPanel2.Controls.IndexOfKey(clickedPicture.Name)].ImageLocation)
                    return;
                if (firstClicked == null)
                {
                    firstClicked = clickedPicture;
                    firstClicked.ImageLocation = images[tableLayoutPanel2.Controls.IndexOfKey(clickedPicture.Name)].ImageLocation;
                    return;
                }
                secondClicked = clickedPicture;
                secondClicked.ImageLocation = images[tableLayoutPanel2.Controls.IndexOfKey(clickedPicture.Name)].ImageLocation;
                if (CheckForWinner()) return;
                if (firstClicked.ImageLocation == secondClicked.ImageLocation)
                {
                    addScore(10);
                    firstClicked = null;
                    secondClicked = null;
                    return;
                }
                timer1.Start();
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Stop();
            addScore(-5);
            firstClicked.ImageLocation = null;
            secondClicked.ImageLocation = null;
            firstClicked = null;
            secondClicked = null;
        }

        private bool CheckForWinner()
        {
            foreach (Control control in tableLayoutPanel2.Controls)
            {
                PictureBox iconPicture = control as PictureBox;
                if (iconPicture != null)
                    if (iconPicture.ImageLocation == null) return false;
            }
            addScore(10);
            if (score > maxScore) maxScore = score;
            MessageBox.Show("Вы выиграли! Со счётом " + score + ". Ваш рекорд: " + maxScore);
            refreshFields(tableLayoutPanel2.RowCount, tableLayoutPanel2.ColumnCount);
            firstClicked = null;
            secondClicked = null;
            return true;
        }

        private void выбратьНаборКартинокToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form3 setPictures = new Form3();
            setPictures.ShowDialog();
            if (setPictures.getTextLabel3 != "")
            {
                dirs = Directory.GetFiles(setPictures.getTextLabel3 + "\\", "*.jpg");
                refreshFields(tableLayoutPanel2.RowCount, tableLayoutPanel2.ColumnCount);
                return;
            }
            int number = setPictures.CmbBox2;
            // обработаем стандартные наборы
            switch (number)
            {
                // выбор поля 6x6
                case 0: dirs = Directory.GetFiles(di1.FullName + "\\images\\", "*.jpg");
                    refreshFields(tableLayoutPanel2.RowCount, tableLayoutPanel2.ColumnCount);
                    break;
                // Выбор поля 4x4
                case 1: dirs = Directory.GetFiles(di1.FullName + "\\images2\\", "*.jpg");
                    refreshFields(tableLayoutPanel2.RowCount, tableLayoutPanel2.ColumnCount);
                    break;
                // Выбор поля 2x2
                case 2: dirs = Directory.GetFiles(di1.FullName + "\\images3\\", "*.jpg");
                    refreshFields(tableLayoutPanel2.RowCount, tableLayoutPanel2.ColumnCount);
                    break;
            }
        }
    }
}