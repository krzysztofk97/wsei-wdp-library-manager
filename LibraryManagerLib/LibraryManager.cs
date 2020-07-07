using System;
using System.Collections.Generic;

namespace LibraryManagerLib
{
    /// <summary>
    /// Kalsa <c>LibraryManager</c> zawiera wszystkie funkcje służące od obsługi biblioteki.
    /// </summary>
    public class LibraryManager
    {
        private List<Book> books;
        private List<Reader> readers;
        private List<Borrowing> borrowings;

        /// <summary>
        /// Konstruktor klasy <c>LibraryManager</c>.
        /// </summary>
        public LibraryManager()
        {
            books = new List<Book>();
            readers = new List<Reader>();
            borrowings = new List<Borrowing>();
        }

        //Tworzenie i modyfikowanie listy książek

        /// <summary>
        /// Funkcja dodająca nową książkę do listy.
        /// </summary>
        /// <param name="title">Tytuł książki</param>
        /// <param name="author">Imię i nazwikso/nazwa autora książki</param>
        /// <param name="isbn">Numer ISBN książki</param>
        /// <param name="shelf">Półka, na której znajduje się książka</param>
        /// <param name="quantity">Ilość danej pozycji oferowana przez bibliotekę (domyślnie równa 0)</param>
        public void AddNewBook(string title, string author, string isbn, string shelf, int quantity)
        {
            int id = books.Count + 1;

            Book b = new Book(id, title, author, isbn, shelf, quantity);
            AddNewBook(b);
        }

        public void AddNewBook(Book b)
        {
            if (!IsISBNDuplicated(b.ISBN))
                books.Add(b);
            else
                throw new ArgumentException("Książka z podanym numerem ISBN już istnieje");
        }

        /// <summary>
        /// Funkcja sprawdzająca czy w bazie danych nie istnieje już pozycja z takim numerem ISBN.
        /// </summary>
        /// <param name="isbn">Numer ISBN do sprawdzenia</param>
        /// <returns><c>true</c> - w przypadku gdy istnieje duplikat numeru ISBN lub <c>false</c> - w innych przypadkach</returns>
        private bool IsISBNDuplicated(string isbn)
        {
            for (int i = 0; i < books.Count; i++)
                if (books[i].ISBN == isbn)
                    return true;

            return false;
        }

        /// <summary>
        /// Funkcja zwracająca liczbę książek znajdujących się na liście <c>books</c>.
        /// </summary>
        /// <returns>Liczba pozycji na liście <c>books</c></returns>
        public int GetBooksCount()
        {
            return books.Count;
        }

        /// <summary>
        /// Funckja zwracająca listę książek z podanego zakresu z listy <c>books</c>.
        /// </summary>
        /// <param name="from">Początek zakresu</param>
        /// <param name="length">Długość zakresu</param>
        /// <returns>Lista książek z podanego zakesu</returns>
        public List<Book> GetBooksList(int from, int length)
        {
            return books.GetRange(from, length);
        }

        public enum BookData
        {
            Title,
            Author,
            Shelf
        }

        /// <summary>
        /// Funkcja pozwalająca na modyfikowanie informacji o książce.
        /// </summary>
        /// <param name="bookID">Identyfikator książki</param>
        /// <param name="newData">Nowe dane</param>
        /// <param name="dataType">Modyfikowany parametr</param>
        public void ModifyBookData(int bookID, string newData, BookData dataType)
        {
            if (IsBookIDExists(bookID))
            {
                if (newData.Trim() != "")
                {
                    switch (dataType)
                    {
                        case BookData.Title:
                            books.Find(b => b.ID.Equals(bookID)).Title = newData;
                            break;

                        case BookData.Author:
                            books.Find(b => b.ID.Equals(bookID)).Author = newData;
                            break;

                        case BookData.Shelf:
                            books.Find(b => b.ID.Equals(bookID)).Shelf = newData;
                            break;
                    }
                }
                else
                    throw new ArgumentException("Nowe dane nie mogą być puste");
            }
            else
                throw new ArgumentException("Książka o podanym identyfikatorze nie istnieje");
        }

        /// <summary>
        /// Funkcja pozwalająca na modyfikowanie całkowitej ilości danego tytułu.
        /// </summary>
        /// <param name="bookID">Identyfikator książki</param>
        /// <param name="quantity">Nowa liczba</param>
        public void ModifyBookQuantity(int bookID, int quantity)
        {
            if (IsBookIDExists(bookID))
            {
                books.Find(b => b.ID.Equals(bookID)).Quantity = quantity;
            }
            else
                throw new ArgumentException("Książka o podanym identyfikatorze nie istnieje");
        }

        //Tworzenie i modyfikowanie listy czytelników

        /// <summary>
        /// Funkcja dodająca nowego czytelnika do listy.
        /// </summary>
        /// <param name="name">Imię i nazwisko/nazwa czytelnika</param>
        /// <param name="phoneNumber">Numer telefonu czytelnika</param>
        /// <param name="emailAddress">Adres email czytelnika</param>
        public void AddNewReader(string name, string phoneNumber, string emailAddress)
        {
            int id = readers.Count + 1;

            Reader r = new Reader(id, name, phoneNumber, emailAddress);
            readers.Add(r);
        }

