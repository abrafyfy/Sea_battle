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
    public partial class Form3 : Form
    {
        Button buttonStart = new Button();
        Button buttonNewGame = new Button();
        Label leftMap = new Label();
        Label rightMap = new Label();
        Button buttonSaveGame = new Button();
        Button buttonLoadGame = new Button();
        Button buttonExit = new Button();
        Button buttonP1ready = new Button();
        Button buttonP2ready = new Button();
        SaveFileDialog saveFileDialog = new SaveFileDialog();
        OpenFileDialog openFileDialog = new OpenFileDialog();
        RichTextBox richTextBox = new RichTextBox();
        int cellSize = 30;
        static int cell = 11;
        string abc = "АБВГДЕЖЗИК";
        int[,] P1Map = new int[cell, cell];
        int[,] P2Map = new int[cell, cell];
        bool[,] checkMap = new bool[cell + 1, cell + 1];
        int[,] CheckKillMap = new int[cell, cell];
        Button[,] P1Button = new Button[cell, cell];
        Button[,] P2Button = new Button[cell, cell];
        Button[,] CheckKillButton = new Button[cell, cell];
        StringBuilder s = new StringBuilder(206);
        string mapinfo;
        bool P1_Ready, P2_Ready, P1_Shot, P2_Shot;
        bool Game_On;

        public Form3()
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
            P1_Ready = P2_Ready = P1_Shot = P2_Shot = false;
            richTextBox.Text = "Игра с игроком\n";
            CreateMap();
        }

        public void CreateMap() // Создание карты
        {
            // Создание поля 1 игрока
            for (int i = 0; i < cell; i++)
            {
                for (int j = 0; j < cell; j++)
                {
                    P1Map[i, j] = 0;
                    Button button = new Button();
                    button.Location = new Point(j * cellSize, i * cellSize + cellSize);
                    button.Size = new Size(cellSize, cellSize);

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
                        button.Click += new EventHandler(MapClick);
                    }
                    button.BackColor = System.Drawing.Color.Transparent;
                    button.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
                    button.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
                    button.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
                    P1Button[i, j] = button;
                    this.Controls.Add(button);
                }
            }
            // Создание поля 2 игрока
            for (int i = 0; i < cell; i++)
            {
                for (int j = 0; j < cell; j++)
                {
                    P2Map[i, j] = 0;
                    Button button = new Button();
                    button.Location = new Point(j * cellSize + cellSize * cell * 2 - cellSize, i * cellSize + cellSize);
                    button.Size = new Size(cellSize, cellSize);
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
                        button.Click += new EventHandler(MapClick);
                    }
                    button.BackColor = System.Drawing.Color.Transparent;
                    button.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
                    button.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
                    button.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
                    P2Button[i, j] = button;
                    this.Controls.Add(button);
                }
            }

            leftMap.Text = "Игрок 1";
            leftMap.Location = new Point(cell * cellSize / 2, cellSize / 3);
            this.Controls.Add(leftMap);
            this.leftMap.BackColor = System.Drawing.Color.Transparent;

            rightMap.Text = "Игрок 2";
            rightMap.Location = new Point(cell * cellSize / 2 + cellSize * 21, cellSize / 3);
            this.Controls.Add(rightMap);
            this.rightMap.BackColor = System.Drawing.Color.Transparent;

            leftMap.Font = rightMap.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));

            buttonStart.Location = new Point(cell * cellSize + 3 * cellSize, 2 * cellSize);
            buttonStart.Size = new Size(cellSize * 4, cellSize);
            buttonStart.Text = "Начать";
            buttonStart.Enabled = false;
            buttonStart.Click += new EventHandler(Start);
            this.Controls.Add(buttonStart);

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

            buttonP1ready.Location = new Point(cell * cellSize - 10 * cellSize, 13 * cellSize);
            buttonP1ready.Size = new Size(cellSize * 4, cellSize);
            buttonP1ready.Text = "Готов";
            buttonP1ready.Click += new EventHandler(P1ready);
            this.Controls.Add(buttonP1ready);

            buttonP2ready.Location = new Point(cell * cellSize + 11 * cellSize, 13 * cellSize);
            buttonP2ready.Size = new Size(cellSize * 4, cellSize);
            buttonP2ready.Text = "Готов";
            buttonP2ready.Click += new EventHandler(P2ready);
            this.Controls.Add(buttonP2ready);

            richTextBox.Location = new Point(cell * cellSize + 16 * cellSize, 13 * cellSize);
            richTextBox.Size = new Size(cellSize * 5, 5 * cellSize);
            richTextBox.ScrollBars = RichTextBoxScrollBars.Vertical;
            richTextBox.ReadOnly = true;
            this.Controls.Add(richTextBox);

            buttonStart.Font = buttonNewGame.Font = buttonSaveGame.Font = buttonLoadGame.Font = buttonExit.Font = buttonP1ready.Font = buttonP2ready.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
        }

        public void MapClick(object sender, EventArgs e) // Нажатие на кнопку поля
        {
            if (!Game_On)
            {
                // Создать корабль
                CreateShip(sender, e);
            }
            else
            {
                // Выстрелить
                Shot(sender, e);
            }
        }

        public void CreateShip(object sender, EventArgs e) // Создание корабля
        {
            Button pressedButton = sender as Button;

            // Установка корабля первого игрока
            if (!P1_Ready)
            {
                try
                {
                    int x = pressedButton.Location.X / cellSize;
                    int y = pressedButton.Location.Y / cellSize - 1;

                    // Временное изменение карты для проверки
                    int previousState = P1Map[y, x];
                    P1Map[y, x] = (previousState == 0) ? 1 : 0;

                    // Проверка расстановки для первого игрока
                    if (!IsValidPlacement(P1Map))
                    {
                        P1Map[y, x] = previousState;
                        MessageBox.Show("Превышено максимальное количество кораблей или некорректное размещение!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    // Применяем изменения после проверки
                    switch (previousState)
                    {
                        case 0:
                            pressedButton.BackColor = Color.Blue;
                            P1Map[y, x] = 1;
                            pressedButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
                            pressedButton.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Blue;
                            pressedButton.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Blue;
                            break;

                        case 1:
                            pressedButton.BackColor = System.Drawing.Color.Transparent;
                            P1Map[y, x] = 0;
                            pressedButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
                            pressedButton.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
                            pressedButton.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
                            break;
                    }
                }
                catch
                {
                    MessageBox.Show("Расставляйте корабли на своей карте");
                }
            }

            // Блокировка полей для первого игрока
            BlockAdjacentCells(P1Map);

            // Установка корабля второго игрока
            if (P1_Ready && !P2_Ready)
            {
                try
                {
                    int x = pressedButton.Location.X / cellSize - 21;
                    int y = pressedButton.Location.Y / cellSize - 1;

                    // Временное изменение карты для проверки
                    int previousState = P2Map[y, x];
                    P2Map[y, x] = (previousState == 0) ? 1 : 0;

                    // Проверка расстановки для второго игрока
                    if (!IsValidPlacement(P2Map))
                    {
                        P2Map[y, x] = previousState;
                        MessageBox.Show("Превышено максимальное количество кораблей или некорректное размещение!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    // Применяем изменения после проверки
                    switch (previousState)
                    {
                        case 0:
                            pressedButton.BackColor = Color.Blue;
                            P2Map[y, x] = 1;
                            pressedButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
                            pressedButton.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Blue;
                            pressedButton.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Blue;
                            break;

                        case 1:
                            pressedButton.BackColor = System.Drawing.Color.Transparent;
                            P2Map[y, x] = 0;
                            pressedButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
                            pressedButton.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
                            pressedButton.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
                            break;
                    }
                }
                catch
                {
                    MessageBox.Show("Расставляйте корабли на своей карте!");
                }
            }

            // Блокировка полей для второго игрока
            BlockAdjacentCells(P2Map);
        }

        // Метод для проверки корректности расстановки кораблей
        private bool IsValidPlacement(int[,] map)
        {
            int singleShips = 0, doubleShips = 0, tripleShips = 0, quadrupleShips = 0;
            bool[,] visited = new bool[cell, cell];

            for (int i = 1; i < cell; i++)
            {
                for (int j = 1; j < cell; j++)
                {
                    if (map[i, j] == 1 && !visited[i, j])
                    {
                        int size = GetShipSize(i, j, map, visited);

                        switch (size)
                        {
                            case 1: singleShips++; break;
                            case 2: doubleShips++; break;
                            case 3: tripleShips++; break;
                            case 4: quadrupleShips++; break;
                            default: return false;
                        }
                    }
                }
            }

            return singleShips <= 4 && doubleShips <= 3 && tripleShips <= 2 && quadrupleShips <= 1;
        }

        // Метод для определения размера корабля
        private int GetShipSize(int x, int y, int[,] map, bool[,] visited)
        {
            visited[x, y] = true;
            int size = 1;

            if (x > 1 && map[x - 1, y] == 1 && !visited[x - 1, y])
                size += GetShipSize(x - 1, y, map, visited);

            if (x < cell - 1 && map[x + 1, y] == 1 && !visited[x + 1, y])
                size += GetShipSize(x + 1, y, map, visited);

            if (y > 1 && map[x, y - 1] == 1 && !visited[x, y - 1])
                size += GetShipSize(x, y - 1, map, visited);

            if (y < cell - 1 && map[x, y + 1] == 1 && !visited[x, y + 1])
                size += GetShipSize(x, y + 1, map, visited);

            return size;
        }

        // Метод для блокировки соседних клеток
        private void BlockAdjacentCells(int[,] map)
        {
            for (int i = 1; i < 11; i++)
            {
                for (int j = 1; j < 11; j++)
                {
                    if (map[i, j] == 1)
                    {
                        if (j != 10 && i != 10)
                        {
                            map[i + 1, j + 1] = 2;
                            map[i + 1, j - 1] = 2;
                            map[i - 1, j + 1] = 2;
                            map[i - 1, j - 1] = 2;
                        }
                        if (i == 10 && j != 10)
                        {
                            map[i - 1, j + 1] = 2;
                            map[i - 1, j - 1] = 2;
                        }
                        if (j == 10 && i != 10)
                        {
                            map[i + 1, j - 1] = 2;
                            map[i - 1, j - 1] = 2;
                        }
                        if (j == 10 && i == 10)
                        {
                            map[i - 1, j - 1] = 2;
                        }
                    }
                }
            }
        }


        public void P1ready(object sender, EventArgs e) // Проверка готовности игрока 1
        {
            for (int i = 1; i < cell; i++)
            {
                for (int j = 1; j < cell; j++)
                {
                    if (P1Map[i, j] == 1)
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

        public void P2ready(object sender, EventArgs e) // Проверка готовности игрока 2
        {
            for (int i = 1; i < cell; i++)
            {
                for (int j = 1; j < cell; j++)
                {
                    if (P2Map[i, j] == 1)
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

        public void CheckShip() // Проверка расстановки кораблей
        {
            int Ship1, Ship2, Ship3, Ship4, ShipLong;
            Ship1 = Ship2 = Ship3 = Ship4 = ShipLong = 0;
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
            if (P1_Ready && !P2_Ready)
            {
                if (Ship1 != 4 || Ship2 != 3 || Ship3 != 2 || Ship4 != 1 || ShipLong != 0)
                {
                    MessageBox.Show("Неверно расставлены корабли", "Ошибка");
                }
                else
                {
                    richTextBox.Text += "Игрок 2 готов\n";
                    P2_Ready = true;
                    buttonP2ready.Enabled = false;
                    buttonStart.Enabled = true;
                    for (int i = 1; i < cell; i++)
                    {
                        for (int j = 1; j < cell; j++)
                        {
                            P2Button[i, j].BackColor = System.Drawing.Color.Transparent;
                            P2Button[i, j].FlatStyle = System.Windows.Forms.FlatStyle.Flat;
                            P2Button[i, j].FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
                            P2Button[i, j].FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
                        }
                    }
                }
            }
            if (!P1_Ready)
            {
                if (Ship1 != 4 || Ship2 != 3 || Ship3 != 2 || Ship4 != 1 || ShipLong != 0)
                {
                    MessageBox.Show("Неверно расставлены корабли", "Ошибка");
                }
                else
                {
                    richTextBox.Text += "Игрок 1 готов\n";
                    P1_Ready = true;
                    buttonP1ready.Enabled = false;
                    for (int i = 1; i < cell; i++)
                    {
                        for (int j = 1; j < cell; j++)
                        {
                            P1Button[i, j].BackColor = System.Drawing.Color.Transparent;
                            P1Button[i, j].FlatStyle = System.Windows.Forms.FlatStyle.Flat;
                            P1Button[i, j].FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
                            P1Button[i, j].FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
                        }
                    }
                }
            }
        }

        public void Start(object sender, EventArgs e) // Начало игры
        {
            if (P1_Ready && P2_Ready)
            {
                Game_On = true;
                Random rnd = new Random();
                int first = rnd.Next(1, 3);
                if (first == 1) P2_Shot = true;
                else P1_Shot = true;
                buttonStart.Enabled = false;
                MessageBox.Show($"Игра началась, первый ход у игрока {first}");
                richTextBox.Text += "Игра началась\n";

            }
        }

        public void Shot(object sender, EventArgs e) // Выстрел
        {
            // Стреляет второй
            if (P1_Shot)
            {
                try
                {
                    Button pressedButton = sender as Button;
                    int x = pressedButton.Location.X / cellSize;
                    int y = pressedButton.Location.Y / cellSize - 1;

                    switch (P1Map[y, x])
                    {
                        case 1:
                            richTextBox.Text += $"2: {y}{abc[x - 1]} ";
                            pressedButton.BackColor = Color.Orange;
                            pressedButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
                            pressedButton.FlatAppearance.MouseDownBackColor = Color.Orange;
                            pressedButton.FlatAppearance.MouseOverBackColor = Color.Orange;
                            richTextBox.Text += $"Попал";
                            P1Map[y, x] = 3;
                            CheckKill();
                            break;
                        case 0:
                            richTextBox.Text += $"2: {y}{abc[x - 1]} ";
                            pressedButton.BackColor = Color.SkyBlue;
                            pressedButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
                            pressedButton.FlatAppearance.MouseDownBackColor = Color.SkyBlue;
                            pressedButton.FlatAppearance.MouseOverBackColor = Color.SkyBlue;
                            richTextBox.Text += $"Мимо\n";
                            P1Map[y, x] = 5;
                            P2_Shot = true;
                            P1_Shot = false;
                            break;
                        case 2:
                            richTextBox.Text += $"2: {y}{abc[x - 1]} ";
                            pressedButton.BackColor = Color.SkyBlue;
                            pressedButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
                            pressedButton.FlatAppearance.MouseDownBackColor = Color.SkyBlue;
                            pressedButton.FlatAppearance.MouseOverBackColor = Color.SkyBlue;
                            richTextBox.Text += $"Мимо\n";
                            P1Map[y, x] = 5;
                            P2_Shot = true;
                            P1_Shot = false;
                            break;
                    }
                    pressedButton.Enabled = false;
                    return;
                }
                catch
                {
                    MessageBox.Show("Сейчас ход другого игрока");
                    return;
                }
            }
            // Стреляет первый
            if (P2_Shot)
            {
                try
                {
                    Button pressedButton = sender as Button;
                    int x = pressedButton.Location.X / cellSize - 21;
                    int y = pressedButton.Location.Y / cellSize - 1;
                    switch (P2Map[y, x])
                    {
                        case 1:
                            richTextBox.Text += $"1: {y}{abc[x - 1]} ";
                            pressedButton.BackColor = Color.Orange;
                            pressedButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
                            pressedButton.FlatAppearance.MouseDownBackColor = Color.Orange;
                            pressedButton.FlatAppearance.MouseOverBackColor = Color.Orange;
                            richTextBox.Text += $"Попал";
                            P2Map[y, x] = 3;
                            CheckKill();
                            break;
                        case 0:
                            richTextBox.Text += $"1: {y}{abc[x - 1]} ";
                            pressedButton.BackColor = Color.SkyBlue;
                            pressedButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
                            pressedButton.FlatAppearance.MouseDownBackColor = Color.SkyBlue;
                            pressedButton.FlatAppearance.MouseOverBackColor = Color.SkyBlue;
                            P2Map[y, x] = 5;
                            P1_Shot = true;
                            P2_Shot = false;
                            richTextBox.Text += $"Мимо\n";
                            break;
                        case 2:
                            richTextBox.Text += $"1: {y}{abc[x - 1]} ";
                            pressedButton.BackColor = Color.SkyBlue;
                            pressedButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
                            pressedButton.FlatAppearance.MouseDownBackColor = Color.SkyBlue;
                            pressedButton.FlatAppearance.MouseOverBackColor = Color.SkyBlue;
                            P2Map[y, x] = 5;
                            P1_Shot = true;
                            P2_Shot = false;
                            richTextBox.Text += $"Мимо\n";
                            break;
                    }
                    pressedButton.Enabled = false;
                    return;
                }
                catch
                {
                    MessageBox.Show("Сейчас ход другого игрока");
                    return;
                }
            }
        }

        public void CheckKill() // Проверка уничтожения корабля при попадании
        {
            if (P1_Shot)
            {
                for (int i = 1; i < cell; i++)
                {
                    for (int j = 1; j < cell; j++)
                    {
                        CheckKillMap[i, j] = P1Map[i, j];
                        CheckKillButton[i, j] = P1Button[i, j];
                    }
                }
            }
            if (P2_Shot)
            {
                for (int i = 1; i < cell; i++)
                {
                    for (int j = 1; j < cell; j++)
                    {
                        CheckKillMap[i, j] = P2Map[i, j];
                        CheckKillButton[i, j] = P2Button[i, j];
                    }
                }
            }
            int count4 = 0;
            for (int i = 1; i < cell; i++)
            {
                for (int j = 1; j < cell; j++)
                {
                    // Вертикальный 4
                    if ((CheckKillMap[i, j] == 3 && i < 8) && CheckKillMap[i + 1, j] == 3 && CheckKillMap[i + 2, j] == 3 && CheckKillMap[i + 3, j] == 3)
                    {
                        CheckKillButton[i, j].Text = CheckKillButton[i + 1, j].Text = CheckKillButton[i + 2, j].Text = CheckKillButton[i + 3, j].Text = "X";
                        CheckKillButton[i, j].BackColor = CheckKillButton[i + 1, j].BackColor = CheckKillButton[i + 2, j].BackColor = CheckKillButton[i + 3, j].BackColor = Color.Red;
                        CheckKillButton[i, j].FlatStyle = CheckKillButton[i + 1, j].FlatStyle = CheckKillButton[i + 2, j].FlatStyle = CheckKillButton[i + 3, j].FlatStyle = System.Windows.Forms.FlatStyle.Flat;
                        CheckKillButton[i, j].FlatAppearance.MouseDownBackColor = CheckKillButton[i + 1, j].FlatAppearance.MouseDownBackColor = CheckKillButton[i + 2, j].FlatAppearance.MouseDownBackColor = CheckKillButton[i + 3, j].FlatAppearance.MouseDownBackColor = Color.Red;
                        CheckKillButton[i, j].FlatAppearance.MouseOverBackColor = CheckKillButton[i + 1, j].FlatAppearance.MouseOverBackColor = CheckKillButton[i + 2, j].FlatAppearance.MouseOverBackColor = CheckKillButton[i + 3, j].FlatAppearance.MouseOverBackColor = Color.Red;
                        CheckKillMap[i, j] = CheckKillMap[i + 1, j] = CheckKillMap[i + 2, j] = CheckKillMap[i + 3, j] = 4;
                        richTextBox.Text += ", убил";
                        if (i == 7 || j == 10)
                        {
                            if (i == 7 && j != 10)
                            {
                                CheckKillMap[i - 1, j - 1] = CheckKillMap[i - 1, j] = CheckKillMap[i - 1, j + 1] = 5;
                                CheckKillMap[i, j - 1] = CheckKillMap[i, j + 1] = 5;
                                CheckKillMap[i + 1, j - 1] = CheckKillMap[i + 1, j + 1] = 5;
                                CheckKillMap[i + 2, j - 1] = CheckKillMap[i + 2, j + 1] = 5;
                                CheckKillMap[i + 3, j - 1] = CheckKillMap[i + 3, j + 1] = 5;
                            }
                            if (i == 7 && j == 10)
                            {
                                CheckKillMap[i - 1, j] = CheckKillMap[i - 1, j - 1] = 5;
                                CheckKillMap[i, j - 1] = CheckKillMap[i + 1, j - 1] = CheckKillMap[i + 2, j - 1] = CheckKillMap[i + 3, j - 1] = 5;
                            }
                            if (i < 7 && j == 10)
                            {
                                CheckKillMap[i - 1, j] = CheckKillMap[i - 1, j - 1] = 5;
                                CheckKillMap[i, j - 1] = CheckKillMap[i + 1, j - 1] = CheckKillMap[i + 2, j - 1] = CheckKillMap[i + 3, j - 1] = 5;
                                CheckKillMap[i + 4, j] = CheckKillMap[i + 4, j - 1] = 5;
                            }
                        }
                        else
                        {
                            CheckKillMap[i - 1, j - 1] = CheckKillMap[i - 1, j] = CheckKillMap[i - 1, j + 1] = 5;
                            CheckKillMap[i + 4, j - 1] = CheckKillMap[i + 4, j] = CheckKillMap[i + 4, j + 1] = 5;
                            CheckKillMap[i, j - 1] = CheckKillMap[i, j + 1] = 5;
                            CheckKillMap[i + 1, j - 1] = CheckKillMap[i + 1, j + 1] = 5;
                            CheckKillMap[i + 2, j - 1] = CheckKillMap[i + 2, j + 1] = 5;
                            CheckKillMap[i + 3, j - 1] = CheckKillMap[i + 3, j + 1] = 5;
                        }
                    }
                    // Горизонтальный 4
                    if ((CheckKillMap[i, j] == 3 && j < 8) && CheckKillMap[i, j + 1] == 3 && CheckKillMap[i, j + 2] == 3 && CheckKillMap[i, j + 3] == 3)
                    {
                        CheckKillButton[i, j].Text = CheckKillButton[i, j + 1].Text = CheckKillButton[i, j + 2].Text = CheckKillButton[i, j + 3].Text = "X";
                        CheckKillButton[i, j].BackColor = CheckKillButton[i, j + 1].BackColor = CheckKillButton[i, j + 2].BackColor = CheckKillButton[i, j + 3].BackColor = Color.Red;
                        CheckKillButton[i, j].FlatStyle = CheckKillButton[i, j + 1].FlatStyle = CheckKillButton[i, j + 2].FlatStyle = CheckKillButton[i, j + 3].FlatStyle = System.Windows.Forms.FlatStyle.Flat;
                        CheckKillButton[i, j].FlatAppearance.MouseDownBackColor = CheckKillButton[i, j + 1].FlatAppearance.MouseDownBackColor = CheckKillButton[i, j + 2].FlatAppearance.MouseDownBackColor = CheckKillButton[i, j + 3].FlatAppearance.MouseDownBackColor = Color.Red;
                        CheckKillButton[i, j].FlatAppearance.MouseOverBackColor = CheckKillButton[i, j + 1].FlatAppearance.MouseOverBackColor = CheckKillButton[i, j + 2].FlatAppearance.MouseOverBackColor = CheckKillButton[i, j + 3].FlatAppearance.MouseOverBackColor = Color.Red;
                        CheckKillMap[i, j] = CheckKillMap[i, j + 1] = CheckKillMap[i, j + 2] = CheckKillMap[i, j + 3] = 4;
                        richTextBox.Text += ", убил";
                        if (j == 7 || i == 10)
                        {
                            if (j == 7 && i != 10)
                            {
                                CheckKillMap[i - 1, j - 1] = CheckKillMap[i, j - 1] = CheckKillMap[i + 1, j - 1] = 5;
                                CheckKillMap[i - 1, j] = CheckKillMap[i + 1, j] = 5;
                                CheckKillMap[i - 1, j + 1] = CheckKillMap[i + 1, j + 1] = 5;
                                CheckKillMap[i - 1, j + 2] = CheckKillMap[i + 1, j + 2] = 5;
                                CheckKillMap[i - 1, j + 3] = CheckKillMap[i + 1, j + 3] = 5;
                            }
                            if (j == 7 && i == 10)
                            {
                                CheckKillMap[i - 1, j - 1] = CheckKillMap[i, j - 1] = 5;
                                CheckKillMap[i - 1, j] = CheckKillMap[i - 1, j + 1] = CheckKillMap[i - 1, j + 2] = CheckKillMap[i - 1, j + 3] = 5;
                            }
                            if (j < 7 && i == 10)
                            {
                                CheckKillMap[i - 1, j - 1] = CheckKillMap[i, j - 1] = 5;
                                CheckKillMap[i - 1, j + 4] = CheckKillMap[i, j + 4] = 5;
                                CheckKillMap[i - 1, j] = CheckKillMap[i - 1, j + 1] = CheckKillMap[i - 1, j + 2] = CheckKillMap[i - 1, j + 3] = 5;
                            }
                        }
                        else
                        {
                            CheckKillMap[i - 1, j] = CheckKillMap[i + 1, j] = 5;
                            CheckKillMap[i - 1, j + 1] = CheckKillMap[i + 1, j + 1] = 5;
                            CheckKillMap[i - 1, j + 2] = CheckKillMap[i + 1, j + 2] = 5;
                            CheckKillMap[i - 1, j + 3] = CheckKillMap[i + 1, j + 3] = 5;
                            CheckKillMap[i - 1, j - 1] = CheckKillMap[i, j - 1] = CheckKillMap[i + 1, j - 1] = 5;
                            CheckKillMap[i - 1, j + 4] = CheckKillMap[i, j + 4] = CheckKillMap[i + 1, j + 4] = 5;
                        }
                    }
                    // Вертикальный 3
                    if ((CheckKillMap[i - 1, j] != 1 && i < 9) && CheckKillMap[i - 1, j] != 3 && CheckKillMap[i, j] == 3 && CheckKillMap[i + 1, j] == 3 && CheckKillMap[i + 2, j] == 3 && (i == 8 || (CheckKillMap[i + 3, j] != 3 && CheckKillMap[i + 3, j] != 1)))
                    {
                        CheckKillButton[i, j].Text = CheckKillButton[i + 1, j].Text = CheckKillButton[i + 2, j].Text = "X";
                        CheckKillButton[i, j].BackColor = CheckKillButton[i + 1, j].BackColor = CheckKillButton[i + 2, j].BackColor = Color.Red;
                        CheckKillButton[i, j].FlatStyle = CheckKillButton[i + 1, j].FlatStyle = CheckKillButton[i + 2, j].FlatStyle = System.Windows.Forms.FlatStyle.Flat;
                        CheckKillButton[i, j].FlatAppearance.MouseDownBackColor = CheckKillButton[i + 1, j].FlatAppearance.MouseDownBackColor = CheckKillButton[i + 2, j].FlatAppearance.MouseDownBackColor = Color.Red;
                        CheckKillButton[i, j].FlatAppearance.MouseOverBackColor = CheckKillButton[i + 1, j].FlatAppearance.MouseOverBackColor = CheckKillButton[i + 2, j].FlatAppearance.MouseOverBackColor = Color.Red;
                        CheckKillMap[i, j] = CheckKillMap[i + 1, j] = CheckKillMap[i + 2, j] = 4;
                        richTextBox.Text += ", убил";
                        if (i == 8 || j == 10)
                        {
                            if (i == 8 && j != 10)
                            {
                                CheckKillMap[i - 1, j - 1] = CheckKillMap[i - 1, j] = CheckKillMap[i - 1, j + 1] = 5;
                                CheckKillMap[i, j - 1] = CheckKillMap[i, j + 1] = 5;
                                CheckKillMap[i + 1, j - 1] = CheckKillMap[i + 1, j + 1] = 5;
                                CheckKillMap[i + 2, j - 1] = CheckKillMap[i + 2, j + 1] = 5;
                            }
                            if (i == 8 && j == 10)
                            {
                                CheckKillMap[i - 1, j] = CheckKillMap[i - 1, j - 1] = 5;
                                CheckKillMap[i, j - 1] = CheckKillMap[i + 1, j - 1] = CheckKillMap[i + 2, j - 1] = 5;
                            }
                            if (i < 8 && j == 10)
                            {
                                CheckKillMap[i - 1, j] = CheckKillMap[i - 1, j - 1] = 5;
                                CheckKillMap[i, j - 1] = CheckKillMap[i + 1, j - 1] = CheckKillMap[i + 2, j - 1] = 5;
                                CheckKillMap[i + 3, j] = CheckKillMap[i + 3, j - 1] = 5;
                            }
                        }
                        else
                        {
                            CheckKillMap[i - 1, j - 1] = CheckKillMap[i - 1, j] = CheckKillMap[i - 1, j + 1] = 5;
                            CheckKillMap[i + 3, j - 1] = CheckKillMap[i + 3, j] = CheckKillMap[i + 3, j + 1] = 5;
                            CheckKillMap[i, j - 1] = CheckKillMap[i, j + 1] = 5;
                            CheckKillMap[i + 1, j - 1] = CheckKillMap[i + 1, j + 1] = 5;
                            CheckKillMap[i + 2, j - 1] = CheckKillMap[i + 2, j + 1] = 5;
                        }
                    }
                    // Горизонтальный 3
                    if ((CheckKillMap[i, j - 1] != 1 && j < 9) && CheckKillMap[i, j - 1] != 3 && CheckKillMap[i, j] == 3 && CheckKillMap[i, j + 1] == 3 && CheckKillMap[i, j + 2] == 3 && (j == 8 || (CheckKillMap[i, j + 3] != 3 && CheckKillMap[i, j + 3] != 1)))
                    {
                        CheckKillButton[i, j].Text = CheckKillButton[i, j + 1].Text = CheckKillButton[i, j + 2].Text = "X";
                        CheckKillButton[i, j].BackColor = CheckKillButton[i, j + 1].BackColor = CheckKillButton[i, j + 2].BackColor = Color.Red;
                        CheckKillButton[i, j].FlatStyle = CheckKillButton[i, j + 1].FlatStyle = CheckKillButton[i, j + 2].FlatStyle = System.Windows.Forms.FlatStyle.Flat;
                        CheckKillButton[i, j].FlatAppearance.MouseDownBackColor = CheckKillButton[i, j + 1].FlatAppearance.MouseDownBackColor = CheckKillButton[i, j + 2].FlatAppearance.MouseDownBackColor = Color.Red;
                        CheckKillButton[i, j].FlatAppearance.MouseOverBackColor = CheckKillButton[i, j + 1].FlatAppearance.MouseOverBackColor = CheckKillButton[i, j + 2].FlatAppearance.MouseOverBackColor = Color.Red;
                        CheckKillMap[i, j] = CheckKillMap[i, j + 1] = CheckKillMap[i, j + 2] = 4;
                        richTextBox.Text += ", убил";
                        if (j == 8 || i == 10)
                        {
                            if (j == 8 && i != 10)
                            {
                                CheckKillMap[i - 1, j - 1] = CheckKillMap[i, j - 1] = CheckKillMap[i + 1, j - 1] = 5;
                                CheckKillMap[i - 1, j] = CheckKillMap[i + 1, j] = 5;
                                CheckKillMap[i - 1, j + 1] = CheckKillMap[i + 1, j + 1] = 5;
                                CheckKillMap[i - 1, j + 2] = CheckKillMap[i + 1, j + 2] = 5;
                            }
                            if (j == 8 && i == 10)
                            {
                                CheckKillMap[i - 1, j - 1] = CheckKillMap[i, j - 1] = 5;
                                CheckKillMap[i - 1, j] = CheckKillMap[i - 1, j + 1] = CheckKillMap[i - 1, j + 2] = 5;
                            }
                            if (j < 8 && i == 10)
                            {
                                CheckKillMap[i - 1, j - 1] = CheckKillMap[i, j - 1] = 5;
                                CheckKillMap[i - 1, j + 3] = CheckKillMap[i, j + 3] = 5;
                                CheckKillMap[i - 1, j] = CheckKillMap[i - 1, j + 1] = CheckKillMap[i - 1, j + 2] = 5;
                            }
                        }
                        else
                        {
                            CheckKillMap[i - 1, j] = CheckKillMap[i + 1, j] = 5;
                            CheckKillMap[i - 1, j + 1] = CheckKillMap[i + 1, j + 1] = 5;
                            CheckKillMap[i - 1, j + 2] = CheckKillMap[i + 1, j + 2] = 5;
                            CheckKillMap[i - 1, j - 1] = CheckKillMap[i, j - 1] = CheckKillMap[i + 1, j - 1] = 5;
                            CheckKillMap[i - 1, j + 3] = CheckKillMap[i, j + 3] = CheckKillMap[i + 1, j + 3] = 5;
                        }
                    }
                    // Вертикальный 2
                    if ((CheckKillMap[i - 1, j] != 1 && i < 10) && CheckKillMap[i - 1, j] != 3 && CheckKillMap[i, j] == 3 && CheckKillMap[i + 1, j] == 3 && (i == 9 || (CheckKillMap[i + 2, j] != 3 && CheckKillMap[i + 2, j] != 1)))
                    {
                        CheckKillButton[i, j].Text = CheckKillButton[i + 1, j].Text = "X";
                        CheckKillButton[i, j].BackColor = CheckKillButton[i + 1, j].BackColor = Color.Red;
                        CheckKillButton[i, j].FlatStyle = CheckKillButton[i + 1, j].FlatStyle = System.Windows.Forms.FlatStyle.Flat;
                        CheckKillButton[i, j].FlatAppearance.MouseDownBackColor = CheckKillButton[i + 1, j].FlatAppearance.MouseDownBackColor = Color.Red;
                        CheckKillButton[i, j].FlatAppearance.MouseOverBackColor = CheckKillButton[i + 1, j].FlatAppearance.MouseOverBackColor = Color.Red;
                        CheckKillMap[i, j] = CheckKillMap[i + 1, j] = 4;
                        richTextBox.Text += ", убил";
                        if (i == 9 || j == 10)
                        {
                            if (i == 9 && j != 10)
                            {
                                CheckKillMap[i - 1, j - 1] = CheckKillMap[i - 1, j] = CheckKillMap[i - 1, j + 1] = 5;
                                CheckKillMap[i, j - 1] = CheckKillMap[i, j + 1] = 5;
                                CheckKillMap[i + 1, j - 1] = CheckKillMap[i + 1, j + 1] = 5;
                            }
                            if (i == 9 && j == 10)
                            {
                                CheckKillMap[i - 1, j] = CheckKillMap[i - 1, j - 1] = 5;
                                CheckKillMap[i, j - 1] = CheckKillMap[i + 1, j - 1] = 5;
                            }
                            if (i < 9 && j == 10)
                            {
                                CheckKillMap[i - 1, j] = CheckKillMap[i - 1, j - 1] = 5;
                                CheckKillMap[i, j - 1] = CheckKillMap[i + 1, j - 1] = 5;
                                CheckKillMap[i + 2, j] = CheckKillMap[i + 2, j - 1] = 5;
                            }
                        }
                        else
                        {
                            CheckKillMap[i - 1, j - 1] = CheckKillMap[i - 1, j] = CheckKillMap[i - 1, j + 1] = 5;
                            CheckKillMap[i + 2, j - 1] = CheckKillMap[i + 2, j] = CheckKillMap[i + 2, j + 1] = 5;
                            CheckKillMap[i, j - 1] = CheckKillMap[i, j + 1] = 5;
                            CheckKillMap[i + 1, j - 1] = CheckKillMap[i + 1, j + 1] = 5;
                        }
                    }
                    // Горизонтальный 2
                    if ((CheckKillMap[i, j - 1] != 1 && j < 10) && CheckKillMap[i, j - 1] != 3 && CheckKillMap[i, j] == 3 && CheckKillMap[i, j + 1] == 3 && (j == 9 || (CheckKillMap[i, j + 2] != 3 && CheckKillMap[i, j + 2] != 1)))
                    {
                        CheckKillButton[i, j].Text = CheckKillButton[i, j + 1].Text = "X";
                        CheckKillButton[i, j].BackColor = CheckKillButton[i, j + 1].BackColor = Color.Red;
                        CheckKillButton[i, j].FlatStyle = CheckKillButton[i, j + 1].FlatStyle = System.Windows.Forms.FlatStyle.Flat;
                        CheckKillButton[i, j].FlatAppearance.MouseDownBackColor = CheckKillButton[i, j + 1].FlatAppearance.MouseDownBackColor = Color.Red;
                        CheckKillButton[i, j].FlatAppearance.MouseOverBackColor = CheckKillButton[i, j + 1].FlatAppearance.MouseOverBackColor = Color.Red;
                        CheckKillMap[i, j] = CheckKillMap[i, j + 1] = 4;
                        richTextBox.Text += ", убил";
                        if (j == 9 || i == 10)
                        {
                            if (j == 9 && i != 10)
                            {
                                CheckKillMap[i - 1, j - 1] = CheckKillMap[i, j - 1] = CheckKillMap[i + 1, j - 1] = 5;
                                CheckKillMap[i - 1, j] = CheckKillMap[i + 1, j] = 5;
                                CheckKillMap[i - 1, j + 1] = CheckKillMap[i + 1, j + 1] = 5;
                            }
                            if (j == 9 && i == 10)
                            {
                                CheckKillMap[i - 1, j - 1] = CheckKillMap[i, j - 1] = 5;
                                CheckKillMap[i - 1, j] = CheckKillMap[i - 1, j + 1] = 5;
                            }
                            if (j < 9 && i == 10)
                            {
                                CheckKillMap[i - 1, j - 1] = CheckKillMap[i, j - 1] = 5;
                                CheckKillMap[i - 1, j + 2] = CheckKillMap[i, j + 2] = 5;
                                CheckKillMap[i - 1, j] = CheckKillMap[i - 1, j + 1] = 5;
                            }
                        }
                        else
                        {
                            CheckKillMap[i - 1, j] = CheckKillMap[i + 1, j] = 5;
                            CheckKillMap[i - 1, j + 1] = CheckKillMap[i + 1, j + 1] = 5;
                            CheckKillMap[i - 1, j - 1] = CheckKillMap[i, j - 1] = CheckKillMap[i + 1, j - 1] = 5;
                            CheckKillMap[i - 1, j + 2] = CheckKillMap[i, j + 2] = CheckKillMap[i + 1, j + 2] = 5;
                        }
                    }
                    // 1
                    if (CheckKillMap[i - 1, j] != 1 && CheckKillMap[i - 1, j] != 3 && CheckKillMap[i, j - 1] != 1 && CheckKillMap[i, j - 1] != 3 && CheckKillMap[i, j] == 3 && (j == 10 || (CheckKillMap[i, j + 1] != 3 && CheckKillMap[i, j + 1] != 1)) && (i == 10 || (CheckKillMap[i + 1, j] != 3 && CheckKillMap[i + 1, j] != 1)))
                    {
                        CheckKillButton[i, j].Text = "X";
                        CheckKillButton[i, j].BackColor = Color.Red;
                        CheckKillButton[i, j].FlatStyle = System.Windows.Forms.FlatStyle.Flat;
                        CheckKillButton[i, j].FlatAppearance.MouseDownBackColor = Color.Red;
                        CheckKillButton[i, j].FlatAppearance.MouseOverBackColor = Color.Red;
                        CheckKillMap[i, j] = 4;
                        richTextBox.Text += ", убил";
                        if (j == 10 || i == 10)
                        {
                            if (j == 10 && i != 10)
                            {
                                CheckKillMap[i - 1, j] = CheckKillMap[i - 1, j - 1] = 5;
                                CheckKillMap[i + 1, j] = CheckKillMap[i + 1, j - 1] = 5;
                                CheckKillMap[i, j - 1] = 5;
                            }
                            if (j == 10 && i == 10)
                            {
                                CheckKillMap[i - 1, j] = CheckKillMap[i - 1, j - 1] = CheckKillMap[i, j - 1] = 5;
                            }
                            if (j != 10 && i == 10)
                            {
                                CheckKillMap[i - 1, j - 1] = CheckKillMap[i - 1, j] = CheckKillMap[i - 1, j + 1] = 5;
                                CheckKillMap[i, j - 1] = CheckKillMap[i, j + 1] = 5;
                            }
                        }
                        else
                        {
                            CheckKillMap[i - 1, j - 1] = CheckKillMap[i - 1, j] = CheckKillMap[i - 1, j + 1] = 5;
                            CheckKillMap[i, j - 1] = CheckKillMap[i, j + 1] = 5;
                            CheckKillMap[i + 1, j - 1] = CheckKillMap[i + 1, j] = CheckKillMap[i + 1, j + 1] = 5;
                        }
                    }
                }
            }
            richTextBox.Text += "\n";
            for (int i = 1; i < cell; i++)
            {
                for (int j = 1; j < cell; j++)
                {
                    switch (CheckKillMap[i, j])
                    {
                        case 4:
                            count4++;
                            break;
                        case 5:
                            CheckKillButton[i, j].Enabled = false;// Окрашивание клеток вокруг потопленных кораблей
                            CheckKillButton[i, j].BackColor = Color.SkyBlue;
                            CheckKillButton[i, j].FlatStyle = System.Windows.Forms.FlatStyle.Flat;
                            CheckKillButton[i, j].FlatAppearance.MouseDownBackColor = Color.SkyBlue;
                            CheckKillButton[i, j].FlatAppearance.MouseOverBackColor = Color.SkyBlue;
                            break;
                    }
                }
            }
            if (count4 == 20) // Если не осталось кораблей
            {
                Game_On = false;
                if (P1_Shot) // Был ход второго
                {
                    richTextBox.Text += "Игрок 2 победил. Конец игры";
                    for (int i = 1; i < cell; i++)
                    {
                        for (int j = 1; j < cell; j++)
                        {
                            if (P2Map[i, j] == 1)
                            {
                                P2Button[i, j].BackColor = Color.Green;
                                P2Button[i, j].FlatStyle = System.Windows.Forms.FlatStyle.Flat;
                                P2Button[i, j].FlatAppearance.MouseDownBackColor = Color.Green;
                                P2Button[i, j].FlatAppearance.MouseOverBackColor = Color.Green;

                            }
                        }
                    }
                     DialogResult result = MessageBox.Show("Игрок №2 пободил! Начать заново ? ", "Конец игры", MessageBoxButtons.YesNo);
                    if (result == DialogResult.Yes)
                    {
                        NewGame(this, new EventArgs());
                    }
                }
                if (P2_Shot) // Был ход первого
                {
                    richTextBox.Text += "Игрок 1 победил. Конец игры";
                    for (int i = 1; i < cell; i++)
                    {
                        for (int j = 1; j < cell; j++)
                        {
                            if (P1Map[i, j] == 1)
                            {
                                P1Button[i, j].BackColor = Color.Green;
                                P1Button[i, j].FlatStyle = System.Windows.Forms.FlatStyle.Flat;
                                P1Button[i, j].FlatAppearance.MouseDownBackColor = Color.Green;
                                P1Button[i, j].FlatAppearance.MouseOverBackColor = Color.Green;
                            }
                        }
                    }
                    DialogResult result = MessageBox.Show("Игрок №1 пободил! Начать заново ? ", "Конец игры", MessageBoxButtons.YesNo);
                    if (result == DialogResult.Yes)
                    {
                        NewGame(this, new EventArgs());
                    }
                }
            }
            if (P1_Shot)
            {
                for (int i = 1; i < cell; i++)
                {
                    for (int j = 1; j < cell; j++)
                    {
                        P1Map[i, j] = CheckKillMap[i, j];
                        P1Button[i, j] = CheckKillButton[i, j];
                    }
                }
            }
            if (P2_Shot)
            {
                for (int i = 1; i < cell; i++)
                {
                    for (int j = 1; j < cell; j++)
                    {
                        P2Map[i, j] = CheckKillMap[i, j];
                        P2Button[i, j] = CheckKillButton[i, j];
                    }
                }
            }
        }
        public void NewGame(object sender, EventArgs e) // "Начать заново"
        {
            DialogResult restart = MessageBox.Show("Вы уверены, что хотите начать заново? ", "Начать заново", MessageBoxButtons.YesNo);
            if (restart == DialogResult.Yes)
            {
                Restart();
            }
        }

        public void Restart() // Обнуление формы
        {
            P1_Ready = P2_Ready = P1_Shot = P2_Shot = Game_On = false;
            buttonP1ready.Enabled = true;
            buttonP2ready.Enabled = true;
            buttonStart.Enabled = false;
            richTextBox.Text = "Игра с игроком\n";
            for (int i = 0; i < cell; i++)
            {
                for (int j = 0; j < cell; j++)
                {
                    P1Map[i, j] = 0;
                    P2Map[i, j] = 0;
                    if (i != 0 && j != 0)
                    {
                        P1Button[i, j].BackColor = System.Drawing.Color.Transparent;
                        P1Button[i, j].FlatStyle = System.Windows.Forms.FlatStyle.Flat;
                        P1Button[i, j].FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
                        P1Button[i, j].FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
                        P1Button[i, j].Enabled = true;
                        P1Button[i, j].Text = "";

                        P2Button[i, j].BackColor = System.Drawing.Color.Transparent;
                        P2Button[i, j].FlatStyle = System.Windows.Forms.FlatStyle.Flat;
                        P2Button[i, j].FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
                        P2Button[i, j].FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
                        P2Button[i, j].Enabled = true;
                        P2Button[i, j].Text = "";
                    }
                }
            }
        }

        public void Exit(object sender, EventArgs e) // Выход из режима
        {
            DialogResult result = MessageBox.Show("Вы уверены, что хотите выйти из игры ? Все несохраненные результаты пропадут", "Выход из игры", MessageBoxButtons.YesNo);
            if (result == DialogResult.Yes)
            {
                this.Close();
            }
        }

        public void SaveGame(object sender, EventArgs e) // Сохранение
        {
            mapinfo = "";
            s.Append("P"); // Сохранение режима игры
            for (int i = 1; i < cell; i++) // Сохранение значений полей
            {
                for (int j = 1; j < cell; j++)
                {
                    s.Append($"{P1Map[i, j]}");
                }
            }
            for (int i = 1; i < cell; i++)
            {
                for (int j = 1; j < cell; j++)
                {
                    s.Append($"{P2Map[i, j]}");
                }
            }
            // Сохранение кнопок
            if (P1_Ready == true) s.Append("1");
            else s.Append("0");
            if (P2_Ready == true) s.Append("1");
            else s.Append("0");
            if (Game_On == true) s.Append("1");
            else s.Append("0");
            if (P1_Shot == true) s.Append("1");
            else s.Append("0");
            if (P2_Shot == true) s.Append("1");
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

        public void LoadGame(object sender, EventArgs e) // Загрузка сохранения
        {
            mapinfo = "";
            int k = 0;
            openFileDialog.Filter = "Text files(*.txt)|*.txt";
            if (openFileDialog.ShowDialog() == DialogResult.Cancel)
                return;
            string filename = openFileDialog.FileName;
            mapinfo = System.IO.File.ReadAllText(filename);
            if (mapinfo[k] != 'P')
            {
                MessageBox.Show("Не удалось загрузить сохранение. Возможно, это сохранение из другого режима", "Ошибка");
                return;
            }
            k++;
            Restart();
            for (int i = 1; i < cell; i++)
            {
                for (int j = 1; j < cell; j++)
                {
                    P1Map[i, j] = (int)(mapinfo[k] - 48);
                    switch (P1Map[i, j])
                    {
                        case 3:
                            P1Button[i, j].BackColor = Color.Orange;
                            P1Button[i, j].FlatStyle = System.Windows.Forms.FlatStyle.Flat;
                            P1Button[i, j].FlatAppearance.MouseDownBackColor = Color.Orange;
                            P1Button[i, j].FlatAppearance.MouseOverBackColor = Color.Orange;
                            P1Button[i, j].Text = "";
                            break;
                        case 4:
                            P1Button[i, j].Text = "X";
                            P1Button[i, j].BackColor = Color.Red;
                            P1Button[i, j].FlatStyle = System.Windows.Forms.FlatStyle.Flat;
                            P1Button[i, j].FlatAppearance.MouseDownBackColor = Color.Red;
                            P1Button[i, j].FlatAppearance.MouseOverBackColor = Color.Red;
                            break;
                        case 5:
                            P1Button[i, j].BackColor = Color.SkyBlue;
                            P1Button[i, j].FlatStyle = System.Windows.Forms.FlatStyle.Flat;
                            P1Button[i, j].FlatAppearance.MouseDownBackColor = Color.SkyBlue;
                            P1Button[i, j].FlatAppearance.MouseOverBackColor = Color.SkyBlue;
                            P1Button[i, j].Text = "";
                            break;
                    }
                    k++;
                }
            }
            for (int i = 1; i < cell; i++)
            {
                for (int j = 1; j < cell; j++)
                {
                    P2Map[i, j] = (int)(mapinfo[k] - 48);
                    switch (P2Map[i, j])
                    {
                        case 3:
                            P2Button[i, j].BackColor = Color.Orange;
                            P2Button[i, j].FlatStyle = System.Windows.Forms.FlatStyle.Flat;
                            P2Button[i, j].FlatAppearance.MouseDownBackColor = Color.Orange;
                            P2Button[i, j].FlatAppearance.MouseOverBackColor = Color.Orange;
                            P2Button[i, j].Text = "";
                            break;
                        case 4:
                            P2Button[i, j].Text = "X";
                            P2Button[i, j].BackColor = Color.Red;
                            P2Button[i, j].FlatStyle = System.Windows.Forms.FlatStyle.Flat;
                            P2Button[i, j].FlatAppearance.MouseDownBackColor = Color.Red;
                            P2Button[i, j].FlatAppearance.MouseOverBackColor = Color.Red;
                            break;
                        case 5:
                            P2Button[i, j].BackColor = Color.SkyBlue;
                            P2Button[i, j].FlatStyle = System.Windows.Forms.FlatStyle.Flat;
                            P2Button[i, j].FlatAppearance.MouseDownBackColor = Color.SkyBlue;
                            P2Button[i, j].FlatAppearance.MouseOverBackColor = Color.SkyBlue;
                            P2Button[i, j].Text = "";
                            break;
                    }
                    k++;
                }
            }
            if (mapinfo[k] == '1')
            {
                P1_Ready = true;
                buttonP1ready.Enabled = false;
            }
            else
            {
                P1_Ready = false;
                buttonP1ready.Enabled = true;
            }
            k++;
            if (mapinfo[k] == '1')
            {
                P2_Ready = true;
                buttonP2ready.Enabled = false;
            }
            else
            {
                P2_Ready = false;
                buttonP2ready.Enabled = true;
            }
            k++;
            if (mapinfo[k] == '1')
            {
                Game_On = true;
                buttonStart.Enabled = false;
            }
            else
            {
                Game_On = false;
                if (P1_Ready && P2_Ready) buttonStart.Enabled = true;
                else buttonStart.Enabled = false;
            }
            k++;
            if (mapinfo[k] == '1') P1_Shot = true;
            else P1_Shot = false;
            k++;
            if (mapinfo[k] == '1') P2_Shot = true;
            else P2_Shot = false;
            // Загрузка протокола и запись его в richTextBox
            richTextBox.Text = "";
            for (int i = 210; i < mapinfo.Length; i++)
            {
                richTextBox.Text += mapinfo[i];
            }
            for (int i = 1; i < cell; i++)
            {
                for (int j = 1; j < cell; j++)
                {
                    if (!P1_Ready && P1Map[i, j] == 1)
                    {
                        P1Button[i, j].BackColor = Color.Blue;
                    }

                }
            }
            for (int i = 1; i < cell; i++)
            {
                for (int j = 1; j < cell; j++)
                {
                    if (!P2_Ready && P2Map[i, j] == 1)
                    {
                        P2Button[i, j].BackColor = Color.Blue;
                    }
                }
            }
            MessageBox.Show("Игра загружена");
        }
    }
}
