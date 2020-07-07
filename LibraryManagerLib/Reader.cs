using System;
using System.Net.Mail;
using System.Text.RegularExpressions;

namespace LibraryManagerLib
{
    /// <summary>
    /// Klasa <c>Reader</c> zawiera wszystkie parametry opisujące pojedynczego czytelnika, oraz funkcje zapewniające poprawność wprowadzanych danych.
    /// </summary>
    public class Reader
    {
        /// <summary>
        /// Parametr <c>ID</c> przechowuje unikalny identyfikator czytelnika.
        /// Tylko do odczytu.
        /// </summary>
        public int ID { get; private set; }

        /// <summary>
        /// Parametr <c>Name</c> przechowuje imię i nazwisko/nazwę czytelnika.
        /// </summary>
        public string Name { get; set; }

        private string phoneNumber;
        /// <summary>
        /// Parametr <c>PhoneNumber</c> przechowuje numer telefonu czytelnika.
        /// </summary>
        public string PhoneNumber {
            get => phoneNumber;

            set
            {
                if (IsValidPhoneNumberFormat(value))
                    phoneNumber = value;
                else
                    throw new FormatException("Numer telefonu ma błędny format");
            }
        }

        /// <summary>
        /// Funkcja sprawdzająca za pomocą zadanego wyrażenia regularnego poprawność wpisywanego przez uzytkownika numeru telefonu.
        /// </summary>
        /// <param name="phoneNumber">Numer telefonu do sprawdzenia</param>
        /// <returns><c>false</c> - w przypadku niezgodności zmiennej <c>phoneNumber</c> ze wzorcem lub <c>true</c> - w innych przypadkach</returns>
        private bool IsValidPhoneNumberFormat(string phoneNumber)
        {
            Regex regexPhoneNumber = new Regex(@"^[+]*[(]{0,1}[0-9]{1,4}[)]{0,1}[-\s\./0-9]*$");

            if (!regexPhoneNumber.IsMatch(phoneNumber))
                return false;

            return true;
        }

        private string emailAddress;
        /// <summary>
        /// Parametr <c>EmailAddress</c> przechowuje adres email czytelnika.
        /// </summary>
        public string EmailAddress 
        {
            get => emailAddress;

            set
            {
                if (IsValidEmailAddressFormat(value))
                    emailAddress = value;
                else
                    throw new FormatException("Adres email ma błędny format");
            }
        }

        /// <summary>
        /// Funkcja sprawdzająca z pomocą obiektu klasy <c>System.Net.Mail.MailAddress</c> poprawność wpisywanego przez użytkownika adresu email.
        /// </summary>
        /// <param name="emailAddress">Adres email do sprawdzenia</param>
        /// <returns><c>true</c> - w przypadku zgodności zmiennej <c>emailAddress</c> ze wzorcem lub <c>false</c> - w przypadku napotkania wyjątku.</returns>
        private bool IsValidEmailAddressFormat(string emailAddress)
        {
            try
            {
                MailAddress emailAddressCheck = new MailAddress(emailAddress);
                return emailAddressCheck.Address == emailAddress;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Konstruktor klasy <c>Reader</c>.
        /// </summary>
        /// <param name="id">Identyfikator czytelnika</param>
        /// <param name="name">Imię i nazwisko/nazwa czytelnika</param>
        /// <param name="phoneNumber">Numer telefonu czytelnika</param>
        /// <param name="emailAddress">Adres email czytelnika</param>
        public Reader(int id, string name, string phoneNumber, string emailAddress)
        {
            ID = id;
            Name = name;
            PhoneNumber = phoneNumber;
            EmailAddress = emailAddress;
        }
    }
}