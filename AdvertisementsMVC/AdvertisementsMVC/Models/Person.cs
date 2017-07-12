using System;
using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;
using AdvertisementsServer;

namespace AdvertisementsMVC
{
    public class Person
    {
        [XmlIgnore]
        [Key]
        public int PersonId { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "First name cannot be longer than 50 characters.")]
        [RegularExpression(@"^([a-zA-Z0-9 \.\&\'\-]+)$", ErrorMessage = "Invalid first Name")]
        [Display(Name = "First Name")]
        public string PersonFirstname { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "Last name cannot be longer than 50 characters.")]
        [RegularExpression(@"^([a-zA-Z0-9 \.\&\'\-]+)$", ErrorMessage = "Invalid last Name")]
        [Display(Name = "Last Name")]
        public string PersonLastname { get; set; }

        [Required]
        [RegularExpression(@"^([+]\d{1,2})?[ ]?-?\d{3}?-?[ ]?\d{7}$", ErrorMessage = "Invalid phone number")]
        [Unique(ErrorMessage = "This phone number already exists")]
        [Display(Name = "Phone number")]
        public string PhoneNumber { get; set; }

        [Required]
        [EmailAddress(ErrorMessage = "Invalid email")]
        [Unique(ErrorMessage = "This email already exists")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        public string RegistrationTime { get; set; }

        [Required]
        [MinLength(6, ErrorMessage = "Password length must be greater then 8")]
        [RegularExpression(@"(?=.*\d)(?=.*[a-z])(?=.*[A-Z]).{8,}", ErrorMessage = "Upper-case, low-case and digits are required")]
        [Display(Name = "Password")]
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
            return (this.Email.Equals(forCompare.Email) && this.PhoneNumber.Equals(forCompare.PhoneNumber));
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

    public class UniqueAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            DbDatabase db = new DbDatabase();
            return db.SearchPerson((string)value).Count == 0;
        }
    }
}
