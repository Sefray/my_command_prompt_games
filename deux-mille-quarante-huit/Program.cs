using System;
using System.Collections.Generic;
using System.Threading;

namespace deux_mille_quarante_huit
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            ConsoleKey key = Console.ReadKey().Key;
            Game(key == ConsoleKey.I);
        }

        public static void Game(bool ai_mode)
        {
            int[,] tab = Init_Tab();
            int point = 0;
            int max = 0;

            List<Tuple<int, int>> tmp_dispo = new List<Tuple<int, int>>();
            
            Place(tab,tmp_dispo);
            while (Modifiable(tab))
            {
                
                Tuple<bool, int> moved = new Tuple<bool, int>(false,0);
                while (!moved.Item1)
                {
                    Show(tab,point);
                    ConsoleKey pressed;
                    if (ai_mode)
                        pressed = IA_Maxi(tab);
                    else
                        pressed = Console.ReadKey().Key;

                    switch (pressed)
                    {
                        case ConsoleKey.UpArrow:
                            moved = MoveUp(tab);
                            break;
                        case ConsoleKey.DownArrow:
                            moved = MoveDown(tab);
                            break;
                        case ConsoleKey.LeftArrow:
                            moved = MoveLeft(tab);
                            break;
                        case ConsoleKey.RightArrow:
                            moved = MoveRight(tab);
                            break;
                    }
                }

                point += moved.Item2;
                Place(tab,tmp_dispo);
                Thread.Sleep(200);
            }
            
            Show(tab,point);
            Console.WriteLine("End of your party");
            Console.ReadKey();
        }

        public static void Place(int[,] tab, List<Tuple<int, int>> tmp_dispo)
        {
            Disponible(tab, tmp_dispo);
            Thread.Sleep(5);
            int tmp = new Random().Next(tmp_dispo.Count);
            tab[tmp_dispo[tmp].Item2,tmp_dispo[tmp].Item1] = 0;
        }

        public static void Show(int[,] tab, int point)
        {
            Console.Clear();
            int x = 2;
            int y = 2;
            Console.ForegroundColor = ConsoleColor.DarkGray;
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    Console.ResetColor();
                    Console.BackgroundColor = Color(tab[j, i]);
                    for (int k = 0; k < 2; k++)
                    {
                        Console.SetCursorPosition(i*4+2,j*3+2+k);
                        if (k == 0 && tab[j,i] != -1)
                            Console.Write("{0}{1} ",(tab[j,i]/10).ToString(),(tab[j,i]%10).ToString());
                        else
                            Console.Write("   ");
                    }
                }
            }
            
            Console.ResetColor();
            Console.SetCursorPosition(2,15);
            Console.WriteLine("You have {0} point",point);
        }
        public static ConsoleColor Color(int n)
        {
            switch (n)
            {
                case -1:
                    return (ConsoleColor)7;
                case 0:
                    return (ConsoleColor) 14;
                case 1:
                    return (ConsoleColor) 6;
                case 2:
                    return (ConsoleColor) 12;
                case 3:
                    return (ConsoleColor) 4;
                case 4:
                    return (ConsoleColor) 5;
                case 5:
                    return (ConsoleColor) 13;
                case 6:
                    return (ConsoleColor) 1;
                case 7:
                    return (ConsoleColor) 9;
                case 8:
                    return (ConsoleColor) 3;
                case 9:
                    return (ConsoleColor) 11;
                case 10:
                    return (ConsoleColor) 10;
                case 11:
                    return (ConsoleColor) 2;
            }
            return ConsoleColor.White;
        }
        
        
        //Move
        public static Tuple<bool,int> MoveUp(int[,] tab)
        {
            int tmp_point = 0;
            bool tmp = false;
            for (int i =0; i <4; i++) 
            {
                for (int j = 0; j < 4; j++)
                {
                    if (j != 3 && tab[j, i] == tab[j + 1, i] && tab[j,i] != -1)
                    {
                        tab[j, i]++;
                        tab[j + 1, i] = -1;
                        tmp_point += (int) Math.Pow(2,tab[j, i]);
                    }
                    
                    if (tab[j,i] != -1)
                        continue;

                    int k = j + 1;
                    while (k < 4 && tab[k,i] == -1)
                    {
                        k++;
                    }

                    if (k == 4)
                        break;

                    tmp = true;
                    tab[j, i] = tab[k, i];
                    tab[k, i] = -1;
                
                    if (j != 0 && tab[j, i] == tab[j - 1, i] && tab[j,i] != -1)
                    {
                        tab[j, i] = -1;
                        tab[j - 1, i]++;
                        tmp_point += (int) Math.Pow(2,tab[j-1, i]);
                    }
                }
            }

            return new Tuple<bool, int>(tmp,tmp_point);

        }
        public static Tuple<bool, int> MoveDown(int[,] tab)
        {
            int tmp_point = 0;
            bool tmp = false;
            
            for (int i =0; i <4; i++) 
            {
                for (int j = 3; j > -1; j--)
                {
                    if (j != 0 && tab[j, i] == tab[j - 1, i] && tab[j,i]!=-1)
                    {
                        tab[j, i]++;
                        tab[j - 1, i] = -1;
                        tmp_point += (int) Math.Pow(2,tab[j, i]);
                    }
                    
                    if (tab[j,i] != -1)
                        continue;

                    int k = j - 1;
                    while (k > -1 && tab[k,i] == -1)
                    {
                        k--;
                    }

                    if (k == -1)
                        break;

                    tmp = true;
                    tab[j, i] = tab[k, i];
                    tab[k, i] = -1;
                
                    if (j != 3 && tab[j, i] == tab[j + 1, i] && tab[j,i]!=-1)
                    {
                        tab[j+1, i]++;
                        tab[j, i] = -1;
                        tmp_point += (int) Math.Pow(2,tab[j+1, i]);
                    }
                    
                }
            }

            return new Tuple<bool, int>(tmp,tmp_point);
        }
        public static Tuple<bool, int> MoveRight(int[,] tab)
        { 
            
            int tmp_point = 0;
            bool tmp = false;
            
            for (int i =0; i <4; i++) 
            {
                for (int j = 3; j > -1; j--)
                {
                    if (j != 0 && tab[i, j] == tab[i, j-1] && tab[i,j]!=-1)
                    {
                        tab[i, j]++;
                        tab[i,j-1] = -1;
                        tmp_point += (int) Math.Pow(2,tab[j, i]);
                    }
                    
                    if (tab[i,j] != -1)
                        continue;

                    int k = j - 1;
                    while (k > -1 && tab[i,k] == -1)
                    {
                        k--;
                    }

                    if (k == -1)
                        break;

                    tmp = true;
                    tab[i,j] = tab[i,k];
                    tab[i,k] = -1;
                
                    if (j != 3 && tab[i,j] == tab[i, j+1] && tab[i,j]!=-1)
                    {
                        tab[i, j+1]++;
                        tab[i,j] = -1;
                        tmp_point +=(int)Math.Pow(2,tab[i,j+1]);
                    }
                }
            }

            return new Tuple<bool, int>(tmp,tmp_point);
        }
        public static Tuple<bool, int> MoveLeft(int[,] tab)
        {
            int tmp_point = 0;
            bool tmp = false;
            
            for (int i =0; i <4; i++) 
            {
                for (int j =0; j <4; j++)
                {
                    if (j != 3 && tab[i, j] == tab[i, j+1] && tab[i,j]!=-1)
                    {
                        tab[i, j]++;
                        tab[i,j+1] = -1;
                        tmp_point += (int) Math.Pow(2,tab[j, i]);
                    }
                    
                    if (tab[i,j] != -1)
                        continue;

                    int k = j + 1;
                    while (k <4 && tab[i,k] == -1)
                    {
                        k++;
                    }

                    if (k == 4)
                        break;

                    tmp = true;
                    tab[i,j] = tab[i,k];
                    tab[i,k] = -1;
                
                    if (j != 0 && tab[i,j] == tab[i, j-1] && tab[i,j]!=-1)
                    {
                        tab[i, j-1]++;
                        tab[i,j] = -1;
                        tmp_point += (int) Math.Pow(2,tab[i,j-1]);
                    }
                }
            }

            return new Tuple<bool, int>(tmp,tmp_point);
        }

        
        
        public static void Disponible(int[,] tab,List<Tuple<int, int>> tmp_dispo)
        {
            tmp_dispo.Clear();
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    if (tab[j,i] == -1)
                        tmp_dispo.Add(new Tuple<int, int>(i,j));
                }
            }
        }
        public static int[,] Init_Tab()
        {
            int[,] tab = new int[4,4];
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    tab[j, i] = -1;
                }
            }
            return tab;
        }
        
        
        
        public static bool Modifiable(int[,] tab)
        {
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    if (tab[j, i] == -1)
                        return true;
                }
            }
            return Modifiable_Horizontale(tab) || Modifiable_Verticale(tab);
        }
        public static bool Modifiable_Horizontale(int[,] tab)
        {
            for (int i = 0; i < 4; i++)
            {
                for (int j = 1; j < 3; j++)
                {
                    if (tab[j, i] == -1)
                        return true;
                    if (tab[j - 1, i] == tab[j, i] || tab[j + 1, i] == tab[j, i]) 
                        return true;
                }
            }

            return false;
        }
        public static bool Modifiable_Verticale(int[,] tab)
        {

            for (int i = 1; i < 3; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    if (tab[j, i] == -1)
                        return true;
                    
                    if (tab[j, i-1] == tab[j, i] || tab[j , i+1] == tab[j, i]) 
                        return true;
                }
            }
            return false;
        }



        public static int[,] Copy(int[,] tab)
        {
            int[,] copy = new int[4,4];
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    copy[j, i] = tab[j, i];
                }
            }

            return copy;
        }
        public static ConsoleKey IA_Maxi(int[,] tab)
        {
            int max = -1;
            ConsoleKey key = ConsoleKey.DownArrow;
            Tuple<bool, int> moved = MoveDown(Copy(tab));

            if (moved.Item1 && moved.Item2 > max)
            {
                key = ConsoleKey.DownArrow;
                max = moved.Item2;
            }
            moved = MoveUp(Copy(tab));
            if (moved.Item1 && moved.Item2 > max)
            {
                key = ConsoleKey.UpArrow;
                max = moved.Item2;
            }
            moved = MoveRight(Copy(tab));
            if (moved.Item1 && moved.Item2 > max)
            {
                key = ConsoleKey.RightArrow;
                max = moved.Item2;
            }
            moved = MoveLeft(Copy(tab));
            if (moved.Item1 && moved.Item2 > max)
            {
                key = ConsoleKey.LeftArrow;
            }

            return key;

        }


        public static ConsoleKey IA_Angle(int[,] tab)
        {
            int max = -1;
            Tuple<int,int> tmp = new Tuple<int, int>(0,0);
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    if (tab[i, j] > max)
                    {
                        tmp = new Tuple<int, int>(j,i);
                        max = tab[i, j];
                    }
                }
            }


            if (tmp.Item1 == 0 || tmp.Item1 == 1)
            {
                if (tmp.Item2 == 0 || tmp.Item2 == 1)
                    return IA_AngleHG(tab);
                return IA_AngleBG(tab);
            }
            if (tmp.Item2 == 0 || tmp.Item2 == 1)
                return IA_AngleHD(tab);
            return IA_AngleBD(tab);
        }
        public static ConsoleKey IA_AngleBG(int[,] tab)
        {
            int max = -1;
            ConsoleKey key = ConsoleKey.A;
            Tuple<bool, int> moved = MoveDown(Copy(tab));

            if (moved.Item1)
            {
                key = ConsoleKey.DownArrow;
                max = moved.Item2;
            }
            moved = MoveLeft(Copy(tab));
            if (moved.Item1 && moved.Item2 > max)
            {
                key = ConsoleKey.LeftArrow;
                max = moved.Item2;
            }

            if (key != ConsoleKey.A)
                return key;
            
            moved = MoveRight(Copy(tab));
            if (moved.Item1 && moved.Item2 > max)
            {
                key = ConsoleKey.RightArrow;
                max = moved.Item2;
            }
            moved = MoveUp(Copy(tab));
            if (moved.Item1 && moved.Item2 > max)
            {
                key = ConsoleKey.UpArrow;
            }
            return key;

        }
        public static ConsoleKey IA_AngleHG(int[,] tab)
        {
            int max = -1;
            ConsoleKey key = ConsoleKey.A;
            Tuple<bool, int> moved = MoveUp(Copy(tab));

            if (moved.Item1)
            {
                key = ConsoleKey.UpArrow;
                max = moved.Item2;
            }
            moved = MoveLeft(Copy(tab));
            if (moved.Item1 && moved.Item2 > max)
            {
                key = ConsoleKey.LeftArrow;
                max = moved.Item2;
            }

            if (key != ConsoleKey.A)
                return key;
            
            moved = MoveRight(Copy(tab));
            if (moved.Item1 && moved.Item2 > max)
            {
                key = ConsoleKey.RightArrow;
                max = moved.Item2;
            }
            moved = MoveDown(Copy(tab));
            if (moved.Item1 && moved.Item2 > max)
            {
                key = ConsoleKey.DownArrow;
            }
            return key;

        }
        public static ConsoleKey IA_AngleBD(int[,] tab)
        {
            int max = -1;
            ConsoleKey key = ConsoleKey.A;
            Tuple<bool, int> moved = MoveDown(Copy(tab));

            if (moved.Item1)
            {
                key = ConsoleKey.DownArrow;
                max = moved.Item2;
            }
            moved = MoveRight(Copy(tab));
            if (moved.Item1 && moved.Item2 > max)
            {
                key = ConsoleKey.RightArrow;
                max = moved.Item2;
            }

            if (key != ConsoleKey.A)
                return key;
            
            moved = MoveLeft(Copy(tab));
            if (moved.Item1 && moved.Item2 > max)
            {
                key = ConsoleKey.LeftArrow;
                max = moved.Item2;
            }
            moved = MoveUp(Copy(tab));
            if (moved.Item1 && moved.Item2 > max)
            {
                key = ConsoleKey.UpArrow;
            }
            return key;

        }
        public static ConsoleKey IA_AngleHD(int[,] tab)
        {
            int max = -1;
            ConsoleKey key = ConsoleKey.A;
            Tuple<bool, int> moved = MoveUp(Copy(tab));

            if (moved.Item1)
            {
                key = ConsoleKey.UpArrow;
                max = moved.Item2;
            }
            moved = MoveRight(Copy(tab));
            if (moved.Item1 && moved.Item2 > max)
            {
                key = ConsoleKey.RightArrow;
                max = moved.Item2;
            }

            if (key != ConsoleKey.A)
                return key;
            
            moved = MoveLeft(Copy(tab));
            if (moved.Item1 && moved.Item2 > max)
            {
                    key = ConsoleKey.LeftArrow;
                max = moved.Item2;
            }
            moved = MoveDown(Copy(tab));
            if (moved.Item1 && moved.Item2 > max)
            {
                key = ConsoleKey.DownArrow;
            }
            return key;

        }
    }
}