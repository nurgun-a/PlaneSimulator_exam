using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Threading;
using static System.Console;

namespace PlaneSimulator_exam
{
    class Program
    {
        static void Main(string[] args)
        {
            Plain plain = new Plain();
            
            Timer_class ts = new Timer_class();           
            Thread th1 = new Thread(ts.Timer_);
           

            Dispatcher dispatcher1 = new Dispatcher("Dispatcher 1", 0, 5000);
            Dispatcher dispatcher2 = new Dispatcher("Dispatcher 2", 5001, 10000);

            Random rand = new Random();
            Thread.Sleep(1000);
            th1.Start();

            try
            {
                while (plain.Distance < 10000)
                {

                    //Thread.Sleep(1000); 
                    Clear();
                    WriteLine("\n");
                    int n1 = rand.Next(-200, 201);
                    int n2 = rand.Next(-200, 201);

                    if (dispatcher1.Control(plain)) { Show(plain, dispatcher1); }                 
                    if (dispatcher2.Control(plain)) { Show(plain, dispatcher2); }
                    plain.OnDistanceChanged(((double)ts.Min + ((double)ts.Sec / 60)));   

                    switch (ReadKey(true).Key)
                    {
                        case ConsoleKey.LeftArrow: plain.ReduceSpeed(50); break;
                        case ConsoleKey.RightArrow: plain.IncreaseSpeed(50); break;
                        case ConsoleKey.UpArrow: plain.IncreaseHeight(50); break;
                        case ConsoleKey.DownArrow: plain.ReduceHeight(50); break;
                        case ConsoleKey.W: plain.IncreaseHeight(150); break;
                        case ConsoleKey.S: plain.ReduceHeight(150); break;
                        case ConsoleKey.A: plain.ReduceSpeed(50); break;
                        case ConsoleKey.D: plain.IncreaseSpeed(50); break;
                    }
                }
                th1.Abort();
                //plain.Land();
               
            }
            catch (Exception ex)
            {
                WriteLine(ex.Message);
                th1.Abort();
                ReadKey();
            }
            

            ReadKey();
        }
        public static void Show(Plain pl, Dispatcher d)
        {
            d.OnSpeedChanged(pl);
            WriteLine(@"
                            
                                      Скорость: " + $"{pl.Speed}" + @"
                                        Высота: " + $"{pl.Height}" + @"
                          Пойденное расстояние: " + $"{pl.Distance}" + @"  километров


                                 Штрафние очки: " + $"{pl.Penalty_points}" + @"
");
        }

        public class Timer_class
        {
            public int Min { get; set; }
            public int Sec { get; set; }
            public void Timer_()
            {
                Stopwatch sw = new Stopwatch();
                sw.Start();
                while (sw.Elapsed.Minutes != 3)
                {
                    //SetCursorPosition(0, 0);
                    //WriteLine($"{sw.Elapsed.Minutes:00}:{sw.Elapsed.Seconds:00}");
                    //SetCursorPosition(0, 0);
                    Min = sw.Elapsed.Minutes;
                    Sec = sw.Elapsed.Seconds;
                    Thread.Sleep(1000);
                    //ClearLine();
                }
                sw.Stop();
            }

        }
        public static void ClearLine()
        {
            SetCursorPosition(0, 0);
            Write(new string(' ', WindowWidth));
            SetCursorPosition(0, 0);
        }
    }
}
