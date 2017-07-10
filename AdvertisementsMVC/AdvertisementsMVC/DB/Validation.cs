using System.Text.RegularExpressions;
using AdvertisementsMVC;

namespace AdvertisementsServer
{
    public class Validation
    {
        public bool isValidPerson(Person person)
        {
            string ErrorMessages = "";
            if (!IsValidFirstname(person.PersonFirstname))
                ErrorMessages +=  "Invalid firstname";
            if (!IsValidLastname(person.PersonLastname))
                ErrorMessages += "\nInvalid lastname";
            if (!IsValidPhone(person.PhoneNumber))
                ErrorMessages += "\nInvalid phone number";
            if (!IsValidEmail(person.Email))
                ErrorMessages += "\nInvalid email";
            if (!IsValidPassword(person.Password))
                ErrorMessages += "\nInvalid password";
            if (ErrorMessages.Length != 0)
                throw new System.ArgumentException(ErrorMessages);
            return true;
        }

        public bool IsValidName(string name)
        {
            int number;
            if (name.Length > 3 && name.Length < 50 && !int.TryParse(name, out number))
                return true;
            return false;
        }

        public bool IsValidType(string type)
        {
            int number = 0;


            if (int.TryParse(type, out number) && number < 3 && number > 0)
                return true;
            return false;
        }

        public bool IsValidPrice(string price)
        {
            int number = 0;


            if (int.TryParse(price, out number) && number >= 0)
                return true;
            return false;
        }

        public bool IsValidFirstname(string name)
        {
            int number;
            if (name.Length > 2 && name.Length < 40 && !int.TryParse(name, out number))
                return true;
            return false;
        }

        public bool IsValidLastname(string name)
        {
            int number;
            if (name.Length > 5 && name.Length < 40 && !int.TryParse(name, out number))
                return true;
            return false;
        }

        public bool IsValidEmail(string email)
        {
            Regex expression = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
            if (expression.IsMatch(email))
                return true;
            return false;
        }

        public bool IsValidPhone(string phone)
        {
            Regex expression = new Regex(@"^([+]\d{1,2})?[ ]?-?\d{3}?-?[ ]?\d{7}$");
            if (expression.IsMatch(phone))
                return true;
            return false;
        }

        public bool IsValidPassword(string pass)
        {
            Regex expression = new Regex(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{8,15}$");
            if (expression.IsMatch(pass))
                return true;
            return false;
        }



    }
}
