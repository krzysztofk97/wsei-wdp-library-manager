using System;
using System.Text.RegularExpressions;

namespace LibraryManagerLib
{
    public class Book
    {
        public int ID { get; private set; }

        public string Title { get; private set; }

        public string Author { get; set; }

        private string isbn;
        public string ISBN {
            get => isbn;

            set
            {
                if (IsValidISBNFormat(value))
                    isbn = value;
                else
                    throw new FormatException("Numer ISBN ma błędny format");
            }
        }

        private int quantity;
        public int Quantity
        {
            get => quantity;

            set
            {
                if (value < 0)
                    throw new ArgumentException("Liczba książek ma niepoprawną wartość");
                else 
                    quantity = value;
            }
        }

        public string Shelf;

        public Book(int id, string title, string author, string isbn, string shelf, int quantity = 0)
        {
            ID = id;
            Title = title;
            Author = author;
            ISBN = isbn;
            Shelf = shelf;
            Quantity = quantity;
        }

        private bool IsValidISBNFormat (string isbn)
        {
            Regex regexISBN = new Regex(@"^(?=(?:\D*\d){10}(?:(?:\D*\d){3})?$)[\d-]+$");

            if (!regexISBN.IsMatch(isbn))
                return false;

            return true;
        }

    }
}
