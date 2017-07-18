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

        public ActionResult Index()
        {
            Global.PersonList = Global.db.GetAllPersons();
            return View(Global.PersonList);
        }

        public ActionResult Details(int id)
        {
            Person person = Global.db.GetPerson(id);
            return View(person);
        }

        public ActionResult Edit(int id)
        {
            Person person = Global.db.GetPerson(id);
            return View(person);
        }

        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "PersonId, PersonFirstname, PersonLastname, PhoneNumber")] Person person)
        {
            int id = person.PersonId;
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var personToUpdate = Global.db.GetPerson(id);
            if (TryUpdateModel(personToUpdate, "",
               new string[] { "PersonFirstname", "PersonLastname", "PhoneNumber" }))  //TODO update bug
            {
                try
                {
                    Global.db.Save();

                    return RedirectToAction("Details", "Person", new {id = id });
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

        public ActionResult SignIn()
        {
            if (Session["PersonId"] != null)
                return RedirectToAction("Cabinet", new { id = int.Parse(Session["PersonId"].ToString()) });
            return View();
        }

        public ActionResult LogIn()
        {
            if (Session["PersonId"] != null)
                return RedirectToAction("Cabinet", new { id = int.Parse(Session["PersonId"].ToString()) });
            return View();
        }

        public ActionResult LogOut()
        {
            if (Session["PersonId"] != null)
                Session.Remove("PersonId");
            return RedirectToAction("Index", "Home");
        }

        public ActionResult Cabinet(int id)
        {
            Global.AdvertisementsList = Global.db.GetAllUserAdvertisements(id);
            return View(Global.AdvertisementsList);
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
                ModelState.Remove("ConfirmPassword");

                if (ModelState.IsValid)
                {
                    if (Global.db.SearchPerson(person.Email)[0].Password == person.Password)
                        return RedirectToAction("Authorization", "Advertisements", Global.db.SearchPerson(person.Email)[0]);
                    ModelState.AddModelError("", "Email or password is wrong.");
                    return View(Global.db.SearchPerson(person.Email)[0]);
                }
            }
            catch (RetryLimitExceededException)
            {

            }
            return View(person);
        }

        public ActionResult Delete(int id)
        {
            Person person = Global.db.GetPerson(id);
            return View(person);
        }

        [HttpPost, ActionName("Create")]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "PersonFirstname, PersonLastname, PhoneNumber, Email, Password, ConfirmPassword")]Person person)
        {
            try
            {
                if (ModelState.IsValid)
                { 
                    person.RegistrationTime = DateTime.Now.ToString();
                    Global.db.AddPerson(person);
                    Person tempPerson = Global.db.SearchPerson(person.Email)[0];
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
            Global.AdvertisementsList = Global.db.GetAllUserAdvertisements(id);
            return View(Global.AdvertisementsList);
        }

        public ActionResult AdvertDetails (int id)
        {
            return RedirectToAction("Details", "Advertisements", new { id = id });
        }

        public int Count
        {
            get
            {
                return Global.PersonList.Count;
            }
        }

        public void Add(Person item)
        {
            if (!Global.db.PersonExist(item))
                Global.db.AddPerson(item);
            Global.PersonList = Global.db.GetAllPersons();
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
            Global.PersonList = Global.db.GetAllPersons();
            foreach (Person person in Global.PersonList)
                if (person.PersonFirstname == keyword || person.PersonLastname == keyword || person.PhoneNumber == keyword ||
                    person.Email == keyword)
                    resultList.Add(person);
            return resultList;
        }


        public override string ToString()
        {
            string result = "";
            foreach (Person person in Global.PersonList)
            {
                result += person.ToString();
            }
            return result;
        }
    }
}