        /// <summary>
        /// Funkcja zwracająca liczbę czytelników znajdujących się na liście <c>readers</c>.
        /// </summary>
        /// <returns>Liczba pozycji na liście <c>readers</c></returns>
        public int GetReadersCount()
        {
            return readers.Count;
        }

        /// <summary>
        /// Funckja zwracająca listę czytelkników z podanego zakresu z listy <c>readers</c>.
        /// </summary>
        /// <param name="from">Początek zakresu</param>
        /// <param name="length">Długość zakresu</param>
        /// <returns>Lista czytelników z podanego zakesu</returns>
        public List<Reader> GetReadersList(int from, int length)
        {
            return readers.GetRange(from, length);
        }

        public enum ReaderData
        {
            Name,
            PhoneNumber,
            EmailAddress
        }

        /// <summary>
        /// Funckja pozawlająca na modyfiikowanie danych czytelnika.
        /// </summary>
        /// <param name="readerID">Identyfikator czytelnika</param>
        /// <param name="newData">Nowe dane</param>
        /// <param name="dataType">Modyfikowany parametr</param>
        public void ModifyReaderData(int readerID, string newData, ReaderData dataType)
        {
            if (IsReaderIDExists(readerID))
            {
                if (newData.Trim() != "")
                {
                    switch (dataType)
                    {
                        case ReaderData.Name:
                            readers.Find(r => r.ID.Equals(readerID)).Name = newData;
                            break;

                        case ReaderData.PhoneNumber:
                            readers.Find(r => r.ID.Equals(readerID)).PhoneNumber = newData;
                            break;

                        case ReaderData.EmailAddress:
                            readers.Find(r => r.ID.Equals(readerID)).EmailAddress = newData;
                            break;
                    }
                }
                else
                    throw new ArgumentException("Nowe dane nie mogą być puste");
            }
            else
                throw new ArgumentException("Czytelnik o podanym identyfikatorze nie istnieje");
        }

        //Tworzenie i modyfikowanie listy wypożyczeń

        /// <summary>
        /// Funkcja dodająca nowe wypożyczenie do listy.
        /// </summary>
        /// <param name="bookID">Identyfikator ksiązki</param>
        /// <param name="readerID">Identyfikator czytelnika</param>
        /// <param name="endDate">Data zakończenia wypożyczenia</param>
        public void AddNewBorrowing(int bookID, int readerID, DateTime endDate)
        {
            int id = borrowings.Count + 1;

            Borrowing b = new Borrowing(id, bookID, readerID, DateTime.Now, endDate, new DateTime(1, 1, 1, 0, 0, 0));
            AddNewBorrowing(b);
        }

        public void AddNewBorrowing(Borrowing b)
        {
            if (IsBookIDExists(b.BookID))
            {
                if (IsBookAvailable(b.BookID))
                {
                    if (IsReaderIDExists(b.ReaderID))
                    {
                        borrowings.Add(b);
                        books.Find(bb => bb.ID.Equals(b.BookID)).BorrowedQuantity++;
                    }
                    else
                        throw new ArgumentException("Czytelnik o podanym identyfikatorze nie istnieje");
                }
                else
                    throw new ArgumentException("Książka nie jest dostępna");
            }
            else
                throw new ArgumentException("Książka o podanym identyfikatorze nie istnieje");
        }

        /// <summary>
        /// Funkcja zwracająca liczbę wypożyczeń znajdujących się na liście <c>borrowings</c>.
        /// </summary>
        /// <returns>Liczba pozycji na liście <c>borrowings</c></returns>
        public int GetBorrowingsCount()
        {
            return borrowings.Count;
        }

        /// <summary>
        /// Funckja zwracająca listę wypożyczeń z podanego zakresu z listy <c>borrowings</c>.
        /// </summary>
        /// <param name="from">Początek zakresu</param>
        /// <param name="length">Długość zakresu</param>
        /// <returns>Lista wypożyczeń z podanego zakesu</returns>
        public List<Borrowing> GetBorrowingsList(int from, int length)
        {
            List<Borrowing> reversedBorrowings = borrowings;
            reversedBorrowings.Reverse();

            return reversedBorrowings.GetRange(from, length);
        }

        /// <summary>
        /// Funkcja sprawdzająca czy czytelnik o podanym identyfikatorze istnieje w bazie danych.
        /// </summary>
        /// <param name="readerID">Identyfikator czytelnika</param>
        /// <returns><c>true</c> - w przypadku kiedy czytelnik o podanym identyfikatorze istnieje w bazie danych lub <c>false</c> - w innych przypadkach</returns>
        private bool IsReaderIDExists(int readerID)
        {
            for (int i = 0; i < GetReadersCount(); i++)
                if (readers[i].ID == readerID)
                    return true;

            return false;
        }

