using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Numerics;


namespace MillerRabinTest
{
    class Program
    {
        static int Main(string[] args)
        {
            /*_________________________________6 PROGRAM_____________________________________*/
            
            /*
            int posInt = 0;
            int rounds = 4;
            bool flag = true;
            int countNumber = 10;
            int count = 0;

            string path = "Data1.txt";

            try
            {
                using (StreamReader sr = new StreamReader(path))
                {
                    rounds = sr.Read() - '0';
                    if (sr.EndOfStream) throw new Exception("File is empty");
                    countNumber = sr.Read() - '0';
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return 0;
            }

            Stopwatch stopwatch = new Stopwatch();
            // Запускаем внутренний таймер объекта Stopwatch
            stopwatch.Start();


            while (flag)
            {
                count++;

                posInt = primeNumberGenerator(countNumber);

                for (int i = 0; i < rounds; i++)
                {
                    if (!Is_prime(posInt))
                    {
                        break;
                    }
                    else if (i == rounds - 1)
                    {
                        flag = false;
                    }
                }
            }

            // Останавливаем внутренний таймер объекта Stopwatch
            stopwatch.Stop();


            Console.WriteLine("Number: " + posInt + " is prime with probability of " + (1 - Math.Pow(0.5, rounds)));
            Console.WriteLine("Iterations: " + count);
            Console.WriteLine("Time: " + stopwatch.ElapsedMilliseconds + " ms");
            return 1;

            */


            /*_________________________________7 PROGRAM_____________________________________*/

            int posInt = 0;           
            string path = "FData1.txt";

            try
            {
                using (StreamReader sr = new StreamReader(path))
                {
                    if (sr.EndOfStream) throw new Exception("File is empty");
                    posInt = Convert.ToInt32(sr.ReadLine());
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return 0;
            }

            //Console.WriteLine("Enter the number to factorize");
            //posInt = Convert.ToInt32(Console.ReadLine());
            bool prime = true;

                for (int i = 0; i < 5; i++)
                {
                    if (!Is_prime(posInt))
                    {
                        prime = false;
                        break;
                    }
                }

                if (prime)
                {
                    Console.WriteLine("The number is prime!");
                    return 0;
                }
            
            Stopwatch stopwatch = new Stopwatch();
                
            // Запускаем внутренний таймер объекта Stopwatch
                stopwatch.Start();

                List<int> fac = new List<int>();
            int k = RoPollardFactorization(posInt, ref fac);

            //Проверяем, даёт ли произведение всех найденных множителей факторизуемое число
            int check = 1;
            for (int j = 0; j < fac.Count(); j++)
                check *= fac[j];
            
            while (check != posInt)
            {
                k += RoPollardFactorization(posInt / check, ref fac);
                check = 1;
                for (int j = 0; j < fac.Count(); j++)
                { check *= fac[j]; 
                    if (!Is_prime(fac[j]))
                    {
                        k += RoPollardFactorization(fac[j], ref fac);
                        break;
                    }
                }
            }

            // Останавливаем внутренний таймер объекта Stopwatch            
            stopwatch.Stop();

                fac.Sort();

                for (int i = 0; i < fac.Count - 1; i++)
                {
                    if (fac[i] != fac[i + 1])
                    { 
                    
                    Console.WriteLine("{0}^{1}", fac[i], fac.Count(x => x == fac[i]));

                    }
                }

            Console.Write("{0}^{1}", fac[fac.Count() - 1], fac.Count(x => x == fac[fac.Count() - 1]));

            Console.WriteLine();
            Console.WriteLine("Iterations: " + k);
            Console.WriteLine("Time: " + stopwatch.ElapsedMilliseconds + " ms");
            
            return 1;

        }

        static int primeNumberGenerator(int capacity)
        {
            string number = "1";

            Random rnd = new Random();

            for (int i = 1; i < capacity - 1; i++) 
            {
                number += rnd.Next(0, 9);
            }

            number += "1";

            char[] arr;
            arr = number.ToCharArray();
            int newNum = 0;
            int dec = 1;

            for (int i= arr.Length - 1; i >= 0; i--)
            {
                newNum += (arr[i] - '0')*dec;
                dec *= 10;
            }

            return newNum;
        }


        /* Этот алгоритм рекурсивно вычисляет символ Якоби */
        static int Jacobi(int a, int b)
        {
            int g;
            Debug.Assert(Odd(b));
            if (a >= b)
                a %= b;
            if (a == 0)
                return 0;
            if (a == 1)
                return 1;

            if (a < 0)
                if (((b - 1) / 2) % 2 == 0)
                    return Jacobi(-a, b);
                else
                    return -Jacobi(-a, b);

            if (a % 2 == 0)
                if (((b * b - 1) / 8) % 2 == 0)
                    return +Jacobi(a / 2, b);
                else
                    return -Jacobi(a / 2, b);

            g = Euclidean(a, b); // g = НОД (a, b)
            Debug.Assert(Odd(a));
            if (g == a)
                return 0;
            else
                if (g != 1)
                return Jacobi(g, b) * Jacobi(a / g, b);
            else
                    if (((a - 1) * (b - 1) / 4) % 2 == 0)
                return +Jacobi(b, a);
            else
                return -Jacobi(b, a);
        }

        static int Euclidean(int a, int b)
        {
            while (a != 0 && b != 0)
            {
                if (a > b) a %= b;
                else b %= a;
            }
            return a + b;
        }

        static bool Is_prime(int n)
        {
            n = Math.Abs(n);
            if (n == 0 || n == 1 || n == 2) return true;
            if (n % 2 == 0) return false;

            Random rand = new Random();
   
            /*1.	Выбираем случайное число a из интервала [1; n – 1].*/
            
            int a = 1 + rand.Next(n - 1);

            /*2.	Проверяем с помощью алгоритма Евклида условие НОД (a, n) = 1. Если оно не выполняется, то n – составное.*/

            if (Euclidean(a, n) != 1) return false;
            /*3.	Проверяем выполнимость сравнения (2). Если оно не выполняется, то n – составное.
            Если сравнение выполнено, то ответ неизвестен (и тест можно повторить еще раз). */
            int j;

            BigInteger temp = a;
            temp = BigInteger.ModPow(temp, (n-1)/2, n);

            j = Jacobi(a, n);
            if (temp == Mod(j, n)) return true;

            return false;
        }

        static bool Odd(int b)
        {
            return (b % 2 == 0 ? false : true);
        }

        static int Mod(int a, int b)
        {
            if ((a > 0) && (b > 0)) return a % b;
            if ((a > 0) && (b < 0)) return -(Math.Abs(b) - (a % Math.Abs(b)));
            if ((a < 0) && (b > 0)) return b - Math.Abs(a) % b;
            if ((a < 0) && (b < 0)) return a % b;
            return 0;
        }

        static int RoPollardFactorization(int num, ref List<int> factors)
        {
            if ((int)Math.Sqrt(num) * (int)Math.Sqrt(num) == num)
            {
                if (Is_prime((int)Math.Sqrt(num)))
                {
                    factors.Add((int)Math.Sqrt(num));
                    factors.Add((int)Math.Sqrt(num));

                    return 0;
                }
                
            }
            BigInteger x;
            int k = 2;
            int i = 1;
            int d;
            Random rnd = new Random();
            x = rnd.Next(0, num - 1);
            BigInteger y = x;
            while (i < (int) Math.Sqrt(num)*2)
            {

                i++;
                x = BigInteger.ModPow((x * x + 1), 1, num);
                int delta = Math.Abs((int) BigInteger.Subtract(y, x));
                d = Euclidean(delta, num);

                    if (d > 1 && d < num)
                    {
                        if (!Is_prime(d))
                            RoPollardFactorization(d, ref factors);
                        else
                            factors.Add(d);

                        if (!Is_prime(num/d))
                            RoPollardFactorization(num/d, ref factors);
                        else                    
                            factors.Add(num / d);
                        
                    break;
                    
                }

                if (i == k)
                    {
                        y = x;
                        k *= 2;
                    }

            }
            return i;
        }

    }

}
