using System;
using System.Globalization;
using LibraryManagerLib;

namespace wsei_wdp_library_manager
{
    class Program
    {
        //Funkcje dodatkowe

        /// <summary>
        /// Funkcja formatująca datę zakończenia wypożyczenia tak aby była ona bardziej czytelna dla użytkownika.
        /// </summary>
        /// <param name="returnedDate">Data zakończenia wypożyczenia</param>
        /// <returns><c>"Nie zwrócono"</c> - w przypadku gdy data jest równa 0001.01.01 00:00:00 (data domyślna oznaczająca, że książka nie została jeszcze zwrócona) lub datę w formacie "YYYY.MM.DD hh:mm:ss" - w innych przypadkach.</returns>
        static string FormatReturnedDate (DateTime returnedDate)
        {
            if (returnedDate == new DateTime(1, 1, 1, 0, 0, 0, 0))
                return "Nie zwrócono";
            else
                return returnedDate.ToString();
        }

        /// <summary>
        /// Funkcja kontrolująca wprowadzaną przez użytkownika wartość, podczas wyboru opcji w menu.
        /// </summary>
        /// <param name="menuSelectedOption">Wybrana opcja</param>
        /// <returns>Wartość absolutna - w przypadku kiedy uda się przekonwertować podaną przez użytkownika wartość na liczbę lub -1 - w innych przypadkach.</returns>
        static int MenuSelectedOptionCorrection (string menuSelectedOption)
        {
            int number;

            if (int.TryParse(menuSelectedOption, out number))
                return Math.Abs(number);
            else
                return -1;
        }

        //Obsługa opcji menu głównego

        //1. Nowe wypożyczenie
        
