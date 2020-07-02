using System;

namespace LibraryManagerLib
{
    public class Borrowing
    { 
        public int ID { get; private set; }
        
        public int BookID { get; private set; }

        public int ReaderID { get; private set; }

        public DateTime StartDate { get; private set; }

        private DateTime endDate;
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

        public DateTime ReturnedDate { get; set; }

        public Borrowing(int id, int bookID, int readerID, DateTime endDate, DateTime returnedDate)
        {
            ID = id;
            BookID = bookID;
            ReaderID = readerID;
            StartDate = DateTime.Now;
            EndDate = endDate;
            ReturnedDate = returnedDate;
        }

        private bool IsEndDateCorrect (DateTime startDate, DateTime endDate)
        {
            if (endDate > startDate)
                return true;
            else
                return false;
        }
    }
}