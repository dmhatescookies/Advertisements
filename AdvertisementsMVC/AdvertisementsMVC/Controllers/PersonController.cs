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
            Global.PersonList = Global.DataAccess.GetAllPersons();
            return View(Global.PersonList);
        }

        public ActionResult Details(int id)
        {
            Person person = Global.DataAccess.GetPerson(id);
            return View(person);
        }

        public ActionResult Edit(int id)
        {
            Person person = Global.DataAccess.GetPerson(id);
            return View(person);
        }

        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public ActionResult Edit (Person updatePerson)
        {
            ModelState.Remove("Email");
            ModelState.Remove("PhoneNumber");
            ModelState.Remove("ConfirmPassword");
            ModelState.Remove("Password");
            try
            {
                Global.DataAccess.UpdatePerson(updatePerson);
                return RedirectToAction("Cabinet");
            }
            catch
            {
                return View(updatePerson);
            }
        }

        public ActionResult Create()
        {
            return View();
        }

        public ActionResult RemoveAdvertisement(int id)
        {
            return RedirectToAction("Remove", "Advertisements", new { id = id });
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
            return RedirectToAction("Index", "Advertisements");
        }

        public ActionResult EditAdvertisement (int id)
        {
            return RedirectToAction("Edit", "Advertisements", new { id = id });
        }

        public ActionResult Cabinet(int? id)
        {
            int Id = 0;
            if (id == null)
                Id = int.Parse(Session["PersonId"].ToString());
            else
                Id = (int)id;
            Global.AdvertisementsList = Global.DataAccess.GetAllUserAdvertisements(Id);
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
                    if (Global.DataAccess.SearchPerson(person.Email)[0].Password == person.Password)
                        return RedirectToAction("Authorization", "Advertisements", Global.DataAccess.SearchPerson(person.Email)[0]);
                    ModelState.AddModelError("", "Email or password is wrong.");
                    return View(Global.DataAccess.SearchPerson(person.Email)[0]);
                }
            }
            catch (RetryLimitExceededException)
            {

            }
            return View(person);
        }

        public ActionResult Delete(int id)
        {
            Person person = Global.DataAccess.GetPerson(id);
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
                    Global.DataAccess.AddPerson(person);
                    Person tempPerson = Global.DataAccess.SearchPerson(person.Email)[0];
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
            Global.AdvertisementsList = Global.DataAccess.GetAllUserAdvertisements(id);
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
            if (!Global.DataAccess.PersonExist(item))
                Global.DataAccess.AddPerson(item);
            Global.PersonList = Global.DataAccess.GetAllPersons();
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
            Global.PersonList = Global.DataAccess.GetAllPersons();
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