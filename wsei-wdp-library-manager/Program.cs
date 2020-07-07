using System;
using System.Globalization;
using LibraryManagerLib;

namespace wsei_wdp_library_manager
{
    class Program
    {
        //Obsługa opcji menu głównego

        //1. Nowe wypożyczenie
        
        /// <summary>
        /// Funkcja rysująca na ekranie menu dodawania nowego wypożyczenia.
        /// </summary>
        /// <param name="library">Biblioteka</param>
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

        /// <summary>
        /// Funkcja rysująca na ekranie menu przeglądania wypożyczeń.
        /// </summary>
        /// <param name="library">Biblioteka</param>
        /// <param name="resultsPerScreen">Ilość wyników na stronę</param>
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
                Console.WriteLine("ID | Czytelnik | Książka | Wypożyczono | Termin zwrotu | Zwrócono");

                foreach (Borrowing item in library.GetBorrowingsList(currentFrom, resultsNumber))
                {
                    Console.WriteLine($"{item.ID} | {item.ReaderID} | {library.GetBookName(item.BookID)} | {item.StartDate} | {item.EndDate.ToString("dd.MM.yyyy")} | {item.ReturnedDate}");
                }

                Console.WriteLine();
                Console.WriteLine($"Obecnie wyświetlane są wyniki {currentFrom + 1} do {currentFrom + resultsNumber} z {library.GetBorrowingsCount()}");
                Console.WriteLine();
                Console.WriteLine("1. Następna strona | 2. Poprzednia strona | 3. Zakończ wypożyczenie | 4. Przedłuż wypożyczenie | 0/Enter. Powrót do menu");
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

                    case 4:
                        Console.Clear();
                        MenuExtendBorrowing(library);
                        break;

