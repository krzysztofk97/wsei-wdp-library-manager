using System;
using LibraryManagerLib;

namespace wsei_wdp_library_manager
{
    class Program
    {
        static void MenuAddNewReader(LibraryManager library)
        {
            Console.WriteLine("Dodaj nowego czytelnika");
            Console.WriteLine();
            Console.Write("Imię i nazwisko: ");

            string name = Console.ReadLine();

            Console.Write("Numer telefonu: ");

            string phoneNumber = Console.ReadLine();

            Console.Write("Adres email: ");

            string emailAddress = Console.ReadLine();

            Console.WriteLine();

            try
            {
                library.AddNewReader(name, phoneNumber, emailAddress);
            }
            catch(FormatException ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine("Czytelnik nie został dodany. Wciśnij dowolny klawisz aby kontynuować...");
                Console.ReadKey();
            }
        }

        static void MenuShowReaders(LibraryManager library, int resultsPerScreen)
        {
            int currentFrom = 0;
            int resultsNumber = resultsPerScreen;

            if (library.GetReadersCount() < resultsPerScreen)
                resultsNumber = library.GetReadersCount();

            bool menuLoop = true;

            while (menuLoop)
            {
                if ((currentFrom + resultsNumber) > library.GetReadersCount())
                    resultsNumber = library.GetReadersCount() - currentFrom;

                Console.Clear();
                Console.WriteLine($"Wyświetl czytelników | Łącznie czytelników: {library.GetReadersCount()}");
                Console.WriteLine();
                Console.WriteLine("ID | Imię i nazwisko | Telefon | Adres Email");

                foreach (Reader item in library.GetReadersList(currentFrom, resultsNumber))
                {
                    Console.WriteLine($"{item.ID} | {item.Name} | {item.PhoneNumber} | {item.EmailAddress}");
                }

                Console.WriteLine();
                Console.WriteLine($"Obecnie wyświetlane są wyniki {currentFrom + 1} do {currentFrom + resultsNumber} z {library.GetReadersCount()}");
                Console.WriteLine();
                Console.WriteLine("1. Następna strona | 2. Poprzednia strona | 0. Powrót do menu");
                Console.WriteLine();
                Console.Write("Wybór: ");

                int menuSelectedOption = int.Parse(Console.ReadLine());

                switch (menuSelectedOption)
                {
                    case 1:
                        if (currentFrom + resultsPerScreen < library.GetReadersCount())
                            currentFrom += resultsPerScreen;
                        break;

                    case 2:
                        if (currentFrom > 0)
                            currentFrom -= resultsPerScreen;

                        if (resultsNumber < resultsPerScreen)
                            resultsNumber = resultsPerScreen;

                        break;

                    case 0:
                        menuLoop = false;
                        break;
                }
            }
        }

        static void Main(string[] args)
        {
            LibraryManager library = new LibraryManager();

            Console.WriteLine("Witaj w Library Manager!");
            Console.WriteLine();
            Console.WriteLine("Aby kontynuować wprowadź swój login.");
            Console.WriteLine();
            Console.Write("Login: ");

            string user = Console.ReadLine();

            Console.Clear();

            while (true)
            {
                Console.WriteLine($"Library Manager | Operator: {user}");
                Console.WriteLine();
                Console.WriteLine("1. Nowe wypożyczenie");
                Console.WriteLine("2. Aktualne wypożyczenia");
                Console.WriteLine("3. Zakończ wypożyczenie");
                Console.WriteLine();
                Console.WriteLine("4. Dodaj czytelnika");
                Console.WriteLine("5. Wyświetl czytelników");
                Console.WriteLine();
                Console.WriteLine("6. Dodaj książkę");
                Console.WriteLine("7. Wyświetl ksiązki");
                Console.WriteLine();
                Console.WriteLine("0. Zakończ program");
                Console.WriteLine();
                Console.Write("Wybór: ");

                int menuOptionSelected = int.Parse(Console.ReadLine());

                Console.Clear();

                switch (menuOptionSelected)
                {
                    case 4:
                        MenuAddNewReader(library);
                        break;
                    
                    case 5:
                        MenuShowReaders(library, 5);
                        break;

                    case 0:
                        Environment.Exit(0);
                        break;
                }

                Console.Clear();
            }
        }
    }
}