        /// <summary>
        /// Funkcja sprawdzająca czy ksiązka o podanym identyfikatorze istnieje w bazie danych.
        /// </summary>
        /// <param name="bookID">Identyfikator książki</param>
        /// <returns><c>true</c> - w przypadku kiedy ksiązka o podanym identyfikatorze istnieje w bazie danych lub <c>false</c> - w innych przypadkach</returns>
        private bool IsBookIDExists(int bookID)
        {
            if (bookID > GetBooksCount())
                return false;

            for (int i = 0; i < GetBooksCount(); i++)
                if (books[i].ID == bookID)
                    return true;

            return false;
        }

        /// <summary>
        /// Funckja znajdująca tytuł ksiązki o podanym identyfikatorze.
        /// </summary>
        /// <param name="bookID">Identyfikator książki</param>
        /// <returns>Parametr <c>Title</c> pozycji o danym identyfikatrze</returns>
        public string GetBookName(int bookID)
        {
            return books.Find(b => b.ID.Equals(bookID)).Title;
        }

        /// <summary>
        /// Funkcja umożliwiająca zakończenie wypożyczenia o podanym identyfikatorze.
        /// </summary>
        /// <param name="borrowingID">Identyfikator wypożyczenia</param>
        public void EndBorrowing(int borrowingID)
        {
            if (IsBorrowingIDExists(borrowingID))
            {
                if (!IsBookReturned(borrowingID))
                {
                    borrowings.Find(b => b.ID.Equals(borrowingID)).ReturnedDate = DateTime.Now;
                    books.Find(bb => bb.ID.Equals(borrowings.Find(b => b.ID.Equals(borrowingID)).BookID)).BorrowedQuantity--;
                }
                else
                    throw new ArgumentException("Wypożyczenie o podanym identyfikatorze zostało już zakończone");
            }
            else
                throw new ArgumentException("Wypożyczenie o podanym identyfikatorze nie istnieje");
        }

        /// <summary>
        /// Funkcja sprawdzająca czy książka o podanym identyfikatorze została zwrócona (data zwrócenia książki jest nowsza od 01.01.0001 00:00:00 - data oznaczająca, że wypożyczenie nie zostało zwrócone).
        /// </summary>
        /// <param name="borrowingID">Identyfikator wypożyczenia</param>
        /// <returns><c>true</c> - w przypadku kiedy data zakończenia jest nowsza niż 01.01.0001 00:00:00 lub <c>false</c> - w innych przypadkach</returns>
        private bool IsBookReturned(int borrowingID)
        {
            if (borrowings.Find(b => b.ID.Equals(borrowingID)).ReturnedDate > new DateTime(1, 1, 1, 0, 0, 0))
                return true;

            return false;
        }

        /// <summary>
        /// Funkcja sprawdzająca czy wypożyczenie o podanym identyfikatorze istnieje w bazie danych.
        /// </summary>
        /// <param name="borrowingID">Identyfikator wypożyczenia</param>
        /// <returns><c>true</c> - w przypadku kiedy wypożyczenie o podanym identyfikatorze istnieje w bazie danych lub <c>false</c> - w innych przypadkach</returns>
        private bool IsBorrowingIDExists(int borrowingID)
        {
            if (borrowingID > GetBorrowingsCount())
                return false;

            for (int i = 0; i < GetBorrowingsCount(); i++)
                if (borrowings[i].ID == borrowingID)
                    return true;

            return false;
        }

        /// <summary>
        /// Funkcja pozwalająca na przedłużenie wypożyczenia o podanym identyfikatorze o podaną liczbę dni.
        /// </summary>
        /// <param name="borrowingID">Identyfikator wypożyczenia</param>
        /// <param name="extendDays">Liczba dni, o które ma zostać przedłużone wypożyczenie</param>
        public void ExtendBorrowing(int borrowingID, int extendDays)
        {
            if (IsBorrowingIDExists(borrowingID))
            {
                if (!IsBookReturned(borrowingID))
                {
                    if (extendDays > 0)
                        borrowings.Find(b => b.ID.Equals(borrowingID)).EndDate = borrowings.Find(b => b.ID.Equals(borrowingID)).EndDate.AddDays(extendDays);
                    else
                        throw new ArgumentException("Liczba dni przedłużenia nie może być mniejsza niż jeden");
                }
                else
                    throw new ArgumentException("Wypożyczenie o podanym identyfikatorze zostało już zakończone");
            }
            else
                throw new ArgumentException("Wypożyczenie o podanym identyfikatorze nie istnieje");
        }

        /// <summary>
        /// Funkcja sprawdzająca czy książka o podanym identyfikatorze jest dostępona w bibliotece.
        /// </summary>
        /// <param name="borrowingID">Identyfikator książki</param>
        /// <returns><c>true</c> - w przypadku kiedy książka o podanym identyfikatorze jest dostępna lub <c>false</c> - w innych przypadkach</returns>
        private bool IsBookAvailable(int bookID)
        {
            if (books.Find(b => b.ID.Equals(bookID)).BorrowedQuantity < books.Find(b => b.ID.Equals(bookID)).Quantity)
                return true;

            return false;
        }
    }
}