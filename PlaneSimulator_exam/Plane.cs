using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using static System.Console;
using System.IO;


namespace PlaneSimulator_exam
{
    class Plain
    {
        public int Speed { get; private set; }
        public int Height { get; private set; }
        public int Penalty_points { get; set; }
        public double Distance { get; private set; }
        public delegate void ChangeParameterEventHandler(int value);
        public event ChangeParameterEventHandler SpeedChanged;
        public event ChangeParameterEventHandler HeightChanged;

        public Plain()
        {
            Speed = 0;
            Height = 0;
            Distance = 0;
            SpeedChanged += OnSpeedChanged;
            HeightChanged += OnHeightChanged;
        }
        public void Land()
        {
            if (Speed == 0 && Height == 0)
            {
                WriteLine("Ок");
            }
            else
            {
                throw new Exception("Вы не смогли прземлиться. Самолет разбился");
            }

        }

        public void IncreaseSpeed(int value)
        {
            if (Speed < 1000)
            {
                Speed += value;
                
                if (SpeedChanged != null) SpeedChanged(value);
            }
        }

        public void ReduceSpeed(int value)
        {
            if (Speed > 50)
            {
                Speed -= value;
                if (Speed < 0)
                {
                    Speed = 0;
                }
                if (SpeedChanged != null) SpeedChanged(value);              
            }
        }

        public void IncreaseHeight(int value)
        {
            if (Height < 10000)
            {
                Height += value;
                
                if (HeightChanged != null) HeightChanged(value);
            }
        }

        public void ReduceHeight(int value)
        {
            if (Height > 50)
            {
                Height -= value;
                if (Height<0)
                {
                    Height = 0;
                }
                if (HeightChanged != null) HeightChanged(value);
            }
        }

        private void OnSpeedChanged(int value)
        {
            Log($"Скорость: {Speed} км/ч, изменение скорости на {value} км/ч {DateTime.Now}");
        }

        private void OnHeightChanged(int value)
        {
            Log($"Высота: {Height} м, изменение высоты на {value} м {DateTime.Now}");
        }

        public void OnDistanceChanged(double value)
        {
            Distance += (value * Speed);
        }

        public void Log(string str)
        {
            string fPath = "Log.txt";
            using (FileStream fs = new FileStream(fPath, FileMode.Append))
            {
                using (StreamWriter sw = new StreamWriter(fs, Encoding.Unicode))
                {
                    sw.WriteLine(str);
                }               
            }
        }
    }

    class Dispatcher
    {
        private string name;
        private int controlDistanceStart;
        private int controlDistanceEnd;
        private int correction;

        public Dispatcher(string name, int controlDistanceStart, int controlDistanceEnd = 0)
        {
            this.name = name;
            this.controlDistanceStart = controlDistanceStart;
            this.controlDistanceEnd = controlDistanceEnd;
            Random rand = new Random();
            correction = rand.Next(-200, 201);
        }

        public bool Control(Plain plain)
        {
            if (plain.Distance >= controlDistanceStart && plain.Distance <= controlDistanceEnd )
            {
                return true;
            }
            else return false;
        }

        public void OnSpeedChanged(Plain plain)
        {
            int recommendedHeight = 7 * plain.Speed - correction;
            if (plain.Distance<8000)
            {
                

                int deviation = Math.Abs((recommendedHeight <= 10000 ? recommendedHeight : 10000) - plain.Height);
                if (deviation < 300)
                {
                    WriteLine($"\n\nДиспетчер {name}: Рекомендуемая высота: {(recommendedHeight <= 0 ? 0 : recommendedHeight)} м, текущая высота: {plain.Height} метров");
                }
                else if (deviation < 600)
                {
                    WriteLine($"\n\nДиспетчер {name}: Рекомендуемая высота: {(recommendedHeight <= 0 ? 0 : recommendedHeight)} м, текущая высота: {plain.Height} метров, Штраф: 25 points");
                    plain.Penalty_points += 25;
                }
                else if (deviation < 1000)
                {
                    WriteLine($"\n\nДиспетчер {name}: Рекомендуемая высота: {(recommendedHeight <= 0 ? 0 : recommendedHeight)} м, текущая высота: {plain.Height} метров, Штраф: 25 points");
                    plain.Penalty_points += 50;
                }
                else if (deviation > 1000)
                {
                    plain.Log("\n\nРазница с рекомендуемой высотой 1000 метров.Самолет разбился\n\n");
                    throw new Exception("Разница с рекомендуемой высотой 1000 метров. Самолет разбился");
                }
                if (plain.Penalty_points >= 1000)
                {
                    plain.Log("\n\nВы не справились, начислено 1000 штрафных очков.\n\n");
                    throw new Exception("Вы не справились, начислено 1000 штрафных очков.");
                }
            }

            else
            {
                recommendedHeight -= plain.Speed;

                int deviation = Math.Abs((recommendedHeight <= 10000 ? recommendedHeight : 10000) - plain.Height);
                if (deviation < 300)
                {
                    WriteLine($"\n\nДиспетчер {name}: Рекомендуемая высота: {(recommendedHeight <= 0 ? 0 : recommendedHeight)} м, текущая высота: {plain.Height} метров");
                }
                else if (deviation < 600)
                {
                    WriteLine($"\n\nДиспетчер {name}: Рекомендуемая высота: {(recommendedHeight <= 0 ? 0 : recommendedHeight)} м, текущая высота: {plain.Height} метров, Штраф: 25 points");
                    plain.Penalty_points += 25;
                }
                else if (deviation < 1000)
                {
                    WriteLine($"\n\nДиспетчер {name}: Рекомендуемая высота: {(recommendedHeight <= 0 ? 0 : recommendedHeight)} м, текущая высота: {plain.Height} метров, Штраф: 25 points");
                    plain.Penalty_points += 50;
                }
                else if (deviation > 1000)
                {
                    plain.Log("\n\nРазница с рекомендуемой высотой 1000 метров.Самолет разбился\n\n");
                    throw new Exception("Разница с рекомендуемой высотой 1000 метров. Самолет разбился");
                }
                if (plain.Penalty_points >= 1000)
                {
                    plain.Log("\n\nВы не справились, начислено 1000 штрафных очков.\n\n");
                    throw new Exception("Вы не справились, начислено 1000 штрафных очков.");
                }
            }
            
        }

    }
}

