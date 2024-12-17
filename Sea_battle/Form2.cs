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
    public partial class Form2 : Form
    {
        Button buttonStart = new Button();
        Button buttonNewGame = new Button();
        Label leftMap = new Label();
        Label rightMap = new Label();
        Button buttonSaveGame = new Button();
        Button buttonLoadGame = new Button();
        Button buttonExit = new Button();
        SaveFileDialog saveFileDialog = new SaveFileDialog();
        OpenFileDialog openFileDialog = new OpenFileDialog();
        RichTextBox richTextBox = new RichTextBox();

        int cellSize;
        static int cell = 11;
        string abc = "АБВГДЕЖЗИК";

        int[,] myMap = new int[cell, cell];
        int[,] botMap = new int[cell, cell];
        bool[,] checkMap = new bool[cell + 1, cell + 1];

        Button[,] myButton = new Button[cell, cell];
        Button[,] botButton = new Button[cell, cell];

        bool Player_On;
        bool Bot_On;
        bool Player_Shot;
        bool Bot_Shot;
        bool Game;

        int x_BotShot;
        int y_BotShot;

        StringBuilder s = new StringBuilder(206);
        string mapinfo;

        public Form2() // Игра против компьютера
        {
            InitializeComponent();
            this.Text = "Морской бой с компьютером";

            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            int x = Screen.PrimaryScreen.WorkingArea.Width;
            int y = Screen.PrimaryScreen.WorkingArea.Height;
            this.Width = x;
            this.Height = y;

            this.Focus();

            cellSize = Width / 33;
            Init();
        }

        public Form1 Form1
        {
            get => default;
            set
            {
            }
        }

        public void Init()
        {
            Player_On = Bot_On = false;
            Player_Shot = false;
            Bot_Shot = true;

            Game = true;
            richTextBox.Text += "Игра с компьютером\n";
            CreateMap();
        }

        public void CreateMap() // Создание полей
        {
            // Создание поля игрока 
            for (int i = 0; i < cell; i++)
            {
                for (int j = 0; j < cell; j++)
                {
                    myMap[i, j] = 0;

                    Button button = new Button();
                    button.Location = new Point(j * cellSize, i * cellSize + cellSize);
                    button.Size = new Size(cellSize, cellSize);
                    button.BackColor = Color.White;
                    if (j == 0 || i == 0)
                    {
                        button.BackColor = this.BackColor;
                        button.FlatAppearance.BorderSize = 0;
                        button.FlatStyle = FlatStyle.Flat;
                        if (j == 0 && i > 0)
                        {
                            button.Text = Convert.ToString(i);
                        }
                        if (i == 0 && j > 0)
                        {
                            button.Text = Convert.ToString(abc[j - 1]);
                        }
                    }
                    else
                    {
                        button.Click += new EventHandler(CreateShip);
                    }
                    myButton[i, j] = button;
                    this.Controls.Add(button);
                    this.myButton[i, j].BackColor = System.Drawing.Color.Transparent;
                    this.myButton[i, j].FlatStyle = System.Windows.Forms.FlatStyle.Flat;
                    this.myButton[i, j].FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
                    this.myButton[i, j].FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
                }
            }

            // Создание правого поля 
            for (int i = 0; i < cell; i++)
            {
                for (int j = 0; j < cell; j++)
                {
                    botMap[i, j] = 0;

                    Button button = new Button();
                    button.Location = new Point(j * cellSize + cellSize * cell * 2 - cellSize, i * cellSize + cellSize);
                    button.Size = new Size(cellSize, cellSize);
                    button.BackColor = Color.White;
                    if (j == 0 || i == 0)
                    {
                        button.BackColor = this.BackColor;
                        button.FlatAppearance.BorderSize = 0;
                        button.FlatStyle = FlatStyle.Flat;
                        if (j == 0 && i > 0)
                        {
                            button.Text = Convert.ToString(i);
                            button.FlatAppearance.BorderSize = 0;
                            button.FlatStyle = FlatStyle.Flat;
                        }

                        if (i == 0 && j > 0)
                        {
                            button.Text = Convert.ToString(abc[j - 1]);
                            button.FlatAppearance.BorderSize = 0;
                            button.FlatStyle = FlatStyle.Flat;
                        }
                    }
                    else
                    {
                        button.Click += new EventHandler(Shot);
                    }
                    botButton[i, j] = button;
                    this.Controls.Add(button);
                    this.botButton[i, j].BackColor = System.Drawing.Color.Transparent;
                    this.botButton[i, j].FlatStyle = System.Windows.Forms.FlatStyle.Flat;
                    this.botButton[i, j].FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
                    this.botButton[i, j].FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
                }
            }
            leftMap.Text = "Игрок";
            leftMap.Location = new Point(cell * cellSize / 2, cellSize / 3);
            this.Controls.Add(leftMap);
            this.leftMap.BackColor = System.Drawing.Color.Transparent;

            rightMap.Text = "Компьютер";
            rightMap.Location = new Point(cell * cellSize / 2 + cellSize * 21, cellSize / 3);
            this.Controls.Add(rightMap);
            this.rightMap.BackColor = System.Drawing.Color.Transparent;

            leftMap.Font = rightMap.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));

            buttonStart.Location = new Point(cell * cellSize + 3 * cellSize, 2 * cellSize);
            buttonStart.Size = new Size(cellSize * 4, cellSize); buttonStart.Text = "Начать";
            buttonStart.Click += new EventHandler(Start); this.Controls.Add(buttonStart);

            buttonNewGame.Location = new Point(cell * cellSize + 3 * cellSize, 3 * cellSize);
            buttonNewGame.Size = new Size(cellSize * 4, cellSize);
            buttonNewGame.Text = "Начать заново";
            buttonNewGame.Click += new EventHandler(NewGame);
            this.Controls.Add(buttonNewGame);

            buttonSaveGame.Location = new Point(cell * cellSize + 3 * cellSize, 4 * cellSize);
            buttonSaveGame.Size = new Size(cellSize * 4, cellSize);
            buttonSaveGame.Text = "Сохранить игру";
            buttonSaveGame.Click += new EventHandler(SaveGame);
            this.Controls.Add(buttonSaveGame);

            buttonLoadGame.Location = new Point(cell * cellSize + 3 * cellSize, 5 * cellSize);
            buttonLoadGame.Size = new Size(cellSize * 4, cellSize);
            buttonLoadGame.Text = "Загрузить игру";
            buttonLoadGame.Click += new EventHandler(LoadGame);
            this.Controls.Add(buttonLoadGame);

            buttonExit.Location = new Point(cell * cellSize + 3 * cellSize, 6 * cellSize);
            buttonExit.Size = new Size(cellSize * 4, cellSize);
            buttonExit.Text = "Выход";
            buttonExit.Click += new EventHandler(Exit);
            this.Controls.Add(buttonExit);

            richTextBox.Location = new Point(cell * cellSize + 16 * cellSize, 13 * cellSize);
            richTextBox.Size = new Size(cellSize * 5, 5 * cellSize);
            richTextBox.ScrollBars = RichTextBoxScrollBars.Vertical;
            richTextBox.ReadOnly = true;
            this.Controls.Add(richTextBox);

            buttonStart.Font = buttonNewGame.Font = buttonSaveGame.Font = buttonLoadGame.Font = buttonExit.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
        }

        public void Exit(object sender, EventArgs e) // Выход в главное меню
        {
            DialogResult result = MessageBox.Show("Вы уверены, что хотите выйти из игры? Все несохраненные результаты пропадут", "Выход из игры", MessageBoxButtons.YesNo);
            if (result == DialogResult.Yes)
            {
                this.Close();
            }
        }


        public void Start(object sender, EventArgs e) // Нажатие на кнопку "Начать"
        {
            for (int i = 1; i < cell; i++)
            {
                for (int j = 1; j < cell; j++)
                {
                    if (myMap[i, j] == 1)
                    {
                        checkMap[i, j] = true;
                    }
                    else
                    {
                        checkMap[i, j] = false;
                    }
                }

            }
            CheckShip(); // Проверка расстановки караблей игрока

            if (Player_On) // Расстановка кораблей компьютера
            {
                do
                {
                    Bot_Generation();
                    for (int i = 1; i < cell; i++)
                    {
                        for (int j = 1; j < cell; j++)
                        {
                            if (botMap[i, j] == 1)
                            {
                                checkMap[i, j] = true;
                            }
                            else
                            {
                                checkMap[i, j] = false;
                            }
                        }
                    }
                    CheckShip();
                }
                while (Bot_On == false);
            }
            if (Player_On && Bot_On)
            {
                buttonStart.Enabled = false;

            }
        }

        public void CreateShip(object sender, EventArgs e) // Создание кораблей игрока
        {
            Button pressedButton = sender as Button;
            if (!Player_On)
            {
                int x = pressedButton.Location.X / cellSize;
                int y = pressedButton.Location.Y / cellSize - 1;

                // Временное обновление карты для проверки
                int previousState = myMap[y, x]; // Сохраняем текущее состояние клетки
                myMap[y, x] = (previousState == 0) ? 1 : 0; // Инвертируем состояние клетки

                // Проверяем корректность расстановки
                if (!IsValidPlacement())
                {
                    // Если размещение некорректно, возвращаем клетке исходное состояние
                    myMap[y, x] = previousState;
                    MessageBox.Show("Превышено максимальное количество кораблей или некорректное размещение!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Применяем изменения только если проверка пройдена
                switch (previousState)
                {
                    case 0: // Если клетка была пустой
                        pressedButton.BackColor = Color.Blue;
                        myMap[y, x] = 1; // Устанавливаем корабль
                        pressedButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
                        pressedButton.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Blue;
                        pressedButton.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Blue;
                        break;

                    case 1: // Если клетка была занята
                        pressedButton.BackColor = System.Drawing.Color.Transparent;
                        myMap[y, x] = 0; // Убираем корабль
                        pressedButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
                        pressedButton.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
                        pressedButton.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
                        break;
                }
            }

            // Сохранение существующего механизма блокировки соседних клеток
            for (int i = 1; i < 11; i++) // Блокировка полей наискосок от занятых полей
            {
                for (int j = 1; j < 11; j++)
                {
                    if (myMap[i, j] == 1)
                    {
                        if (j != 10 && i != 10)
                        {
                            myMap[i + 1, j + 1] = 2;
                            myMap[i + 1, j - 1] = 2;
                            myMap[i - 1, j + 1] = 2;
                            myMap[i - 1, j - 1] = 2;
                        }
                        if (i == 10 && j != 10)
                        {
                            myMap[i - 1, j + 1] = 2;
                            myMap[i - 1, j - 1] = 2;
                        }
                        if (j == 10 && i != 10)
                        {
                            myMap[i + 1, j - 1] = 2;
                            myMap[i - 1, j - 1] = 2;
                        }
                        if (j == 10 && i == 10)
                        {
                            myMap[i - 1, j - 1] = 2;
                        }
                    }
                }
            }
        }

        // Метод для проверки корректности расстановки кораблей
        private bool IsValidPlacement()
        {
            int singleShips = 0, doubleShips = 0, tripleShips = 0, quadrupleShips = 0;
            bool[,] visited = new bool[cell, cell]; // Для отслеживания проверенных клеток

            for (int i = 1; i < cell; i++)
            {
                for (int j = 1; j < cell; j++)
                {
                    if (myMap[i, j] == 1 && !visited[i, j])
                    {
                        int size = GetShipSize(i, j, visited); // Определяем размер корабля

                        // Увеличиваем соответствующий счётчик
                        switch (size)
                        {
                            case 1: singleShips++; break;
                            case 2: doubleShips++; break;
                            case 3: tripleShips++; break;
                            case 4: quadrupleShips++; break;
                            default: return false; // Корабль больше 4 клеток — некорректно
                        }
                    }
                }
            }

            // Проверка ограничения на количество кораблей
            return singleShips <= 4 && doubleShips <= 3 && tripleShips <= 2 && quadrupleShips <= 1;
        }

        // Метод для определения размера корабля
        private int GetShipSize(int x, int y, bool[,] visited)
        {
            visited[x, y] = true;
            int size = 1; // Начинаем с одной клетки

            // Проверяем все соседние клетки (по горизонтали и вертикали)
            if (x > 1 && myMap[x - 1, y] == 1 && !visited[x - 1, y])
                size += GetShipSize(x - 1, y, visited);

            if (x < cell - 1 && myMap[x + 1, y] == 1 && !visited[x + 1, y])
                size += GetShipSize(x + 1, y, visited);

            if (y > 1 && myMap[x, y - 1] == 1 && !visited[x, y - 1])
                size += GetShipSize(x, y - 1, visited);

            if (y < cell - 1 && myMap[x, y + 1] == 1 && !visited[x, y + 1])
                size += GetShipSize(x, y + 1, visited);

            return size;
        }





        public void CheckShip() // Проверка расстановки кораблей
        {
            int Ship1, Ship2, Ship3, Ship4, ShipLong;
            Ship1 = Ship2 = Ship3 = Ship4 = ShipLong = 0; // Счетчики кораблей
            for (int i = 1; i < cell; i++)
            {
                for (int j = 1; j < cell; j++)
                {
                    if ((checkMap[i - 1, j] == false && checkMap[i, j] == true && checkMap[i + 1, j] == true && checkMap[i + 2, j] == true && checkMap[i + 3, j] == true && checkMap[i + 4, j] == false) || (checkMap[i, j - 1] == false && checkMap[i, j] == true && checkMap[i, j + 1] == true && checkMap[i, j + 2] == true && checkMap[i, j + 3] == true && checkMap[i, j + 4] == false))
                    {
                        Ship4++;
                    }

                    if ((checkMap[i - 1, j] == false && checkMap[i, j] == true && checkMap[i + 1, j] == true && checkMap[i + 2, j] == true && checkMap[i + 3, j] == false) || (checkMap[i, j - 1] == false && checkMap[i, j] == true && checkMap[i, j + 1] == true && checkMap[i, j + 2] == true && checkMap[i, j + 3] == false))
                    {
                        Ship3++;
                    }

                    if ((checkMap[i - 1, j] == false && checkMap[i, j] == true && checkMap[i + 1, j] == true && checkMap[i + 2, j] == false) || (checkMap[i, j - 1] == false && checkMap[i, j] == true && checkMap[i, j + 1] == true && checkMap[i, j + 2] == false))
                    {
                        Ship2++;
                    }

                    if (checkMap[i - 1, j] == false && checkMap[i, j] == true && checkMap[i + 1, j] == false && checkMap[i, j - 1] == false && checkMap[i, j + 1] == false)
                    {
                        Ship1++;
                    }

                    if ((checkMap[i - 1, j] == false && checkMap[i, j] == true && checkMap[i + 1, j] == true && checkMap[i + 2, j] == true && checkMap[i + 3, j] == true && checkMap[i + 4, j] == true) || (checkMap[i, j - 1] == false && checkMap[i, j] == true && checkMap[i, j + 1] == true && checkMap[i, j + 2] == true && checkMap[i, j + 3] == true && checkMap[i, j + 4] == true))
                    {
                        ShipLong++;
                    }

                }

            }

            if (!Bot_On && Player_On && Ship1 == 4 && Ship2 == 3 && Ship3 == 2 && Ship4 == 1 && ShipLong == 0)
            {
                Bot_On = true;
                MessageBox.Show("Игра началась", "Игра началась");
                richTextBox.Text += "Компьютер расставил корабли\n";
                richTextBox.Text += "Игра началась\n";
            }

            if (!Player_On)
            {
                if (Ship1 != 4 || Ship2 != 3 || Ship3 != 2 || Ship4 != 1 || ShipLong != 0)
                {
                    MessageBox.Show("Неверно расставлены корабли", "Ошибка");
                    Player_On = false;
                }
                else
                {
                    Player_On = true;
                    richTextBox.Text += "Игрок расставил корабли\n";
                }
            }


        }

        public void Bot_Generation() // Расстановка кораблей компьютера
        {
            int long_boat, count_boat;
            for (int i = 0; i < cell; i++)
            {
                for (int j = 0; j < cell; j++)
                {
                    botMap[i, j] = 0;
                }
            }
            try
            {
                for (long_boat = 4; long_boat > 0; long_boat--)
                {
                    switch (long_boat)
                    {

                        case 1:
                            count_boat = 4; Create_Boat(count_boat, long_boat); break;
                        case 2:
                            count_boat = 3; Create_Boat(count_boat, long_boat); break;
                        case 3:
                            count_boat = 2; Create_Boat(count_boat, long_boat); break;
                        case 4:
                            count_boat = 1; Create_Boat(count_boat, long_boat); break;
                    }
                }
            }
            catch
            {
                for (int i = 0; i < cell; i++)
                {
                    for (int j = 0; j < cell; j++)
                    {
                        botMap[i, j] = 0;
                    }
                }
            }
        }

        public void Create_Boat(int count, int long_boat) // Расстановка отдельного корабля
        {
            int x0, y0;
            string napr;
            for (int j = count; j > 0; j--)
            {
                Random rnd = new Random();

                //Направление установки
                string[] napravlenie = new string[4];
                napravlenie[0] = "left";
                napravlenie[1] = "top";
                napravlenie[2] = "right";
                napravlenie[3] = "bottom";

                try
                {
                    do
                    {
                        // Координита, с которой начнется установка корабля
                        x0 = rnd.Next(1, 11);
                        y0 = rnd.Next(1, 11);
                    }
                    while (botMap[y0, x0] != 0);

                    if (long_boat != 1)
                    {
                        napr = napravlenie[rnd.Next(0, napravlenie.Length)];
                        // Создание корабля 
                        switch (napr)
                        {
                            case "left":
                                while (x0 < long_boat) x0++;
                                for (int i = x0; i > x0 - long_boat; i--) botMap[y0, i] = 1; Block();
                                break;
                            case "top":
                                while (y0 < long_boat) y0++;
                                for (int i = y0; i > y0 - long_boat; i--) botMap[i, x0] = 1; Block();
                                break;
                            case "right":
                                while (x0 > cell - long_boat) x0--;
                                for (int i = x0; i < x0 + long_boat; i++) botMap[y0, i] = 1; Block();
                                break;
                            case "bottom":
                                while (y0 > cell - long_boat) y0--;
                                for (int i = y0; i < y0 + long_boat; i++) botMap[i, x0] = 1; Block();
                                break;

                        }
                    }
                    else
                    {
                        botMap[y0, x0] = 1;
                        Block();
                    }
                }
                catch (Exception error)
                {
                    MessageBox.Show($"{error.Message}{error.StackTrace}{error.InnerException}");
                }
            }
        }

        public void Block() // Блокировка полей вокруг корабля компьютера
        {
            for (int i = 1; i < cell; i++)
            {
                for (int j = 1; j < cell; j++)
                {
                    if (botMap[i, j] == 1)
                    {
                        if (j != 10 && i != 10)
                        {
                            botMap[i + 1, j + 1] = 2;
                            botMap[i + 1, j - 1] = 2;
                            botMap[i - 1, j + 1] = 2;
                            botMap[i - 1, j - 1] = 2;
                        }
                        if (i == 10 && j != 10)
                        {
                            botMap[i - 1, j + 1] = 2;
                            botMap[i - 1, j - 1] = 2;
                        }

                        if (j == 10 && i != 10)
                        {
                            botMap[i + 1, j - 1] = 2;
                            botMap[i - 1, j - 1] = 2;
                        }

                        if (j == 10 && i == 10)
                        {
                            botMap[i - 1, j - 1] = 2;
                        }
                    }
                }

            }

        }

        public void Shot(object sender, EventArgs e) // Выстрел игроока
        {
            if (Bot_On && Bot_Shot && Game)
            {

                Button pressedButton = sender as Button;

                int x = pressedButton.Location.X / cellSize - 21;
                int y = pressedButton.Location.Y / cellSize - 1;
                switch (botMap[y, x])
                {
                    case 1:
                        richTextBox.Text += $"Игрок: {y}{abc[x - 1]} попал";
                        pressedButton.BackColor = Color.Orange;
                        botMap[y, x] = 3;
                        CheckKill();
                        richTextBox.Text += "\n";
                        break;
                    case 0:
                        richTextBox.Text += $"Игрок: {y}{abc[x - 1]} мимо\n";
                        pressedButton.BackColor = Color.SkyBlue;
                        botMap[y, x] = 5;
                        Player_Shot = true;
                        Bot_Shot = false;
                        BotHit(sender, e);
                        break;
                    case 2:
                        richTextBox.Text += $"Игрок: {y}{abc[x - 1]} мимо\n";
                        pressedButton.BackColor = Color.SkyBlue;
                        botMap[y, x] = 5;
                        Player_Shot = true;
                        Bot_Shot = false;
                        BotHit(sender, e);
                        break;
                }

                pressedButton.Enabled = false;
                richTextBox.SelectionStart = richTextBox.Text.Length;
                richTextBox.ScrollToCaret();

            }
        }

        public void BotHit(object sender, EventArgs e) // Выстрел компьютера
        {

            Random rnd = new Random();
            while (Bot_On && !Bot_Shot && Game)
            {
                int wound = 0;
                int[] wound_y = new int[21];
                int[] wound_x = new int[21];

                // Нахождение раненых кораблей
                for (int i = 1; i < cell; i++)
                {
                    for (int j = 1; j < cell; j++)
                    {
                        if (myMap[i, j] == 3)
                        {
                            wound++;
                            wound_y[wound] = i;
                            wound_x[wound] = j;
                        }
                    }
                }
                switch (wound)
                {
                    case 0: // Нет раненых кораблей
                        x_BotShot = rnd.Next(1, 11);
                        y_BotShot = rnd.Next(1, 11);
                        break;
                    case 1: // 1 поле с раненым кораблем
                        string[] napravlenie = new string[4];
                        napravlenie[0] = "left";
                        napravlenie[1] = "top";
                        napravlenie[2] = "right";
                        napravlenie[3] = "bottom";

                    // Направление дальнейшей стрельбы (рядом с раненым)
                    a:
                        string napr = napravlenie[rnd.Next(0, napravlenie.Length - 1)];
                        switch (napr)
                        {
                            case "left":
                                if (wound_x[1] != 1 && myMap[wound_y[1], wound_x[1] - 1] != 5)
                                {
                                    x_BotShot = wound_x[1] - 1;
                                    y_BotShot = wound_y[1];
                                }
                                else
                                {
                                    napravlenie[0] = "right";
                                    goto a;
                                }
                                break;
                            case "top":
                                if (wound_y[1] != 1 && myMap[wound_y[1] - 1, wound_x[1]] != 5)
                                {
                                    x_BotShot = wound_x[1];
                                    y_BotShot = wound_y[1] - 1;
                                }
                                else
                                {
                                    napravlenie[1] = "bottom";
                                    goto a;
                                }
                                break;
                            case "right":
                                if (wound_x[1] != 10 && myMap[wound_y[1], wound_x[1] + 1] != 5)
                                {
                                    x_BotShot = wound_x[1] + 1;
                                    y_BotShot = wound_y[1];
                                }
                                else
                                {
                                    napravlenie[2] = "left";
                                    goto a;
                                }
                                break;
                            case "bottom":
                                if (wound_y[1] != 10 && myMap[wound_y[1] + 1, wound_x[1]] != 5)
                                {
                                    x_BotShot = wound_x[1];
                                    y_BotShot = wound_y[1] + 1;
                                }
                                else
                                {
                                    napravlenie[3] = "top";
                                    goto a;
                                }
                                break;
                        }
                        break;

                    // 2 поля с попаданиями
                    case 2:
                        // Расположены горизонтально
                        if (wound_y[1] == wound_y[2])
                        {
                            string[] napr2hor = new string[2];
                            napr2hor[0] = "left";
                            napr2hor[1] = "right";
                            string naprHor = napr2hor[rnd.Next(0, napr2hor.Length - 1)];
                            switch (naprHor)
                            {
                                case "left":
                                    if (wound_x[1] != 1 && myMap[wound_y[1], wound_x[1] - 1] != 5)
                                    {
                                        x_BotShot = wound_x[1] - 1;
                                        y_BotShot = wound_y[1];
                                    }
                                    else goto case "right";
                                    break;
                                case "right":
                                    if (wound_x[2] != 10 && myMap[wound_y[2], wound_x[2] + 1] != 5)
                                    {
                                        x_BotShot = wound_x[2] + 1;
                                        y_BotShot = wound_y[2];
                                    }
                                    else goto case "left";
                                    break;
                            }
                        }

                        // Расположены вертикально
                        if (wound_x[1] == wound_x[2])
                        {
                            string[] napr2vert = new string[2];
                            napr2vert[0] = "top";
                            napr2vert[1] = "bottom";
                            string naprVert = napr2vert[rnd.Next(0, napr2vert.Length - 1)];
                            switch (naprVert)
                            {
                                case "top":
                                    if (wound_y[1] != 1 && myMap[wound_y[1] - 1,
                                   wound_x[1]] != 5)
                                    {
                                        x_BotShot = wound_x[1];
                                        y_BotShot = wound_y[1] - 1;
                                    }
                                    else goto case "bottom";
                                    break;
                                case "bottom":
                                    if (wound_y[2] != 10 && myMap[wound_y[2] + 1,
                                   wound_x[2]] != 5)
                                    {
                                        x_BotShot = wound_x[2];
                                        y_BotShot = wound_y[2] + 1;
                                    }
                                    else goto case "top";
                                    break;
                            }
                        }
                        break;

                    // 3 поля с попаданиями
                    case 3:
                        // Расположены горизонтально
                        if (wound_y[1] == wound_y[2] && wound_y[2] == wound_y[3])
                        {
                            string[] napr3hor = new string[2];
                            napr3hor[0] = "left";
                            napr3hor[1] = "right";
                            string naprHor = napr3hor[rnd.Next(0, napr3hor.Length - 1)];
                            switch (naprHor)
                            {
                                case "left":
                                    if (wound_x[1] != 1 && myMap[wound_y[1], wound_x[1] - 1] != 5)
                                    {
                                        x_BotShot = wound_x[1] - 1;
                                        y_BotShot = wound_y[1];
                                    }
                                    else goto case "right";
                                    break;

                                case "right":
                                    if (wound_x[3] != 10 && myMap[wound_y[3], wound_x[3] + 1] != 5)
                                    {
                                        x_BotShot = wound_x[3] + 1;
                                        y_BotShot = wound_y[3];
                                    }
                                    else goto case "left";
                                    break;
                            }
                        }

                        // Расположены вертикально
                        if (wound_x[1] == wound_x[2] && wound_x[2] == wound_x[3])
                        {
                            string[] napr3vert = new string[2];
                            napr3vert[0] = "top";
                            napr3vert[1] = "bottom";
                            string naprVert = napr3vert[rnd.Next(0, napr3vert.Length - 1)];
                            switch (naprVert)
                            {
                                case "top":
                                    if (wound_y[1] != 1 && myMap[wound_y[1] - 1, wound_x[1]] != 5)
                                    {
                                        x_BotShot = wound_x[1];
                                        y_BotShot = wound_y[1] - 1;
                                    }
                                    else goto case "bottom";
                                    break;
                                case "bottom":
                                    if (wound_y[3] != 10 && myMap[wound_y[3] + 1, wound_x[3]] != 5)
                                    {
                                        x_BotShot = wound_x[3];
                                        y_BotShot = wound_y[3] + 1;
                                    }
                                    else goto case "top";
                                    break;
                            }
                        }
                        break;

                    // Нет полей с попаданиями
                    default:
                        x_BotShot = rnd.Next(1, 11);
                        y_BotShot = rnd.Next(1, 11);
                        break;
                }

                switch (myMap[y_BotShot, x_BotShot]) // Выстрел компьютера
                {
                    case 1:

                        richTextBox.Text += $"Компьютер: {y_BotShot}{abc[x_BotShot - 1]} попал";
                        myMap[y_BotShot, x_BotShot] = 3;
                        CheckKill_Bot();
                        richTextBox.Text += "\n";
                        break;
                    case 0:
                        richTextBox.Text += $"Компьютер: {y_BotShot}{abc[x_BotShot - 1]} мимо\n";
                        myButton[y_BotShot, x_BotShot].BackColor = Color.SkyBlue;
                        myButton[y_BotShot, x_BotShot].FlatStyle = System.Windows.Forms.FlatStyle.Flat;
                        myButton[y_BotShot, x_BotShot].FlatAppearance.MouseDownBackColor = Color.SkyBlue;
                        myButton[y_BotShot, x_BotShot].FlatAppearance.MouseOverBackColor = Color.SkyBlue;
                        myMap[y_BotShot, x_BotShot] = 5;
                        Player_Shot = false;
                        Bot_Shot = true;
                        break;
                    case 2:
                        richTextBox.Text += $"Компьютер: {y_BotShot}{abc[x_BotShot - 1]} мимо\n";
                        myButton[y_BotShot, x_BotShot].BackColor = Color.SkyBlue;
                        myButton[y_BotShot, x_BotShot].FlatStyle = System.Windows.Forms.FlatStyle.Flat;
                        myButton[y_BotShot, x_BotShot].FlatAppearance.MouseDownBackColor = Color.SkyBlue;
                        myButton[y_BotShot, x_BotShot].FlatAppearance.MouseOverBackColor = Color.SkyBlue;
                        myMap[y_BotShot, x_BotShot] = 5;
                        Player_Shot = false;
                        Bot_Shot = true;
                        break;
                }

                richTextBox.SelectionStart = richTextBox.Text.Length;
                richTextBox.ScrollToCaret();

            }
        }

        public void NewGame(object sender, EventArgs e) // Начать заново
        {
            DialogResult restart = MessageBox.Show("Вы уверены, что хотите начать заново ? ", "Начать заново", MessageBoxButtons.YesNo);
            if (restart == DialogResult.Yes)
            {
                Restart();
            }
        }

        public void Restart() // Сброс полей до исходного состояния
        {
            buttonStart.Enabled = true;
            Player_On = Bot_On = false;
            Player_Shot = false;
            Bot_Shot = true;
            Game = true;
            richTextBox.Text = "Игра с компьютером\n";
            for (int i = 0; i < cell; i++)
            {
                for (int j = 0; j < cell; j++)
                {
                    myMap[i, j] = 0;
                    botMap[i, j] = 0;
                    if (i != 0 && j != 0)
                    {
                        myButton[i, j].BackColor = System.Drawing.Color.Transparent;
                        myButton[i, j].FlatStyle = System.Windows.Forms.FlatStyle.Flat;
                        myButton[i, j].FlatAppearance.MouseDownBackColor = Color.Transparent;
                        myButton[i, j].FlatAppearance.MouseOverBackColor = Color.Transparent;
                        myButton[i, j].Text = "";
                        myButton[i, j].Enabled = true;

                        botButton[i, j].BackColor = System.Drawing.Color.Transparent;
                        botButton[i, j].FlatStyle = System.Windows.Forms.FlatStyle.Flat;
                        botButton[i, j].FlatAppearance.MouseDownBackColor = Color.Transparent;
                        botButton[i, j].FlatAppearance.MouseOverBackColor = Color.Transparent;
                        botButton[i, j].Text = "";
                        botButton[i, j].Enabled = true;
                    }
                }
            }
        }

        public void SaveGame(object sender, EventArgs e) // Сохранить игру
        {
            mapinfo = "";
            s.Append('B');
            for (int i = 1; i < cell; i++)
            {
                for (int j = 1; j < cell; j++)
                {
                    s.Append($"{myMap[i, j]}");
                }
            }
            for (int i = 1; i < cell; i++)
            {
                for (int j = 1; j < cell; j++)
                {
                    s.Append($"{botMap[i, j]}");
                }
            }
            if (Player_On == true) s.Append("1");
            else s.Append("0");
            if (Bot_On == true) s.Append("1");
            else s.Append("0");
            if (Game == true) s.Append("1");
            else s.Append("0");
            if (Bot_Shot == true) s.Append("1");
            else s.Append("0");
            if (buttonStart.Enabled == true) s.Append("1");
            else s.Append("0");
            mapinfo = s.ToString();
            mapinfo += "\n||\n";
            mapinfo += richTextBox.Text;
            saveFileDialog.Filter = "Text files(*.txt)|*.txt";
            if (saveFileDialog.ShowDialog() == DialogResult.Cancel)
                return;
            string filename = saveFileDialog.FileName;
            System.IO.File.WriteAllText(filename, mapinfo);
            MessageBox.Show("Игра сохранена");
        }

        public void LoadGame(object sender, EventArgs e) // Загрузить игру
        {

            mapinfo = "";
            int k = 0;
            openFileDialog.Filter = "Text files(*.txt)|*.txt";
            if (openFileDialog.ShowDialog() == DialogResult.Cancel)
                return;
            string filename = openFileDialog.FileName;
            mapinfo = System.IO.File.ReadAllText(filename);
            if (mapinfo[k] != 'B')
            {
                MessageBox.Show("Не удалось загрузить сохранение. Возможно, это сохранение из другого режима");
                return;
            }
            k++;
            Restart();
            for (int i = 1; i < cell; i++)
            {
                for (int j = 1; j < cell; j++)
                {
                    myMap[i, j] = (int)(mapinfo[k] - 48);
                    switch (myMap[i, j])
                    {
                        case 1:
                            myButton[i, j].BackColor = Color.Blue;
                            myButton[i, j].FlatStyle = System.Windows.Forms.FlatStyle.Flat;
                            myButton[i, j].FlatAppearance.MouseDownBackColor = Color.Blue;
                            myButton[i, j].FlatAppearance.MouseOverBackColor = Color.Blue;
                            myButton[i, j].Text = "";
                            break;
                        case 3:
                            myButton[i, j].BackColor = Color.Orange;
                            myButton[i, j].FlatStyle = System.Windows.Forms.FlatStyle.Flat;
                            myButton[i, j].FlatAppearance.MouseDownBackColor = Color.Orange;
                            myButton[i, j].FlatAppearance.MouseOverBackColor = Color.Orange;
                            myButton[i, j].Text = "";
                            break;
                        case 4:
                            myButton[i, j].BackColor = Color.Red;
                            myButton[i, j].FlatStyle = System.Windows.Forms.FlatStyle.Flat;
                            myButton[i, j].FlatAppearance.MouseDownBackColor = Color.Red;
                            myButton[i, j].FlatAppearance.MouseOverBackColor = Color.Red;
                            myButton[i, j].Text = "X";
                            break;
                        case 5:
                            myButton[i, j].BackColor = Color.SkyBlue;
                            myButton[i, j].FlatStyle = System.Windows.Forms.FlatStyle.Flat;
                            myButton[i, j].FlatAppearance.MouseDownBackColor = Color.SkyBlue;
                            myButton[i, j].FlatAppearance.MouseOverBackColor = Color.SkyBlue;
                            myButton[i, j].Text = "";
                            break;
                    }
                    k++;
                }
            }

            for (int i = 1; i < cell; i++)
            {
                for (int j = 1; j < cell; j++)
                {
                    botMap[i, j] = (int)(mapinfo[k] - 48);
                    switch (botMap[i, j])
                    {
                        case 3:
                            botButton[i, j].BackColor = Color.Orange;
                            botButton[i, j].FlatStyle = System.Windows.Forms.FlatStyle.Flat;
                            botButton[i, j].FlatAppearance.MouseDownBackColor = Color.Orange;
                            botButton[i, j].FlatAppearance.MouseOverBackColor = Color.Orange;
                            botButton[i, j].Enabled = false;
                            botButton[i, j].Text = "";
                            break;
                        case 4:
                            botButton[i, j].Text = "X";
                            botButton[i, j].BackColor = Color.Red;
                            botButton[i, j].FlatStyle = System.Windows.Forms.FlatStyle.Flat;
                            botButton[i, j].FlatAppearance.MouseDownBackColor = Color.Red;
                            botButton[i, j].FlatAppearance.MouseOverBackColor = Color.Red;
                            botButton[i, j].Enabled = false;
                            break;
                        case 5:
                            botButton[i, j].BackColor = Color.SkyBlue;
                            botButton[i, j].FlatStyle = System.Windows.Forms.FlatStyle.Flat;
                            botButton[i, j].FlatAppearance.MouseDownBackColor = Color.SkyBlue;
                            botButton[i, j].FlatAppearance.MouseOverBackColor = Color.SkyBlue;
                            botButton[i, j].Text = "";
                            botButton[i, j].Enabled = false;
                            break;
                    }
                    k++;
                }
            }
            if (mapinfo[k] == '1') Player_On = true;
            else Player_On = false;
            k++;
            if (mapinfo[k] == '1') Bot_On = true;
            else Bot_On = false;
            k++;
            if (mapinfo[k] == '1') Game = true;
            else Game = false;
            k++;
            if (mapinfo[k] == '1') Bot_Shot = true;
            else Bot_Shot = false;
            k++;
            if (mapinfo[k] == '1') buttonStart.Enabled = true;
            else buttonStart.Enabled = false;
            for (int i = 210; i < mapinfo.Length; i++)
            {
                richTextBox.Text += mapinfo[i];
            }
            MessageBox.Show("Игра загружена");
        }

        public void CheckKill() // Проверка на убийство корабля компьютера
        {
            int count4 = 0;
            for (int i = 1; i < cell; i++)
            {
                for (int j = 1; j < cell; j++)
                {

                    // Вертикальный 4
                    if ((botMap[i, j] == 3 && i < 8) && botMap[i + 1, j] == 3 && botMap[i + 2, j] == 3 && botMap[i + 3, j] == 3)
                    {
                        botButton[i, j].Text = botButton[i + 1, j].Text = botButton[i + 2, j].Text = botButton[i + 3, j].Text = "X";
                        botButton[i, j].BackColor = botButton[i + 1, j].BackColor = botButton[i + 2, j].BackColor = botButton[i + 3, j].BackColor = Color.Red;
                        botButton[i, j].FlatStyle = botButton[i + 1, j].FlatStyle = botButton[i + 2, j].FlatStyle = botButton[i + 3, j].FlatStyle = System.Windows.Forms.FlatStyle.Flat;
                        botButton[i, j].FlatAppearance.MouseDownBackColor = botButton[i + 1, j].FlatAppearance.MouseDownBackColor = botButton[i + 2, j].FlatAppearance.MouseDownBackColor = botButton[i + 3, j].FlatAppearance.MouseDownBackColor = Color.Red;
                        botButton[i, j].FlatAppearance.MouseOverBackColor = botButton[i + 1, j].FlatAppearance.MouseOverBackColor = botButton[i + 2, j].FlatAppearance.MouseOverBackColor = botButton[i + 3, j].FlatAppearance.MouseOverBackColor = Color.Red;

                        botMap[i, j] = botMap[i + 1, j] = botMap[i + 2, j] = botMap[i + 3, j] = 4;
                        richTextBox.Text += ", убил";
                        if (i == 7 || j == 10)
                        {
                            if (i == 7 && j != 10)
                            {
                                botMap[i - 1, j - 1] = botMap[i - 1, j] = botMap[i - 1, j + 1] = 5;
                                botMap[i, j - 1] = botMap[i, j + 1] = 5;
                                botMap[i + 1, j - 1] = botMap[i + 1, j + 1] = 5;
                                botMap[i + 2, j - 1] = botMap[i + 2, j + 1] = 5;
                                botMap[i + 3, j - 1] = botMap[i + 3, j + 1] = 5;
                            }
                            if (i == 7 && j == 10)
                            {
                                botMap[i - 1, j] = botMap[i - 1, j - 1] = 5;
                                botMap[i, j - 1] = botMap[i + 1, j - 1] = botMap[i + 2, j - 1] = botMap[i + 3, j - 1] = 5;
                            }
                            if (i < 7 && j == 10)
                            {
                                botMap[i - 1, j] = botMap[i - 1, j - 1] = 5;
                                botMap[i, j - 1] = botMap[i + 1, j - 1] = botMap[i + 2, j - 1] = botMap[i + 3, j - 1] = 5;
                                botMap[i + 4, j] = botMap[i + 4, j - 1] = 5;
                            }
                        }
                        else
                        {
                            botMap[i - 1, j - 1] = botMap[i - 1, j] = botMap[i - 1, j + 1] = 5;
                            botMap[i + 4, j - 1] = botMap[i + 4, j] = botMap[i + 4, j + 1] = 5;
                            botMap[i, j - 1] = botMap[i, j + 1] = 5;
                            botMap[i + 1, j - 1] = botMap[i + 1, j + 1] = 5;
                            botMap[i + 2, j - 1] = botMap[i + 2, j + 1] = 5;
                            botMap[i + 3, j - 1] = botMap[i + 3, j + 1] = 5;
                        }
                    }
                    // Горизонтальный 4
                    if ((botMap[i, j] == 3 && j < 8) && botMap[i, j + 1] == 3 && botMap[i, j + 2] == 3 && botMap[i, j + 3] == 3)
                    {
                        botButton[i, j].Text = botButton[i, j + 1].Text = botButton[i, j + 2].Text = botButton[i, j + 3].Text = "X";
                        botButton[i, j].BackColor = botButton[i, j + 1].BackColor = botButton[i, j + 2].BackColor = botButton[i, j + 3].BackColor = Color.Red;
                        botButton[i, j].FlatStyle = botButton[i, j + 1].FlatStyle = botButton[i, j + 2].FlatStyle = botButton[i, j + 3].FlatStyle = System.Windows.Forms.FlatStyle.Flat;
                        botButton[i, j].FlatAppearance.MouseDownBackColor = botButton[i, j + 1].FlatAppearance.MouseDownBackColor = botButton[i, j + 2].FlatAppearance.MouseDownBackColor = botButton[i, j + 3].FlatAppearance.MouseDownBackColor = Color.Red;
                        botButton[i, j].FlatAppearance.MouseOverBackColor = botButton[i, j + 1].FlatAppearance.MouseOverBackColor = botButton[i, j + 2].FlatAppearance.MouseOverBackColor = botButton[i, j + 3].FlatAppearance.MouseOverBackColor = Color.Red;
                        botMap[i, j] = botMap[i, j + 1] = botMap[i, j + 2] = botMap[i, j + 3] = 4;
                        richTextBox.Text += ", убил";
                        if (j == 7 || i == 10)
                        {
                            if (j == 7 && i != 10)
                            {
                                botMap[i - 1, j - 1] = botMap[i, j - 1] = botMap[i + 1, j - 1] = 5;
                                botMap[i - 1, j] = botMap[i + 1, j] = 5;
                                botMap[i - 1, j + 1] = botMap[i + 1, j + 1] = 5;
                                botMap[i - 1, j + 2] = botMap[i + 1, j + 2] = 5;
                                botMap[i - 1, j + 3] = botMap[i + 1, j + 3] = 5;
                            }
                            if (j == 7 && i == 10)
                            {
                                botMap[i - 1, j - 1] = botMap[i, j - 1] = 5;
                                botMap[i - 1, j] = botMap[i - 1, j + 1] = botMap[i - 1, j + 2] = botMap[i - 1, j + 3] = 5;
                            }
                            if (j < 7 && i == 10)
                            {
                                botMap[i - 1, j - 1] = botMap[i, j - 1] = 5;
                                botMap[i - 1, j + 4] = botMap[i, j + 4] = 5;
                                botMap[i - 1, j] = botMap[i - 1, j + 1] = botMap[i - 1, j + 2] = botMap[i - 1, j + 3] = 5;
                            }
                        }
                        else
                        {
                            botMap[i - 1, j] = botMap[i + 1, j] = 5;
                            botMap[i - 1, j + 1] = botMap[i + 1, j + 1] = 5;
                            botMap[i - 1, j + 2] = botMap[i + 1, j + 2] = 5;
                            botMap[i - 1, j + 3] = botMap[i + 1, j + 3] = 5;
                            botMap[i - 1, j - 1] = botMap[i, j - 1] = botMap[i + 1, j - 1] = 5;
                            botMap[i - 1, j + 4] = botMap[i, j + 4] = botMap[i + 1, j + 4] = 5;
                        }
                    }
                    // Вертикальный 3
                    if ((botMap[i - 1, j] != 1 && i < 9) && botMap[i - 1, j] != 3 && botMap[i, j] == 3 && botMap[i + 1, j] == 3 && botMap[i + 2, j] == 3 && (i == 8 || (botMap[i + 3, j] != 3 && botMap[i + 3, j] != 1)))
                    {
                        botButton[i, j].Text = botButton[i + 1, j].Text = botButton[i + 2, j].Text = "X";
                        botButton[i, j].BackColor = botButton[i + 1, j].BackColor = botButton[i + 2, j].BackColor = Color.Red;
                        botButton[i, j].FlatStyle = botButton[i + 1, j].FlatStyle = botButton[i + 2, j].FlatStyle = System.Windows.Forms.FlatStyle.Flat;
                        botButton[i, j].FlatAppearance.MouseDownBackColor = botButton[i + 1, j].FlatAppearance.MouseDownBackColor = botButton[i + 2, j].FlatAppearance.MouseDownBackColor = Color.Red;
                        botButton[i, j].FlatAppearance.MouseOverBackColor = botButton[i + 1, j].FlatAppearance.MouseOverBackColor = botButton[i + 2, j].FlatAppearance.MouseOverBackColor = Color.Red;
                        botMap[i, j] = botMap[i + 1, j] = botMap[i + 2, j] = 4;
                        richTextBox.Text += ", убил";
                        if (i == 8 || j == 10)
                        {
                            if (i == 8 && j != 10)
                            {
                                botMap[i - 1, j - 1] = botMap[i - 1, j] = botMap[i - 1, j + 1] = 5;
                                botMap[i, j - 1] = botMap[i, j + 1] = 5;
                                botMap[i + 1, j - 1] = botMap[i + 1, j + 1] = 5;
                                botMap[i + 2, j - 1] = botMap[i + 2, j + 1] = 5;
                            }
                            if (i == 8 && j == 10)
                            {
                                botMap[i - 1, j] = botMap[i - 1, j - 1] = 5;
                                botMap[i, j - 1] = botMap[i + 1, j - 1] = botMap[i + 2, j - 1] = 5;
                            }
                            if (i < 8 && j == 10)
                            {
                                botMap[i - 1, j] = botMap[i - 1, j - 1] = 5;
                                botMap[i, j - 1] = botMap[i + 1, j - 1] = botMap[i + 2, j - 1] = 5;
                                botMap[i + 3, j] = botMap[i + 3, j - 1] = 5;
                            }
                        }
                        else
                        {
                            botMap[i - 1, j - 1] = botMap[i - 1, j] = botMap[i - 1, j + 1] = 5;
                            botMap[i + 3, j - 1] = botMap[i + 3, j] = botMap[i + 3, j + 1] = 5;
                            botMap[i, j - 1] = botMap[i, j + 1] = 5;
                            botMap[i + 1, j - 1] = botMap[i + 1, j + 1] = 5;
                            botMap[i + 2, j - 1] = botMap[i + 2, j + 1] = 5;
                        }
                    }
                    // Горизонтальный 3
                    if ((botMap[i, j - 1] != 1 && j < 9) && botMap[i, j - 1] != 3 && botMap[i, j] == 3 && botMap[i, j + 1] == 3 && botMap[i, j + 2] == 3 && (j == 8 || (botMap[i, j + 3] != 3 && botMap[i, j + 3] != 1)))
                    {
                        botButton[i, j].Text = botButton[i, j + 1].Text = botButton[i, j + 2].Text = "X";
                        botButton[i, j].BackColor = botButton[i, j + 1].BackColor = botButton[i, j + 2].BackColor = Color.Red;
                        botButton[i, j].FlatStyle = botButton[i, j + 1].FlatStyle = botButton[i, j + 2].FlatStyle = System.Windows.Forms.FlatStyle.Flat;
                        botButton[i, j].FlatAppearance.MouseDownBackColor = botButton[i, j + 1].FlatAppearance.MouseDownBackColor = botButton[i, j + 2].FlatAppearance.MouseDownBackColor = Color.Red;
                        botButton[i, j].FlatAppearance.MouseOverBackColor = botButton[i, j + 1].FlatAppearance.MouseOverBackColor = botButton[i, j + 2].FlatAppearance.MouseOverBackColor = Color.Red;
                        botMap[i, j] = botMap[i, j + 1] = botMap[i, j + 2] = 4;
                        richTextBox.Text += ", убил";
                        if (j == 8 || i == 10)
                        {
                            if (j == 8 && i != 10)
                            {
                                botMap[i - 1, j - 1] = botMap[i, j - 1] = botMap[i + 1, j - 1] = 5;
                                botMap[i - 1, j] = botMap[i + 1, j] = 5;
                                botMap[i - 1, j + 1] = botMap[i + 1, j + 1] = 5;
                                botMap[i - 1, j + 2] = botMap[i + 1, j + 2] = 5;
                            }
                            if (j == 8 && i == 10)
                            {
                                botMap[i - 1, j - 1] = botMap[i, j - 1] = 5;
                                botMap[i - 1, j] = botMap[i - 1, j + 1] = botMap[i - 1, j + 2] = 5;
                            }
                            if (j < 8 && i == 10)
                            {
                                botMap[i - 1, j - 1] = botMap[i, j - 1] = 5;
                                botMap[i - 1, j + 3] = botMap[i, j + 3] = 5;
                                botMap[i - 1, j] = botMap[i - 1, j + 1] = botMap[i - 1, j + 2] = 5;
                            }
                        }
                        else
                        {
                            botMap[i - 1, j] = botMap[i + 1, j] = 5;
                            botMap[i - 1, j + 1] = botMap[i + 1, j + 1] = 5;
                            botMap[i - 1, j + 2] = botMap[i + 1, j + 2] = 5;
                            botMap[i - 1, j - 1] = botMap[i, j - 1] = botMap[i + 1, j - 1] = 5;
                            botMap[i - 1, j + 3] = botMap[i, j + 3] = botMap[i + 1, j + 3] = 5;
                        }
                    }
                    // Вертикальный 2
                    if ((botMap[i - 1, j] != 1 && i < 10) && botMap[i - 1, j] != 3 && botMap[i, j] == 3 && botMap[i + 1, j] == 3 && (i == 9 || (botMap[i + 2, j] != 3 && botMap[i + 2, j] != 1)))
                    {
                        botButton[i, j].Text = botButton[i + 1, j].Text = "X";
                        botButton[i, j].BackColor = botButton[i + 1, j].BackColor = Color.Red;
                        botButton[i, j].FlatStyle = botButton[i + 1, j].FlatStyle = System.Windows.Forms.FlatStyle.Flat;
                        botButton[i, j].FlatAppearance.MouseDownBackColor = botButton[i + 1, j].FlatAppearance.MouseDownBackColor = Color.Red;
                        botButton[i, j].FlatAppearance.MouseOverBackColor = botButton[i + 1, j].FlatAppearance.MouseOverBackColor = Color.Red;
                        botMap[i, j] = botMap[i + 1, j] = 4;
                        richTextBox.Text += ", убил";
                        if (i == 9 || j == 10)
                        {
                            if (i == 9 && j != 10)
                            {
                                botMap[i - 1, j - 1] = botMap[i - 1, j] = botMap[i - 1, j + 1] = 5;
                                botMap[i, j - 1] = botMap[i, j + 1] = 5;
                                botMap[i + 1, j - 1] = botMap[i + 1, j + 1] = 5;
                            }
                            if (i == 9 && j == 10)
                            {
                                botMap[i - 1, j] = botMap[i - 1, j - 1] = 5;
                                botMap[i, j - 1] = botMap[i + 1, j - 1] = 5;
                            }
                            if (i < 9 && j == 10)
                            {
                                botMap[i - 1, j] = botMap[i - 1, j - 1] = 5;
                                botMap[i, j - 1] = botMap[i + 1, j - 1] = 5;
                                botMap[i + 2, j] = botMap[i + 2, j - 1] = 5;
                            }
                        }
                        else
                        {
                            botMap[i - 1, j - 1] = botMap[i - 1, j] = botMap[i - 1, j + 1] = 5;
                            botMap[i + 2, j - 1] = botMap[i + 2, j] = botMap[i + 2, j + 1] = 5;
                            botMap[i, j - 1] = botMap[i, j + 1] = 5;
                            botMap[i + 1, j - 1] = botMap[i + 1, j + 1] = 5;
                        }
                    }
                    // Горизонтальный 2
                    if ((botMap[i, j - 1] != 1 && j < 10) && botMap[i, j - 1] != 3 && botMap[i, j] == 3 && botMap[i, j + 1] == 3 && (j == 9 || (botMap[i, j + 2] != 3 && botMap[i, j + 2] != 1)))
                    {
                        botButton[i, j].Text = botButton[i, j + 1].Text = "X";
                        botButton[i, j].BackColor = botButton[i, j + 1].BackColor = Color.Red;
                        botButton[i, j].FlatStyle = botButton[i, j + 1].FlatStyle = System.Windows.Forms.FlatStyle.Flat;
                        botButton[i, j].FlatAppearance.MouseDownBackColor = botButton[i, j + 1].FlatAppearance.MouseDownBackColor = Color.Red;
                        botButton[i, j].FlatAppearance.MouseOverBackColor = botButton[i, j + 1].FlatAppearance.MouseOverBackColor = Color.Red;
                        botMap[i, j] = botMap[i, j + 1] = 4;
                        richTextBox.Text += ", убил";
                        if (j == 9 || i == 10)
                        {
                            if (j == 9 && i != 10)
                            {
                                botMap[i - 1, j - 1] = botMap[i, j - 1] = botMap[i + 1, j - 1] = 5;
                                botMap[i - 1, j] = botMap[i + 1, j] = 5;
                                botMap[i - 1, j + 1] = botMap[i + 1, j + 1] = 5;
                            }
                            if (j == 9 && i == 10)
                            {
                                botMap[i - 1, j - 1] = botMap[i, j - 1] = 5;
                                botMap[i - 1, j] = botMap[i - 1, j + 1] = 5;
                            }
                            if (j < 9 && i == 10)
                            {
                                botMap[i - 1, j - 1] = botMap[i, j - 1] = 5;
                                botMap[i - 1, j + 2] = botMap[i, j + 2] = 5;
                                botMap[i - 1, j] = botMap[i - 1, j + 1] = 5;
                            }
                        }
                        else
                        {
                            botMap[i - 1, j] = botMap[i + 1, j] = 5;
                            botMap[i - 1, j + 1] = botMap[i + 1, j + 1] = 5;
                            botMap[i - 1, j - 1] = botMap[i, j - 1] = botMap[i + 1, j - 1] = 5;
                            botMap[i - 1, j + 2] = botMap[i, j + 2] = botMap[i + 1, j + 2] = 5;
                        }
                    }
                    // 1
                    if (botMap[i - 1, j] != 1 && botMap[i - 1, j] != 3 && botMap[i, j - 1] != 1 && botMap[i, j - 1] != 3 && botMap[i, j] == 3 && (j == 10 || (botMap[i, j + 1] != 3 && botMap[i, j + 1] != 1)) && (i == 10 || (botMap[i + 1, j] != 3 && botMap[i + 1, j] != 1)))
                    {
                        botButton[i, j].Text = "X";
                        botButton[i, j].BackColor = Color.Red;
                        botButton[i, j].FlatStyle = System.Windows.Forms.FlatStyle.Flat;
                        botButton[i, j].FlatAppearance.MouseDownBackColor = Color.Red;
                        botButton[i, j].FlatAppearance.MouseOverBackColor = Color.Red;
                        botMap[i, j] = 4;
                        richTextBox.Text += ", убил";
                        if (j == 10 || i == 10)
                        {
                            if (j == 10 && i != 10)
                            {
                                botMap[i - 1, j] = botMap[i - 1, j - 1] = 5;
                                botMap[i + 1, j] = botMap[i + 1, j - 1] = 5;
                                botMap[i, j - 1] = 5;
                            }
                            if (j == 10 && i == 10)
                            {
                                botMap[i - 1, j] = botMap[i - 1, j - 1] = botMap[i, j - 1] = 5;
                            }
                            if (j != 10 && i == 10)
                            {
                                botMap[i - 1, j - 1] = botMap[i - 1, j] = botMap[i - 1, j + 1] = 5;
                                botMap[i, j - 1] = botMap[i, j + 1] = 5;
                            }
                        }
                        else
                        {
                            botMap[i - 1, j - 1] = botMap[i - 1, j] = botMap[i - 1, j + 1] = 5;
                            botMap[i, j - 1] = botMap[i, j + 1] = 5;
                            botMap[i + 1, j - 1] = botMap[i + 1, j] = botMap[i + 1, j + 1] = 5;
                        }
                    }
                }
            }
            for (int i = 1; i < cell; i++)
            {
                for (int j = 1; j < cell; j++)
                {
                    switch (botMap[i, j])
                    {
                        case 4:
                            count4++;
                            break;
                        case 5:
                            botButton[i, j].Enabled = false;
                            botButton[i, j].BackColor = Color.SkyBlue;
                            break;
                    }
                }
            }
            if (count4 == 20)
            {
                Game = false;
                DialogResult result = MessageBox.Show("Вы победили! Начать заново? ", "Конец игры", MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                {
                    NewGame(this, new EventArgs());
                }
            }
        }

        public void CheckKill_Bot() // Проверка компьютера на убийство корабля игрока
        {
            for (int i = 1; i < cell; i++)
            {
                for (int j = 1; j < cell; j++)
                {
                    // Вертикальный 4
                    if ((myMap[i, j] == 3 && i < 8) && myMap[i + 1, j] == 3 && myMap[i + 2, j] == 3 && myMap[i + 3, j] == 3)
                    {
                        myButton[i, j].Text = myButton[i + 1, j].Text = myButton[i + 2, j].Text = myButton[i + 3, j].Text = "X";
                        myButton[i, j].BackColor = myButton[i + 1, j].BackColor = myButton[i + 2, j].BackColor = myButton[i + 3, j].BackColor = Color.Red;
                        myButton[i, j].FlatStyle = myButton[i + 1, j].FlatStyle = myButton[i + 2, j].FlatStyle = myButton[i + 3, j].FlatStyle = System.Windows.Forms.FlatStyle.Flat;
                        myButton[i, j].FlatAppearance.MouseDownBackColor = myButton[i + 1, j].FlatAppearance.MouseDownBackColor = myButton[i + 2, j].FlatAppearance.MouseDownBackColor = myButton[i + 3, j].FlatAppearance.MouseDownBackColor = Color.Red;
                        myButton[i, j].FlatAppearance.MouseOverBackColor = myButton[i + 1, j].FlatAppearance.MouseOverBackColor = myButton[i + 2, j].FlatAppearance.MouseOverBackColor = myButton[i + 3, j].FlatAppearance.MouseOverBackColor = Color.Red;
                        myMap[i, j] = myMap[i + 1, j] = myMap[i + 2, j] = myMap[i + 3, j] = 4;
                        richTextBox.Text += ", убил";
                        if (i == 7 || j == 10)
                        {
                            if (i == 7 && j != 10)
                            {
                                myMap[i - 1, j - 1] = myMap[i - 1, j] = myMap[i - 1, j + 1] = 5;
                                myMap[i, j - 1] = myMap[i, j + 1] = 5;
                                myMap[i + 1, j - 1] = myMap[i + 1, j + 1] = 5;
                                myMap[i + 2, j - 1] = myMap[i + 2, j + 1] = 5;
                                myMap[i + 3, j - 1] = myMap[i + 3, j + 1] = 5;
                            }
                            if (i == 7 && j == 10)
                            {
                                myMap[i - 1, j] = myMap[i - 1, j - 1] = 5;
                                myMap[i, j - 1] = myMap[i + 1, j - 1] = myMap[i + 2, j - 1] = myMap[i + 3, j - 1] = 5;
                            }
                            if (i < 7 && j == 10)
                            {
                                myMap[i - 1, j] = myMap[i - 1, j - 1] = 5;
                                myMap[i, j - 1] = myMap[i + 1, j - 1] = myMap[i + 2, j - 1] = myMap[i + 3, j - 1] = 5;
                                myMap[i + 4, j] = myMap[i + 4, j - 1] = 5;
                            }
                        }
                        else
                        {
                            myMap[i - 1, j - 1] = myMap[i - 1, j] = myMap[i - 1, j + 1] = 5;
                            myMap[i + 4, j - 1] = myMap[i + 4, j] = myMap[i + 4, j + 1] = 5;
                            myMap[i, j - 1] = myMap[i, j + 1] = 5;
                            myMap[i + 1, j - 1] = myMap[i + 1, j + 1] = 5;
                            myMap[i + 2, j - 1] = myMap[i + 2, j + 1] = 5;
                            myMap[i + 3, j - 1] = myMap[i + 3, j + 1] = 5;
                        }
                    }
                    // Горизонтальный 4
                    if ((myMap[i, j] == 3 && j < 8) && myMap[i, j + 1] == 3 && myMap[i, j + 2] == 3 && myMap[i, j + 3] == 3)
                    {
                        myButton[i, j].Text = myButton[i, j + 1].Text = myButton[i, j + 2].Text = myButton[i, j + 3].Text = "X";
                        myButton[i, j].BackColor = myButton[i, j + 1].BackColor = myButton[i, j + 2].BackColor = myButton[i, j + 3].BackColor = Color.Red;
                        myButton[i, j].FlatStyle = myButton[i, j + 1].FlatStyle = myButton[i, j + 2].FlatStyle = myButton[i, j + 3].FlatStyle = System.Windows.Forms.FlatStyle.Flat;
                        myButton[i, j].FlatAppearance.MouseDownBackColor = myButton[i, j + 1].FlatAppearance.MouseDownBackColor = myButton[i, j + 2].FlatAppearance.MouseDownBackColor = myButton[i, j + 3].FlatAppearance.MouseDownBackColor = Color.Red;
                        myButton[i, j].FlatAppearance.MouseOverBackColor = myButton[i, j + 1].FlatAppearance.MouseOverBackColor = myButton[i, j + 2].FlatAppearance.MouseOverBackColor = myButton[i, j + 3].FlatAppearance.MouseOverBackColor = Color.Red;
                        myMap[i, j] = myMap[i, j + 1] = myMap[i, j + 2] = myMap[i, j + 3] = 4;
                        richTextBox.Text += ", убил";
                        if (j == 7 || i == 10)
                        {
                            if (j == 7 && i != 10)
                            {
                                myMap[i - 1, j - 1] = myMap[i, j - 1] = myMap[i + 1, j - 1] = 5;
                                myMap[i - 1, j] = myMap[i + 1, j] = 5;
                                myMap[i - 1, j + 1] = myMap[i + 1, j + 1] = 5;
                                myMap[i - 1, j + 2] = myMap[i + 1, j + 2] = 5;
                                myMap[i - 1, j + 3] = myMap[i + 1, j + 3] = 5;
                            }
                            if (j == 7 && i == 10)
                            {
                                myMap[i - 1, j - 1] = myMap[i, j - 1] = 5;
                                myMap[i - 1, j] = myMap[i - 1, j + 1] = myMap[i - 1, j + 2] = myMap[i - 1, j + 3] = 5;
                            }
                            if (j < 7 && i == 10)
                            {
                                myMap[i - 1, j - 1] = myMap[i, j - 1] = 5;
                                myMap[i - 1, j + 4] = myMap[i, j + 4] = 5;
                                myMap[i - 1, j] = myMap[i - 1, j + 1] = myMap[i - 1, j + 2] = myMap[i - 1, j + 3] = 5;
                            }
                        }
                        else
                        {
                            myMap[i - 1, j] = myMap[i + 1, j] = 5;
                            myMap[i - 1, j + 1] = myMap[i + 1, j + 1] = 5;
                            myMap[i - 1, j + 2] = myMap[i + 1, j + 2] = 5;
                            myMap[i - 1, j + 3] = myMap[i + 1, j + 3] = 5;
                            myMap[i - 1, j - 1] = myMap[i, j - 1] = myMap[i + 1, j - 1] = 5;
                            myMap[i - 1, j + 4] = myMap[i, j + 4] = myMap[i + 1, j + 4] = 5;
                        }
                    }
                    // Вертикальный 3
                    if ((myMap[i - 1, j] != 1 && i < 9) && myMap[i - 1, j] != 3 && myMap[i, j] == 3 && myMap[i + 1, j] == 3 && myMap[i + 2, j] == 3 && (i == 8 || (myMap[i + 3, j] != 3 && myMap[i + 3, j] != 1)))
                    {
                        myButton[i, j].Text = myButton[i + 1, j].Text = myButton[i + 2, j].Text = "X";
                        myButton[i, j].BackColor = myButton[i + 1, j].BackColor = myButton[i + 2, j].BackColor = Color.Red;
                        myButton[i, j].FlatStyle = myButton[i + 1, j].FlatStyle = myButton[i + 2, j].FlatStyle = System.Windows.Forms.FlatStyle.Flat;
                        myButton[i, j].FlatAppearance.MouseDownBackColor = myButton[i + 1, j].FlatAppearance.MouseDownBackColor = myButton[i + 2, j].FlatAppearance.MouseDownBackColor = Color.Red;
                        myButton[i, j].FlatAppearance.MouseOverBackColor = myButton[i + 1, j].FlatAppearance.MouseOverBackColor = myButton[i + 2, j].FlatAppearance.MouseOverBackColor = Color.Red;
                        myMap[i, j] = myMap[i + 1, j] = myMap[i + 2, j] = 4;
                        richTextBox.Text += ", убил";
                        if (i == 8 || j == 10)
                        {
                            if (i == 8 && j != 10)
                            {
                                myMap[i - 1, j - 1] = myMap[i - 1, j] = myMap[i - 1, j + 1] = 5;
                                myMap[i, j - 1] = myMap[i, j + 1] = 5;
                                myMap[i + 1, j - 1] = myMap[i + 1, j + 1] = 5;
                                myMap[i + 2, j - 1] = myMap[i + 2, j + 1] = 5;
                            }
                            if (i == 8 && j == 10)
                            {
                                myMap[i - 1, j] = myMap[i - 1, j - 1] = 5;
                                myMap[i, j - 1] = myMap[i + 1, j - 1] = myMap[i + 2, j - 1] = 5;
                            }
                            if (i < 8 && j == 10)
                            {
                                myMap[i - 1, j] = myMap[i - 1, j - 1] = 5;
                                myMap[i, j - 1] = myMap[i + 1, j - 1] = myMap[i + 2, j - 1] = 5;
                                myMap[i + 3, j] = myMap[i + 3, j - 1] = 5;
                            }
                        }
                        else
                        {
                            myMap[i - 1, j - 1] = myMap[i - 1, j] = myMap[i - 1, j + 1] = 5;
                            myMap[i + 3, j - 1] = myMap[i + 3, j] = myMap[i + 3, j + 1] = 5;
                            myMap[i, j - 1] = myMap[i, j + 1] = 5;
                            myMap[i + 1, j - 1] = myMap[i + 1, j + 1] = 5;
                            myMap[i + 2, j - 1] = myMap[i + 2, j + 1] = 5;
                        }
                    }
                    // Горизонтальный 3
                    if ((myMap[i, j - 1] != 1 && j < 9) && myMap[i, j - 1] != 3 && myMap[i, j] == 3 && myMap[i, j + 1] == 3 && myMap[i, j + 2] == 3 && (j == 8 || (myMap[i, j + 3] != 3 && myMap[i, j + 3] != 1)))
                    {
                        myButton[i, j].Text = myButton[i, j + 1].Text = myButton[i, j + 2].Text = "X";
                        myButton[i, j].BackColor = myButton[i, j + 1].BackColor = myButton[i, j + 2].BackColor = Color.Red;
                        myButton[i, j].FlatStyle = myButton[i, j + 1].FlatStyle = myButton[i, j + 2].FlatStyle = System.Windows.Forms.FlatStyle.Flat;
                        myButton[i, j].FlatAppearance.MouseDownBackColor = myButton[i, j + 1].FlatAppearance.MouseDownBackColor = myButton[i, j + 2].FlatAppearance.MouseDownBackColor = Color.Red;
                        myButton[i, j].FlatAppearance.MouseOverBackColor = myButton[i, j + 1].FlatAppearance.MouseOverBackColor = myButton[i, j + 2].FlatAppearance.MouseOverBackColor = Color.Red;
                        myMap[i, j] = myMap[i, j + 1] = myMap[i, j + 2] = 4;
                        richTextBox.Text += ", убил";
                        if (j == 8 || i == 10)
                        {
                            if (j == 8 && i != 10)
                            {
                                myMap[i - 1, j - 1] = myMap[i, j - 1] = myMap[i + 1, j - 1] = 5;
                                myMap[i - 1, j] = myMap[i + 1, j] = 5;
                                myMap[i - 1, j + 1] = myMap[i + 1, j + 1] = 5;
                                myMap[i - 1, j + 2] = myMap[i + 1, j + 2] = 5;
                            }
                            if (j == 8 && i == 10)
                            {
                                myMap[i - 1, j - 1] = myMap[i, j - 1] = 5;
                                myMap[i - 1, j] = myMap[i - 1, j + 1] = myMap[i - 1, j + 2] = 5;
                            }
                            if (j < 8 && i == 10)
                            {
                                myMap[i - 1, j - 1] = myMap[i, j - 1] = 5;
                                myMap[i - 1, j + 3] = myMap[i, j + 3] = 5;
                                myMap[i - 1, j] = myMap[i - 1, j + 1] = myMap[i - 1, j + 2] = 5;
                            }
                        }
                        else
                        {
                            myMap[i - 1, j] = myMap[i + 1, j] = 5;
                            myMap[i - 1, j + 1] = myMap[i + 1, j + 1] = 5;
                            myMap[i - 1, j + 2] = myMap[i + 1, j + 2] = 5;
                            myMap[i - 1, j - 1] = myMap[i, j - 1] = myMap[i + 1, j - 1] = 5;
                            myMap[i - 1, j + 3] = myMap[i, j + 3] = myMap[i + 1, j + 3] = 5;
                        }
                    }
                    // Вертикальный 2
                    if ((myMap[i - 1, j] != 1 && i < 10) && myMap[i - 1, j] != 3 && myMap[i, j] == 3 && myMap[i + 1, j] == 3 && (i == 9 || (myMap[i + 2, j] != 3 && myMap[i + 2, j] != 1)))
                    {
                        myButton[i, j].Text = myButton[i + 1, j].Text = "X";
                        myButton[i, j].BackColor = myButton[i + 1, j].BackColor = Color.Red;
                        myButton[i, j].FlatStyle = myButton[i + 1, j].FlatStyle = System.Windows.Forms.FlatStyle.Flat;
                        myButton[i, j].FlatAppearance.MouseDownBackColor = myButton[i + 1, j].FlatAppearance.MouseDownBackColor = Color.Red;
                        myButton[i, j].FlatAppearance.MouseOverBackColor = myButton[i + 1, j].FlatAppearance.MouseOverBackColor = Color.Red;
                        myMap[i, j] = myMap[i + 1, j] = 4;
                        richTextBox.Text += ", убил";
                        if (i == 9 || j == 10)
                        {
                            if (i == 9 && j != 10)
                            {
                                myMap[i - 1, j - 1] = myMap[i - 1, j] = myMap[i - 1, j + 1] = 5;
                                myMap[i, j - 1] = myMap[i, j + 1] = 5;
                                myMap[i + 1, j - 1] = myMap[i + 1, j + 1] = 5;
                            }
                            if (i == 9 && j == 10)
                            {
                                myMap[i - 1, j] = myMap[i - 1, j - 1] = 5;
                                myMap[i, j - 1] = myMap[i + 1, j - 1] = 5;
                            }
                            if (i < 9 && j == 10)
                            {
                                myMap[i - 1, j] = myMap[i - 1, j - 1] = 5;
                                myMap[i, j - 1] = myMap[i + 1, j - 1] = 5;
                                myMap[i + 2, j] = myMap[i + 2, j - 1] = 5;
                            }
                        }
                        else
                        {
                            myMap[i - 1, j - 1] = myMap[i - 1, j] = myMap[i - 1, j + 1] = 5;
                            myMap[i + 2, j - 1] = myMap[i + 2, j] = myMap[i + 2, j + 1] = 5;
                            myMap[i, j - 1] = myMap[i, j + 1] = 5;
                            myMap[i + 1, j - 1] = myMap[i + 1, j + 1] = 5;
                        }
                    }
                    // Горизонтальный 2
                    if ((myMap[i, j - 1] != 1 && j < 10) && myMap[i, j - 1] != 3 && myMap[i, j] == 3 && myMap[i, j + 1] == 3 && (j == 9 || (myMap[i, j + 2] != 3 && myMap[i, j + 2] != 1)))
                    {
                        myButton[i, j].Text = myButton[i, j + 1].Text = "X";
                        myButton[i, j].BackColor = myButton[i, j + 1].BackColor = Color.Red;
                        myButton[i, j].FlatStyle = myButton[i, j + 1].FlatStyle = System.Windows.Forms.FlatStyle.Flat;
                        myButton[i, j].FlatAppearance.MouseDownBackColor = myButton[i, j + 1].FlatAppearance.MouseDownBackColor = Color.Red;
                        myButton[i, j].FlatAppearance.MouseOverBackColor = myButton[i, j + 1].FlatAppearance.MouseOverBackColor = Color.Red;
                        myMap[i, j] = myMap[i, j + 1] = 4;
                        richTextBox.Text += ", убил";
                        if (j == 9 || i == 10)
                        {
                            if (j == 9 && i != 10)
                            {
                                myMap[i - 1, j - 1] = myMap[i, j - 1] = myMap[i + 1, j - 1] = 5;
                                myMap[i - 1, j] = myMap[i + 1, j] = 5;
                                myMap[i - 1, j + 1] = myMap[i + 1, j + 1] = 5;
                            }
                            if (j == 9 && i == 10)
                            {
                                myMap[i - 1, j - 1] = myMap[i, j - 1] = 5;
                                myMap[i - 1, j] = myMap[i - 1, j + 1] = 5;
                            }
                            if (j < 9 && i == 10)
                            {
                                myMap[i - 1, j - 1] = myMap[i, j - 1] = 5;
                                myMap[i - 1, j + 2] = myMap[i, j + 2] = 5;
                                myMap[i - 1, j] = myMap[i - 1, j + 1] = 5;
                            }
                        }
                        else
                        {
                            myMap[i - 1, j] = myMap[i + 1, j] = 5;
                            myMap[i - 1, j + 1] = myMap[i + 1, j + 1] = 5;
                            myMap[i - 1, j - 1] = myMap[i, j - 1] = myMap[i + 1, j - 1] = 5;
                            myMap[i - 1, j + 2] = myMap[i, j + 2] = myMap[i + 1, j + 2] = 5;
                        }
                    }
                    // 1
                    if (myMap[i - 1, j] != 1 && myMap[i - 1, j] != 3 && myMap[i, j - 1] != 1 && myMap[i, j - 1] != 3 && myMap[i, j] == 3 && (j == 10 || (myMap[i, j + 1] != 3 && myMap[i, j + 1] != 1)) && (i == 10 || (myMap[i + 1, j] != 3 && myMap[i + 1, j] != 1)))
                    {
                        myButton[i, j].Text = "X";
                        myButton[i, j].BackColor = Color.Red;
                        myButton[i, j].FlatStyle = System.Windows.Forms.FlatStyle.Flat;
                        myButton[i, j].FlatAppearance.MouseDownBackColor = Color.Red;
                        myButton[i, j].FlatAppearance.MouseOverBackColor = Color.Red;
                        myMap[i, j] = 4;
                        richTextBox.Text += ", убил";
                        if (j == 10 || i == 10)
                        {
                            if (j == 10 && i != 10)
                            {
                                myMap[i - 1, j] = myMap[i - 1, j - 1] = 5;
                                myMap[i + 1, j] = myMap[i + 1, j - 1] = 5;
                                myMap[i, j - 1] = 5;
                            }
                            if (j == 10 && i == 10)
                            {
                                myMap[i - 1, j] = myMap[i - 1, j - 1] = myMap[i, j - 1] = 5;
                            }
                            if (j != 10 && i == 10)
                            {
                                myMap[i - 1, j - 1] = myMap[i - 1, j] = myMap[i - 1, j + 1] = 5;
                                myMap[i, j - 1] = myMap[i, j + 1] = 5;
                            }
                        }
                        else
                        {
                            myMap[i - 1, j - 1] = myMap[i - 1, j] = myMap[i - 1, j + 1] = 5;
                            myMap[i, j - 1] = myMap[i, j + 1] = 5;
                            myMap[i + 1, j - 1] = myMap[i + 1, j] = myMap[i + 1, j + 1] = 5;
                        }
                    }
                }
            }

            for (int i = 1; i < cell; i++) // Окраска в синий цвет полей вокруг убитого корабля
            {
                for (int j = 1; j < cell; j++)
                {
                    if (myMap[i, j] == 5)
                    {
                        myButton[i, j].Enabled = false;
                        myButton[i, j].BackColor = Color.SkyBlue;
                    }
                }
            }

            int cell4 = 0;
            for (int i = 1; i < cell; i++) // Счетчик "убитых" клеток
            {
                for (int j = 1; j < cell; j++)
                {
                    if (myMap[i, j] == 4)
                    {
                        cell4++;
                    }
                }
            }
            if (cell4 == 20) // Если убитых клеток 20, то все корабли убиты
            {
                Game = false;
                for (int i = 1; i < cell; i++)
                {
                    for (int j = 1; j < cell; j++)
                    {
                        if (botMap[i, j] == 1)
                        {
                            botButton[i, j].BackColor = Color.Green;
                            botButton[i, j].FlatStyle = System.Windows.Forms.FlatStyle.Flat;
                            botButton[i, j].FlatAppearance.MouseDownBackColor = Color.Green;
                            botButton[i, j].FlatAppearance.MouseOverBackColor = Color.Green;
                        }
                    }
                }
                DialogResult result = MessageBox.Show("Вы проиграли! Начать заново ? ", "Конец игры", MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                {
                    Restart();
                }
            }
        }
    }
}