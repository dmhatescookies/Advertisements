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

        public ActionResult Index()
        {
            Global.AdvertisementsList = Global.db.GetAllAdvertisements();
            return View(Global.AdvertisementsList);
        }

        public ActionResult Authorization(Person person)
        {
            Session["PersonId"] = person.PersonId.ToString();
            return RedirectToAction("Cabinet", "Person", new { id = person.PersonId});
        }

        public ActionResult Details(int id)
        {
            Advertisement advert = Global.db.GetAdvert(id);
            ViewBag.Message = Global.db.GetPerson(advert.PersonId).PersonFirstname + " " +
                Global.db.GetPerson(advert.PersonId).PersonLastname;
            return View(advert);
        }

        public ActionResult AuthorDetails(int id)
        {
            return RedirectToAction("Details", "Person", new { id = id });
        }


        public ActionResult Create ()
        {
            if (Session["PersonId"] == null)
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
                    advert.PersonId = int.Parse(Session["PersonId"].ToString());
                    Global.db.AddAdvert(advert);
                    return RedirectToAction("Index");
                }
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "Authorization Error");
            }
            return View(advert);
        }
    }
}