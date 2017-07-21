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
            Global.AdvertisementsList = Global.DataAccess.GetAllAdvertisements();
            return View(Global.AdvertisementsList);
        }

        public ActionResult Authorization(Person person)
        {
            Session["PersonId"] = person.PersonId.ToString();
            return RedirectToAction("Cabinet", "Person", new { id = person.PersonId});
        }

        public ActionResult Details(int id)
        {
            Advertisement advert = Global.DataAccess.GetAdvert(id);
            ViewBag.Message = Global.DataAccess.GetPerson(advert.PersonId).PersonFirstname + " " +
                Global.DataAccess.GetPerson(advert.PersonId).PersonLastname;
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
        public ActionResult Create([Bind(Include = "Type, Title, Description, Price")]Advertisement advert)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    advert.PersonId = int.Parse(Session["PersonId"].ToString());
                    Global.DataAccess.AddAdvert(advert);
                    return RedirectToAction("Index");
                }
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "Authorization Error");
            }
            return View(advert);
        }

        public ActionResult Edit(int id)
        {
            Advertisement Advertise = Global.DataAccess.GetAdvert(id);
            return View(Advertise);
        }

        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Advertisement advert)
        {
            ModelState.Remove("PersonId");
            if (!ModelState.IsValid)
                throw new ArgumentException();
            try
            {
                Global.DataAccess.UpdateAdvertisement(advert);
                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public ActionResult Remove (int id)
        {
            Global.DataAccess.RemoveAdvertisement(id);

            return RedirectToAction("Cabinet", "Person");
        }
    }
}