using System;
using System.Collections.Generic;
using System.Text;

namespace LibraryManagerLib
{
    public class LibraryManager
    {
        private List <Book> books;
        private List <Reader> readers;

        public LibraryManager()
        {
            readers = new List<Reader>();
            books = new List<Book>();
        }

        public void AddNewBook(string title, string author, string isbn)
        {
            Book b = new Book(title, author, isbn);
            AddNewBook(b);             
        }

        public void AddNewBook(Book b)
        {
            if (!IsISBNDuplicated(b.ISBN))
                books.Add(b);
            else
                throw new ArgumentException("Book with given ISBN already exists");
        }

        private bool IsISBNDuplicated (string ISBN)
        {
            for (int i = 0; i < books.Count; i++)
                if (books[i].ISBN == ISBN)
                    return true;

            return false;
        }

        public List<Book> GetBooksList(int from, int length)
        {
            return books.GetRange(from, length);
        }

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
    }
}
