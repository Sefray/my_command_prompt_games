﻿namespace Scrabble
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            Console.CursorVisible = false;
            Menu();
        }

        //Dans le menu
        public static void Menu()
        {
            char[,] board = Init_Board();
            List<Tuple<Tuple<int, int>, int>> special = Init_Special(new Random().Next(2));
            HashSet<string> word = Init_World();
            
            Write(board,"SCRABBLE",10,0,false);
            Write(board,"SCRABBL",3,7,true);
            Write(board,"EPIT",6,3,false);
            Write(board,"#",11,1,true);
            Write(board,"PRPA",4,3,true);
            Write(board,"IGNAL",3,8,false);
            Write(board,"VILEJUIF",1,12,true);
            Show(board,special);
            

            int cursorpos = 0;
            
            while (cursorpos != 3)
            {
                ConsoleKey pressed = ConsoleKey.UpArrow;
                while (pressed != ConsoleKey.Enter)
                {
                    Show(board,special);
                    
                    Console.SetCursorPosition(48, 36);
                    Console.WriteLine("Jouer");
                    Console.SetCursorPosition(48, 38);
                    Console.WriteLine("Recherche");
                    Console.SetCursorPosition(48, 40);
                    Console.WriteLine("Comptage");
                    Console.SetCursorPosition(48, 42);
                    Console.WriteLine("Fin");
                    
                    
                    Console.SetCursorPosition(46,cursorpos*2 + 36);
                    Console.Write(">");

                    pressed = Console.ReadKey().Key;
                    switch (pressed)
                    {
                        case ConsoleKey.UpArrow :
                            cursorpos = (cursorpos + 3) % 4;
                            break;
                        case ConsoleKey.DownArrow :
                            cursorpos = (cursorpos + 1) % 4;
                            break;
                    }
                }
                
                switch (cursorpos)
                {
                    case 0 :
                        Game(word);
                        break;
                    case 1 :
                        Recherche(word,"aaaaaaaaaaaaaa");
                        //Mot a chercher
                        break;
                    case 2 :
                        Comptage(word);
                        //Compteur de point
                        break;
                }
            }
        }
        
        public static void Game(HashSet<string> word)
        {
            List<char> pioche = Init_Pioche();
            char[,] board = Init_Board();
            List<Tuple<Tuple<int, int>, int>> special = Init_Special(new Random().Next(2));
            
            Show(board, special);

            Tuple<int, List<Player>> tup = Init_Players(pioche);
            int nb = tup.Item1;
            List<Player> players = tup.Item2;
            
            int player_act = -1;
            Tuple<int, int, bool, string, int> best_choice;
            int manche = 0;
            while (Still_Letters(players))
            {
                player_act = (player_act + 1) % nb;
                if (player_act == 0)
                    manche++;
                
                Asterisque(board,word);

                Random rand;
                int a;
                string enter;

                do
                {
                    Show(board, special);

                    Tableau_score(players);
                    
                    Console.SetCursorPosition(25, 35);
                    Console.Write("MANCHE : " + manche+ "        Pioche : " + pioche.Count);
                    Console.SetCursorPosition(30, 37);
                    Console.WriteLine(players[player_act]._name);
                    Console.SetCursorPosition(30, 38);
                    foreach (var c in players[player_act].Letters)
                    {
                        Console.Write(c + " ");
                    }
                  
                    
                    
                    if (player_act == 0)
                    {
                        Console.SetCursorPosition(30,44);
                        Console.Write("Changer de lettre : !C ");
                        Console.SetCursorPosition(30,45);
                        Console.Write("Ensemble des combinaisons possible : !T ");
                        Console.SetCursorPosition(30, 40);
                        Console.Write("Votre mot : ");
                        enter = Console.ReadLine().ToUpper();
                    }
                    else
                    {
                        Console.SetCursorPosition(39, 40);
                        enter = "!H";
                    }
                    
                    if (enter == "!C" || enter == "!H")
                        break;
                    
                    if (enter == "!T")
                        Recherche(word,To_String(players[player_act].Letters));
                    
                } while (Incorrect(enter, Copy(players[player_act].Letters)) ||
                         Placer(board, enter, special, players[player_act], word) == 1);

                if (enter == "!H")
                {
                    best_choice = Trouve(board, players[player_act].Letters, word, special);
                    if (best_choice.Item4 == "")
                        enter = "!C";
                    else
                    {
                        enter = best_choice.Item4;
                        Write(board, enter, best_choice.Item1, best_choice.Item2, best_choice.Item3);
                        players[player_act].point += Point(best_choice.Item1, best_choice.Item2, best_choice.Item3,
                            special, 1, board, enter);
                    }
                }
                
                if (enter != "!C")
                    MajLettre(enter, players[player_act].Letters);
                else
                {
                    foreach (var l in players[player_act].Letters)
                    {
                        pioche.Add(l);
                    }
                    players[player_act].Letters.Clear();
                }
                
                
                while (pioche.Count != 0 && players[player_act].Letters.Count != 7)
                {
                    rand = new Random();
                    a = rand.Next(pioche.Count);
                    Thread.Sleep(20);
                    players[player_act].Letters.Add(pioche[a]);
                    pioche.RemoveAt(a);
                }
            }

            Maj(players,player_act);
            Show(board,special);
            Winner(players);
            
            Console.ReadKey();
        }

        public static void Best(HashSet<string> word)
        {
            List<char> pioche = Init_Pioche();
            char[,] board = Init_Board();
            List<Tuple<Tuple<int, int>, int>> special = Init_Special(new Random().Next(2));
            
            Show(board, special);

            List<Player> players = new List<Player>();
            players.Add(new Player("Best"));
            players[0].Letters = pioche;
            pioche = new List<char>();
            
            Tuple<int, int, bool, string, int> best_choice;
            while (Still_Letters(players))
            {
                Asterisque(board, word);
                Show(board, special);
                Tableau_score(players);
                
                best_choice = Trouve(board, players[0].Letters, word, special);
                string enter = best_choice.Item4;
                Write(board, enter, best_choice.Item1, best_choice.Item2, best_choice.Item3);
                players[0].point += Point(best_choice.Item1, best_choice.Item2, best_choice.Item3,
                    special, 1, board, enter);
            }

            Show(board,special);
            Tableau_score(players);
            Console.ReadKey();
        }

        public static void Recherche(HashSet<string> word, string enter)
        {
            Console.Clear();
            string letters = enter;
            while (letters.Length > 7)
            {
                Console.Clear();
                Console.SetCursorPosition(25,7);
                Console.Write("Donnez toutes vos lettres : ");
                letters = Console.ReadLine().ToUpper();
            }
            
            Stack<Tuple<string,List<char>>> pile = new Stack<Tuple<string, List<char>>>();
            
            List<string> auto = new List<string>();
            
            List<char> l = new List<char>();
            foreach (var letter in letters)
            {
                l.Add(letter);
            } 
            pile.Push(new Tuple<string, List<char>>("",l));
            
            
            Tuple<string, List<char>> tmp;
            
            while (pile.Count != 0)
            {
                tmp = pile.Pop();
                for (int i = 0; i < tmp.Item2.Count; i++)
                {
                    string tmpo = tmp.Item1 + tmp.Item2[i];
                    if (Contains(word,tmpo) && !auto.Contains(tmpo))
                        auto.Add(tmpo);
                    List<char> tmpolist = Copy(tmp.Item2);
                    tmpolist.RemoveAt(i);
                    pile.Push(new Tuple<string, List<char>>(tmpo, tmpolist));
                }
            }
            
            auto = auto.OrderBy(e => e.Length).ThenBy(e => e).ToList();
            int len_tot_ligne = 0;
            int k = 1;
            int act_y =1;
            
            foreach (var mot in auto)
            {
                if (mot.Length != k)
                {
                    k++;
                    act_y += 2;
                    Console.SetCursorPosition(25,act_y + 7);
                    len_tot_ligne = 0;
                }
                Console.Write(mot + " ");
                len_tot_ligne += mot.Length + 1;
                
                if (len_tot_ligne > 90)
                {
                    act_y++;
                    Console.SetCursorPosition(25, act_y + 7);
                    len_tot_ligne = 0;
                }
            }
            
            Console.ReadKey();
        }
        public static void Comptage(HashSet<string> word)
        {
            Tuple<char[,], List<Tuple<Tuple<int, int>, int>>> tuple = new Tuple<char[,], List<Tuple<Tuple<int, int>, int>>>(Init_Board(),Init_Special(0));
            Stack< Tuple<char[,], List<Tuple<Tuple<int, int>, int>>>> pile = new Stack<Tuple<char[,], List<Tuple<Tuple<int, int>, int>>>>();
            pile.Push(tuple);

            char[,] board = Init_Board();
            
            List<Tuple<Tuple<int, int>, int>> special = Init_Special(new Random().Next(2));
            Show(board,special);
            List<Player> players = new List<Player>();
            
            Console.SetCursorPosition(35,38);
            Console.WriteLine("Combien de joueur(s) ?");
            Console.SetCursorPosition(35,39);
            Console.Write("> ");
            int nb = Int32.Parse(Console.ReadLine());

            for (int i = 0; i < nb; i++)
            {
                players.Add(new Player("Joueur " + (i+1)));
            }
            int player_act = 0;
            int manche = 0;
            string letter;
            Tuple<int, int, bool, string, int> best_choice;
            int best_move;
            
            
            string enter = "a";
            while ( enter != "!E")
            {
                if (player_act == 0)
                    manche++;
                
                Asterisque(board,word);
                Show(board,special);
                Tableau_score(players);
                
                Console.SetCursorPosition(30, 37);
                Console.WriteLine(players[player_act]._name);
                
                Console.SetCursorPosition(30, 41);
                Console.Write("Lettre à disposition : ");
                letter = Console.ReadLine().ToUpper();

                int point_joueur_act = players[player_act].point;
                List<char> lettre_dispo = new List<char>();
                foreach (var l in letter)
                {
                    lettre_dispo.Add(l);    
                }
               
                best_choice = Trouve(board, lettre_dispo, word, special);
                char[,] copy_board = Copy(board);
                enter = best_choice.Item4;
                Write(copy_board, enter, best_choice.Item1, best_choice.Item2, best_choice.Item3);

                if (best_choice.Item4 == "")
                    best_move = 0;
                else
                    best_move = best_choice.Item5;
                    
                do
                {
                    Console.Clear();
                    Show(board, special);

                    Tableau_score(players);
                    
                    Console.SetCursorPosition(25, 35);
                    Console.Write("MANCHE : " + manche);
                    
                    Console.SetCursorPosition(30, 37);
                    Console.WriteLine(players[player_act]._name);
                    Console.SetCursorPosition(30, 38);
                    Console.WriteLine("Meilleur mouvement : " + best_move);
                    
                    Console.SetCursorPosition(30,46);
                    Console.Write("Changement de lettre : !C ");
                    Console.SetCursorPosition(30,47);
                    Console.Write("Ensemble des combinaisons possible : !T ");
                    Console.SetCursorPosition(30,48);
                    Console.Write("Partie terminée : !E ");
                    
                    Console.SetCursorPosition(30, 42);
                    Console.Write("Vos lettres : " + letter );
                    
                    Console.SetCursorPosition(30, 43);
                    Console.Write("Votre mot: ");
                    enter = Console.ReadLine().ToUpper();
                    
                    if (enter == "!C" || enter == "!E")
                        break;
                    
                    if (enter == "!T")
                        Recherche(word,letter);
                    
                } while (Incorrect(enter, Copy(lettre_dispo)) || Placer(board, enter, special, players[player_act], word) == 1);

                Show(copy_board,special);
                Console.SetCursorPosition(25,3);
                Console.WriteLine("Le meilleurs mouvement était :");
                
                int point_reel = players[player_act].point;
                players[player_act].point = point_joueur_act + best_move;
                Tableau_score(players);
                
                Console.ReadKey();

                players[player_act].point = point_reel;
                player_act = (player_act + 1) % nb;
            }
            
            Show(board,special);
            Winner(players);
            Console.ReadKey();
        }
        
        //Affichage
        public static void Show(char[,] board, List<Tuple<Tuple<int, int>, int>> Special)
        {
            Console.Clear();
            Console.CursorVisible = false;
            Console.SetCursorPosition(20,4);
            Console.WriteLine("------------------------------------------------------------");
            for (int j = 0; j < 15; j++)
            {
                Console.SetCursorPosition(20,5+j*2);
                for (int i = 0; i < 15; i++)
                {
                    if (board[j, i] == ' ' )
                    {
                        int tmp = Is_Special(i, j, Special, 0);
                        Console.Write("|");
                        Console.BackgroundColor = Dicobis[tmp];
                        Console.Write("   ");
                    }
                    else
                    {
                        Console.Write("|");
                        Console.BackgroundColor = ConsoleColor.Yellow;
                        Console.ForegroundColor = ConsoleColor.Black;
                        Console.Write( " " + board[j, i] + " ");
                    }
                    Console.ResetColor();
                }
                Console.WriteLine("|");
                Console.SetCursorPosition(20,5+j*2+1);
                Console.WriteLine("------------------------------------------------------------");
            }
        }
        public static int Placer(char[,] board, string enter, List<Tuple<Tuple<int, int>, int>> Special, Player player, HashSet<string> world )
        {
            int y = -1 ;
            int x = -1 ;
            bool horizontal = true;
            do
            {
                x =  (x + 1) % (15 - enter.Length);
                if (x == 0)
                {
                    y++;
                    if (y == 15)
                    {
                        if (!horizontal)
                            return 3;
                        y = 0;
                        horizontal = false;
                    }
                }
            } while (!Posable(enter.Length,board,x,y,horizontal) || Is_empty(board) && !Center(enter.Length,x,y,horizontal));
            

            ConsoleKey pressed;

            do
            {
                pressed = ConsoleKey.UpArrow;
                
                while (pressed == ConsoleKey.UpArrow || pressed == ConsoleKey.DownArrow ||
                       pressed == ConsoleKey.RightArrow || pressed == ConsoleKey.LeftArrow)
                {
                    char[,] tmp = Copy(board);
                    Write(tmp, enter, x , y , horizontal);
                    Show(tmp, Special);
                    Console.SetCursorPosition(30,35);
                    Console.Write("Horizontal/Vertical : Touche S ");
                    
                    if (Is_empty(board) && Contains(world,enter)|| Connected(Copy(board), horizontal, x, y, enter, world))
                    {
                        Console.SetCursorPosition(25,40);
                        Console.WriteLine("Possiblement " + Point(x, y, horizontal,Special,0,tmp, enter) + " points");
                    }
                    
                    pressed = Console.ReadKey().Key;
                    do
                    {    
                        if (horizontal)
                        {
                            switch (pressed)
                            {
                                case ConsoleKey.UpArrow:
                                    y = (y + 14) % 15;
                                    break;
                                case ConsoleKey.DownArrow:
                                    y = (y + 1) % 15;
                                    break;
                                case ConsoleKey.RightArrow:
                                    x = (x + 1) % (16 - enter.Length);
                                    break;
                                case ConsoleKey.LeftArrow:
                                    x = (x + 15 - enter.Length) % (16 - enter.Length);
                                    break;
                            }
                        }
                        else
                        {
                            switch (pressed)
                            {
                                case ConsoleKey.UpArrow:
                                    y = (y + 15 - enter.Length) % (16 - enter.Length);
                                    break;
                                case ConsoleKey.DownArrow:
                                    y = (y + 1) % (16 - enter.Length);
                                    break;
                                case ConsoleKey.RightArrow:
                                    x = (x + 1) % 15;
                                    break;
                                case ConsoleKey.LeftArrow:
                                    x = (x + 14) % 15;   
                                    break;
                            }
                        }
                    } while (!Posable(enter.Length,board,x,y,horizontal) || Is_empty(board) && !Center(enter.Length,x,y,horizontal));
                    

                    switch (pressed)
                    {
                        case ConsoleKey.B :
                            return 1;
                        case ConsoleKey.S :
                            horizontal = !horizontal;
                            x = 0 ;
                            y = 0 ;
                            if (Is_empty(board))
                            {
                                x = 7;
                                y = 7;
                            }
                            break;
                    }
                }
            } while (pressed != ConsoleKey.F && (pressed != ConsoleKey.Enter || (!Is_empty(board) || !Contains(world,enter)) && !Connected(Copy(board),horizontal,x,y,enter,world)) ) ;
            
            Write(board, enter, x , y , horizontal);
            player.point += Point(x, y, horizontal, Special,1,board, enter);
            
            return 0;

        }
        public static void Tableau_score(List<Player> players)
        {
            int k = 0;
            Console.SetCursorPosition(55, 36);
            Console.WriteLine("|   JOUEUR   |  POINTS    ");
            foreach (var p in players)
            {
                Console.SetCursorPosition(55, 37 + k);
                Console.Write("|  " + p._name + "  |    " + p.point + "  ");
                k++;
            }
        }
        
        //Mot
        public static bool Contains(HashSet<string> word, string mot)
        {
            if (word.Contains(mot))
                return true;

            if (mot.Contains('*'))
            {
                foreach (var c in "ABCDEFGHIJKLMNOPQRSTUVWXYZ")
                {
                    int kl = 0;
                    while (mot[kl] != '*')
                    {
                        kl++;
                    }

                    string tmp = "";
                    for (int s = 0; s < kl; s++)
                    {
                        tmp += mot[s];
                    }

                    tmp += c;
                    for (int s = kl + 1; s < mot.Length; s++)
                    {
                        tmp += mot[s];
                    }

                    if (word.Contains(tmp))
                        return true;
                }
            }

            return false;
        }
        public static string To_String(List<char> lettre)
        {
            string tmp = "";
            foreach (var c in lettre)
            {
                tmp += c;
            }

            return tmp;
        }
        
        //Poser
        public static bool Posable(int longueur, char[,] board, int x, int y, bool horizontal)
        {
            if (board[y, x] != ' ')
                return false;
            
            if (horizontal)
            {
                for (int i = 0; i < longueur; i++)
                {
                    if (x + i > 14)
                        return false;

                    if (board[y, x + i] != ' ')
                        longueur++;
                }
            }
            else
            {
                for (int i = 0; i < longueur; i++)
                {
                    if (y+i > 14)
                        return false;

                    if (board[y + i, x] != ' ')
                        longueur++;
                }
            }
            return true;
        }
        public static bool Connected(char[,] board, bool horizontal, int x, int y, string enter, HashSet<string> word)
        {
            Write(board, enter, x, y, horizontal);
            List<string> mot = new List<string>();
            int kl;
            
            string fait = "";

            int i = 0;
            int j = 0;
            int k = 0;

            if (horizontal)
                i++;
            else
                j++;

            while (y - j > -1 && x - i > -1 && board[y - j, x - i] != ' ')
            {
                fait = board[y - j, x - i] + fait;
                if (horizontal)
                    i++;
                else
                    j++;
            }

            i = 0;
            j = 0;

            bool asterix = false;
            while (y + j < 15 && x + i < 15 && board[y + j, i + x] != ' ')
            {
                fait += board[y + j, i + x];
                if (k < enter.Length && board[y + j, i + x] == enter[k])
                {
                    string tmp = RecupMot(board, x + i, j + y, !horizontal);
                    if (tmp.Length != 1)
                    { 
                        mot.Add(tmp);
                        if (tmp.Contains('*'))
                        {
                            kl = 0;
                            while (tmp[kl] != '*')
                            {
                                kl++;
                            }

                            asterix = true;
                        }
                    }
                    k++;
                }

                if (horizontal)
                    i++;
                else
                    j++;
            }

            if (mot.Count == 0 && fait.Length == enter.Length)
                return false;

            mot.Add(fait);

            if (fait.Contains('*'))
                asterix = true;

            if (asterix)
            {
                foreach (var c in "ABCDEFGHIJKLMNOPQRSTUVWXYZ")
                {
                    bool bon = true;
                    foreach (var w in mot)
                    {
                        if (w.Contains('*'))
                        {
                            kl = 0;
                            while (w[kl] != '*')
                            {
                                kl++;
                            }
                            
                            string tmp = "";
                            for (int s = 0; s < kl; s++)
                            {
                                tmp += w[s];
                            }

                            tmp += c;
                            for (int s = kl + 1; s < w.Length; s++)
                            {
                                tmp += w[s];
                            }

                            if (!word.Contains(tmp))
                            {
                                bon = false;
                                break;
                            }
                        }
                        else
                        {
                            if (!word.Contains(w))
                                return false;
                        }
                    }

                    if (bon)
                        return true;
                    
                    if (c == 'Z')
                        return false;
                }
            }
            else
            {
                foreach (var w in mot)
                {
                    if (!word.Contains(w))
                        return false;
                }
            }
            return true;
    }
        public static string RecupMot(char[,] board, int x, int y, bool horizontal)
        {
            string ret = board[y, x].ToString();
            int i = 1;
            if (horizontal)
            {
                while (x - i > -1 && board[y,x-i] != ' ')
                {
                    ret = board[y, x - i] + ret;
                    i++;
                }
                i = 1;
                while (x + i < 15 && board[y,x+i] != ' ')
                {
                    ret += board[y, x + i];
                    i++;
                }
            }
            else
            {
                while (y - i > -1 && board[y-i,x] != ' ')
                {
                    ret = board[y-i, x ] + ret;
                    i++;
                }
                i = 1;
                while (y + i < 15 && board[y+i,x] != ' ')
                {
                    ret += board[y+i, x ];
                    i++;
                }
            }
            return ret;
        }
        public static bool Incorrect(string enter, List<char> letter)
        {
            if (enter.Length == 0)
                return true;

            bool tmp;
            int tmpint = 0;
            
            for (int i = 0; i < enter.Length; i++)
            {
                tmp = true;
                for (int j = 0; j < letter.Count; j++)
                {
                    if(enter[i] == letter[j])
                    {
                        tmpint = j;
                        tmp = false;
                        break;
                    }
                }

                if (tmp)
                    return true;
                letter.RemoveAt(tmpint);
            }

            return false;
        }
        public static void Write(char[,] board, string enter, int x, int y, bool horizontal)
        {
            int k = 0;
            if (horizontal)
            {
                foreach (var c in enter)
                {
                    while (board[ y , x + k ] != ' ')
                        k++;
                    board[ y , x + k ] = c;
                    k++;
                }
            }
            else
            {
                foreach (var c in enter)
                {
                    while (board[ y + k , x ] != ' ')
                        k++;
                    board[ y + k, x ] = c;
                    k++;
                }
            }
        }

       
        public static int Point(int x, int y, bool horizontal, List<Tuple<Tuple<int, int>, int>> Special,int mode, char[,] board, string enter)
        {
            int tot = 0;
            int mult = 1;
            
            int tmp;
            int i = 0;
            int j = 0;
            int multilettre;
            int bonus = 0;
            int k = 0;
            
            if (horizontal)
                i++;
            else
                j++;

            while (y-j > -1 && x-i > -1 && board[y-j,x-i] != ' ')
            {
                tot += Point_Lettre[board[y - j, x - i]];
                if (horizontal)
                    i++;
                else
                    j++;
            }

            i = 0;
            j = 0;
            while (y+j < 15 && i + x < 15 && board[y+j,x+i] != ' ')
            {
                tmp = Is_Special(x + i, y + j , Special, mode);
                
                if (k < enter.Length && board[y + j, i + x] == enter[k])
                {
                    string tmpbis = RecupMot(board, x + i, j + y, !horizontal);
                    if (tmpbis.Length != 1)
                    {
                        int multbis = 1;
                        switch (tmp)
                        {
                            case 32 :
                                multbis *= 3;
                                break;
                            case 22 :
                                multbis *= 2;
                                break;
                            case 21 :
                                bonus += Point_Lettre[board[y + j, x + i]];
                                break;
                            case 31 :
                                bonus += Point_Lettre[board[y + j, x + i]]*2;
                                break;
                        }
                        
                        foreach (var cha in tmpbis)
                        {
                            bonus += Point_Lettre[cha] * multbis;
                        }
                        
                    }

                    k++;
                }
                
                multilettre = 1;
                
                if (tmp != 0)
                {
                    switch (tmp)
                    {
                        case 21 :
                            multilettre = 2;
                            break;
                        case 22 :
                            mult = 2;
                            break;
                        case 31 :
                            multilettre = 3;
                            break;
                        case 32 :
                            mult = 3;
                            break;
                    }
                }

                tot += multilettre * Point_Lettre[board[y + j, x + i]];
                    
                if (horizontal)
                    i++;
                else
                    j++;
            }

            tot *= mult;
            
            if (enter.Length == 7)
                tot += 50;
            if (enter == "EPITA")
                tot += 20;
            
            return tot + bonus;
        }
        
        //Premier joueur
        public static bool Is_empty(char[,] board)
        {
            for (int i = 0; i < 15; i++)
            {
                for (int j = 0; j < 15; j++)
                {
                    if (board[j, i] != ' ')
                        return false;
                }
            }
            return true;
        }
        public static bool Center(int longueur, int x, int y, bool horizontal)
        {
            if (horizontal)
            {
                if (y != 7)
                    return false;

                return 7 >= x && 7 < x + longueur;
            }
            if (x != 7)
                return false;

            return 7 >= y && 7 < y + longueur;
        }
        
        //Pendant
        public static bool Still_Letters(List<Player> players)
        {
            foreach (var p in players)
            {
                if (p.Letters.Count == 0)
                    return false;
            }
            return true;
        }
        public static void Asterisque(char[,] board, HashSet<string> word)
        {
            for (int i = 0; i < 15; i++)
            {
                for (int j = 0; j < 15; j++)
                {
                    if (board[j, i] == '*')
                    {
                        foreach (var c in "ABCDEFGHIJKLMNOPQRSTUVWXYZ")
                        {
                            board[j, i] = c;   
                            
                            string tmp1 = RecupMot(board, i, j, true);
                            string tmp2 = RecupMot(board, i, j, false);
    
                            if (tmp1.Length > 1 && tmp2.Length > 1 && word.Contains(tmp1) && word.Contains(tmp2))
                                return;
                            if (tmp2.Length < 2 && Contains(word,tmp1))
                                return;
                            if (tmp1.Length < 2 && Contains(word,tmp2))
                                return;
                        }
                    }
                }
            }
        }
        public static void MajLettre(string enter, List<char> letters)
        {
            foreach (var c in enter)
            {
                for (int i = 0; i < letters.Count; i++)
                {
                    if (c == letters[i] || c == letters[i] - 32)
                    {
                        letters.RemoveAt(i);
                        break;
                    }
                }
            }
        }
        
        //Fin
        public static void Winner(List<Player> players)
        {
            Player winner = new Player("hello");
            int score_total = 0;

            foreach (var player in players)
            {
                score_total += player.point;
                if (winner.point < player.point)
                    winner = player;
            }
            
            Console.SetCursorPosition(40,35);
            Console.Write("Score total : " + score_total);
            Console.SetCursorPosition(40,36);
            Console.Write("Le gagant est " + winner._name);
            
            int k = 0;
            Console.SetCursorPosition(30, 38);
            Console.WriteLine("|   JOUEUR   |  POINTS   |  RESTE");
            foreach (var p in players)
            {
                Console.SetCursorPosition(30, 39 + k);
                Console.Write("|  " + p._name + "  |   " + p.point + "        ");
                foreach (var lettre in p.Letters)
                    Console.Write(lettre + " ");
                k++;
            }

        }
        public static void Maj(List<Player> players, int end)
        {
            int add = 0;
            for (int i = 0; i < players.Count; i++)
            {
                if (i != end)
                {
                    foreach (var letter in players[i].Letters)
                    {
                        players[i].point -= Point_Lettre[letter];
                        add += Point_Lettre[letter];
                    }
                }
            }

            players[end].point += add;
        }

        //Brut
        public static Tuple<int, int,bool, string,int> Trouve(char[,] board, List<char> letters, HashSet<string> word, List<Tuple<Tuple<int, int>, int>> special)
        {
            Stack<Tuple<string,List<char>>> pile = new Stack<Tuple<string, List<char>>>();
            
            pile.Push(new Tuple<string, List<char>>("",letters));
            
            Tuple<string, List<char>> tmp;
            Tuple<int, int, bool,string,int> tup = new Tuple<int, int,bool, string,int>(0,0,true,"",0);

            string tmpo;
            int tmp_score;
            char[,] tmp_board;
            
            Console.SetCursorPosition(42, 40);
            Console.Write('.');
            
            if (Is_empty(board))
            {
                int ale = new Random().Next(0,2);
                while (pile.Count != 0)
                {
                    tmp = pile.Pop();
                    for (int i = 0; i < tmp.Item2.Count; i++)
                    {
                        tmpo = tmp.Item1 + tmp.Item2[i];
                        if (Contains(word,tmpo))
                        {
                            tmp_board = Copy(board);
                            Write(tmp_board,tmpo,7,7,true);
                            tmp_score = Point(7, 7, true, special, 0,tmp_board, tmpo);
                            if (tmp_score > tup.Item5)
                                tup = new Tuple<int, int, bool, string, int>(7,7, ale == 0,tmpo,tmp_score);
                        }
                        
                        List<char> tmpolist = Copy(tmp.Item2);
                        tmpolist.RemoveAt(i);
                        if (tmpo.Length < 7)
                            pile.Push(new Tuple<string, List<char>>(tmpo, tmpolist));
                    }
                }
            }
            else
            {
                while (pile.Count != 0)
                {
                    tmp = pile.Pop();
                    for (int i = 0; i < tmp.Item2.Count; i++)
                    {
                        tmpo = tmp.Item1 + tmp.Item2[i];
                        //cherche dans la grille si on peut placer ce mot

                        bool h = true;
                        for (int s = 0; s < 2; s++)
                        {
                            for (int j = 0; j < 15; j++)
                            {
                                for (int k = 0; k < 15; k++)
                                {
                                    if (Posable(tmpo.Length, board, j, k, h))
                                    {
                                        tmp_board = Copy(board);
                                        if (Connected(tmp_board, h, j, k, tmpo, word))
                                        {
                                            tmp_score = Point(j, k, h, special, 0, tmp_board, tmpo);
                                            if (tmp_score > tup.Item5)
                                                tup = new Tuple<int, int, bool, string, int>(j, k, h, tmpo, tmp_score);
                                        }
                                    }
                                }
                            }

                            h = !h;
                        }

                        List<char> tmpolist = Copy(tmp.Item2);
                        tmpolist.RemoveAt(i);
                        if (tmpo.Length < 7)
                            pile.Push(new Tuple<string, List<char>>(tmpo, tmpolist));
                        
                        Console.SetCursorPosition(43, 40);
                        Console.Write('.');
                    }
                }
            }
            
            Console.SetCursorPosition(44, 40);
            Console.Write('.');

            return tup;
        }

        //Copy
        public static char[,] Copy(char[,] board)
        {
            char[,] ret = new char[board.GetLength(0),board.GetLength(1)];

            for (int i = 0; i < 15; i++)
            {
                for (int j = 0; j < 15; j++)
                {
                    ret[j, i] = board[j, i];
                }
            }
            return ret;
        }
        public static List<char> Copy(List<char> list)
        {
            List<char> ret = new List<char>();
            foreach (var c in list)
            {
                ret.Add(c);
            }
            return ret;
        }
        public static List<string> Copy(List<string> list)
        {
            List<string> ret = new List<string>();
            foreach (var c in list)
            {
                ret.Add(c);
            }
            return ret;
        }
        
        //Dico
        public static Dictionary<int,ConsoleColor> Dicobis = new Dictionary<int, ConsoleColor>()
        {
            {0,ConsoleColor.DarkGreen},
            {21,ConsoleColor.Cyan},
            {22,ConsoleColor.DarkYellow},
            {31,ConsoleColor.DarkBlue},
            {32,ConsoleColor.DarkRed}
        };
        public static Dictionary<char, int> Point_Lettre = new Dictionary<char, int>()
        {
            {'A',1},
            {'B',3},
            {'C',3},
            {'D',2},
            {'E',1},
            {'F',4},
            {'G',2},
            {'H',4},
            {'I',1},
            {'J',8},
            {'K',10},
            {'L',1},
            {'M',2},
            {'N',1},
            {'O',1},
            {'P',3},
            {'Q',8},
            {'R',1},
            {'S',1},
            {'T',1},
            {'U',1},
            {'V',4},
            {'W',10},
            {'X',10},
            {'Y',10},
            {'Z',10},
            {'*',0}
        };
        
        //Init
        public static char[,] Init_Board()
        {
            char[,] board = new char[15,15];
            
            for (int i = 0; i < 15; i++)
            {
                for (int j = 0; j < 15; j++)
                {
                    board[i, j] = ' ';
                }
            }
            return board;
        }
        
        public static HashSet<string> Init_World()
        {
            HashSet<string> ret = new HashSet<string>();
            string[] tmp = mots().Split(' ', '\r', '\n'); 
            foreach (var mot in tmp)
            {
                if (!ret.Contains(mot) && mot.Length > 1 && mot.Length < 16)
                    ret.Add(mot);
            }
                
            return ret;
        }
        public static bool Valide(string enter)
        {
            foreach (var c in enter)
            {
                if (!"ABCDEFGHIJKLMNOPQRSTUVWXYZ".Contains(c))
                    return false;
            }
            return true;
        }
        
        public static List<char> Init_Pioche()
        {
            List<Tuple<char,int>> tmp = new List<Tuple<char, int>>();

            tmp.Add(new Tuple<char, int>('A',9));
            tmp.Add(new Tuple<char, int>('B',2));
            tmp.Add(new Tuple<char, int>('C',2));
            tmp.Add(new Tuple<char, int>('D',3));
            tmp.Add(new Tuple<char, int>('E',15));
            tmp.Add(new Tuple<char, int>('F',2));
            tmp.Add(new Tuple<char, int>('G',2));
            tmp.Add(new Tuple<char, int>('H',2));
            tmp.Add(new Tuple<char, int>('I',8));
            tmp.Add(new Tuple<char, int>('J',1));
            tmp.Add(new Tuple<char, int>('K',1));
            tmp.Add(new Tuple<char, int>('L',5));
            tmp.Add(new Tuple<char, int>('M',3));
            tmp.Add(new Tuple<char, int>('N',6));
            tmp.Add(new Tuple<char, int>('O',6));
            tmp.Add(new Tuple<char, int>('P',2));
            tmp.Add(new Tuple<char, int>('Q',1));
            tmp.Add(new Tuple<char, int>('R',6));
            tmp.Add(new Tuple<char, int>('S',6));
            tmp.Add(new Tuple<char, int>('T',6));
            tmp.Add(new Tuple<char, int>('U',6));
            tmp.Add(new Tuple<char, int>('V',2));
            tmp.Add(new Tuple<char, int>('W',1));
            tmp.Add(new Tuple<char, int>('X',1));
            tmp.Add(new Tuple<char, int>('Y',1));
            tmp.Add(new Tuple<char, int>('Z',1));
            tmp.Add(new Tuple<char, int>('*',2));

            List<char> ret = new List<char>();
            
            foreach (var tup in tmp)
            {
                for (int i = 0; i < tup.Item2; i++)
                {
                    ret.Add(tup.Item1);
                }
            }
            return ret;
        }
        public static Tuple<int, List<Player>> Init_Players(List<char> pioche)
        {
            List<Player>  players = new List<Player>();
            Console.SetCursorPosition(40,40);
            Console.Write("  Nombre de joueur(s) : ");
            int nb = Int32.Parse(Console.ReadLine())  ;
            //int nb = 4;
            nb = nb > 4 || nb < 1 ? 4 : nb;
            
            for (int i = 0; i < nb; i++)
            {
                players.Add(new Player("Joueur " +(i+1)));
            }

            foreach (var player in players)
            {
                while (pioche.Count != 0 && player.Letters.Count != 7)
                {
                    Random and = new Random();
                    int ba = and.Next(pioche.Count);
                    Thread.Sleep(20);
                    if (!player.Letters.Contains('*'))
                    {
                        player.Letters.Add(pioche[ba]);
                        pioche.RemoveAt(ba);
                    }
                }
            }
            Thread.Sleep(10);
            return new Tuple<int, List<Player>>(nb,players);
        }
        public static List<Tuple<Tuple<int, int>, int>> Init_Special(int mode )
        {
            List<Tuple<Tuple<int, int>, int>> Special = new List<Tuple<Tuple<int, int>, int>>();
            
            //diago
            for (int i = 0; i < 15; i++)
            {
                switch (i)
                {
                    case 0:
                    case 14 :
                        Special.Add(new Tuple<Tuple<int, int>, int>(new Tuple<int, int>(i,i),32 ));
                        Special.Add(new Tuple<Tuple<int, int>, int>(new Tuple<int, int>(i,14-i),32 ));
                        break;
                    case 5:
                    case 9:
                        Special.Add(new Tuple<Tuple<int, int>, int>(new Tuple<int, int>(i,i),31 ));
                        Special.Add(new Tuple<Tuple<int, int>, int>(new Tuple<int, int>(i,14-i),31 ));   
                        break;
                    case 6:
                    case 8 :
                        Special.Add(new Tuple<Tuple<int, int>, int>(new Tuple<int, int>(i,i),21 ));
                        Special.Add(new Tuple<Tuple<int, int>, int>(new Tuple<int, int>(i,14-i),21 ));
                        break;
                    case 7 : 
                        continue;
                    default:
                        Special.Add(new Tuple<Tuple<int, int>, int>(new Tuple<int, int>(i,i),22 ));
                        Special.Add(new Tuple<Tuple<int, int>, int>(new Tuple<int, int>(i,14-i),22 ));
                        break;
                }
            }
            Special.Add(new Tuple<Tuple<int, int>, int>(new Tuple<int, int>(7,7),22 ));
            
            Special.Add(new Tuple<Tuple<int, int>, int>(new Tuple<int, int>(0,3),21 ));
            Special.Add(new Tuple<Tuple<int, int>, int>(new Tuple<int, int>(0,11),21 ));
            Special.Add(new Tuple<Tuple<int, int>, int>(new Tuple<int, int>(14,3),21 ));
            Special.Add(new Tuple<Tuple<int, int>, int>(new Tuple<int, int>(14,11),21 ));
            
            Special.Add(new Tuple<Tuple<int, int>, int>(new Tuple<int, int>(3,0),21 ));
            Special.Add(new Tuple<Tuple<int, int>, int>(new Tuple<int, int>(11,0),21 ));
            Special.Add(new Tuple<Tuple<int, int>, int>(new Tuple<int, int>(3,14),21 ));
            Special.Add(new Tuple<Tuple<int, int>, int>(new Tuple<int, int>(11,14),21 ));
            
            
            
            Special.Add(new Tuple<Tuple<int, int>, int>(new Tuple<int, int>(7,0),32 ));
            Special.Add(new Tuple<Tuple<int, int>, int>(new Tuple<int, int>(0,7),32 ));
            Special.Add(new Tuple<Tuple<int, int>, int>(new Tuple<int, int>(7,14),32 ));
            Special.Add(new Tuple<Tuple<int, int>, int>(new Tuple<int, int>(14,7),32 ));

            
            
            Special.Add(new Tuple<Tuple<int, int>, int>(new Tuple<int, int>(1,5),31 ));
            Special.Add(new Tuple<Tuple<int, int>, int>(new Tuple<int, int>(1,9),31 ));
            Special.Add(new Tuple<Tuple<int, int>, int>(new Tuple<int, int>(2,6),21 ));
            Special.Add(new Tuple<Tuple<int, int>, int>(new Tuple<int, int>(2,8),21 ));
            Special.Add(new Tuple<Tuple<int, int>, int>(new Tuple<int, int>(3,7),21 ));
            
            Special.Add(new Tuple<Tuple<int, int>, int>(new Tuple<int, int>(13,5),31 ));
            Special.Add(new Tuple<Tuple<int, int>, int>(new Tuple<int, int>(13,9),31 ));
            Special.Add(new Tuple<Tuple<int, int>, int>(new Tuple<int, int>(12,6),21 ));
            Special.Add(new Tuple<Tuple<int, int>, int>(new Tuple<int, int>(12,8),21 ));
            Special.Add(new Tuple<Tuple<int, int>, int>(new Tuple<int, int>(11,7),21 ));
            
            Special.Add(new Tuple<Tuple<int, int>, int>(new Tuple<int, int>(5,13),31 ));
            Special.Add(new Tuple<Tuple<int, int>, int>(new Tuple<int, int>(9,13),31 ));
            Special.Add(new Tuple<Tuple<int, int>, int>(new Tuple<int, int>(6,12),21 ));
            Special.Add(new Tuple<Tuple<int, int>, int>(new Tuple<int, int>(8,12),21 ));
            Special.Add(new Tuple<Tuple<int, int>, int>(new Tuple<int, int>(7,11),21 ));
            
            Special.Add(new Tuple<Tuple<int, int>, int>(new Tuple<int, int>(5,1),31 ));
            Special.Add(new Tuple<Tuple<int, int>, int>(new Tuple<int, int>(9,1),31 ));
            Special.Add(new Tuple<Tuple<int, int>, int>(new Tuple<int, int>(6,2),21 ));
            Special.Add(new Tuple<Tuple<int, int>, int>(new Tuple<int, int>(8,2),21 ));
            Special.Add(new Tuple<Tuple<int, int>, int>(new Tuple<int, int>(7,3),21 ));
            
            return Special;
        }
        public static int Is_Special(int x, int y, List<Tuple<Tuple<int,int>,int>> Restant,int mode)
        {
            int tmp = 0;

            Tuple<Tuple<int, int>, int> tmptup = new Tuple<Tuple<int, int>, int>(new Tuple<int, int>(1, 1), 1); 
            foreach (var s in Restant)
            {
                if (s.Item1.Equals(new Tuple<int, int>(x, y)))
                {
                    tmp = s.Item2;
                    tmptup = s;
                    break;
                }
            }

            if (mode == 1 && tmp != 0)
                Restant.Remove(tmptup);
            
            return tmp;
        }

        public static string mots()
        {
            return
        }
    }
}