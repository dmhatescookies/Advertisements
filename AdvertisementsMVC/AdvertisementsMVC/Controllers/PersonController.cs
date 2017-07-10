using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.Web.Mvc;
using AdvertisementsServer;
using System.Data.Entity.Infrastructure;

namespace AdvertisementsMVC.Controllers
{
    public class PersonController : Controller
    {
        public List<Person> PersonList = new List<Person>();
        public List<Advertisement> AdvertisementsList = new List<Advertisement>();
        private DbDatabase db = new DbDatabase();
        private Validation Valid = new Validation(); 

        public ActionResult Index()
        {
            PersonList = db.GetAllPersons();
            return View(PersonList);
        }   

        public ActionResult Details(int id)
        {
            Person person = db.GetPerson(id);
            return View(person);
        }

        public ActionResult Edit (int id)
        {
            Person person = db.GetPerson(id);
            return View(person);
        }

        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public ActionResult EditPost(int id)
        {
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var personToUpdate = db.GetPerson(id);
            if (TryUpdateModel(personToUpdate, "",
               new string[] { "PersonFirstname", "PersonLastname", "PhoneNumber", "Email", "RegistrationTime", "Password" }))
            {
                try
                {
                    db.Save();

                    return RedirectToAction("Index");
                }
                catch (RetryLimitExceededException)
                {
                    ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
                }
            }
            return View(personToUpdate);
        }

        public ActionResult Create()
        {
            return View();  
        }

        public  ActionResult SignIn()
        {
            return View();
        }

        public ActionResult LogIn ()
        {
            return View();
        }

        [HttpPost, ActionName("LogIn")]
        [ValidateAntiForgeryToken]
        public ActionResult LogIn([Bind(Include = "Email, Password")]Person person)
        {
            try
            {
                ModelState.Remove("Email");
                ModelState.Remove("PersonFirstname");
                ModelState.Remove("PersonLastname");
                ModelState.Remove("PhoneNumber");
                if (ModelState.IsValid)
                {
                    if (db.SearchPerson(person.Email)[0].Password == person.Password)
                        return RedirectToAction("Authorization", "Advertisements", person);
                    return View(person);
                }
            }
            catch (RetryLimitExceededException)
            {

            }
            return View(person);
        }

        public ActionResult Delete(int id)
        {
            Person person = db.GetPerson(id);
            return View(person);
        }

        [HttpPost, ActionName("Create")]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "PersonFirstname, PersonLastname, PhoneNumber, Email, Password")]Person person)
        {
            try
            {
                if (ModelState.IsValid)
                { 
                    person.RegistrationTime = DateTime.Now.ToString();
                    db.AddPerson(person);
                    Person tempPerson = db.SearchPerson(person.Email)[0];
                    return RedirectToAction("Authorization", "Advertisements", tempPerson);
                }
            }
            catch (RetryLimitExceededException)
            {
                
            }
            return View(person);
        }

        public ActionResult ShowAllPosts (int id)
        {
            AdvertisementsList = db.GetAllUserAdvertisements(id);
            return View(AdvertisementsList);
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