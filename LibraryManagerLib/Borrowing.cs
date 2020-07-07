using System;

namespace LibraryManagerLib
{
    /// <summary>
    /// Klasa <c>Borrowing</c> zawiera wszystkie parametry opisujące pojedyncze wyożyczenie, oraz funkcje zapewniające poprawność wprowadzanych danych.
    /// </summary>
    public class Borrowing
    {
        /// <summary>
        /// Parametr <c>ID</c> przechowuje unikalny identyfikator wypożyczenia.
        /// Tylko do odczytu.
        /// </summary>
        public int ID { get; private set; }

        /// <summary>
        /// Parametr <c>BookID</c> przechowuje identyfikator wypożyczonej książki.
        /// Tylko do odczytu.
        /// </summary>
        public int BookID { get; private set; }

        /// <summary>
        /// Parametr <c>ReaderID</c> przechowuje identyfikator czytelnika, który wypożycza daną książkę.
        /// Tylko do odczytu.
        /// </summary>
        public int ReaderID { get; private set; }

        /// <summary>
        /// Parametr <c>StartDate</c> przechowuje datę wypożyczenia.
        /// Tylko do odczytu.
        /// </summary>
        public DateTime StartDate { get; private set; }

        private DateTime endDate;
        /// <summary>
        /// Parametr <c>EndDate</c> przechowuje datę zakończenia wypożyczenia.
        /// </summary>
        public DateTime EndDate
        {
            get => endDate;

            set
            {
                if (IsEndDateCorrect(StartDate, value))
                    endDate = value;
                else
                    throw new ArgumentException("Data zakończenia wypożyczenia jest nieprawidłowa");
            }
        }

        /// <summary>
        /// Parametr <c>ReturnedDate</c> przechowuje datę, kiedy dokonano zwrotu książki.
        /// W przypadku kiedy pozycja nie została jeszcze zwrócona posiada ona wartość: <c>0001.01.01 00:00:00</c>.
        /// </summary>
        public DateTime ReturnedDate { get; set; }

        /// <summary>
        /// Konstruktor klsy <c>Borrowing</c>.
        /// </summary>
        /// <param name="id">Identyfikator wypożyczenia</param>
        /// <param name="bookID">Identyfikator książki</param>
        /// <param name="readerID">Identyfikator czytelnika</param>
        /// <param name="startDate">Data rozpoczęcia wypożyczenia</param>
        /// <param name="endDate">Data zakończenia wypożyczenia</param>
        /// <param name="returnedDate">Data zwrócenia książki</param>
        public Borrowing(int id, int bookID, int readerID, DateTime startDate, DateTime endDate, DateTime returnedDate)
        {
            ID = id;
            BookID = bookID;
            ReaderID = readerID;
            StartDate = startDate;
            EndDate = endDate;
            ReturnedDate = returnedDate;
        }

        /// <summary>
        /// Funkcja sprawdzająca poprawność daty zakończenia wypożyczenia.
        /// </summary>
        /// <param name="startDate">Data rozpoczęcia wypożyczenia</param>
        /// <param name="endDate">Data zakończenia wypożyczenia</param>
        /// <returns><c>true</c> - w przypadku kiedy data zakończenia jest nowsza niż data rozpoczęcia wypożyczenia lub <c>false</c> - w innych przypadkach</returns>
        private bool IsEndDateCorrect(DateTime startDate, DateTime endDate)
        {
            if (endDate > startDate)
                return true;
            else
                return false;
        }
    }
}