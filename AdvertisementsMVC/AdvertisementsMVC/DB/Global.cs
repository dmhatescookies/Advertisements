﻿using AdvertisementsMVC;
using System.Collections.Generic;

namespace AdvertisementsServer
{
    public static class Global
    {
        public static List<Person> PersonList = new List<Person>();
        public static List<Advertisement> AdvertisementsList = new List<Advertisement>();
        public static DbDatabase DataAccess = new DbDatabase();
        public static Validation Valid = new Validation();
    }
}