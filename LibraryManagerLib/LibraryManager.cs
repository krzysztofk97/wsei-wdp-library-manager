using System;
using System.Collections.Generic;

namespace LibraryManagerLib
{
    public class LibraryManager
    {
        private List<Book> books;
        private List<Reader> readers;
        private List<Borrowing> borrowings;

        public LibraryManager()
        {
            books = new List<Book>();
            readers = new List<Reader>();
            borrowings = new List<Borrowing>();
        }

        //Tworzenie i modyfikowanie listy książek

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

        private bool IsISBNDuplicated (string isbn)
        {
            for (int i = 0; i < books.Count; i++)
                if (books[i].ISBN == isbn)
                    return true;

            return false;
        }

        public int GetBooksCount()
        {
            return books.Count;
        }

        public List<Book> GetBooksList(int from, int length)
        {
            return books.GetRange(from, length);
        }

        //Tworzenie i modyfikowanie listy czytelników

        public void AddNewReader(string name, string phoneNumber, string emailAddress)
        {
            int id = readers.Count + 1;

            Reader r = new Reader(id, name, phoneNumber, emailAddress);
            readers.Add(r);
        }

        public int GetReadersCount()
        {
            return readers.Count;
        }

        public List<Reader> GetReadersList(int from, int length)
        {
            return readers.GetRange(from, length);
        }

        //Tworzenie i modyfikowanie listy wypożyczeń

        public void AddNewBorrowing(int bookID, int readerID, DateTime endTime)
        {
            int id = borrowings.Count + 1;
           
            Borrowing b = new Borrowing(id, bookID, readerID, endTime, new DateTime(1, 1, 1, 0, 0, 0));
            AddNewBorrowing(b);
        }

        public void AddNewBorrowing(Borrowing b)
        {
            if (IsBookIDExists(b.BookID))
            {
                if (IsReaderIDExists(b.ReaderID))
                    borrowings.Add(b);
                else
                    throw new ArgumentException("Czytelnik o podanym identyfikatorze nie istnieje");
            }
            else
                throw new ArgumentException("Książka o podanym identyfikatorze nie istnieje");
        }

        public int GetBorrowingsCount()
        {
            return borrowings.Count;
        }

        public List<Borrowing> GetBorrowingsList(int from, int length)
        {
            List<Borrowing> reversedBorrowings = borrowings;
            reversedBorrowings.Reverse();

            return reversedBorrowings.GetRange(from, length);
        }

        private bool IsReaderIDExists(int readerID)
        {
            for (int i = 0; i < GetReadersCount(); i++)
                if (readers[i].ID == readerID)
                    return true;

            return false;
        }
        
        private bool IsBookIDExists(int bookID)
        {
            if (bookID > GetBooksCount())
                return false;

            for (int i = 0; i < GetBooksCount(); i++)
                if (books[i].ID == bookID)
                    return true;

            return false;
        }

        public string GetBookName(int bookID)
        {
            return books.Find(b => b.ID.Equals(bookID)).Title;
        }

        public void EndBorrowing(int borrowingID)
        {
            if (IsBorrowingIDExists(borrowingID))
            {
                if (!IsBookReturned(borrowingID))
                    borrowings.Find(b => b.ID.Equals(borrowingID)).ReturnedDate = DateTime.Now;
                else
                    throw new ArgumentException("Wypożyczenie o podanym identyfikatorze zostało już zakończone");
            }
            else
                throw new ArgumentException("Wypożyczenie o podanym identyfikatorze nie istnieje");
        }

        private bool IsBookReturned(int borrowingID)
        {
            if (borrowings.Find(b => b.ID.Equals(borrowingID)).ReturnedDate > new DateTime(1, 1, 1, 0, 0, 0))
                return true;

            return false;
        }

        private bool IsBorrowingIDExists(int borrowingID)
        {
            if (borrowingID > GetBorrowingsCount())
                return false;

            for (int i = 0; i < GetBorrowingsCount(); i++)
                if (borrowings[i].ID == borrowingID)
                    return true;

            return false;
        }
    }
}