        /// <summary>
        /// Funkcja rysująca na ekranie kreator nowego wypożyczenia.
        /// </summary>
        /// <param name="library">Biblioteka</param>
        static void MenuAddNewBorrowing(LibraryManager library)
        {
            Console.WriteLine("Nowe wypożyczenie");
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
        /// Funkcja rysująca na ekranie przeglądarkę wypożyczeń.
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
                Console.WriteLine("Wyświetl wypożyczenia");
                Console.WriteLine();
                Console.WriteLine("ID | Czytelnik | Książka | Wypożyczono | Termin zwrotu | Zwrócono");

                foreach (Borrowing item in library.GetBorrowingsList(currentFrom, resultsNumber))
                {
                    Console.WriteLine($"{item.ID} | {item.ReaderID} | {library.GetBookName(item.BookID)} | {item.StartDate} | {item.EndDate.ToString("dd.MM.yyyy")} | {FormatReturnedDate(item.ReturnedDate)}");
                }

                Console.WriteLine();
                Console.WriteLine($"Obecnie wyświetlane są wyniki od {currentFrom + 1} do {currentFrom + resultsNumber} z {library.GetBorrowingsCount()}");
                Console.WriteLine();
                Console.WriteLine("1. Następna strona | 2. Poprzednia strona | 3. Zakończ wypożyczenie | 4. Przedłuż wypożyczenie | 0. Powrót do menu");
                Console.WriteLine();
                Console.Write("Wybór: ");

                string menuSelectedOption = Console.ReadLine();

                switch (MenuSelectedOptionCorrection(menuSelectedOption))
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

                    default:
                        break;
                }
            }
        }

        //3. Zakończ wypożyczenie

        /// <summary>
        /// Funkcja rysująca na ekranie opcję kończenia wypożyczeń.
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
        /// Funkcja rysująca na ekranie kreator nowego czytelnika.
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
            catch(ArgumentException ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine("Czytelnik nie został dodany, wciśnij dowolny klawisz aby kontynuować...");
                Console.ReadKey();
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
        /// Funkcja rysująca na ekranie przeglądarkę czytelników.
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
                Console.WriteLine("ID | Imię i nazwisko | Numer telefonu | Adres email");

                foreach (Reader item in library.GetReadersList(currentFrom, resultsNumber))
                {
                    Console.WriteLine($"{item.ID} | {item.Name} | {item.PhoneNumber} | {item.EmailAddress}");
                }

                Console.WriteLine();
                Console.WriteLine($"Obecnie wyświetlane są wyniki od {currentFrom + 1} do {currentFrom + resultsNumber} z {library.GetReadersCount()}");
                Console.WriteLine();
                Console.WriteLine("1. Następna strona | 2. Poprzednia strona | 3. Modyfikacja | 0. Powrót do menu");
                Console.WriteLine();
                Console.Write("Wybór: ");

                string menuSelectedOption = Console.ReadLine();

                switch (MenuSelectedOptionCorrection(menuSelectedOption))
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

                    default:
                        break;
                }
            }
        }

        //6. Dodaj książkę

        /// <summary>
        /// Funkcja rysująca na ekranie kreator nowej ksiązki.
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
            catch (ArgumentException ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine("Książka nie została dodana, wciśnij dowolny klawisz aby kontynuować...");
                Console.ReadKey();
            }
            catch (FormatException ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine("Książka nie została dodana, wciśnij dowolny klawisz aby kontynuować...");
                Console.ReadKey();
            }
        }

        //7. Wyświetl książki

        /// <summary>
        /// Funkcja rysująca na ekranie przeglądarkę książek.
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
                Console.WriteLine($"Obecnie wyświetlane są wyniki od {currentFrom + 1} do {currentFrom + resultsNumber} z {library.GetBooksCount()}");
                Console.WriteLine();
                Console.WriteLine("1. Następna strona | 2. Poprzednia strona | 3. Modyfikacja | 4. Edytuj ilość | 0. Powrót do menu");
                Console.WriteLine();
                Console.Write("Wybór: ");

                string menuSelectedOption = Console.ReadLine();

                switch (MenuSelectedOptionCorrection(menuSelectedOption))
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

                    default:
                        break;
                }
            }
        }

        //9. Ustawienia

        /// <summary>
        /// Funckcja rysująca menu ustawień aplikacji.
        /// </summary>
        /// <param name="settings">Ustawienia</param>
        static void MenuSettings(Settings settings)
        {
            bool menuLoop = true;

            while (menuLoop)
            {
                Console.Clear();

                Console.WriteLine("Ustawienia");
                Console.WriteLine();
                Console.WriteLine($"1. Ilość wyświetlanych na ekranie rekordów: {settings.MenuResultsPerScreen}");
                Console.WriteLine();
                Console.WriteLine("0. Powrót do menu");
                Console.WriteLine();
                Console.Write("Wybór: ");

                string menuSelectedOption = Console.ReadLine();

                Console.Clear();

                int newData;

                switch (MenuSelectedOptionCorrection(menuSelectedOption))
                {
                    case 1:
                        Console.WriteLine("Ilość wyświetlanych na ekranie rekordów");
                        Console.WriteLine();
                        Console.Write("Nowe dane: ");
                        int.TryParse(Console.ReadLine(), out newData);

                        Console.WriteLine();

                        try
                        {
                            settings.MenuResultsPerScreen = newData;
                        }
                        catch (ArgumentException ex)
                        {
                            Console.WriteLine(ex.Message);
                            Console.WriteLine("Ustawienie nie zostało zmienione, wciśnij dowolny klawisz aby kontynuować...");
                            Console.ReadKey();
                        }

                        break;

                    case 0:
                        menuLoop = false;
                        break;

                    default:
                        break;
                }
            }
        }

        //Modyfikacje obiektów
        //Menu -> Opcja Nazwa

        //2. -> 4. Przedłuż wypożyczenie

        /// <summary>
        /// Funkcja rysująca na ekranie opcję przedłużania wypożyczenia.
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
                Console.WriteLine("0. Powrót do menu");
                Console.WriteLine();
                Console.Write("Wybór: ");

                string menuSelectedOption = Console.ReadLine();

                Console.Clear();

                int readerID = -1;
                string newData = "";

                switch (MenuSelectedOptionCorrection(menuSelectedOption))
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
        /// Funkcja rysująca na ekranie opcję modyfikacji ilości danej pozycji oferowanej przez bibliotekę.
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
                Console.WriteLine("0. Powrót do menu");
                Console.WriteLine();
                Console.Write("Wybór: ");

                string menuSelectedOption = Console.ReadLine();

                Console.Clear();

                int bookID = -1;
                string newData = "";

                switch (MenuSelectedOptionCorrection(menuSelectedOption))
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
            Settings settings = new Settings();

            while (true)
            {
                Console.WriteLine($"Library Manager | {DateTime.Now.ToString("D", CultureInfo.CreateSpecificCulture("pl-PL"))}");
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
                Console.WriteLine("9. Ustawienia");
                Console.WriteLine();
                Console.WriteLine("0. Zakończ program");
                Console.WriteLine();
                Console.Write("Wybór: ");

                string menuSelectedOption = Console.ReadLine();
                
                Console.Clear();

                switch (MenuSelectedOptionCorrection(menuSelectedOption))
                {
                    case 1:
                        MenuAddNewBorrowing(library);
                        break;

                    case 2:
                        MenuShowBorrowings(library, settings.MenuResultsPerScreen);
                        break;

                    case 3:
                        MenuEndBorrowing(library);
                        break;

                    case 4:
                        MenuAddNewReader(library);
                        break;
                    
                    case 5:
                        MenuShowReaders(library, settings.MenuResultsPerScreen);
                        break;

                    case 6:
                        MenuAddNewBook(library);
                        break;

                    case 7:
                        MenuShowBooks(library, settings.MenuResultsPerScreen);
                        break;

                    case 9:
                        MenuSettings(settings);
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