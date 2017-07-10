using System;
using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace Ad
{
    public class Person
    {
        [XmlIgnore]
        [Key]
        public int PersonId { get; set; }
        public string PersonFirstname { get; set; }
        public string PersonLastname { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string RegistrationTime { get; set; }
        public string Password { get; set; }

        public Person() { }

        public Person(string firstName, string lastName, string phoneNumber, string email, string pass)
        {
            this.RegistrationTime = DateTime.Now.ToString();
            this.PersonFirstname = firstName;
            this.PersonLastname = lastName;
            this.PhoneNumber = phoneNumber;
            this.Email = email;
            this.Password = pass;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            Person forCompare = (Person)obj;
            return this.Email.Equals(forCompare.Email);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            return String.Format("ID:{0}, Firstname: {1},  Lastname: {2}, Phone number: " +
                "{3}, Email: {4}, Data of Registration: {5}",
                PersonId, PersonFirstname, PersonLastname, PhoneNumber, Email, RegistrationTime.ToString());
        }

    }
}
