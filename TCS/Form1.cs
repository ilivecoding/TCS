using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Windows.Forms;

namespace TCS
{
    public partial class Form1 : Form
    {
        private int score = 0;  // 得分
        private int snakeLength = 2;  // 贪吃蛇初始长度
        private int snakeSize = 20;  // 贪吃蛇每个方块的大小
        private int foodX, foodY;  // 食物的位置
        private List<Point> snake = new List<Point>();  // 贪吃蛇的位置集合
        private Direction direction = Direction.Right;  // 贪吃蛇的移动方向
        private Random random = new Random();  // 随机数生成器
        private Stopwatch stopwatch = new Stopwatch();

        public Form1()
        {
            InitializeComponent();
            Text = "TCS";
            MaximizeBox = false;
            StartPosition = FormStartPosition.CenterScreen;
            pictureBox1.Width = snakeSize * 20;
            pictureBox1.Height = snakeSize * 10;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            timer1.Tick += timer1_Tick;
            pictureBox1.Paint += pictureBox1_Paint;
            KeyDown += Form1_KeyDown;
            InitGame();
        }

        // 初始化游戏
        private void InitGame()
        {
            // 初始化贪吃蛇位置和方向
            snake.Clear();
            for (int i = 0; i < snakeLength; i++)
            {
                snake.Add(new Point(i, 0));
            }
            direction = Direction.Right;
            // 随机生成食物位置
            GenerateFood();
            // 得分
            ShowScore();
            // 开始定时器
            timer1.Interval = 300;
            timer1.Start();
            stopwatch.Start();
        }

        private void ShowScore()
        {
            label1.Text = "得分：" + score;
        }

        // 生成食物
        private void GenerateFood()
        {
            foodX = random.Next(pictureBox1.Width / snakeSize) * snakeSize;
            foodY = random.Next(pictureBox1.Height / snakeSize) * snakeSize;
        }

        // 定时器事件
        private void timer1_Tick(object sender, EventArgs e)
        {
            //定时加速
            var time = stopwatch.ElapsedMilliseconds;
            if (time > 10000)
            {
                if (timer1.Interval - 100 <= 0)
                {
                    timer1.Interval = 10;
                }
                else
                {
                    timer1.Interval -= 100;
                }
                stopwatch.Restart();
            }
            
            // 移动贪吃蛇
            MoveSnake();
            // 判断是否吃到食物
            if (snake[0].X == foodX && snake[0].Y == foodY)
            {
                snakeLength++;
                score++;
                ShowScore();
                GenerateFood();
            }
            // 判断是否撞墙或者撞到自己
            if (snake[0].X < 0 || snake[0].X >= pictureBox1.Width / snakeSize ||
                snake[0].Y < 0 || snake[0].Y >= pictureBox1.Height / snakeSize ||
                snake.Skip(1).Any(p => p == snake[0]))
            {
                // 游戏结束
                timer1.Stop();
                ShowScore();
                MessageBox.Show("游戏结束！");
                InitGame();
            }
            // 刷新游戏区域
            pictureBox1.Invalidate();
        }

        // 移动贪吃蛇
        private void MoveSnake()
        {
            // 移动身体
            for (int i = snake.Count - 1; i > 0; i--)
            {
                snake[i] = snake[i - 1];
            }
            // 移动头部
            switch (direction)
            {
                case Direction.Left:
                    snake[0] = new Point(snake[0].X - 1, snake[0].Y);
                    break;
                case Direction.Right:
                    snake[0] = new Point(snake[0].X + 1, snake[0].Y);
                    break;
                case Direction.Up:
                    snake[0] = new Point(snake[0].X, snake[0].Y - 1);
                    break;
                case Direction.Down:
                    snake[0] = new Point(snake[0].X, snake[0].Y + 1);
                    break;
            }
        }

        // 绘制贪吃蛇和食物
        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.Clear(Color.White);
            e.Graphics.FillRectangle(Brushes.Red, foodX, foodY, snakeSize, snakeSize);
            for (int i = 0; i < snake.Count; i++)
            {
                e.Graphics.FillRectangle(Brushes.Green, snake[i].X * snakeSize, snake[i].Y * snakeSize, snakeSize, snakeSize);
            }
        }

        // 键盘事件
        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Left:
                    if (direction != Direction.Right)
                    {
                        direction = Direction.Left;
                    }
                    break;
                case Keys.Right:
                    if (direction != Direction.Left)
                    {
                        direction = Direction.Right;
                    }
                    break;
                case Keys.Up:
                    if (direction != Direction.Down)
                    {
                        direction = Direction.Up;
                    }
                    break;
                case Keys.Down:
                    if (direction != Direction.Up)
                    {
                        direction = Direction.Down;
                    }
                    break;
            }
        }
    }

    // 贪吃蛇的移动方向
    public enum Direction
    {
        Left,
        Right,
        Up,
        Down
    }
}
