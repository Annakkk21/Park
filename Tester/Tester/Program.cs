using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tester
{
    class Program
    {
        static void Main(string[] args)
        {
            List<TimeSpan> startTimesList = new List<TimeSpan>();
            Console.WriteLine($"");

            Console.Write($"Введите время начала рабочего дня:");
            string timestart = Console.ReadLine();

            string hourstart = timestart.ToString().Substring(0, 2);
            int hourintstart = Convert.ToInt32(hourstart);
            string minutesstart = timestart.ToString().Substring(3, 2);
            int minutesintstart = Convert.ToInt32(minutesstart);
            TimeSpan beginWorkingTime = new TimeSpan(hourintstart, minutesintstart, 0);

            Console.WriteLine($"");

            Console.Write($"Введите время конца рабочего дня: ");
            string timeend = Console.ReadLine();

            string hourend = timeend.ToString().Substring(0, 2);
            int hourintend = Convert.ToInt32(hourend);
            string minutesend = timeend.ToString().Substring(3, 2);
            int minutesintend = Convert.ToInt32(minutesend);
            TimeSpan endWorkingTime = new TimeSpan(hourintend, minutesintend, 0);



            Console.WriteLine($"");

            Console.Write($"Введите количество временных промежутков: ");
            int count = Convert.ToInt32(Console.ReadLine());
            List<int> durationsList = new List<int>();

            Console.WriteLine($"");

            Console.WriteLine($"Введите время в формате hh:mm, а после время занятости в минутах:");
            for (int i = 0; i < count; i++)
            {
                Console.Write($"Время: ");
                string time = Console.ReadLine();

                Console.Write($"Время занятости (минуты): ");


                durationsList.Add(Convert.ToInt32(Console.ReadLine()));
                Console.WriteLine($"");
                string hour = time.ToString().Substring(0, 2);

                int hourint = Convert.ToInt32(hour);
                string minutes = time.ToString().Substring(3, 2);
                int minutesint = Convert.ToInt32(minutes);
                TimeSpan timespam = new TimeSpan(hourint, minutesint, 0);

                startTimesList.Add(timespam);

            }



            TimeSpan[] startTimes = new TimeSpan[startTimesList.Count];
            int[] durations = new int[durationsList.Count];

            for (int i = 0; i < startTimesList.Count; i++)
            {
                startTimes[i] = startTimesList[i];
                durations[i] = durationsList[i];
            }

            Console.Write($"Введите время консультации в минутах: ");
            int consultationTime = Convert.ToInt32(Console.ReadLine());

            Console.WriteLine($"");

            Console.WriteLine($"startTime | duration");
            for (int i = 0; i < startTimesList.Count; i++)
            {
                string res = startTimes[i].ToString().Substring(0, 5);
                Console.WriteLine($"{res} {durations[i]}");
            }

            string beginWorkingTimeRES = beginWorkingTime.ToString().Substring(0, 5);
            string endWorkingTimeRES = endWorkingTime.ToString().Substring(0, 5);
            Console.WriteLine($"Working Times");
            Console.WriteLine($"{beginWorkingTimeRES} - {endWorkingTimeRES}");
            Console.WriteLine($"Consultation Time");
            Console.WriteLine($"{consultationTime}");
            Console.WriteLine($"");


            Console.ReadKey();

            string[] itog;

            itog = AvailablePeriods(startTimes, durations, beginWorkingTime, endWorkingTime, consultationTime);

            for (int i = 0; i < itog.Length; i++)
            {
                Console.WriteLine($"{itog[i]}");
            }
            Console.WriteLine($"");
            Console.ReadKey();




           


            Console.ReadKey();











        }


        public static string[] AvailablePeriods(TimeSpan[] startTimes, int[] durations, TimeSpan beginWorkingTime, TimeSpan endWorkingTime, int consultationTime)
        {
            try
            {
                List<string> rez = new List<string>();
                TimeSpan consultationTimeSpan = new TimeSpan(0, consultationTime, 0); // Промежуток консультации
                TimeSpan OneMin = new TimeSpan(0, 1, 0); // 1 минута для проверки
                TimeSpan ProverkaTimeSpan = new TimeSpan(); // промежуточная переменная для проверки

                ProverkaTimeSpan = beginWorkingTime;
                int triger = -1;

                if (ProverkaTimeSpan == startTimes[0])
                {
                    TimeSpan durationsTimeSpan = new TimeSpan(0, durations[0], 0);
                    ProverkaTimeSpan = startTimes[0] + durationsTimeSpan;
                }

                for (int iX = 0; iX != triger; iX++)
                {
                    for (int i = 0; i < consultationTime; i++)
                    {
                        ProverkaTimeSpan = ProverkaTimeSpan + OneMin;
                        for (int j = 0; j < startTimes.Length; j++)
                        {
                            if (ProverkaTimeSpan == startTimes[j] && i == consultationTime - 1)
                            {
                                rez.Add(ProverkaTimeSpan - consultationTimeSpan + "-" + ProverkaTimeSpan);
                                TimeSpan durationsTimeSpan = new TimeSpan(0, durations[j], 0);
                                ProverkaTimeSpan = startTimes[j] + durationsTimeSpan;
                                i = -1;
                            }
                            else if (ProverkaTimeSpan == startTimes[j] && i != consultationTime - 1)
                            {
                                TimeSpan durationsTimeSpan = new TimeSpan(0, durations[j], 0);
                                ProverkaTimeSpan = startTimes[j] + durationsTimeSpan;
                                i = -1;
                            }
                            else if (ProverkaTimeSpan != startTimes[j] && i == consultationTime - 1 && iX != -2 && ProverkaTimeSpan < endWorkingTime)
                            {
                                rez.Add(ProverkaTimeSpan - consultationTimeSpan + "-" + ProverkaTimeSpan);
                                i = -1;
                            }

                            if (ProverkaTimeSpan == endWorkingTime && i == consultationTime - 1)
                            {
                                rez.Add(ProverkaTimeSpan - consultationTimeSpan + "-" + ProverkaTimeSpan);

                                iX = -2;
                                i = -1;
                            }
                            else if (ProverkaTimeSpan == endWorkingTime && i != consultationTime - 1)
                            {
                                iX = -2;

                                i = -1;
                            }
                            else if(ProverkaTimeSpan > endWorkingTime)
                            {
                                iX = -2;
                            }
                        }
                    }
                }

                string[] result = new string[rez.Count];
                for (int i = 0; i < rez.Count; i++)
                {
                    result[i] = rez[i];
                }

                if (rez.Count == 0)
                {
                    string[] nuls = new string[1];
                    nuls[0] = "Null";
                    return nuls;
                }
                return result;
            }
            catch
            {
                string[] result = new string[1];
                result[0] = "Error!";
                return result;
            }
        }
    }
}

