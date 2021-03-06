﻿using System;
using System.Collections.Generic;
using System.Linq;
using Ad;

namespace AdvertisementsServer
{
    public class DbDatabase
    {
        private AdvertisementsContext db;

        public DbDatabase()  
        {
            db = new AdvertisementsContext();
        }

        public void AddAdvert(Advertisement adv)
        {
            db.Advertisements.Add(adv);
            db.SaveChanges();
        }

        public void AddPerson(Person person)
        {
            db.Persons.Add(person);
            db.SaveChanges();
        }

        public List<Advertisement> GetAllAdvertisements()
        {
            return db.Advertisements.ToList();
        }

        public List<Person> GetAllPersons()
        {
            return db.Persons.ToList();
        }

        public List<Advertisement> GetAllUserAdvertisements(int id)
        {
            List<Advertisement> resultList = new List<Advertisement>();
            List<Advertisement> list = db.Advertisements.ToList();
            foreach (Advertisement adv in list)
                if (adv.Person.PersonId == id)
                    resultList.Add(adv);
            return resultList;
        }

        public Advertisement GetAdvert(int id)
        {
            return db.Advertisements.FirstOrDefault(x => x.AdvertId == id);
        }

        public Person GetPerson(int id)
        {
            return db.Persons.FirstOrDefault(x => x.PersonId == id);
        }

        public bool AdvertisementExist(int id)
        {
            return db.Advertisements.FirstOrDefault(x => x.AdvertId == id) != null;
        }

        public bool PersonExist(Person person)
        {
            return db.Persons.FirstOrDefault(x => x.Email == person.Email) != null;
        }

        public void RemoveAdvertisement(int id)
        {
            Advertisement removeAdvert = db.Advertisements.FirstOrDefault(x => x.AdvertId == id);
            if (removeAdvert != null)
            {
                db.Advertisements.Remove(removeAdvert);
            }
            else
            {
                throw new Exception();
            }
            db.SaveChanges();
        }

        public void Create(List<Advertisement> services)
        {
            foreach (Advertisement adv in services)
                if (!db.Advertisements.ToList().Any(x => x.Type == adv.Type
                                               && x.Name == adv.Name
                                               && x.PersonId == adv.PersonId))
                    db.Advertisements.Add(adv);
            db.SaveChanges();
        }
                                                      
        public void Dispose()
        {
            db.Dispose();
        }

    }
}