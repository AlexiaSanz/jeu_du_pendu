using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AsciiArt;

namespace jeu_du_pendu
{
    class Program
    {
        static void AfficherMot(string mot, List<char> lettres)
        {
            for (int i = 0; i < mot.Length; i++)
            {
                char lettre = mot[i];

                if (lettres.Contains(lettre))
                {
                    Console.Write(lettre + " ");
                }
                else
                {
                    Console.Write("_ ");
                }
            }
        }

        static char DemanderUneLettre(string message = "Entrez une lettre : ")
        {
            while (true)
            {
                Console.Write(message);
                string reponse = Console.ReadLine();
                if (reponse.Length == 1)
                {
                    reponse = reponse.ToUpper();
                    return reponse[0];
                }
                else
                {
                    Console.WriteLine("Veuillez entrer une lettre");
                }
            }
        }

        static bool ToutesLettresDevinees(string mot, List<char> lettres)
        {
            for (int i = 0; i < lettres.Count; i++)
            {
                mot = mot.Replace(lettres[i].ToString(), "");
            }

            return mot.Length == 0;
        }

        static void DevinerMot(string mot)
        {
            var lettresDevinees = new List<char>();
            var mauvaisesLettres = new List<char>();
            const int NB_VIES = 6;
            int viesRestantes = NB_VIES;

            while (viesRestantes > 0)
            {
                AfficherTableau(mot, lettresDevinees, NB_VIES, viesRestantes);
                var lettre = DemanderUneLettre();
                Console.Clear();

                Console.WriteLine();
                if (mot.Contains(lettre))
                {
                    Console.WriteLine("Cette lettre est dans le mot");
                    lettresDevinees.Add(lettre);
                }
                else
                {
                    Console.WriteLine("Cette lettre n'est pas dans le mot");

                    if (!mauvaisesLettres.Contains(lettre))
                    {
                        mauvaisesLettres.Add(lettre);
                        viesRestantes--;
                    }
                    else
                    {
                        Console.WriteLine("Lettre déjà proposée");
                    }

                    Console.WriteLine("Il vous reste : " + viesRestantes + " vies");
                }

                if (mauvaisesLettres.Count > 0)
                {
                    AfficherMauvaisesLettres(mauvaisesLettres);
                }

                if (viesRestantes == 0)
                {
                    AfficherGameOver(mot, NB_VIES, viesRestantes);
                }

                if (ToutesLettresDevinees(mot, lettresDevinees))
                {
                    AfficherVictoire(mot);
                    break;
                }
            }
        }

        private static void AfficherTableau(string mot, List<char> lettresDevinees, int NB_VIES, int viesRestantes)
        {
            Console.WriteLine(Ascii.PENDU[NB_VIES - viesRestantes]);
            Console.WriteLine();
            Console.WriteLine();
            AfficherMot(mot, lettresDevinees);
            Console.WriteLine();
            Console.WriteLine();
        }

        private static void AfficherVictoire(string mot)
        {
            Console.Clear();
            Console.WriteLine();
            Console.WriteLine("BRAVO ! Vous avez gagné ! Le mot était : " + mot);
        }

        private static void AfficherGameOver(string mot, int NB_VIES, int viesRestantes)
        {
            Console.Clear();
            Console.WriteLine(Ascii.PENDU[NB_VIES - viesRestantes]);
            Console.WriteLine("Perdu ! Le mot était : " + mot);
        }

        private static void AfficherMauvaisesLettres(List<char> mauvaisesLettres)
        {
            Console.WriteLine();
            Console.WriteLine("Le mot ne contient pas les lettres : " + string.Join(", ", mauvaisesLettres));
            Console.WriteLine();
        }

        static string[] ChargerLesMots(string nomFichier)
        {
            try
            {
                return File.ReadAllLines(nomFichier);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erreur de lecture du fichier : " + nomFichier + "( " + ex.Message + " )");
            }
            return null;
        }

        static bool DemanderDeRejouer()
        {
            char reponse = 'A';
            do
            {
                Console.WriteLine();
                reponse = DemanderUneLettre("Voulez-vous rejouer ? (O/N) : ");
                Console.Clear();
                if (reponse == 'O')
                {
                    return true;
                }

                if (reponse == 'N')
                {
                    return false;
                }

                Console.WriteLine($"Veuillez entrer 'O' pour oui, 'N' pour non.");
            }
            while (true);
        }

        static void Main(string[] args)
        {
            var mots = ChargerLesMots("mots.txt");

            if ((mots == null) || (mots.Length == 0))
            {
                Console.WriteLine("La liste de mots est vide");
                return;
            }

            do
            {
                Random rand = new Random();
                string mot = mots[rand.Next(0, mots.Length)].Trim().ToUpper();
                DevinerMot(mot);
            }
            while (DemanderDeRejouer());

            Console.WriteLine("A Bientôt !");
        }
    }
}
