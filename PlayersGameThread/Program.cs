using System;
using System.Threading;

namespace PlayersGameThread
{
    class Program
    {
        static object l = new object();
        static int n1;
        static int n2;
        static bool running = true;
        static bool movFig = true;
        static int cont = 0;
        static char[] figura = { '|', '/', '-', '\\' };
        static int i = 0;

        static void p1()
        {
            while (running)
            {
                Random random1 = new Random();
                lock (l)
                {
                    if (running)
                    {
                        if (n1 == 10)
                        {
                            Console.SetCursorPosition(1, 1);
                            Console.Write("{0,15}", " ");
                        }
                        Console.SetCursorPosition(1, 1);
                        n1 = random1.Next(1, 11);
                        Console.Write("PLAYER 1: " + n1);
                        if (n1 == 5 || n1 == 7)
                        {
                            if (!movFig)
                            {
                                cont += 5;
                            }
                            else
                            {
                                movFig = false;
                                cont++;
                            }
                            puntos();
                        }
                        if (cont >= 20)
                        {
                            running = false;
                            Monitor.Pulse(l);
                        }
                    }
                }
                Thread.Sleep(random1.Next(100, 100 * n1));
                //Thread.Sleep(2000);
            }
        }

        static void p2()
        {
            while (running)
            {
                Random ramdon2 = new Random();
                lock (l)
                {
                    if (running)
                    {
                        if (n2 == 10)
                        {
                            Console.SetCursorPosition(1, 3);
                            Console.Write("{0,15}", " ");
                        }
                        Console.SetCursorPosition(1, 3);
                        n2 = ramdon2.Next(1, 11);
                        Console.Write("PLAYER 2: " + n2);
                        if (n2 == 5 || n2 == 7)
                        {
                            if (movFig && cont != 0)
                            {
                                cont -= 5;
                            }
                            else
                            {
                                movFig = true;
                                cont--;
                            }
                            puntos();
                        }
                        if (cont <= -20)
                        {
                            running = false;
                            Monitor.Pulse(l);
                        }
                    }
                }
                Thread.Sleep(ramdon2.Next(100, 100 * n2));
                //Thread.Sleep(1000);
            }
        }

        static void dplay()
        {
            while (running)
            {
                lock (l)
                {
                    if (movFig)
                    {
                        Console.SetCursorPosition(20, 2);
                        Console.Write(figura[i]);
                        i++;
                        if (i == figura.Length)
                        {
                            i = 0;
                        }
                    }
                }
                Thread.Sleep(200);
            }
        }

        static void puntos()
        {
            lock (l)
            {
                Console.SetCursorPosition(1, 7);
                Console.Write("{0,20}", " ");
                Console.SetCursorPosition(1, 7);
                Console.Write("POINTS: " + cont);
            }
        }

        static void Main(string[] args)
        {
            Thread player1 = new Thread(p1);
            Thread player2 = new Thread(p2);
            Thread display = new Thread(dplay);
            display.Start();
            display.Priority = ThreadPriority.Highest;
            player1.Start();
            player2.Start();
            puntos();

            lock (l)
            {
                while (running)
                {
                    Monitor.Wait(l);
                }
            }
            Console.SetCursorPosition(1, 10);
            Console.WriteLine("*** WINNER: " + (cont > 0 ? "Player 1" : "Player2") + " ***");
            Console.ReadKey();
        }
    }
}
