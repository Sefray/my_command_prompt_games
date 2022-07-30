using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.IO;
using System.Threading;

namespace Duel_Seven_Wonder
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            Seven_Wonder();
        }


        /////////////////////////////////////////////
        /********************************************
         *
         *    CLASS - STRUCT 
         *
         * *******************************************/

        public class Jeu
        {
            public List<Carte[,]> ages;
            public List<Joueur> joueurs;
            public HashSet<Carte> fosse;
            public List<int> progres;
            public int position;
            public List<Carte> merveille;

            public int age_actuel;
            public int carte_age_actuel;
        }

        public class Joueur
        {
            public int piece;
            public int victoire;

            public List<Carte> cartes;
            public List<Carte> merveilles_non_uti;
            public List<Carte> merveilles_uti;
            public List<int> progres;
            public List<int> symbole;

            public List<int> premiere;
            public List<int> manufacture;

            public HashSet<int> lien;

            public List<bool> bouclier;

            public List<int> cout;
            public List<int> type_nombre;
        }

        public class Carte
        {
            public Batiment batiment;

            public List<List<Cout_Effet>> cout;
            public List<Cout_Effet> effet;
        }

        public enum Batiment
        {
            premiere,
            manufacture,
            civil,
            scientifique,
            commerce,
            militaire,
            merveille,
            guilde
        }


        /////////////////////////////////////////////
        /********************************************
         *
         *    INITIALISATION
         *
         * *******************************************/

        public static Jeu Init_jeu()
        {
            Jeu jeu = new Jeu();

            jeu.ages = Init_ages();
            jeu.merveille = Init_merveilles();
            jeu.fosse = new HashSet<Carte>();
            jeu.progres = Init_progres();
            jeu.position = 0;

            jeu.joueurs = new List<Joueur>();
            jeu.joueurs.Add(Init_joueur());
            jeu.joueurs.Add(Init_joueur());

            jeu.age_actuel = 0;
            jeu.carte_age_actuel = 20;

            return jeu;
        }

        public static Joueur Init_joueur()
        {
            Joueur joueur = new Joueur();

            joueur.cout = new List<int>();
            for (int i = 0; i < 5; i++)
            {
                joueur.cout.Add(2);
            }

            joueur.symbole = new List<int>();
            for (int i = 0; i < 7; i++)
            {
                joueur.symbole.Add(0);
            }

            joueur.premiere = new List<int>();
            for (int i = 0; i < 3; i++)
            {
                joueur.premiere.Add(0);
            }

            joueur.manufacture = new List<int>();
            for (int i = 0; i < 3; i++)
            {
                joueur.manufacture.Add(0);
            }

            joueur.bouclier = new List<bool>();
            for (int i = 0; i < 2; i++)
            {
                joueur.bouclier.Add(false);
            }

            joueur.type_nombre = new List<int>();
            for (int i = 0; i < 7; i++)
            {
                joueur.type_nombre.Add(0);
            }

            joueur.piece = 7;

            joueur.cartes = new List<Carte>();
            joueur.merveilles_non_uti = new List<Carte>();
            joueur.merveilles_uti = new List<Carte>();
            joueur.progres = new List<int>();

            joueur.lien = new HashSet<int>();

            return joueur;
        }

        public static List<int> Init_progres()
        {
            List<int> l = new List<int>();
            while (l.Count != 5)
            {
                Thread.Sleep(15);
                int a = new Random().Next(0, 9);
                if (!l.Contains(a))
                    l.Add(a);
            }

            return l;
        }

        public static String get_ages_string() {
            return 
            "1 0 e 1 b X\n" +
            "1 0 e 1 p X\n" +
            "1 0 e 1 a X\n" +
            "1 0 c 1 g e 1 p X\n" +
            "1 0 c 1 g e 1 b X\n" +
            "1 0 c 1 g e 1 a X\n" +
            "1 1 c 1 g e 1 f X\n" +
            "1 1 c 1 g e 1 p X\n" +
            "1 2 c 1 p e 3 v 2 l X\n" +
            "1 2 e 3 v 0 l X\n" +
            "1 2 e 3 v 1 l X\n" +
            "1 3 c 2 g e 3 l 1 s X\n" +
            "1 3 c 2 g e 4 l 0 s X\n" +
            "1 3 c 1 f e 1 v 2 s X\n" +
            "1 3 c 1 h e 1 v 3 s X\n" +
            "1 4 e 4 g 5 l X\n" +
            "1 4 c 3 g e 1 P X\n" +
            "1 4 c 3 g e 1 A X\n" +
            "1 4 c 3 g e 1 B X\n" +
            "1 5 c 1 b e 1 o 6 l X\n" +
            "1 5 c 1 a e 1 o 7 l X\n" +
            "1 5 c 2 g e 1 o 8 l X\n" +
            "1 5 e 1 o X\n" +
            "2 0 c 2 g e 2 b X\n" +
            "2 0 c 2 g e 2 a X\n" +
            "2 0 c 2 g e 2 p X\n" +
            "2 1 e 1 h X\n" +
            "2 1 e 1 f X\n" +
            "2 2 c 2 l c 3 p e 5 v X\n" +
            "2 2 c 1 l c 1 b 1 h e 4 v 9 l X\n" +
            "2 2 c 0 l c 2 a e 4 v 10 l X\n" +
            "2 2 c 1 p 1 b e 4 v 11 l X\n" +
            "2 2 c 2 b 1 f e 5 v X\n" +
            "2 3 c 3 l c 2 a 1 p e 1 s 2 v X\n" +
            "2 3 c 4 l c 1 b 1 f 1 p e 0 s 2 v X\n" +
            "2 3 c 1 b 2 h e 2 s 1 v 12 l X\n" +
            "2 3 c 1 b 2 f e 3 s 1 v 13 l X\n" +
            "2 4 e 6 g 16 l X\n" +
            "2 4 c 3 g 1 a e 1 f 1 h X\n" +
            "2 4 c 4 g e 1 F 1 H X\n" +
            "2 4 c 2 g 1 f 1 h e 1 b 1 a 1 p X\n" +
            "2 5 c 7 l c 3 g e 1 o X\n" +
            "2 5 c 6 l c 1 a 1 b e 1 o X\n" +
            "2 5 c 2 p e 2 o X\n" +
            "2 5 c 2 a 1 f e 2 o 14 l X\n" +
            "2 5 c 1 p 1 b 1 h e 2 o 15 l X\n" +
            "3 2 c 3 p 2 b e 7 v X\n" +
            "3 2 c 11 l c 2 a 1 p 1 h e 5 v X\n" +
            "3 2 c 9 l c 2 h 1 b 1 a e 6 v X\n" +
            "3 2 c 1 a 1 p 1 b 2 f e 7 v X\n" +
            "3 2 c 2 p 1 f e 5 v X\n" +
            "3 2 c 10 l c 2 a 2 b e 6 v X\n" +
            "3 3 c 2 b 1 f 1 h e 3 v 4 s X\n" +
            "3 3 c 1 p 1 b 2 f e 4 s 3 v X\n" +
            "3 3 c 13 l c 1 p 2 h e 5 s 2 v X\n" +
            "3 3 c 12 l c 1 a 1 f 1 h e 5 s 2 v X\n" +
            "3 4 c 1 b 1 f 1 h e 2 M 3 v X\n" +
            "3 4 c 2 h e 3 G 3 v X\n" +
            "3 4 c 5 l c 2 a 1 f e 1 D 3 v X\n" +
            "3 4 c 16 l c 1 a 1 p 1 b e 3 v 2 L X\n" +
            "3 4 c 2 p 1 f e 3 v 1 R X\n" +
            "3 5 c 8 g e 3 o X\n" +
            "3 5 c 15 l c 3 b 1 f e 2 o X\n" +
            "3 5 c 14 l c 2 p 2 a e 2 o X\n" +
            "3 5 c 8 l 2 p 1 a 1 h e 2 o X\n" +
            "3 5 c 3 a 2 b e 3 o X\n" +
            "3 4 c 2 g e 1 R 3 v X\n" +
            "4 7 c 2 b 1 a 1 h e 0 B X\n" +
            "4 7 c 2 a 2 b e 0 V X\n" +
            "4 7 c 1 a 1 b 1 f 1 h e 1 D X\n" +
            "4 7 c 2 p 1 a 1 h e 1 R X\n" +
            "4 7 c 2 p 1 a 1 b 1 f e 0 L X\n" +
            "4 7 c 2 p 2 b e 3 P X\n" +
            "4 7 c 1 a 1 p 1 f 1 h e 0 M 0 G X";
        }

        public static List<Carte[,]> Init_ages()
        {
            List<List<Carte>> ages = new List<List<Carte>>();
            for (int i = 0; i < 4; i++)
            {
                ages.Add(new List<Carte>());
            }

            String ages_txt = get_ages_string();

            foreach (var age_line in ages_txt.Split('\n'))
            {
                string[] line = age_line.Split(' ');
                Carte carte = Init_carte(line);
                ages[Int32.Parse(line[0]) - 1].Add(carte);
            }

            List<Carte[,]> age = new List<Carte[,]>();
            for (int i = 0; i < 2; i++)
            {
                Enlever_cartes(ages[i], 3);
                Melanger(ages[i]);
                age.Add(Placer_Ages(ages[i], i));
            }
            Enlever_cartes(ages[2], 3);
            Enlever_cartes(ages[3], 4);
            foreach (var carte in ages[3])
            {
                ages[2].Add(carte);
            }
            Melanger(ages[2]);
            age.Add(Placer_Ages(ages[2], 2));
            return age;
        }

        public static Carte[,] Placer_Ages(List<Carte> l, int n)
        {
            switch (n)
            {
                case 0:
                    return Premier_Age(l);
                case 1:
                    return Deuxieme_Age(l);
                default:
                    return Troisieme_Age(l);
            }
        }

        public static Carte[,] Init_array(int w, int h)
        {
            Carte[,] ret = new Carte[h, w];
            for (int i = 0; i < h; i++)
            {
                for (int j = 0; j < w; j++)
                {
                    ret[i, j] = null;
                }
            }

            return ret;
        }

        public static Carte[,] Premier_Age(List<Carte> age_1)
        {
            Carte[,] ret = Init_array(11, 5);
            int k = 0;
            for (int j = 0; j < 5; j++)
            {
                for (int i = 5 - j - 1; i <= 5 + j + 1; i += 2)
                {
                    ret[j, i] = age_1[k];
                    k++;
                }
            }

            return ret;
        }

        public static Carte[,] Deuxieme_Age(List<Carte> age_2)
        {
            Carte[,] ret = Init_array(11, 5);

            int k = 0;
            for (int j = 0; j < 5; j++)
            {
                for (int i = 5 - j - 1; i <= 5 + j + 1; i += 2)
                {
                    ret[4 - j, i] = age_2[k];
                    k++;
                }
            }

            return ret;
        }

        public static Carte[,] Troisieme_Age(List<Carte> age_3)
        {
            Carte[,] ret = Init_array(7, 7);
            int k = 0;

            for (int j = 0; j < 3; j++)
            {
                for (int i = 3 - j - 1; i <= 3 + j + 1; i += 2)
                {
                    ret[j, i] = age_3[k];
                    k++;
                }
            }

            for (int i = 1; i < 6; i += 4)
            {
                ret[3, i] = age_3[k];
                k++;
            }

            for (int j = 0; j < 3; j++)
            {
                for (int i = 3 - j - 1; i <= 3 + j + 1; i += 2)
                {
                    ret[6 - j, i] = age_3[k];
                    k++;
                }
            }

            return ret;
        }

        public static void Melanger(List<Carte> cartes)
        {
            int lim = cartes.Count * 100;
            for (int i = 0; i < 40; i++)
            {
                int a = new Random().Next(lim);
                Swap(a / 100, a % 100 % cartes.Count, cartes);
                Thread.Sleep(50);
            }

            int h = 0;
        }

        public static void Swap(int a, int b, List<Carte> list)
        {
            Carte tmp = list[a];
            list[a] = list[b];
            list[b] = tmp;
        }

        public static void Enlever_cartes(List<Carte> cartes, int n)
        {
            for (int i = 0; i < n; i++)
            {
                int a = new Random().Next(230);
                cartes.RemoveAt(a % cartes.Count);
                Thread.Sleep(50);
            }
        }

        public static List<Carte> Init_merveilles()
        {
            List<Carte> l = new List<Carte>();
            using (StreamReader streamReader = new StreamReader("merveilles.txt"))
            {
                string lecture = streamReader.ReadLine();
                while (lecture != null)
                {
                    string[] line = lecture.Split(' ');
                    Carte carte = Init_carte(line);
                    l.Add(carte);
                    lecture = streamReader.ReadLine();
                }
            }

            Melanger(l);
            Enlever_cartes(l, 4);
            return l;
        }

        public class Cout_Effet
        {
            public string nature;
            public int n;
        }

        public static Carte Init_carte(string[] line)
        {
            int indice = 2;

            Carte carte = new Carte();

            carte.batiment = (Batiment) Int32.Parse(line[1]);

            carte.cout = new List<List<Cout_Effet>>();
            carte.effet = new List<Cout_Effet>();

            while (line[indice] != "e")
            {
                List<Cout_Effet> list = new List<Cout_Effet>();
                indice += 1;

                while (line[indice] != "c" && line[indice] != "e")
                {
                    Cout_Effet c = Init_Cout_Effet(line, indice);
                    list.Add(c);
                    indice += 2;
                }

                carte.cout.Add(list);
            }

            indice += 1;
            while (line[indice] != "X")
            {
                Cout_Effet e = Init_Cout_Effet(line, indice);
                carte.effet.Add(e);
                indice += 2;
            }

            return carte;
        }

        public static Cout_Effet Init_Cout_Effet(string[] line, int indice)
        {
            Cout_Effet ce = new Cout_Effet();
            ce.nature = line[indice + 1];
            ce.n = Int32.Parse(line[indice]);
            return ce;
        }

        /////////////////////////////////////////////
        /********************************************
         *
         *    EFFETS CARTES
         *
         * *******************************************/

        public static Dictionary<string, Func<Jeu, int, int, int, int>> Correspondance_Effet =
            new Dictionary<string, Func<Jeu, int, int, int, int>>
            {
                {"a", Matiere},
                {"p", Matiere},
                {"b", Matiere},

                {"f", Manufacture},
                {"h", Manufacture},

                {"l", Lien},
                {"s", Scientifique},

                {"o", Bouclier},

                {"v", Victoire},
                {"g", Piece},

                {"A", Cout_Premiere},
                {"P", Cout_Premiere},
                {"B", Cout_Premiere},

                {"F", Cout_Manufacture},
                {"H", Cout_Manufacture},

                {"R", Rejouer},
                {"G", Piece_moins},
                {"J", Merveille_jeton},
                {"M", Detruire_matiere},
                {"I", Detruire_manufacture},
                {"D", Depuis_fosse}
                
            };

        public static Dictionary<string, int> Correspondance_Objet = new Dictionary<string, int>()
        {
            {"a", 0},
            {"b", 1},
            {"p", 2},

            {"f", 0},
            {"h", 1},

            {"l", 0},
            {"s", 0},

            {"o", 0},

            {"v", 0},
            {"g", 0},

            {"A", 0},
            {"P", 2},
            {"B", 1},

            {"F", 0},
            {"H", 1}
        };

        public static Dictionary<string, int> Correspondance_Type = new Dictionary<string, int>()
        {
            {"M", 0},
            {"G", 1},
            {"V", 2},
            {"D", 3},
            {"R", 4}
        };

        public static Dictionary<Batiment, ConsoleColor> Famille_color = new Dictionary<Batiment, ConsoleColor>()
        {
            {Batiment.premiere, ConsoleColor.DarkGray},
            {Batiment.civil, ConsoleColor.Blue},
            {Batiment.commerce, ConsoleColor.DarkYellow},
            {Batiment.guilde, ConsoleColor.Magenta},
            {Batiment.militaire, ConsoleColor.DarkRed},
            {Batiment.manufacture, ConsoleColor.Gray},
            {Batiment.scientifique, ConsoleColor.Green}
        };
        
        public static int Matiere(Jeu jeu, int joueur, int n, int type)
        {
            jeu.joueurs[joueur].type_nombre[0]++;
            jeu.joueurs[joueur].premiere[type] += n;
            if (jeu.joueurs[(joueur + 1) % 2].cout[type] != 1)
                jeu.joueurs[(joueur + 1) % 2].cout[type] += n;
            return 0;
        }

        public static int Manufacture(Jeu jeu, int joueur, int n, int type)
        {
            jeu.joueurs[joueur].type_nombre[1]++;
            jeu.joueurs[joueur].manufacture[type] += n;
            if (jeu.joueurs[(joueur + 1) % 2].cout[type + 3] != -1)
                jeu.joueurs[(joueur + 1) % 2].cout[type + 3] += n;
            return 0;
        }

        public static int Lien(Jeu jeu, int joueur, int n, int type)
        {
            jeu.joueurs[joueur].lien.Add(n);
            return 0;
        }

        public static int Scientifique(Jeu jeu, int joueur, int n, int type)
        {
            jeu.joueurs[joueur].symbole[n] += 1;
            if (jeu.joueurs[joueur].symbole[n] == 2)
                Jeton(jeu, joueur);
            return Victoire_scientifique(jeu, joueur);
        }
        public static int Victoire_scientifique(Jeu jeu, int joueur)
        {
            int k = 0;
            for (int i = 0; i < 7; i++)
            {
                if (jeu.joueurs[joueur].symbole[i] > 0)
                    k++;
            }

            if (k >= 6)
                return -1;
            return 0;
        }

        public static int Bouclier(Jeu jeu, int joueur, int n, int type)
        {
            if (jeu.joueurs[joueur].progres.Contains(0))
                n += 1;
            jeu.position += (joueur == 1 ? -1 : 1) * n;
            int pos = Math.Abs(jeu.position);
            if (pos >= 3)
            {
                if (pos >= 6)
                {
                    if (pos >= 9)
                        return 1;
                    if (!jeu.joueurs[(joueur + 1) % 2].bouclier[1])
                        Piece(jeu, (joueur + 1) % 2, -5, 0);
                    jeu.joueurs[(joueur + 1) % 2].bouclier[1] = true;
                }

                if (!jeu.joueurs[(joueur + 1) % 2].bouclier[0])
                    Piece(jeu, (joueur + 1) % 2, -2, 0);
                jeu.joueurs[(joueur + 1) % 2].bouclier[0] = true;
            }

            return 0;
        }

        public static int Piece(Jeu jeu, int joueur, int n, int type)
        {
            jeu.joueurs[joueur].piece += n;
            if (jeu.joueurs[joueur].piece < 0)
                jeu.joueurs[joueur].piece = 0;
            return 0;
        }

        public static int Victoire(Jeu jeu, int joueur, int n, int type)
        {
            jeu.joueurs[joueur].victoire += n;
            return 0;
        }

        public static int Cout_Manufacture(Jeu jeu, int joueur, int n, int type)
        {
            jeu.joueurs[joueur].cout[type + 3] = 1;
            return 0;
        }

        public static int Cout_Premiere(Jeu jeu, int joueur, int n, int type)
        {
            jeu.joueurs[joueur].cout[type] = 1;
            return 0;
        }

        public static void Jeton(Jeu jeu, int joueur)
        {
            int choix = 5; // Choix_Manuelle(jeu.progres);
            jeu.progres.Add(choix);
            switch (choix)
            {
                case 2:
                    Piece(jeu, joueur, 6, 0);
                    Victoire(jeu, joueur, 4, 0);
                    break;
                case 6:
                    Victoire(jeu, joueur, 7, 0);
                    break;
                case 7:
                    jeu.joueurs[joueur].symbole[6]++;
                    break;
                case 8:
                    Piece(jeu, joueur, 6, 0);
                    break;
            }

            jeu.joueurs[joueur].victoire += 3;
        }
        public static int Rejouer(Jeu jeu, int joueur, int n, int type)
        {
            return 2;

        }
        public static int Piece_moins(Jeu jeu, int joueur, int n, int type)
        {
            Piece(jeu, (joueur + 1) % 2, -n, 0);
            return 0;
        }
        public static int Merveille_jeton(Jeu jeu, int joueur, int n, int type)
        {
            Jeton(jeu, joueur);
            return Victoire_scientifique(jeu, joueur);
        }
        public static int Detruire_matiere(Jeu jeu, int joueur, int n, int type)
        {
            Detruire(jeu, joueur, Batiment.premiere);
            return 0;
        }
        public static int Detruire_manufacture(Jeu jeu, int joueur, int n, int type)
        {
            Detruire(jeu, joueur, Batiment.manufacture);
            return 0;
        }

        public static void Detruire(Jeu jeu, int joueur, Batiment batiment)
        {
            List<Carte> l = new List<Carte>();
            foreach (var c in jeu.joueurs[(joueur+1)%2].cartes)
            {
                if (c.batiment == batiment)
                    l.Add(c);
            }

            Carte carte = null;//Choix_carte(joueur, l);
            Joueur j = jeu.joueurs[(joueur + 1) % 2];
            j.cartes.Remove(carte);
            jeu.fosse.Add(carte);
            j.type_nombre[(int) batiment]--;
            j.premiere[Correspondance_Type[carte.effet[0].nature]] -= carte.effet[0].n;
            if (jeu.joueurs[joueur].cout[Correspondance_Type[carte.effet[0].nature]] != -1)
                jeu.joueurs[joueur].cout[Correspondance_Type[carte.effet[0].nature] + ( batiment == Batiment.premiere ? 0 : 3)] -= carte.effet[0].n;
        }

        public static int Depuis_fosse(Jeu jeu, int joueur, int n, int type)
        {
            List<Carte> l = new List<Carte>();
            foreach (var c in jeu.fosse)
            {
                l.Add(c);
            }

            Carte carte = null; //Choix_carte(joueur, l);
            int p = jeu.joueurs[joueur].piece;
            jeu.joueurs[joueur].piece += 100;
            int ret = Invoquer_Carte(jeu,joueur,carte);
            jeu.joueurs[joueur].piece = p;
            return ret;
        }
        
        /////////////////////////////////////////////
        /********************************************
         *
         *    ACTION
         * 
         * *******************************************/

        public static void Choix_merveille(Jeu jeu)
        {
            List<Carte> l2 = new List<Carte>();
            List<Carte> l1 = new List<Carte>();
            for (int i = 0; i < 4; i++)
            {
                l2.Add(jeu.merveille[i]);
                l1.Add(jeu.merveille[4+i]);
            }
            Choix_merveille_bis(jeu,0,l1);
            Choix_merveille_bis(jeu,1,l2);
            jeu.merveille.Clear();
        }

        public static void Choix_merveille_bis(Jeu jeu, int n, List<Carte> merveille)
        {
            int x = 85;
            int y = 22;

            int z = 0;
            int k = 4;
            ConsoleKey pressed;
            while (k != 0)
            {
                Affichage_actuel_joueur(n);
                Init_Choix();
                int g = 0;
                Console.SetCursorPosition(x-5,y-4);
                Console.ForegroundColor = ConsoleColor.Black;
                Console.Write("Choisissez vos merveilles!");
                foreach (var m in merveille)
                {
                    Console.SetCursorPosition(x,y);
                    if (m != null)
                    {
                        Affichage_effet_carte(m.cout[0], x, y + g*2, false);
                        Affichage_effet_carte(m.effet, x + 8,  g*2+y, false);
                    }
                    g++;
                }
                
                Console.ForegroundColor = ConsoleColor.Black;
                Console.BackgroundColor = ConsoleColor.White;
                int y_bef = 0;
                do
                {
                    print_cursor(x - 3, y_bef, y + 2*z);
                    pressed = Console.ReadKey().Key;
                    y_bef = y+2*z;
                    int a = 0;
                    switch (pressed)
                    {
                        case ConsoleKey.UpArrow:
                            a = 3;
                            break;
                        case ConsoleKey.DownArrow:
                            a = 1;
                            break;
                        case ConsoleKey.J:
                            Information_jetons(jeu);
                            break;
                    }
                    do
                    {
                        z = (z + a) % 4;
                    } while (merveille[z] == null);

                }while (pressed != ConsoleKey.Enter && pressed != ConsoleKey.J);

                if (pressed == ConsoleKey.J)
                    continue;
                jeu.joueurs[n].merveilles_non_uti.Add(merveille[z]);
                merveille[z] = null;
                k--;
                if (k == 3 || k == 1)
                    n = (n + 1) % 2;
            }
        }

        public static void print_cursor(int x, int y_bef, int y)
        {
            Console.SetCursorPosition(x,y_bef);
            Console.Write("  ");
            Console.SetCursorPosition(x,y);
            Console.Write(">");
        }

        public static int Invoquer_Carte(Jeu jeu, int joueur, Carte carte)
        {
            int cout = Cout(jeu,joueur,carte);
            if (cout != 0)
                Piece(jeu, joueur, -cout, 0);
            jeu.joueurs[joueur].cartes.Add(carte);
            jeu.joueurs[joueur].type_nombre[(int) carte.batiment] += 1;
            return Effet(jeu, joueur, carte);
        }
        public static int Invoquer_Merveille(Jeu jeu, int joueur)
        {
            Carte carte = Joueur_merveille_choix(jeu,joueur,jeu.joueurs[joueur].merveilles_non_uti);
            jeu.joueurs[joueur].merveilles_non_uti.Remove(carte);
            jeu.joueurs[joueur].merveilles_uti.Add(carte);
            return Invoquer_Carte(jeu, joueur, carte);
        }
        public static void Deffosser(Jeu jeu, int joueur)
        {
            Piece(jeu, joueur, jeu.joueurs[joueur].type_nombre[4] + 2, 0);
        }
        
        public static int Cout(Jeu jeu, int joueur, Carte carte)
        {
            int ret = 0;
            int g = 0;

            Joueur j = jeu.joueurs[joueur];
            foreach (var cout in carte.cout)
            {
                foreach (var poss in cout)
                {
                    switch (poss.nature)
                    {
                        case "a":
                        case "b":
                        case "p":
                            if (j.premiere[Correspondance_Objet[poss.nature]] < poss.n)
                                ret += Payer(j.premiere, poss, j.cout, 0);
                            break;
                        case "h":
                        case "f":
                            if (j.manufacture[Correspondance_Objet[poss.nature]] < poss.n)
                                ret += Payer(j.manufacture, poss, j.cout, 2);
                            break;
                        
                        case "l":
                            if (jeu.joueurs[joueur].symbole.Contains(poss.n))
                            {
                                if (jeu.joueurs[joueur].progres.Contains(8))
                                    Piece(jeu, joueur, 4, 0);
                                return 0;
                            }
                            break;
                        
                        case "g":
                            g += poss.n;
                            break;
                    }
                }
            }
            
            if (jeu.joueurs[(joueur+1)%2].progres.Contains(1))
                Piece(jeu, (joueur+1)%2, ret, 0);
            return ret + g;
        }
        public static int Payer(List<int> possession, Cout_Effet ce, List<int> Cout, int indice)
        {
            int indic_obj = Correspondance_Objet[ce.nature];
            return Cout[indic_obj + indice] * (ce.n - possession[indic_obj]);
        }
        public static int Effet(Jeu jeu, int joueur, Carte carte)
        {
            int etat = 0;
            foreach (var effet in carte.effet)
            {
                Func<Jeu, int, int, int, int> f = Correspondance_Effet[effet.nature];
                int tmp = f(jeu, joueur, effet.n, Correspondance_Objet[effet.nature]);
                if (tmp*tmp == 1)
                    return tmp;
                etat = etat == 2 ? 2 : tmp;
            }

            if (carte.batiment == Batiment.merveille && jeu.joueurs[joueur].progres.Contains(3))
                etat = 2;
            return etat;
        }
        
        
        /////////////////////////////////////////////
        /********************************************
         *
         *    VAINQUEUR
         *
         * *******************************************/

        public static void Affichage_Vainqueur(int joueur, string type_victoire)
        {
            Console.WriteLine("Le joueur {0} à gagné {1}", joueur, type_victoire);
        }
        public static void Victoire_Point(Jeu jeu)
        {
            int p1 = Point_Joueur(jeu.joueurs[0]);
            int p2 = Point_Joueur(jeu.joueurs[1]);
            Affichage_Vainqueur(p1 > p2 ? 1 : 2, "grâce aux point de victoire : " + p1 + " - "+ p2 + "!");
        }
        public static int Point_Joueur(Joueur j)
        {
            int ret = j.victoire;
            if (j.progres.Contains(3))
                ret += 3 * j.progres.Count;
            if (j.type_nombre[ (int) Batiment.guilde] != 0)
            {
                foreach (var carte in j.cartes)
                {
                    if (carte.batiment == Batiment.guilde)
                    {
                        foreach (var effet in carte.effet)
                        {
                            switch (effet.nature)
                            {
                                case "L":
                                    ret += j.type_nombre[6] * 2;
                                    break;
                                case "P":
                                    ret += j.piece / 3;
                                    break;
                                default:
                                    ret += j.type_nombre[Correspondance_Type[effet.nature]];
                                    break;
                            }
                        }
                    }
                }
            }
            return ret;
        }

        public static void Fin(Jeu jeu, int joueur, int etat)
        {
            if (etat * etat == 1)
            {
                if (etat == -1)
                    Affichage_Vainqueur(joueur, " par victoire scientifique!");
                else
                    Affichage_Vainqueur(joueur, " par victoire militaire!");
            }
            else
                Victoire_Point(jeu);
        }
        
        /////////////////////////////////////////////
        /********************************************
         *
         *    AFFICHAGE
         *
         * *******************************************/

        public static void Affichage(Jeu jeu, int joueur)
        {
            Console.CursorVisible = false;
            int milieu = 99;
            Console.ResetColor();
            Console.Clear();
            Affichage_plateau(jeu, milieu);
            Affichage_cartes(jeu.ages[jeu.age_actuel], milieu, jeu.age_actuel == 2);

            int x_j1 = 2;
            int x_j2 = 160;
            
            Affichage_joueur(jeu.joueurs[0], x_j1);
            Affichage_joueur(jeu.joueurs[1], x_j2);

            Affichage_actuel_joueur(joueur);
        }

        public static void Affichage_actuel_joueur(int joueur)
        {
            switch (joueur)
            {
                case 0:
                    Console.BackgroundColor = ConsoleColor.Green;
                    Affichage_joueur_actuel(0);
                    Console.BackgroundColor = ConsoleColor.White;
                    Affichage_joueur_actuel(158);
                    break;
                case 1:
                    Console.BackgroundColor = ConsoleColor.Green;
                    Affichage_joueur_actuel(158);
                    Console.BackgroundColor = ConsoleColor.White;
                    Affichage_joueur_actuel(0);
                    break;
            }
        }

        public static void Affichage_cartes(Carte[,] formation, int milieu, bool age_2)
        {
            Console.BackgroundColor = ConsoleColor.White;
            Rectangle(milieu - 56, 12, 112, 35);

            for (int i = 0; i < formation.GetLength(0); i++)
            {
                for (int j = 0; j < formation.GetLength(1); j++)
                {
                    if (formation[i, j] != null)
                        Afficher_carte(j, i, formation, milieu - ( age_2 ? 22 : 34) );
                }
            }
        }

        public static void Afficher_carte(int x, int y, Carte[,] cartes, int init_x)
        {
            Carte carte = cartes[y, x];

            Console.ForegroundColor = ConsoleColor.Black;
            bool vis = (Visible(cartes, x, y));
            for (int i = 0; i < 3; i++)
            {
                Console.BackgroundColor = vis ? Famille_color[carte.batiment] : ConsoleColor.White;
                Rectangle(x * 6 + init_x + i, y * 4 + 14  + i, 8- i*2,5 -i*2);
            }
            if (vis)
            {
                x = x * 6 + init_x+2;
                y = y * 4 + 15;
                
                Console.ForegroundColor = ConsoleColor.Black;
                Affichage_effet_carte(carte.effet,x, y,false);
                y += 1;
                Console.BackgroundColor = Famille_color[carte.batiment];
                Console.SetCursorPosition(x-2,y);
                Console.Write("---------");
                y += 1;
                foreach (var cout in carte.cout)
                {
                    Affichage_effet_carte(cout, x, y,false);
                    y += 1;
                }
            }
        }

        public static bool Visible(Carte[,] cartes, int x, int y)
        {
            if (y % 2 == 0)
                return true;
            return Accessible(cartes, x, y);
        }
        public static void Rectangle(int x, int y, int lon_x, int lon_y)
        {
            Console.SetCursorPosition(x,y);
            for (int i = 0; i <= lon_x; i++)
            {
                Console.Write(" ");
            }
            for (int i = 0; i < lon_y; i++)
            {
                Console.SetCursorPosition(x,y + i);
                Console.Write(" ");
                Console.SetCursorPosition(x + lon_x, y + i);
                Console.Write(" ");
            }
            Console.SetCursorPosition(x,y + lon_y);
            for (int i = 0; i <= lon_x; i++)
            {
                Console.Write(" ");
            }
        }
        public static void Affichage_plateau(Jeu jeu, int milieu)
        {
            int y = 5;

            Console.BackgroundColor = ConsoleColor.White;
            Rectangle(milieu - 56, 2, 112, 8);
                
            Console.BackgroundColor = ConsoleColor.DarkRed;
            for (int i = -9; i <10; i++)
            {
                Console.SetCursorPosition(milieu+2*i,y);
                Console.Write(" ");
            }
            
            Console.BackgroundColor = ConsoleColor.Red;
            Console.SetCursorPosition(milieu+2*jeu.position,y);
            Console.Write("X");
            
            Affichage_bouclier(jeu.joueurs[0].bouclier,-1,milieu, y);
            Affichage_bouclier(jeu.joueurs[1].bouclier,1,milieu, y);

            Affichage_jeton(jeu.progres, milieu - 12, y + 3);
            
            Console.SetCursorPosition(163, 4);
            Console.Write("Information jetons : J");
        }
        public static void Affichage_bouclier(List<bool> l, int sens, int milieu, int y)
        {
            int x = milieu + sens * 5;
            Console.BackgroundColor = ConsoleColor.DarkYellow;
            string s = "2";
            foreach (var b in l)
            {
                if (!b)
                {
                    Console.SetCursorPosition(x,y );
                    Console.Write(s);
                }
                x = milieu + sens * 11 ;
                s = "5";
            }

            Console.BackgroundColor = ConsoleColor.Blue;
            y += 1;
            Affichage_victoire_point(milieu, y , "0");
            
            Affichage_victoire_point(milieu + 2, y , " 2 ");
            Affichage_victoire_point(milieu - 4, y , " 2 ");
            
            Affichage_victoire_point(milieu + 6, y , "  5  ");
            Affichage_victoire_point(milieu - 10, y, "  5  ");
            
            Affichage_victoire_point(milieu + 12, y , " 10  ");
            Affichage_victoire_point(milieu - 16, y , " 10  ");
            
        }
        public static void Affichage_victoire_point(int x, int y, string s)
        {
            Console.SetCursorPosition(x,y );
            Console.Write(s);
        }

        public static void Affichage_joueur_actuel(int x)
        {
            Rectangle(x,12,40,35);
        }
        public static void Ligne(int x, int y)
        {
            Console.ResetColor();
            Console.SetCursorPosition(x,y);
            Console.Write("-------------------");
        }
        public static void Affichage_joueur(Joueur joueur, int x)
        {
            int y = 30;
            Affichage_joueur_merveille(joueur, x);
            Affichage_jeton(joueur.progres, x, y + 14);
            Console.SetCursorPosition(x, y + 15);
            Console.Write("Pièce : {0}",joueur.piece);
            
            Ligne(x,y-1);
            int x_min = x;
            y++;
            for (int i = 0; i < 6; i++)
            {
                x_min = x;
                foreach (var carte in joueur.cartes)
                {
                    if (carte.batiment == (Batiment) i)
                    {
                        x_min = Afficher_carte_joueur(carte, x_min, y + 2 * i);
                    }
                }
                Console.ResetColor();
                if (x_min != x)
                    Console.Write("|");
            }

            x_min = x;
            foreach (var carte in joueur.cartes)
            {
                if (carte.batiment == Batiment.guilde)
                {
                    x_min = Afficher_carte_joueur(carte, x_min, y + 12);
                }
            }
            if (x_min != x)
                Console.Write("|");
            Ligne(x,y+13);
        }
        public static int Afficher_carte_joueur(Carte carte, int x, int y)
        {
            Console.ResetColor();
            Console.SetCursorPosition(x, y);
            Console.Write("|");
            return Affichage_effet_carte(carte.effet, x + 1, y,true);
        }
        public static void Affichage_jeton(List<int> l, int x, int y )
        {
            Console.ResetColor();
            Console.SetCursorPosition(x, y);
            Console.Write("Jetons de progres: ");
            foreach (var i in l)
            {
                Console.BackgroundColor = ConsoleColor.DarkGreen;
                Console.Write(i);
                Console.ResetColor();
                Console.Write(" ");
            }
            Console.ResetColor();
        }
        public static void Affichage_joueur_merveille(Joueur joueur, int x)
        {
            int y = 16;
            List<Carte> uti = joueur.merveilles_uti;
            List<Carte> non_uti = joueur.merveilles_non_uti;

            Console.ResetColor();
            Console.SetCursorPosition(x,y-2);
            Console.Write("Merveille non utilisée:");

            foreach (var merveille in non_uti)
            {
                Console.SetCursorPosition(x,y);
                Affichage_effet_carte(merveille.cout[0],x,y,false);
                Affichage_effet_carte(merveille.effet, x + 8,y,false);
                y+=2;
            }
            
            Console.ResetColor();
            Console.SetCursorPosition(x,y+1);
            Console.Write("Merveille utilisée:");
            y += 2;
            foreach (var merveille in uti)
            {
                Console.SetCursorPosition(x,y);
                Affichage_effet_carte(merveille.effet, x ,y,true);
                y+=2;
            }
        }

        public static int Affichage_effet_carte(List<Cout_Effet> carte, int x, int y, bool cout)
        {
            Console.SetCursorPosition(x,y);
            int x_min = x;
            foreach (var effet in carte)
            {
                if (cout && "gGMLRDJ".Contains(effet.nature))
                    continue;
                
                Console.BackgroundColor = Matiere_Color[effet.nature.ToLower()];
                if (effet.nature.ToUpper() == effet.nature)
                {
                    Console.Write(effet.nature);
                }
                else
                {
                    Console.Write(effet.n);
                }
                x_min += 1;
            }
            return x_min;
        }

        public static Dictionary<string, ConsoleColor> Matiere_Color = new Dictionary<string, ConsoleColor>()
        {
            {"f", ConsoleColor.Cyan},
            {"b", ConsoleColor.DarkGreen},
            {"p", ConsoleColor.DarkGray},
            {"a", ConsoleColor.Red},
            {"h", ConsoleColor.DarkYellow},
            {"o", ConsoleColor.DarkRed},
            {"v",ConsoleColor.DarkBlue},
            {"s",ConsoleColor.DarkGreen},
            {"m",ConsoleColor.DarkYellow},
            {"i",ConsoleColor.Gray},
            {"r",ConsoleColor.White},
            {"g", ConsoleColor.Yellow},
            {"j", ConsoleColor.Green},
            {"l",ConsoleColor.White},
            {"d",ConsoleColor.DarkYellow}
        };

        
        
        /////////////////////////////////////////////
        /********************************************
         *
         *    CHOIX
         *
         * *******************************************/
        
        public static void Init_Choix()
        {
            Console.BackgroundColor = ConsoleColor.Gray;
            Rectangle(68,15,62,20);

            Console.BackgroundColor = ConsoleColor.White;
            for (int i = 1; i < 62; i++)
            {
                for (int j = 1; j < 20; j++)
                {
                    Console.SetCursorPosition(68+i,15+j);
                    Console.Write(" ");
                }
            }
        }
        public static int Choix_Action(Jeu jeu, int joueur, Carte carte)
        {
            List<string> ecr = new List<string>() {"Choix de l'action:","Invoquer la carte", "Deffosser la carte", "Invoquer une merveille"};
            int pos;
            bool possible_to_invoq;
            do
            {
                pos = Choix(ecr);
                possible_to_invoq = true;
                if (pos != 1)
                {
                    if (pos == 0)
                        possible_to_invoq = Invocable_carte(jeu, joueur, carte);
                    else
                        possible_to_invoq = Invocable_merveille(jeu, joueur);
                }
            } while (!possible_to_invoq);
            return pos;
        }

        public static int Choix(List<string> ecr)
        {
            ConsoleKey key;
            int pos = 0;
            do
            {
                Init_Choix();
                Console.SetCursorPosition(93, 18);
                Console.ForegroundColor = ConsoleColor.Black;
                Console.Write(ecr[0]);
                int a = 21;
                for (int i = 1; i < ecr.Count; i++)
                {
                    Console.SetCursorPosition(95, a);
                    Console.Write(ecr[i]);
                    a += 3;
                }
                Console.SetCursorPosition(93, 21 + 3 * pos);
                Console.Write(">");
                key = Console.ReadKey().Key;
                if (key == ConsoleKey.UpArrow)
                    pos = (pos + 3 - 1) % 3;
                if (key == ConsoleKey.DownArrow)
                    pos = (pos + 1) % 3;
            } while (key != ConsoleKey.Enter);
            return pos;
        }
        public static bool Invocable_carte(Jeu jeu, int joueur, Carte carte)
        {
            int p0 = jeu.joueurs[0].piece;
            int p1 = jeu.joueurs[1].piece;

            int cout = Cout(jeu, joueur, carte);

            jeu.joueurs[0].piece = p0;
            jeu.joueurs[1].piece = p1;
            
            if (joueur == 0)
            {
                if (cout > p0)
                    return false;
            }
            if (joueur == 1)
            {
                if (cout > p1)
                    return false;
            }
            return true;
        }
        public static bool Invocable_merveille(Jeu jeu, int joueur)
        {
            foreach (var merv in jeu.joueurs[joueur].merveilles_non_uti)
            {
                if (Invocable_carte(jeu, joueur, merv))
                    return true;
            }
            return false;
        }
        public static int Prochain_joueur(Jeu jeu, int joueur)
        {
            if (jeu.position < 0)
                return 0;
            if (jeu.position > 0)
                return 1;
            return joueur;
        }
        public static Carte Joueur_merveille_choix(Jeu jeu, int joueur, List<Carte> non_uti)
        {
            int x = joueur == 0 ? 1 : 150;
            ConsoleKey key;
            int pos = 0;
            int old = pos;
            Affichage(jeu,joueur);
            do
            {
                Console.ResetColor();
                Console.SetCursorPosition(x + 17, 16 + 2 * old);
                Console.Write(" ");
                
                Console.SetCursorPosition(x + 17, 16 + 2 * pos);
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write("<");

                old = pos;
                key = Console.ReadKey().Key;
                if (key == ConsoleKey.UpArrow)
                    pos = (pos + non_uti.Count - 1) % non_uti.Count;
                if (key == ConsoleKey.DownArrow)
                    pos = (pos + 1) % non_uti.Count;
            } while (key != ConsoleKey.Enter || !Invocable_carte(jeu,joueur,non_uti[pos]));
            return non_uti[pos];
        }

        public static bool Accessible(Carte[,] cartes, int x, int y)
        {
            if (cartes[y, x] == null)
                return false;
            if (y < cartes.GetLength(0)-1)
            {
                if (x != 0 && cartes[y + 1, x - 1] != null)
                    return false;
                if (x < cartes.GetLength(1)-1 && cartes[y + 1, x + 1] != null)
                    return false;
            }
            return true;
        }

        public static Carte Choix_Carte(Jeu jeu,int joueur)
        {
            Carte[,] pl = jeu.ages[jeu.age_actuel];
            int init_x = jeu.age_actuel == 2 ? 77 : 65;
            
            List<Tuple<int,int>> cc = new List<Tuple<int, int>>();
            for (int i = 0; i < pl.GetLength(0); i++)
            {
                for (int j = 0; j < pl.GetLength(1); j++)
                {
                    if (Accessible(pl,j,i))
                        cc.Add(new Tuple< int, int>(j,i));
                }
            }

            int ind = 0;
            ConsoleKey key;
            int x = cc[ind].Item1;
            int y = cc[ind].Item2;
            do
            {
                Console.BackgroundColor = Famille_color[pl[y, x].batiment];
                Rectangle(x * 6 + init_x , y * 4 + 14 , 8,5);
                
                x = cc[ind].Item1;
                y = cc[ind].Item2;

                Console.BackgroundColor = ConsoleColor.DarkGreen;
                Rectangle(x * 6 + init_x , y * 4 + 14 , 8,5);
                Console.SetCursorPosition(x * 6 + init_x , y * 4 + 14 );
                key = Console.ReadKey().Key;

                switch (key)
                {
                    case ConsoleKey.UpArrow:
                    case ConsoleKey.LeftArrow:
                        ind = (ind - 1 + cc.Count) % cc.Count;
                        break;
                    case ConsoleKey.DownArrow:
                    case ConsoleKey.RightArrow:
                        ind = (ind + 1) % cc.Count;
                        break;
                    case ConsoleKey.J:
                        Information_jetons(jeu);
                        Affichage(jeu,joueur);
                        break;
                    case ConsoleKey.M:
                        //Information_merveilles();
                        Affichage(jeu,joueur);
                        break;
                    case ConsoleKey.C:
                        //Information_cartes();
                        Affichage(jeu,joueur);
                        break;
                }
            } while (key != ConsoleKey.Enter);

            Carte tmp = pl[y, x];
            pl[y, x] = null;
            return tmp;
        }

        public static void Information_jetons(Jeu jeu)
        {
            int y = 20;
            int x = 70;
            Init_Choix();
            Console.SetCursorPosition(x+4,y-2);
            Console.ForegroundColor = ConsoleColor.Black;
            Console.Write("Information sur les jetons");
            
            for (int i = 0; i < 9; i++)
            {
                if (jeu.progres.Contains(i) || jeu.joueurs[1].progres.Contains(i) || jeu.joueurs[0].progres.Contains(i))
                {
                    Console.SetCursorPosition(x,y);
                    Console.BackgroundColor = ConsoleColor.DarkGreen;
                    Console.Write(i);
                    Console.BackgroundColor = ConsoleColor.White;
                    Console.SetCursorPosition(x + 2,y);
                    Console.Write(": {0}",jeton_dic[i]);
                    y += 2;
                }
            }
            Console.ReadKey();
        }
        
        public static Dictionary<int,string> jeton_dic = new Dictionary<int, string>(){{0,"Les cartes militaires ont un effet en plus"},
            {1,"Recever l'argent que dépense pour des ressources l'adversaire"},
            {2,"6 Pieces - 6 Points de victoire"},
            {4,"Rejouer apres chaque invocation de merveille"},
            {3,"3 points de victoire en plus par jeton possedé"},
            {5,""},
            {7,"Vous avez l'équivalent d'un symbole en plus"},
            {6,"7 Points de victoire"},
            {8,"6 pieces et à chaque construction par lien 4 pièces"},
            {9,""}
        };

        /////////////////////////////////////////////
        /********************************************
         *
         *    LE JEU
         *
         * *******************************************/

        public static void Seven_Wonder()
        {
            Console.ReadKey();
            Console.CursorVisible = false;
            
            Jeu jeu = Init_jeu();
            int joueur = 1;
            int etat = 0;

            Affichage_plateau(jeu,99);
            Choix_merveille(jeu);
            
            while (jeu.age_actuel != 3 && etat * etat != 1)
            {
                while (jeu.carte_age_actuel > 0 && etat * etat != 1)
                {
                    if (etat == 0)
                        joueur = (joueur + 1) % 2;
                    else
                        etat = 0;

                    Affichage(jeu,joueur);

                    Carte carte = Choix_Carte(jeu,joueur);
                    int action = Choix_Action(jeu,joueur,carte);
                    switch (action)
                    {
                        // Invoquer
                        case 0:
                            etat = Invoquer_Carte(jeu, joueur, carte);
                            break;
                        // Deffosser
                        case 1:
                            Deffosser(jeu, joueur);
                            break;
                        //Merveille
                        case 2:
                            etat = Invoquer_Merveille(jeu, joueur); 
                            break;
                    }
                    jeu.carte_age_actuel -= 1;
                }
                
                if (etat * etat != 1)
                    joueur = Prochain_joueur(jeu,joueur) + 1;
                jeu.carte_age_actuel = 20;
                jeu.age_actuel++;
            }
            Fin(jeu,joueur,etat);
        }
    }
}