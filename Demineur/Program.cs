namespace Demineur
{
    internal class Program
    {
        static int nbCols;
        static int nbLignes;
        static int nbBombes;
        static Case[,] champDeMines = null!;

        static void Main(string[] args)
        {
            nbCols = DemanderUnNombre("Nombre de colonnes");
            nbLignes = DemanderUnNombre("Nombre de lignes");
            nbBombes = DemanderUnNombre("Nombre de bombes");
            champDeMines = new Case [nbCols, nbLignes];
            AjouterBombes();


            while (true)
            {
                AfficherGrille();
                Console.WriteLine();
                int x = DemanderUnNombre("x ?");
                int y = DemanderUnNombre("y ?");
                DecouvrirCase(x, y);
            }

            // Console.WriteLine((nbCols, nbLignes, nbBombes));
        }

        private static void DecouvrirCase(int x, int y)
        {
            if (champDeMines[x, y].Visible)
            {
                return;
            }
            champDeMines[x, y].Visible = true;
            if (champDeMines[x, y].Valeur == 0)
            {
                TournerAutour(x, y, DecouvrirCase);
            }
        }

        private static void AjouterBombes()
        {
            for (int i = 0; i < nbBombes; i++)
            {
                //int x = new Random().Next(0, nbCols);
                //int y = new Random().Next(0, nbLignes);
                //if (champDeMines[x, y] == 9)
                //{
                //    i--;
                //}
                //else
                //{
                //    AjouterBombe(x, y);
                //}
                int x, y;
                do
                {
                    x = new Random().Next(0, nbCols);
                    y = new Random().Next(0, nbLignes);
                } while (champDeMines[x, y].Valeur == 9);
                AjouterBombe(x, y);
            }
        }

        private static void AjouterBombe(int x, int y)
        {
            champDeMines[x, y].Valeur = 9;

            TournerAutour(x, y, (newX, newY) =>
            {
                if (champDeMines[newX, newY].Valeur != 9)
                {
                    champDeMines[newX, newY].Valeur++;
                }
            });

            //champDeMines[x - 1, y - 1]++;
            //champDeMines[x - 1, y + 0]++;
            //champDeMines[x - 1, y + 1]++;
            //champDeMines[x + 0, y - 1]++;
            //champDeMines[x + 0, y + 0]++;
            //champDeMines[x + 0, y + 1]++;
            //champDeMines[x + 1, y - 1]++;
            //champDeMines[x + 1, y + 0]++;
            //champDeMines[x + 1, y + 1]++;
        }

        private static void AfficherGrille()
        {
            Console.Clear();
            for(int y = 0; y < nbLignes; y++)
            {
                for (int x = 0; x < nbCols; x++)
                {
                    Case c = champDeMines[x, y];
                    if (c.Visible)
                    {
                        if (c.Valeur == 9)
                        {
                            Console.BackgroundColor = ConsoleColor.Red;
                        }
                        Console.Write(c.Valeur);
                        Console.ResetColor();
                    }
                    else
                    {
                        Console.Write("■");
                    }
                }
                Console.WriteLine();
            }
        }

        private static int DemanderUnNombre(string message)
        {
            Console.WriteLine(message);
            int nb;
            while (!int.TryParse(Console.ReadLine(), out nb))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("La valeur est incorrecte");
                Console.ResetColor();
                Console.WriteLine(message);
            }
            return nb;
        }

        private static void TournerAutour(int x, int y, Action<int, int> f)
        {
            for (int dx = -1; dx <= 1; dx++)
            {
                for (int dy = -1; dy <= 1; dy++)
                {
                    int newX = x + dx;
                    int newY = y + dy;
                    // je verifie que je ne suis pas en dehors du tableau
                    if (newX >= 0 && newX < nbCols && newY >= 0 && newY < nbLignes)
                    {
                        f(newX, newY);
                    }
                }
            }
        }
    }
}
