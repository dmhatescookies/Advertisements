using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AdvertisementsServer;
using System.Xml.Serialization;

namespace AdvertisementsMVC
{
    public class Advertisement
    {
        [XmlIgnore]
        [Key]
        public int AdvertId { get; private set; }

        public string Type { get; set; }

        [Required(ErrorMessage = "This field is required")]
        [StringLength(50, ErrorMessage = "Title cannot be longer than 50 characters")]
        [RegularExpression(@"^[a-zA-Z]{1,}$", ErrorMessage = "Invalid title")]
        [Display(Name = "Title")]
        public string Title { get; set; }

        [Required(ErrorMessage = "This field is required")]
        [StringLength(300, ErrorMessage = "Description cannot be longer than 300 characters")]
        [Display(Name = "Description")]
        public string Description { get; set; }

        [Required(ErrorMessage = "This field is required")]
        [RegularExpression(@"^[0-9]{1,}$", ErrorMessage = "Invalid price")]
        [Display(Name = "Price")]
        public int Price { get; set; }


        [ForeignKey("PersonId")]
        [NotMapped]
        internal protected virtual Person Person { get; set; }

        public int PersonId { get; set; }


        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            Advertisement forCompare = (Advertisement)obj;
            return this.Title.Equals(forCompare.Title) &&
                this.Person.Email.Equals(forCompare.Person.Email);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public Advertisement() { }

        public Advertisement(string type, string name, string description, int price, Person person)
        {
            this.Type = type;
            this.Title = name;
            this.Description = description;
            this.Price = price;
            this.PersonId = person.PersonId;
        }

        public override string ToString()
        {
            DbDatabase db = new DbDatabase();
            this.Person = db.GetPerson(this.PersonId);
            return String.Format("index: {0}, Type: {1},  Name: {2}, Description: " +
                "{3}, Price: {4}, Phone: {5}, Owner: {6} {7}\n",
                 AdvertId, Type, Title, Description, Price, Person.PhoneNumber, Person.PersonFirstname, Person.PersonLastname);
        }
    }
}
