using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Reflection;

namespace MovingObject
{
    //public partial class MainWindow : Form
    //{
    //    //список для координат
    //    List<MovingObj> MOList = new List<MovingObj>();
    //    public MainWindow()
    //    {
    //        InitializeComponent();

    //        //таймер для реализации движения
    //        Timer t = new Timer();
    //        t.Tick += T_Tick;
    //        t.Interval = 10;
    //        t.Start();

    //        //для плавного движения объектов на форме
    //        typeof(Panel).InvokeMember("DoubleBuffered", BindingFlags.SetProperty | BindingFlags.Instance | BindingFlags.NonPublic, null, this, new object[] { true } );
    //    }


    //    //нажали на кнопку и на форме появился новый шарик, к этому шарику привязана задача
    //    public void StartBtn_Click(object sender, EventArgs e)
    //    {
    //        var SomeNewTask = Task.Run(() => SomeActionWithObject());
    //    }

    //    public async Task SomeActionWithObject()
    //    {
    //        //создание объекта
    //        //добавление объекта в список MOList
    //        //выполнение каких-то действий с объектом (без отрисовки);
    //        //например, бесконечный цикл с определением новых координат объекта

    //        //чтобы программа не зависала
    //        await Task.Delay(100);
    //    }


    public partial class MainWindow : Form
    {
        public MainWindow()
        {
            InitializeComponent();

            numericUpDown1.Value = pictureBox1.Width;
            numericUpDown2.Value = pictureBox1.Height;

            //таймер для реализации движения
            
            timer1.Tick += Tick;
            timer1.Interval = 10;
            timer1.Start();


            //для плавного движения объектов на форме
            typeof(Panel).InvokeMember("DoubleBuffered", BindingFlags.SetProperty | BindingFlags.Instance | BindingFlags.NonPublic, null, this, new object[] { true } );

        }

        // список шаров
        readonly List<BALL> balllist = new List<BALL>(); 
        int size;
        readonly Random rand = new Random(DateTime.Now.Millisecond);
        readonly Timer timer1 = new Timer();

        /// <summary>
        /// Контейнерный класс, который содержит коллекцию объектов (шаров)
        /// </summary>
        public class BALL
        {
            public int X, Y; // Координаты
            public int R; // Радиус (половина размера)
            public int dX, dY; // Путь за 1 тик таймера

            public Random r = new Random();


            /// <summary>
            /// Рисование
            /// </summary>
            public void DrawBall(PaintEventArgs e)
            {
                Graphics rg = e.Graphics;
                rg.FillEllipse(Brushes.LimeGreen, X - (R / 2), Y - (R / 2), R, R);
            }

            /// <summary>
            /// Рассчет траектории
            /// </summary>
            /// <param name="width">Ширина контейнера</param>
            /// <param name="height">Высота контейнера</param>
            /// <param name="b_size">Размер шарика</param>
            public void Trajectory(int width, int height, int b_size)
            {


                // Если координата Х меньше радиуса, меняем направление движения по оси Х на противоположное
                if (X <= (b_size / 2) || (X >= width - (b_size / 2))) 
                    dX = -dX; 
                // Если координата У меньше радиуса, меняем направление движения по оси У на противоположное
                if (Y <= (b_size / 2) || (Y >= height - (b_size / 2))) 
                    dY = -dY; 

                // прибавляем к текущей координате изменение пути
                X += dX;
                Y += dY;
            }


            /// <summary>
            /// Заполнение данных о шаре 
            /// </summary>
            /// <param name="Razm">Размер шара</param>
            /// <param name="rand">Рандом</param>
            public BALL(int Razm, Random rand)
            {
                R = Razm; // Размер шара        
                X = rand.Next(R, 700); // Координаты центра
                Y = rand.Next(R, 300);
                dX = rand.Next(1, 10); // Путь за один tick таймера (в пикселях)
                dY = rand.Next(1, 10);
            }

        }

        /// <summary>
        /// нажали на кнопку и на форме появился новый шарик, к этому шарику привязана задача
        /// </summary>
        public void StartBtn_Click(object sender, EventArgs e)
        {
            var NewBall = Task.Run(() => AddBalls());
            label7.Text = Convert.ToString(balllist.Count()+1);
        }

        /// <summary>
        /// добавление объекта в список balllist
        /// </summary>
        public async Task AddBalls()
        {
            //создание объекта
            //выполнение каких-то действий с объектом (без отрисовки);
            //например, бесконечный цикл с определением новых координат объекта

            balllist.Add(new BALL(Convert.ToInt32(numericUpDown4.Value), rand));
      
            //чтобы программа не зависала
            await Task.Delay(100);
        }

        /// <summary>
        /// Обработчик кнопки Обновить
        /// </summary>
        private void button1_Click(object sender, EventArgs e)
        {
            //определяем размер шара согласно введённым данным
            size = Convert.ToInt32(numericUpDown4.Value);

            // Очищаем список шаров
            balllist.Clear(); 

            //определяем размер контейнера согласно введённым данным
            pictureBox1.Width = Convert.ToInt32(numericUpDown1.Value);
            pictureBox1.Height = Convert.ToInt32(numericUpDown2.Value);


            
        }

        /// <summary>
        /// таймер сообщает, что данные на форме устарели
        /// </summary>
        private void Tick(object sender, EventArgs e)
        {
            // Метод проверки соударения со стенками
            foreach (BALL ball in balllist)
                ball.Trajectory(pictureBox1.Width, pictureBox1.Height, size);
            // Перерисовка
            pictureBox1.Invalidate(); 
        }

        /// <summary>
        /// Отрисовка всех объектов из balllist (обновление формы с помощью e.Graphics)
        /// </summary>
        private void Paint_ball(object sender, PaintEventArgs e)
        {
            //не рекомендуется использвать специально созданные элементы управления
            foreach (BALL ball in balllist)
                ball.DrawBall(e); // Само рисование
        }

        /// <summary>
        /// Скорость шарика
        /// </summary>
        private void Speed(object sender, EventArgs e)
        {
            
            timer1.Interval = (trackBar1.Maximum + 1 - trackBar1.Value) * 10; // Скорость
        }

        private void MainWindow_SizeChanged(object sender, EventArgs e)
        {
            Control control = new Control();
            control.Width = pictureBox1.Width + 10;
            control.Height = pictureBox1.Height + 20;
        }
    }
}
