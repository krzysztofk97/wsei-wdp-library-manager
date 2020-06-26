using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace LibraryManagerLib
{
    public class Book
    {
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
                    throw new FormatException("ISBN has not valid format");
            }
        }

        public Book(string title, string author, string isbn)
        {
            Title = title;
            Author = author;
            ISBN = isbn;
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
