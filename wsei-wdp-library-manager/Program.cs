using System;
using LibraryManagerLib;

namespace wsei_wdp_library_manager
{
    class Program
    {
        //1. Nowe wypożyczenie
        static void MenuAddNewBorrowing(LibraryManager library)
        {
            Console.WriteLine("Dodaj nowego czytelnika");
            Console.WriteLine();
            Console.Write("ID Książki: ");

            int bookID;
            int.TryParse(Console.ReadLine(), out bookID);

            Console.Write("ID Czytelnika: ");

            int readerID;
            int.TryParse(Console.ReadLine(), out readerID);

            Console.Write("Długość wypożyczenia (dni): ");

            int borrowingTimeDays;
            int.TryParse(Console.ReadLine(), out borrowingTimeDays);
            DateTime endTime = DateTime.Now.AddDays(borrowingTimeDays);

            Console.WriteLine();

            try
            {
                library.AddNewBorrowing(bookID, readerID, new DateTime(endTime.Year, endTime.Month, endTime.Day, 23, 59, 59));
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine("Wypożyczenie nie zostało dodane, wciśnij dowolny klawisz aby kontynuować...");
                Console.ReadKey();
            }
        }

        //2. Wyświetl wypożyczenia
        static void MenuShowBorrowings(LibraryManager library, int resultsPerScreen)
        {
            int currentFrom = 0;
            int resultsNumber = resultsPerScreen;

            if (library.GetBorrowingsCount() < resultsPerScreen)
                resultsNumber = library.GetBorrowingsCount();

            bool menuLoop = true;

            while (menuLoop)
            {
                if ((currentFrom + resultsNumber) > library.GetBorrowingsCount())
                    resultsNumber = library.GetBorrowingsCount() - currentFrom;

                Console.Clear();
                Console.WriteLine($"Wyświetl wypożyczenia");
                Console.WriteLine();
                Console.WriteLine("ID | Książka | Czytelnik | Wypożyczono | Termin zwrotu | Zwrócono");

                foreach (Borrowing item in library.GetBorrowingsList(currentFrom, resultsNumber))
                {
                    Console.WriteLine($"{item.ID} | {item.ReaderID} | {library.GetBookName(item.BookID)} | {item.StartDate} | {item.EndDate.ToString("dd.MM.yyyy")} | {item.ReturnedDate}");
                }

                Console.WriteLine();
                Console.WriteLine($"Obecnie wyświetlane są wyniki {currentFrom + 1} do {currentFrom + resultsNumber} z {library.GetBorrowingsCount()}");
                Console.WriteLine();
                Console.WriteLine("1. Następna strona | 2. Poprzednia strona | 3. Zakończ wypożyczenie | 0. Powrót do menu");
                Console.WriteLine();
                Console.Write("Wybór: ");

                int menuSelectedOption;
                int.TryParse(Console.ReadLine(), out menuSelectedOption);

                switch (menuSelectedOption)
                {
                    case 1:
                        if (currentFrom + resultsPerScreen < library.GetBorrowingsCount())
                            currentFrom += resultsPerScreen;
                        break;

                    case 2:
                        if (currentFrom > 0)
                            currentFrom -= resultsPerScreen;

                        if (resultsNumber < resultsPerScreen)
                            resultsNumber = resultsPerScreen;

                        break;

                    case 3:
                        Console.Clear();
                        MenuEndBorrowing(library);
                        break;

                    case 0:
                        menuLoop = false;
                        break;
                }
            }
        }

        //3. Zakończ wypożyczenie
        static void MenuEndBorrowing(LibraryManager library)
        {
            Console.WriteLine("Zakończ wypożyczenie");
            Console.WriteLine();
            Console.Write("ID wypożyczenia: ");

            int borrowingID;
            int.TryParse(Console.ReadLine(), out borrowingID);

            Console.WriteLine();

            try
            {
                library.EndBorrowing(borrowingID);
            }
            catch(ArgumentException ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine("Wypożyczenie nie zostało zakończone, wciśnij dowolny klawisz aby kontynuować...");
                Console.ReadKey();
            }
        }

