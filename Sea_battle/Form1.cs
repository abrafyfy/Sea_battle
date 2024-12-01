using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sea_battle
{
    public partial class Form1 : Form
    {
        Button buttonPvB = new Button(); Button buttonPvP = new Button(); Button buttonExit = new Button(); Label label1 = new Label();

        public Form1() //Первая форма, предназначенная для выбора режима
        {
            InitializeComponent(); this.Text = "Морской бой";

            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None; this.WindowState =
            System.Windows.Forms.FormWindowState.Maximized; int x = Screen.PrimaryScreen.WorkingArea.Width; int y = Screen.PrimaryScreen.WorkingArea.Height; this.Width = x;
            this.Height = y; this.Focus();
            label1.Text = "Выберите режим игры"; label1.Width = x;
            label1.Height = 150; label1.Dock = DockStyle.Top;
            label1.TextAlign = ContentAlignment.MiddleCenter; Controls.Add(label1);
            this.label1.BackColor = System.Drawing.Color.Transparent;

            buttonPvB.Size = new Size(400, 100);
            buttonPvB.Text = "Играть с компьютером";
            buttonPvB.Location = new Point(this.Width / 2 - buttonPvB.Width / 2 , 200);

            buttonPvB.Click += new EventHandler(PvB_Start); Controls.Add(buttonPvB);

            buttonPvP.Size = new Size(400, 100); buttonPvP.Text = "Играть с игроком";
            buttonPvP.Location = new Point(this.Width / 2 - buttonPvP.Width / 2 , 400); buttonPvP.Click += new EventHandler(PvP_Start); Controls.Add(buttonPvP);

            buttonExit.Size = new Size(400, 100); buttonExit.Text = "Выход";
            buttonExit.Location = new Point(this.Width / 2 - buttonExit.Width / 2, 600);

            buttonExit.Click += new EventHandler(Exit); Controls.Add(buttonExit);


            buttonExit.Font = buttonPvB.Font = buttonPvP.Font = label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular,
            System.Drawing.GraphicsUnit.Point, ((byte)(204)));
        }

        public Form3 Form3
        {
            get => default;
            set
            {
            }
        }

        public Form2 Form2
        {
            get => default;
            set
            {
            }
        }

        public void PvB_Start(object sender, EventArgs e) // Открытие формы с игрой против компьютера
        {
            Form2 f2 = new Form2(); 
            f2.FormClosed += formClosed;
            this.Hide();
            f2.Show();
        }

        public void PvP_Start(object sender, EventArgs e) // Открытие формы с игрой против человека
        {
            Form3 f3 = new Form3();
            f3.FormClosed += formClosed;
            this.Hide();
            f3.Show();
        }

        void formClosed(object sender, FormClosedEventArgs e) // Закрытие формы
        {
            this.Show();
        }

        void Exit(object sender, EventArgs e) // Выход из приложения
        {
            this.Close();
        }
    }
}
