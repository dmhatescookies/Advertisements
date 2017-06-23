using System.Text.RegularExpressions;

namespace Ad
{
    public class Validation
    {
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

        public bool IsValidMainPerson(string name)
        {
            int number;
            if (name.Length > 5 && name.Length < 40 && !int.TryParse(name, out number))
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


    }
}
