using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Threading;

namespace Snake
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            ConsoleKey key = Console.ReadKey().Key;
            
            if (key == ConsoleKey.I)
                IA_Game(1,0);
            else
                Game();
        }
        
        static int ToInt(char c)
        {
            return (int)(c - Convert.ToChar(0));
        }
        
        public static void ChiffreR(int n)
        {
            string rec = FiboEncode(n);

            string str = "";
            foreach (var c in rec)
            {
                int i = new Random().Next(1,255);
                str = str + BinaryEncode(i) + c + FiboEncode(i);
            }
            
            int k = (str.Length + 3 ) % 8;
            int raj = 0;
            if (k != 0)
            {
                while (k != 8)
                {
                    raj++;
                    k++;
                    str += "0";
                }
                str = ((raj) / 4).ToString() + ((raj) / 2) % 2 + ((raj) % 2).ToString() + str;

            }    
            else
            {
                str = "000" + str;
            }


            using (StreamWriter streamWriter = new StreamWriter(".best_score"))
            {
                for (int i = 0; i < str.Length; i+=8)
                {
                    streamWriter.Write(Convert.ToChar(BinaryDecode8(str.Substring(i,8))));
                }
            }
        }

        public static int DeChiffreR()
        {
            try
            {
                string s;
                using (StreamReader streamReader = new StreamReader(".best_score"))
                {
                    s = streamReader.ReadLine();
                }
    
                string str = "";
                foreach (var c in s)
                {
                    str += BinaryEncode(ToInt(c));
                }
    
                int ig = BinaryDecode8("00000" + str.Substring(0, 3));
                str = str.Substring(3, str.Length - 3);
                if (ig != 0)
                    str = str.Substring(0, str.Length - ig);          
                
                List<int> l = new List<int>();
                int k = 0;
                int a;
                int b;
                while (k < str.Length)
                {
                    int bin = BinaryDecode8(str.Substring(k, 8));
                    k += 8;
                    l.Add(Int32.Parse(str[k].ToString()));
                    k++;
                    a = 2;
                    b = 1;
                    int tmp = k;
                    while (true)
                    {
                        if (k != tmp && str[k] == str[k-1] && str[k] == '1')
                            break;
    
                        int tmpn = Int32.Parse(str[k].ToString());
                        bin -= tmpn * b;
                        a = a + b;
                        b = a - b;
                        k++;
                    }
    
                    if (bin != 0)
                        return 0;
    
                    k++;
                }
    
                a = 2;
                b = 1;
                int ret = 0;
                for (int i = 0; i < l.Count - 1; i++)
                {
                    ret += l[i] * b;
                    a = a + b;
                    b = a - b;
                }
    
                return ret;
                
            }
            catch (Exception e)
            {
                return 0;
            }
        }

        public static string FiboEncode(int n)
        {
            int a = 2;
            int b = 1;

            List<int> Fibo = new List<int>();
            while (b <= n)
            {
                Fibo.Add(b);
                a = a + b;
                b = a - b;
            }

            string rec = "";
            for (int i = Fibo.Count-1; i >= 0; i--)
            {
                if (n >= Fibo[i])
                {
                    rec = "1" + rec;
                    n -= Fibo[i];
                }
                else
                    rec = "0" + rec;
            }

            return rec + "1";
        }
        
        public static string BinaryEncode(int n )
        {
            string ret = "";
            int k = 128;
            while (k != 0)
            {
                ret = ret + (n / k);
                n = n % k;
                k /= 2;
                
            }

            return ret;
        }

        public static int BinaryDecode8(string str)
        {
            int k = 128;
            int ret = 0;
            foreach (var n in str)
            {
                ret += Int32.Parse(n.ToString())*k;
                k /= 2;
            }

            return ret;
        }
        public static int[,] Init(int map)
        {
            int[,] ret;
            int h;
            int w;
            switch (map)
            {
                case 0 :
                    h = 25;
                    w = 50;
                    ret = new int[h,w];
                    for (int i = 0; i < h; i++)
                    {
                        for (int j = 0; j < w; j++)
                        {
                            if (j == 0 || i == 0 || j == w - 1 || i == h - 1)
                                ret[i, j] = -1;
                            else
                            {
                                if (j > 18 && j < 33 && i > 7 && i < 16)
                                    ret[i, j] = -1;
                                else
                                    ret[i, j] = 0;
                            }
                        }
                    }

                    for (int i = 0; i < 5; i++)
                    {
                        ret[19 ,1+i ] = i;
                    }
                    break;
                default:
                    h = 25;
                    w = 50;
                    ret = new int[h,w];
                    for (int i = 0; i < h; i++)
                    {
                        for (int j = 0; j < w; j++)
                        {
                            if (j == 0 || i == 0 || j == w - 1 || i == h - 1)
                                ret[i, j] = -1;
                            else
                                ret[i, j] = 0;
                        }
                    }

                    for (int i = 0; i < 5; i++)
                    {
                        ret[h / 2 , w / 2 + i - 9 ] = i;
                    }
                    break;
            }
            

            return ret;
        }

        public static void Show(int[,] ret, int y, int x)
        {
            Console.ResetColor();
            Console.Clear();

            for (int i = 0; i < ret.GetLength(0); i++)
            {
                for (int j = 0; j < ret.GetLength(1); j++)
                {
                    switch (ret[i,j])
                    {
                        case -1 :
                            Console.BackgroundColor = ConsoleColor.DarkGray;
                        break;
                        case 0 :
                            Console.BackgroundColor = ConsoleColor.White;
                            break;
                        case -2 :
                            Console.BackgroundColor = ConsoleColor.Red;
                            break;
                        default:
                            if (i == y && j == x)
                                Console.BackgroundColor = ConsoleColor.DarkGreen;
                            else
                                Console.BackgroundColor = ConsoleColor.Green;
                        break;
                    }    
                    Console.Write("  ");
                }
                Console.WriteLine();
            }
            
            
            Console.ResetColor();
            Console.SetCursorPosition(49,ret.GetLength(0)+1);
            Console.Write("Pause : Spacebar");
        }
        public static void ShowRS(int h, int best_score, int point)
        {
            Console.ResetColor();
            Console.SetCursorPosition(2,h+1);
            Console.Write("Actual point : {0}         Best score : {1}",point,point > best_score ? point : best_score);
        }

        public static int NextPos(int[,] board, int tall,int y, int x, int h, int w)
        {
            int tmp = tall;
            for (int i = 0; i < h; i++)
            {
                for (int j = 0; j < w; j++)
                {
                    if (board[i, j] > 0)
                    {
                        if (board[i,j] == 1)
                        {
                            Console.SetCursorPosition(j * 2, i);
                            Console.BackgroundColor = ConsoleColor.White;
                            Console.Write("  ");
                        }
                        if(board[i,j] == tall)
                        {
                            Console.SetCursorPosition(j * 2, i);
                            Console.BackgroundColor = ConsoleColor.Green;
                            Console.Write("  ");
                        }
                        board[i, j]--;
                    }
                    if (i == y && j == x)
                    {
                        if (board[i, j] == -1 || board[i, j] > 0)
                            return -1;
                        if (board[i, j] == -2)
                            tmp++;
                        Console.SetCursorPosition(j * 2, i);
                        Console.BackgroundColor = ConsoleColor.DarkGreen;
                        Console.Write("  ");
                        board[i, j] = tall;
                    }
                }
            }

            if (tmp != tall)
            {
                List<Tuple<int,int>> l = new List<Tuple<int, int>>();
                for (int i = 0; i < h; i++)
                {
                    for (int j = 0; j < w; j++)
                    {
                        if (board[i, j] == 0)
                            l.Add(new Tuple<int, int>(j, i));
                    }
                }
                Tuple<int, int> cerisePos = l[new Random().Next(l.Count)];
                Console.SetCursorPosition(cerisePos.Item1 * 2, cerisePos.Item2);
                Console.BackgroundColor = ConsoleColor.DarkRed;
                Console.Write("  ");
                board[cerisePos.Item2, cerisePos.Item1] = -2;
            }
            return tmp;
        }
        public static readonly Func<string> ReadLineFunc = () =>
        {
            ConsoleKeyInfo key;
            
            while ((key = Console.ReadKey(true)).Key != ConsoleKey.Escape)
            {  
                switch (key.Key)
                {
                    case ConsoleKey.Z :
                    case ConsoleKey.UpArrow:
                        return "up";
                    case ConsoleKey.Q :
                    case ConsoleKey.LeftArrow:
                        return "le";
                    case ConsoleKey.D:
                    case ConsoleKey.RightArrow:
                        return "ri";
                    case ConsoleKey.S :
                    case ConsoleKey.DownArrow:
                        return "do";
                    case ConsoleKey.Spacebar:
                        return "pa";
                }
            }
            return "es";
        };
        /*public static ConsoleKey  ClearKeyBuffer()
        {
            ConsoleKey kp = ConsoleKey.A;
            while (Console.KeyAvailable)
                kp= Console.ReadKey(false).Key;
            return kp;
        }*/

        public static Tuple<bool,int> WillDie(int[,] board, int x, int y, int len)
        {
            if (board[y, x] == -1 || board[y,x] > 0)
                return new Tuple<bool, int>(true,0);

            int possibility = Possibilities(board, x,y, new HashSet<int>());
            if (board[y,x] == -2)
                len += 2;
            return new Tuple<bool, int>(possibility < len,possibility);
        }

        public static int Possibilities(int[,] board, int x, int y, HashSet<int> h )
        {
            if (h.Contains(x * 1000 + y) || board[y, x] == -1 || board[y,x] > 0)
                return 0;
            h.Add(x * 1000 + y);
            int tmp = 1;
            if (x == 38 || x == 1)
                tmp-=3;
            if (y == 1 || y == 23)
                tmp-=3;
            
            for (int i = -1; i < 2; i++)
            {
                for (int j = -1; j < 2; j++)
                {
                    if (Math.Abs(j) != Math.Abs(i))
                    {
                        tmp += Possibilities(board, x + i, y + j, h);
                        if (board[y + j, x + i] == -2)
                            tmp -= 1;
                        
                    }
                }
            }
            return tmp;
        }

        public static Tuple<int, int> PosCerise(int[,] board)
        {
            for (int i = 0; i < board.GetLength(0); i++)
            {
                for (int j = 0; j < board.GetLength(1); j++)
                {
                    if (board[i,j] == -2 )
                        return new Tuple<int, int>(j,i);
                }
            }
            return new Tuple<int, int>(0,0);
        }

        public static double DistCerise(int x, int y, int XCerise, int YCerise)
        {
            //return Math.Abs(XCerise - x) + Math.Abs(YCerise- y);
            return Math.Sqrt(Math.Pow(x - XCerise, 2) + Math.Pow(y - YCerise, 2));
        }

        public static int[,] Chemin(int[,] board)
        {
            int[,] ret = new int[board.GetLength(0),board.GetLength(1)];
            for (int i = 0; i < board.GetLength(1); i++)
            {
                for (int j = 0; j < board.GetLength(0); j++)
                {
                    ret[j, i] = -1;
                }
            }

            Stack<Tuple<int,int,int,int>> pile = new Stack<Tuple<int, int, int,int>>();
            pile.Push(new Tuple<int, int, int, int>(0,0,0,0));
            while (true)
            {
                Tuple<int, int, int,int > tmp = pile.Pop();
                
                int x = tmp.Item1;
                int y = tmp.Item2;
                int n = tmp.Item3;
                int a = tmp.Item4;

                if (a == 5)
                {
                    ret[y, x] = -1;
                    continue;
                }
                if (!In_board(x, y, ret.GetLength(1), ret.GetLength(0)))
                    continue;

                if (ret[y, x] == 0 && n + 1 == ret.GetLength(0) * ret.GetLength(1))
                    return ret;

                if (ret[y, x] != -1 && ret[y, x] != n)
                    continue;

                ret[y, x] = n;
                
                if ( a < 5)
                    pile.Push(new Tuple<int, int, int, int>(x,y,n,a+1));
                
                
                switch ((a+n)%4)
                {
                    case 0 :
                        pile.Push(new Tuple<int, int, int, int>(x+1,y,n+1,0));
                        break;
                    case 1 :
                        pile.Push(new Tuple<int, int, int, int>(x-1,y,n+1,0));
                        break;
                    case 2 :
                        pile.Push(new Tuple<int, int, int, int>(x,y+1,n+1,0));
                        break;
                    case 3 :
                        pile.Push(new Tuple<int, int, int, int>(x,y-1,n+1,0));
                        break;
                }
            }
        }

        public static bool In_board(int x, int y, int width, int height)
        {
            return !(x < 0 || y < 0 || x >= width || y >= height);
        }

        public static void New_Cerise(int[,] board)
        {
            List<int> h = new List<int>();
            for (int i = 0; i < board.GetLength(0); i++)
            {
                for (int j = 0; j < board.GetLength(1); j++)
                {
                    if (board[i, j] == 0)
                        h.Add(i * 1000 + j);
                }
            }
            
            int rd = h[new Random().Next(h.Count)];
            board[rd / 1000, rd % 1000] = -2;
        }
        public static void IA_Game(int mode, int map)
        {
            //Init
            Console.ReadKey();
            
            int[,] board = Init(map);

            int x;
            int y;
            int tall = 4;

            switch (map)
            {
                case 0:
                    x = 5;
                    y = 19;
                    break;
                default:
                    x = 50 / 2+ 4;
                    y = 25 / 2;
                    break;
            }
            New_Cerise(board);
            Show(board,y,x);
            if (mode == 0)
            ShowRS(board.GetLength(0),0,0);

            //Game
            var read = Task.Run(ReadLineFunc);
            string dir =  "ri";
            bool b = true;
            while (b)
            {
                Console.CursorVisible = false;
                if (mode == 0)
                    Thread.Sleep(750);
                Thread.Sleep(200-tall*16 < 5 ?5:200-tall*16 );
                if (mode == 0)
                {
                    if (read.IsCompleted)
                    {
                        //PrintMenu
                        
                        
                        
                    }
                }
                   

                Tuple<int, int> Cerise = PosCerise(board);

                double distu = DistCerise(x, y - 1, Cerise.Item1, Cerise.Item2);
                double distd = DistCerise(x, y + 1, Cerise.Item1, Cerise.Item2);
                double distr = DistCerise(x + 1, y, Cerise.Item1, Cerise.Item2);
                double distl = DistCerise(x - 1, y, Cerise.Item1, Cerise.Item2);
                

                Tuple<bool,int> possdisu = WillDie(board, x ,y - 1,tall);
                Tuple<bool,int> possdisd = WillDie(board, x ,y + 1,tall );
                Tuple<bool,int> possdisl = WillDie(board, x - 1, y,tall);
                Tuple<bool,int> possdisr = WillDie(board, x + 1, y,tall);

                bool possu = !possdisu.Item1;
                bool possd = !possdisd.Item1;
                bool possl = !possdisl.Item1;
                bool possr = !possdisr.Item1;
                
                switch (dir)
                {
                    case "up":
                        if (possu && possr && possl)
                        {
                            if (distl < distu)
                            {
                                if (distl < distr)
                                    dir = "le";
                                else
                                    dir = "ri";
                            }
                            else
                            {
                                if (distr < distu)
                                    dir = "ri";
                            }
                        }
                        
                        if (!possr && possu && possl && distl < distu)
                            dir = "le";
                        if (!possl && possu && possr && distr < distu)
                            dir = "ri";
                        
                        if (!possu && possl && possr)
                        {
                            if (distr < distl)
                                    dir = "ri";
                            else
                                    dir = "le";
                        }
                        
                        if (!possu && !possl && possr)
                            dir = "ri";
                        if (!possu && possl && !possr)
                            dir = "le";
                        break;

                            
                    case "do":
                        if (possd && possr && possl)
                        {
                            if (distl < distd)
                            {
                                if (distl < distr)
                                    dir = "le";
                                else
                                    dir = "ri";
                            }
                            else
                            {
                                if (distr < distd)
                                    dir = "ri";
                            }
                        }
                        
                        if (!possr && possd && possl && distl < distd)
                            dir = "le";
                        if (!possl && possd && possr && distr < distd)
                            dir = "ri";
                        
                        if (!possd && possl && possr)
                        {
                            if (distr < distl)
                                dir = "ri";
                            else
                                dir = "le";
                        }
                        
                        if (!possd && !possl && possr)
                            dir = "ri";
                        
                        if (!possd && possl && !possr)
                            dir = "le";
                        break;
                    
                    case "ri":
                        if (possr && possd && possu)
                        {
                            if (distd < distr)
                            {
                                if (distu < distd)
                                    dir = "up";
                                else
                                    dir = "do";
                            }
                            else
                            {
                                if (distu < distr)
                                    dir = "up";
                            }
                        }
                        
                        if (!possd && possr && possu && distu < distr)
                            dir = "up";
                        if (!possu && possr && possd && distd < distr)
                            dir = "do";
                        
                        if (!possr && possd && possu)
                        {
                            if (distd < distu)
                                dir = "do";
                            else
                                dir = "up";
                        }
                        if (!possr && !possu && possd)
                            dir = "do";
                        if (!possr && !possd && possu)
                            dir = "up";
                        break;
                    case "le":
                        if (possl && possd && possu)
                        {
                            if (distd < distl)
                            {
                                if (distu < distd)
                                    dir = "up";
                                else
                                    dir = "do";
                            }
                            else
                            {
                                if (distu < distl)
                                    dir = "up";
                            }
                        }
                        
                        if (!possd && possl && possu && distu < distl)
                            dir = "up";
                        if (!possu && possl && possd && distd < distl)
                            dir = "do";
                        
                        if (!possl && possd && possu)
                        {
                            if (distd < distu)
                                dir = "do";
                            else
                                dir = "up";
                        }
                        if (!possl && !possu && possd)
                            dir = "do";
                        if (!possl && !possd && possu)
                            dir = "up";
                        break;
                }

                if (!possd && !possl && !possr && !possu)
                {
                    if (possdisd.Item2 > possdisl.Item2 && possdisd.Item2 > possdisr.Item2 &&
                        possdisd.Item2 > possdisu.Item2)
                        dir = "do";
                    if (possdisu.Item2 > possdisl.Item2 && possdisu.Item2 > possdisr.Item2 &&
                        possdisu.Item2 > possdisd.Item2)
                        dir = "up";
                    if (possdisr.Item2 > possdisl.Item2 && possdisr.Item2 > possdisd.Item2 &&
                        possdisr.Item2 > possdisu.Item2)
                        dir = "ri";
                    if (possdisl.Item2 > possdisd.Item2 && possdisl.Item2 > possdisr.Item2 &&
                        possdisl.Item2 > possdisu.Item2)
                        dir = "le";
                }
                
                switch (dir)
                {
                    case "le":
                        x--;
                        break;
                    case "ri":
                        x++;
                        break;
                    case "up":
                        y--;
                        break;
                    case "do":
                        y++;
                        break;
                }

                int tmp = NextPos(board,  tall, y, x, board.GetLength(0),board.GetLength(1));
                switch (tmp)
                {
                    case -1:
                        b = false;
                        break;
                    default:
                        if (tall != tmp )
                            ShowRS(board.GetLength(0),0,tmp-4);
                        tall = tmp;
                        break;
                }
            }
            
            //Restart
            ConsoleKey key = Console.ReadKey().Key;
            if (key == ConsoleKey.I)
                IA_Game(mode,map);
            
        }
        public static void Game(int r = -1)
        {
            //Init
            Console.ReadKey();
            
            int best_score;
            if (r == -1)
                best_score = DeChiffreR();
            else
                best_score = r;
            
            int h = 25;
            int w = 50;
            int[,] board = Init(1);
            
            
            int tall = 4;
            int x = w / 2 - 5;
            int y = h / 2;
            board[new Random().Next(1,h-1), new Random().Next(1,w-1)] = -2;
            Show(board,y,x);
            ShowRS(h,best_score,0);

            //Game
            var read = Task.Run(ReadLineFunc);
            string dir =  "ri";
            bool b = true;
            while (b)
            {
                Console.CursorVisible = false;
                Thread.Sleep(200-tall*2 < 50 ?50:200-tall*4 );
                
                bool pause = false;
                do
                {
                    if (read.IsCompleted)
                    {
                        if (read.Result == "pa")
                            pause = !pause;
                        else
                        {
                            if (!pause)
                            {
                                switch (dir)
                                {
                                    case "up":
                                    case "do":
                                        if (read.Result != "up" && read.Result != "do")
                                            dir = read.Result;
                                        break;

                                    case "ri":
                                    case "le":
                                        if (read.Result != "le" && read.Result != "ri")
                                            dir = read.Result;
                                        break;
                                }
                            }
                        }
                        read = Task.Run(ReadLineFunc);
                    }
                } while (pause);

                switch (dir)
                {
                    case "le":
                        x--;
                        break;
                    case "ri":
                        x++;
                        break;
                    case "up":
                        y--;
                        break;
                    case "do":
                        y++;
                        break;
                }

                int tmp = NextPos(board,  tall, y, x, h, w);
                switch (tmp)
                {
                    case -1:
                        b = false;
                        break;
                    default:
                        if (tall != tmp )
                            ShowRS(h,best_score,tmp-4);
                        tall = tmp;
                        break;
                }
            }
            
            //End of the Game
            Console.SetCursorPosition(w/2-10,h/2);
            Console.ResetColor();
            Console.BackgroundColor = ConsoleColor.White;
            Console.ForegroundColor = ConsoleColor.Black;
            int nr = best_score > tall - 4 ? best_score : tall - 4;
            Console.WriteLine("{0} : {1}", best_score > tall - 4 ? "Points" : "New Computer best_score", tall - 4);
            ChiffreR(nr);

            
            //Restart
            int k = 0;
            Console.SetCursorPosition(w/2-10<0 ? 0 : w/2-10,h/2+1);
            Console.Write("   Keep pressing any key to Restart");
            while (k < 5000)
            {
                Thread.Sleep(15);
                if (Console.KeyAvailable)
                {
                    ConsoleKey ck = ConsoleKey.A;
                    while (Console.KeyAvailable)
                        ck = Console.ReadKey(false).Key;
                    Game(nr);
                    return;
                }
                k += 15;
            }
        }
    }
}