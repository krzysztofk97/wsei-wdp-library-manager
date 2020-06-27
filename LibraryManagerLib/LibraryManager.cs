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

        public int GetBooksCount()
        {
            return books.Count;
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
