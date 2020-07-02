using System;
using System.Net.Mail;
using System.Text.RegularExpressions;

namespace LibraryManagerLib
{
    public class Reader
    {
        public int ID { get; private set; }

        public string Name { get; set; }

        private string phoneNumber;
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

        private bool IsValidPhoneNumberFormat(string phoneNumber)
        {
            Regex regexPhoneNumber = new Regex(@"^[+]*[(]{0,1}[0-9]{1,4}[)]{0,1}[-\s\./0-9]*$");

            if (!regexPhoneNumber.IsMatch(phoneNumber))
                return false;

            return true;
        }

        private string emailAddress;
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

        public Reader(int id, string name, string phoneNumber, string emailAddress)
        {
            ID = id;
            Name = name;
            PhoneNumber = phoneNumber;
            EmailAddress = emailAddress;
        }
    }
}