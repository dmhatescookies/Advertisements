using System.Collections.Generic;
using System.Linq;
using AdvertisementsServer;

namespace Ad
{
    class People
    {
        public List<Person> PersonList;
        private DbDatabase db;

        public People ()
        {
            db = new DbDatabase();
            PersonList = db.GetAllPersons();
        }

        public int Count
        {
            get
            {
                return PersonList.Count;
            }
        }

        public void Add(Person item)
        {
            if (!db.PersonExist(item))
                db.AddPerson(item);
            PersonList = db.GetAllPersons();
        }


        public List<Person> Sort(List<Person> list, int byWhat)
        {
            List<Person> sortedList = null;
            switch (byWhat)
            {
                case 1: //Firstname
                    sortedList = list.OrderBy(a => a.PersonFirstname).ToList();
                    break;

                case 2: //Lastname
                    sortedList = list.OrderBy(a => a.PersonLastname).ToList();
                    break;

                case 3: //RegistrationTime
                    sortedList = list.OrderBy(a => a.RegistrationTime).ToList();
                    break;

                default:
                    break;

            }
            return sortedList;
        }

        public List<Person> Search(string keyword)
        {
            List<Person> resultList = new List<Person>();
            PersonList = db.GetAllPersons();
            foreach (Person person in PersonList)
                if (person.PersonFirstname == keyword || person.PersonLastname == keyword || person.PhoneNumber == keyword ||
                    person.Email == keyword)
                    resultList.Add(person);
            return resultList;
        }


        public override string ToString()
        {
            string result = "";
            foreach (Person person in PersonList)
            {
                result += person.ToString();
            }
            return result;
        }
    }
}
