using System;
using System.Threading;

namespace PlayersGameThread
{
    class Program
    {
        static object l = new object();
        //static object m = new object();
        static int n1;
        static int n2;
        static bool running = true;
        static bool movFig = true;
        static int cont = 0;
        static char[] figura = { '|', '/', '-', '\\' };
        static int i = 0;
        static Random random = new Random();

        static void p1()
        {
            while (running)
            {
                lock (l)
                {
                    if (running)
                    {
                        Console.SetCursorPosition(1, 1);
                        Console.Write("{0,15}", " ");
                        Console.SetCursorPosition(1, 1);
                        n1 = random.Next(1, 11);
                        Console.Write("PLAYER 1: " + n1);
                        if (n1 == 5 || n1 == 7)
                        {
                            if (!movFig)
                            {
                                cont += 10;
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
                            Monitor.Pulse(l);   //se estaba wait hai que despertala para marchar todas
                        }
                    }
                }
                Thread.Sleep(random.Next(100, 100 * n1));
                //Thread.Sleep(2000);                
            }
            //lock (m)
            //{                
            //    Monitor.Pulse(m);   
            //}
        }

        static void p2()
        {           
            while (running)
            {
                lock (l)
                {
                    if (running)
                    {
                        Console.SetCursorPosition(1, 3);
                        Console.Write("{0,15}", " ");
                        Console.SetCursorPosition(1, 3);
                        n2 = random.Next(1, 11);
                        Console.Write("PLAYER 2: " + n2);
                        if (n2 == 5 || n2 == 7)
                        {
                            if (movFig && cont != 0)
                            {
                                cont -= 10;
                            }
                            else
                            {
                                movFig = true;
                                cont--;
                                Monitor.Pulse(l);
                            }
                            puntos();
                        }
                        if (cont <= -20)
                        {
                            running = false;
                        }
                    }                    
                }                
                Thread.Sleep(random.Next(100, 100 * n2));
                //Thread.Sleep(3000);
            }
            //lock (m)
            //{
            //    Monitor.Pulse(m);   //libera testigo
            //}
        }

        static void dplay()
        {
            while (running)
            {
                lock (l)
                {
                    if (running)
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
                        else
                        {
                            Monitor.Wait(l);
                        }
                    }                    
                }
                Thread.Sleep(200);
            }
        }
       
        static void puntos()
        {
            Console.SetCursorPosition(9, 7);
            Console.Write("{0,10}", " ");
            Console.SetCursorPosition(9, 7);
            Console.Write(cont);
        }

        static void Main(string[] args)
        {
            Console.SetCursorPosition(1, 7);
            Console.Write("POINTS: " + cont);
            Thread player1 = new Thread(p1);
            Thread player2 = new Thread(p2);
            Thread display = new Thread(dplay);

            display.Start();            
            player2.Start();
            player1.Start();

            player1.Join();
            player2.Join();
            display.Join();

            //lock (m)
            //{
            //    while (running)
            //    {
            //        Monitor.Wait(m);
            //    }
            //}

            Console.SetCursorPosition(1, 10);
            Console.WriteLine("*** WINNER: " + (cont > 0 ? "Player 1" : "Player2") + " ***");
            Console.ReadKey();
        }
    }
}
