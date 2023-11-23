namespace Demineur
{
    internal class Program
    {
        static int nbCols;
        static int nbLignes;
        static int nbBombes;
        static Case[,] champDeMines = null!;
        static int cursorX = 0;
        static int cursorY = 0;

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
                (int x, int y) = ChoisirCase();
                DecouvrirCase(x, y);
                if (champDeMines[x, y].Valeur == 9)
                {
                    AfficherGrille();
                    Console.SetCursorPosition(0, nbLignes + 3);
                    Console.WriteLine("Vous avez perdu !!");
                    break;
                }
                if (CheckVictory())
                {
                    AfficherGrille();
                    Console.SetCursorPosition(0, nbLignes + 3);
                    Console.WriteLine("Vous avez gagné !!");
                    break;
                }
            }
        }

        private static bool CheckVictory()
        {
            for (int x = 0; x < nbCols; x++)
            {
                for(int y = 0; y < nbLignes; y++)
                {
                    if (!champDeMines[x, y].Visible && champDeMines[x, y].Valeur != 9)
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        private static (int,int) ChoisirCase()
        {
            ConsoleKey consoleKey = default;
            while(consoleKey != ConsoleKey.Enter)
            {
                Console.SetCursorPosition(cursorX, cursorY);
                Console.BackgroundColor = ConsoleColor.DarkGray;
                Console.Write(
                    champDeMines[cursorX, cursorY].Visible ?
                    champDeMines[cursorX, cursorY].Valeur : 
                    "■"
                );
                Console.ResetColor();

                consoleKey = Console.ReadKey(true).Key;

                Console.SetCursorPosition(cursorX, cursorY);
                Console.Write(
                    champDeMines[cursorX, cursorY].Visible ?
                    champDeMines[cursorX, cursorY].Valeur :
                    "■"
                );
                switch (consoleKey)
                {
                    case ConsoleKey.LeftArrow:
                        cursorX = (cursorX - 1 + nbCols) % nbCols;
                        break;
                    case ConsoleKey.RightArrow:
                        cursorX = (cursorX + 1) % nbCols;
                        break;
                    case ConsoleKey.UpArrow:
                        cursorY = (cursorY - 1 + nbLignes) % nbLignes;
                        break;
                    case ConsoleKey.DownArrow:
                        cursorY = (cursorY + 1) % nbLignes;
                        break;
                }
            }
            return (cursorX, cursorY);
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
