using System;
using System.Text.RegularExpressions;

namespace LibraryManagerLib
{
    /// <summary>
    /// Klasa <c>Book</c> zawiera wszystkie parametry opisujące pojedynczą książkę, oraz funkcje zapewniające poprawność wprowadzanych danych.
    /// </summary>
    public class Book
    {
        /// <summary>
        /// Parametr <c>ID</c> przechowuje unikalny identyfikator książki.
        /// Tylko do odczytu.
        /// </summary>
        public int ID { get; private set; }

        private string title;
        /// <summary>
        /// Parametr <c>Title</c> przechowuje tytuł danej książki.
        /// </summary>
        public string Title 
        {
            get => title;

            set
            {
                if (value.Trim() == "")
                    throw new ArgumentException("Nie podano tytułu książki");
                else
                    title = value;
            }
        }

        private string author;
        /// <summary>
        /// Parametr <c>Author</c> przechowuje imię i nazwisko/nazwę autora danej ksiązki.
        /// </summary>
        public string Author
        {
            get => author;

            set
            {
                if (value.Trim() == "")
                    throw new ArgumentException("Kasiążka musi mieć autora");
                else
                    author = value;
            }
        }

        private string isbn;
        /// <summary>
        /// Parametr <c>ISBN</c> przechowuje numer ISBN danej książki.
        /// Tylko do odczytu.
        /// </summary>
        public string ISBN {
            get => isbn;

            private set
            {
                if (IsValidISBNFormat(value))
                    isbn = value;
                else
                    throw new FormatException("Numer ISBN ma błędny format");
            }
        }

        /// <summary>
        /// Funkcja sprawdzająca za pomocą zadanego wyrażenia regularnego poprawność wpisywanego przez uzytkownika numeru ISBN.
        /// </summary>
        /// <param name="isbn">Numer ISBN do sprawdzenia</param>
        /// <returns><c>false</c> - w przypadku niezgodności zmiennej <c>isbn</c> ze wzorcem lub <c>true</c> - w innych przypadkach.</returns>
        private bool IsValidISBNFormat(string isbn)
        {
            Regex regexISBN = new Regex(@"^(?=(?:\D*\d){10}(?:(?:\D*\d){3})?$)[\d-]+$");

            if (!regexISBN.IsMatch(isbn))
                return false;

            return true;
        }

        private int quantity;
        /// <summary>
        /// Parametr <c>Quantity</c> przechwuje całkowitą liczbę danego tytułu, posiadanych przez bibliotekę.
        /// </summary>
        public int Quantity
        {
            get => quantity;

            set
            {
                if (value < 0 || value < borrowedQuantity)
                    throw new ArgumentException("Liczba książek ma niepoprawną wartość");
                else 
                    quantity = value;
            }
        }

        private int borrowedQuantity;
        /// <summary>
        /// Parametr <c>BorrowedQuantity</c> przechowuje liczbę aktualnie wypożyczonych egzemplarzy danej książki.
        /// </summary>
        public int BorrowedQuantity
        {
            get => borrowedQuantity;

            set
            {
                if (value >= 0)
                {
                    if (value <= Quantity)
                        borrowedQuantity = value;
                    else
                        throw new ArgumentException("Liczba wypożyczonych egzemplarzy nie może być większa niż liczba dostępnych");
                }
                else
                    throw new ArgumentException("Liczba wypożyczonych egzemplarzy nie może być ujemna");
            }
        }

        /// <summary>
        /// Parametr <c>Shelf</c> przechowuje fizyczną pozycję książki w bibliotece. 
        /// </summary>
        public string Shelf;

        /// <summary>
        /// Konstruktor klasy <c>Book</c>.
        /// </summary>
        /// <param name="id">Identyfikator książki</param>
        /// <param name="title">Tytuł książki</param>
        /// <param name="author">Imię i nazwisko/nazwa autora książki</param>
        /// <param name="isbn">Numer ISBN książki</param>
        /// <param name="shelf">Półka, na której znajduje się książka</param>
        /// <param name="quantity">Ilość danej pozycji oferowana przez bibliotekę (domyślnie równa 0)</param>
        public Book(int id, string title, string author, string isbn, string shelf, int quantity = 0)
        {
            ID = id;
            Title = title;
            Author = author;
            ISBN = isbn;
            Shelf = shelf;
            Quantity = quantity;
            BorrowedQuantity = 0;
        }
    }
}