                    case 0:
                        menuLoop = false;
                        break;
                }
            }
        }

        //3. Zakończ wypożyczenie

        /// <summary>
        /// Funkcja rysująca na ekranie menu kończenia wypożyczeń.
        /// </summary>
        /// <param name="library">Biblioteka</param>
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

        /// <summary>
        /// Funkcja rysująca na ekranie menu dodawania nowego czytelnika.
        /// </summary>
        /// <param name="library">Biblioteka</param>
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

        /// <summary>
        /// Funkcja rysująca na ekranie menu przeglądania czytelników.
        /// </summary>
        /// <param name="library">Biblioteka</param>
        /// <param name="resultsPerScreen">Ilość wyników na stronę</param>
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
                Console.WriteLine("1. Następna strona | 2. Poprzednia strona | 3. Modyfikacja | 0/Enter. Powrót do menu");
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

                    case 3:
                        MenuModifyReaderData(library);
                        break;

                    case 0:
                        menuLoop = false;
                        break;
                }
            }
        }

        //6. Dodaj książkę

        /// <summary>
        /// Funkcja rysująca na ekranie menu dodawania nowej ksiązki.
        /// </summary>
        /// <param name="library">Biblioteka</param>
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

        /// <summary>
        /// Funkcja rysująca na ekranie menu przeglądania ksiązek.
        /// </summary>
        /// <param name="library">Biblioteka</param>
        /// <param name="resultsPerScreen">Ilość wyników na stronę</param>
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
                    Console.WriteLine($"{item.ID} | {item.Title} | {item.Author} | {item.ISBN} | {item.Quantity - item.BorrowedQuantity}/{item.Quantity} | {item.Shelf}");
                }

                Console.WriteLine();
                Console.WriteLine($"Obecnie wyświetlane są wyniki {currentFrom + 1} do {currentFrom + resultsNumber} z {library.GetBooksCount()}");
                Console.WriteLine();
                Console.WriteLine("1. Następna strona | 2. Poprzednia strona | 3. Modyfikacja | 4. Edytuj ilość | 0/Enter. Powrót do menu");
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

                    case 3:
                        MenuModifyBookData(library);
                        break;

                    case 4:
                        MenuModifyBookQuantity(library);
                        break;

                    case 0:
                        menuLoop = false;
                        break;
                }
            }
        }

        //Modyfikacje obiektów

        //Menu -> Opcja

        //2. -> 4. Przedłuż wypożyczenie

        /// <summary>
        /// Funkcja rysująca na ekranie podmenu przedłużania wypożyczenia.
        /// </summary>
        /// <param name="library">Biblioteka</param>
        static void MenuExtendBorrowing(LibraryManager library)
        {
            Console.WriteLine("Przedłuż wypożyczenie");
            Console.WriteLine();
            Console.Write("ID wypożyczenia: ");

            int borrowingID;
            int.TryParse(Console.ReadLine(), out borrowingID);

            Console.Write("Przedłużenie wypożyczenia o (dni): ");

            int extendDays;
            int.TryParse(Console.ReadLine(), out extendDays);

            Console.WriteLine();

            try
            {
                library.ExtendBorrowing(borrowingID, extendDays);
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine("Wypożyczenie nie zostało przedłużone, wciśnij dowolny klawisz aby kontynuować...");
                Console.ReadKey();
            }
        }

        //5. -> 3. Modyfikacja (czytelnika)

        /// <summary>
        /// Funkcja rysująca na ekranie podmenu modyfikacji danych czytelnika.
        /// </summary>
        /// <param name="library">Biblioteka</param>
        static void MenuModifyReaderData(LibraryManager library)
        {
            bool menuLoop = true;

            while (menuLoop)
            {
                Console.Clear();

                Console.WriteLine("Modyfikacja danych czytelnika");
                Console.WriteLine();
                Console.WriteLine("1. Zmiana imienia i nazwiska");
                Console.WriteLine("2. Zmiana numeru telefonu");
                Console.WriteLine("3. Zmiana adresu email");
                Console.WriteLine();
                Console.WriteLine("0/Enter. Powrót do menu");
                Console.WriteLine();
                Console.Write("Wybór: ");

                int menuSelectedOption;
                int.TryParse(Console.ReadLine(), out menuSelectedOption);

                Console.Clear();

                int readerID = -1;
                string newData = "";

                switch (menuSelectedOption)
                {
                    case 1:
                        Console.WriteLine("Meodyfikacja imienia i nazwiska");
                        Console.WriteLine();
                        Console.Write("ID czytelnika: ");
                        int.TryParse(Console.ReadLine(), out readerID);

                        Console.Write("Nowe dane: ");
                        newData = Console.ReadLine();

                        Console.WriteLine();

                        try
                        {
                            library.ModifyReaderData(readerID, newData, LibraryManager.ReaderData.Name);
                        }
                        catch (ArgumentException ex)
                        {
                            Console.WriteLine(ex.Message);
                            Console.WriteLine("Imię i nazwisko nie zostały zmienione, wciśnij dowolny klawisz aby kontynuować...");
                            Console.ReadKey();
                        }

                        break;

                    case 2:
                        Console.WriteLine("Meodyfikacja numeru telefonu");
                        Console.WriteLine();
                        Console.Write("ID czytelnika: ");
                        int.TryParse(Console.ReadLine(), out readerID);

                        Console.Write("Nowe dane: ");
                        newData = Console.ReadLine();

                        Console.WriteLine();

                        try
                        {
                            library.ModifyReaderData(readerID, newData, LibraryManager.ReaderData.PhoneNumber);
                        }
                        catch (ArgumentException ex)
                        {
                            Console.WriteLine(ex.Message);
                            Console.WriteLine("Numer telefonu nie został zmieniony, wciśnij dowolny klawisz aby kontynuować...");
                            Console.ReadKey();
                        }
                        catch (FormatException ex)
                        {
                            Console.WriteLine(ex.Message);
                            Console.WriteLine("Numer telefonu nie został zmieniony, wciśnij dowolny klawisz aby kontynuować...");
                            Console.ReadKey();
                        }

                        break;

                    case 3:
                        Console.WriteLine("Meodyfikacja adresu email");
                        Console.WriteLine();
                        Console.Write("ID czytelnika: ");
                        int.TryParse(Console.ReadLine(), out readerID);

                        Console.Write("Nowe dane: ");
                        newData = Console.ReadLine();

                        Console.WriteLine();

                        try
                        {
                            library.ModifyReaderData(readerID, newData, LibraryManager.ReaderData.EmailAddress);
                        }
                        catch (ArgumentException ex)
                        {
                            Console.WriteLine(ex.Message);
                            Console.WriteLine("Adres email nie został zmieniony, wciśnij dowolny klawisz aby kontynuować...");
                            Console.ReadKey();
                        }
                        catch (FormatException ex)
                        {
                            Console.WriteLine(ex.Message);
                            Console.WriteLine("Numer telefonu nie został zmieniony, wciśnij dowolny klawisz aby kontynuować...");
                            Console.ReadKey();
                        }

                        break;

                    case 0:
                        menuLoop = false;
                        break;
                }
            }
        }

        //5. -> 4. Edytuj ilość 

        /// <summary>
        /// Funkcja rysująca na ekranie podmenu modyfikacji ilości danej pozycji oferowanej przez bibliotekę.
        /// </summary>
        /// <param name="library">Biblioteka</param>
        static void MenuModifyBookQuantity(LibraryManager library)
        {
            Console.Clear();

            Console.WriteLine("Edytuj ilość");
            Console.WriteLine();
            Console.Write("ID książki: ");

            int bookID;
            int.TryParse(Console.ReadLine(), out bookID);

            Console.Write("Ilość: ");

            int quantity;
            int.TryParse(Console.ReadLine(), out quantity);

            Console.WriteLine();

            try
            {
                library.ModifyBookQuantity(bookID, quantity);
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine("Ilość nie została zmieniona, wciśnij dowolny klawisz aby kontynuować...");
                Console.ReadKey();
            }
        }

        //7. -> 3. Modyfikacja (ksiązki)

        /// <summary>
        /// Funkcja rysująca na ekranie podmenu modyfikacji danych ksiązki.
        /// </summary>
        /// <param name="library">Biblioteka</param>
        static void MenuModifyBookData(LibraryManager library)
        {
            bool menuLoop = true;

            while (menuLoop)
            {
                Console.Clear();

                Console.WriteLine("Modyfikacja danych ksiązki");
                Console.WriteLine();
                Console.WriteLine("1. Zmiana tytułu");
                Console.WriteLine("2. Zmiana autora");
                Console.WriteLine("3. Zmiana półki");
                Console.WriteLine();
                Console.WriteLine("0/Enter. Powrót do menu");
                Console.WriteLine();
                Console.Write("Wybór: ");

                int menuSelectedOption;
                int.TryParse(Console.ReadLine(), out menuSelectedOption);

                Console.Clear();

                int bookID = -1;
                string newData = "";

                switch (menuSelectedOption)
                {
                    case 1:
                        Console.WriteLine("Modyfikacja tytułu");
                        Console.WriteLine();
                        Console.Write("ID ksiązki: ");
                        int.TryParse(Console.ReadLine(), out bookID);

                        Console.Write("Nowe dane: ");
                        newData = Console.ReadLine();

                        Console.WriteLine();

                        try
                        {
                            library.ModifyBookData(bookID, newData, LibraryManager.BookData.Title);
                        }
                        catch (ArgumentException ex)
                        {
                            Console.WriteLine(ex.Message);
                            Console.WriteLine("Tytuł nie został zmieniony, wciśnij dowolny klawisz aby kontynuować...");
                            Console.ReadKey();
                        }

                        break;
                    
                    case 2:
                        Console.WriteLine("Modyfikacja autora");
                        Console.WriteLine();
                        Console.Write("ID ksiązki: ");
                        int.TryParse(Console.ReadLine(), out bookID);

                        Console.Write("Nowe dane: ");
                        newData = Console.ReadLine();

                        Console.WriteLine();

                        try
                        {
                            library.ModifyBookData(bookID, newData, LibraryManager.BookData.Author);
                        }
                        catch (ArgumentException ex)
                        {
                            Console.WriteLine(ex.Message);
                            Console.WriteLine("Autor nie został zmieniony, wciśnij dowolny klawisz aby kontynuować...");
                            Console.ReadKey();
                        }

                        break;
                    
                    case 3:
                        Console.WriteLine("Modyfikacja półki");
                        Console.WriteLine();
                        Console.Write("ID ksiązki: ");
                        int.TryParse(Console.ReadLine(), out bookID);

                        Console.Write("Nowe dane: ");
                        newData = Console.ReadLine();

                        Console.WriteLine();

                        try
                        {
                            library.ModifyBookData(bookID, newData, LibraryManager.BookData.Shelf);
                        }
                        catch (ArgumentException ex)
                        {
                            Console.WriteLine(ex.Message);
                            Console.WriteLine("Półka nie została zmieniona, wciśnij dowolny klawisz aby kontynuować...");
                            Console.ReadKey();
                        }

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

            string user = "";

            bool loginLoop = true;

            while (loginLoop)
            {
                Console.WriteLine("Witaj w Library Manager!");
                Console.WriteLine();
                Console.WriteLine("Aby kontynuować wprowadź swój login.");
                Console.WriteLine();
                Console.Write("Login: ");

                user = Console.ReadLine();

                Console.Clear();

                if (user.Trim() != "")
                    loginLoop = false;
            }

            DateTime date = DateTime.Now;

            while (true)
            {
                Console.WriteLine($"Library Manager | Operator: {user} | {date.ToString("D", CultureInfo.CreateSpecificCulture("pl-PL"))}");
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
                Console.WriteLine("0/Enter. Zakończ program");
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