        //4. Dodaj czytelnika
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
                Console.WriteLine("Czytelnik nie został dodany, wciśnij dowolny klawisz aby kontynuować...");
                Console.ReadKey();
            }
        }

        //5. Wyświetl czytelników
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
                Console.WriteLine("ID | Imię i nazwisko | Telefon | Adres email");

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

                int menuSelectedOption;
                int.TryParse(Console.ReadLine(), out menuSelectedOption);

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

        //6. Dodaj książkę
        static void MenuAddNewBook(LibraryManager library)
        {
            Console.WriteLine("Dodaj nową książkę");
            Console.WriteLine();
            Console.Write("Tytuł: ");

            string title = Console.ReadLine();

            Console.Write("Autor: ");

            string author = Console.ReadLine();

            Console.Write("ISBN: ");

            string isbn = Console.ReadLine();
            
            Console.Write("Ilość (domyślnie 0): ");

            int quantity;
            int.TryParse(Console.ReadLine(), out quantity);

            Console.Write("Półka: ");

            string shelf = Console.ReadLine();

            Console.WriteLine();

            try
            {
                library.AddNewBook(title, author, isbn, shelf, quantity);
            }
            catch (FormatException ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine("Książka nie została dodana, wciśnij dowolny klawisz aby kontynuować...");
                Console.ReadKey();
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine("Książka nie została dodana, wciśnij dowolny klawisz aby kontynuować...");
                Console.ReadKey();
            }
        }

        //7. Wyświetl książki
        static void MenuShowBooks(LibraryManager library, int resultsPerScreen)
        {
            int currentFrom = 0;
            int resultsNumber = resultsPerScreen;

            if (library.GetBooksCount() < resultsPerScreen)
                resultsNumber = library.GetBooksCount();

            bool menuLoop = true;

            while (menuLoop)
            {
                if ((currentFrom + resultsNumber) > library.GetBooksCount())
                    resultsNumber = library.GetBooksCount() - currentFrom;

                Console.Clear();
                Console.WriteLine($"Wyświetl książki | Łącznie książek: {library.GetBooksCount()}");
                Console.WriteLine();
                Console.WriteLine("ID | Tytuł | Autor | ISBN | Ilość | Półka");

                foreach (Book item in library.GetBooksList(currentFrom, resultsNumber))
                {
                    Console.WriteLine($"{item.ID} | {item.Title} | {item.Author} | {item.ISBN} | {item.Quantity} | {item.Shelf}");
                }

                Console.WriteLine();
                Console.WriteLine($"Obecnie wyświetlane są wyniki {currentFrom + 1} do {currentFrom + resultsNumber} z {library.GetBooksCount()}");
                Console.WriteLine();
                Console.WriteLine("1. Następna strona | 2. Poprzednia strona | 0. Powrót do menu");
                Console.WriteLine();
                Console.Write("Wybór: ");

                int menuSelectedOption;
                int.TryParse(Console.ReadLine(), out menuSelectedOption);

                switch (menuSelectedOption)
                {
                    case 1:
                        if (currentFrom + resultsPerScreen < library.GetBooksCount())
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
                Console.WriteLine("2. Wyświetl wypożyczenia");
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

                int menuOptionSelected;
                int.TryParse(Console.ReadLine(), out menuOptionSelected);

                Console.Clear();

                switch (menuOptionSelected)
                {
                    case 1:
                        MenuAddNewBorrowing(library);
                        break;

                    case 2:
                        MenuShowBorrowings(library, 5);
                        break;

                    case 3:
                        MenuEndBorrowing(library);
                        break;

                    case 4:
                        MenuAddNewReader(library);
                        break;
                    
                    case 5:
                        MenuShowReaders(library, 5);
                        break;

                    case 6:
                        MenuAddNewBook(library);
                        break;

                    case 7:
                        MenuShowBooks(library, 5);
                        break;

                    case 0:
                        Environment.Exit(0);
                        break;

                    default:
                        break;
                }

                Console.Clear();
            }
        }
    }
}