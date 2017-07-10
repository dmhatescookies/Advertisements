using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AdvertisementsServer;

namespace AdvertisementsMVC.Controllers
{
    public class AdvertisementsController : Controller
    {

        public List<Person> PersonList = new List<Person>();
        public List<Advertisement> AdvertisementsList = new List<Advertisement>();
        private DbDatabase db = new DbDatabase();
        private Validation Valid = new Validation();

        public ActionResult Index()
        {
            AdvertisementsList = db.GetAllAdvertisements();
            return View(AdvertisementsList);
        }

        public ActionResult Authorization(Person person)
        {
            Global.AuthorizedUser = person;
            return RedirectToAction("Create");
        }

        public ActionResult AuthorDetails(int id)
        {
            return RedirectToAction("Details", "Person", id);
        }

        public ActionResult Create ()
        {
            if (Global.AuthorizedUser == null)
            {
                return RedirectToAction("SignIn", "Person");
            }
            return View();
        }

        [HttpPost, ActionName("Create")]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Type, Name, Description, Price")]Advertisement advert)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    advert.PersonId = Global.AuthorizedUser.PersonId;
                    db.AddAdvert(advert);
                    return RedirectToAction("Index");
                }
            }
            catch (Exception)
            {

            }
            return View(advert);
        }
    }
}