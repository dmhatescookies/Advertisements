using System;
using Ad;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.IO;
using Newtonsoft.Json;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AdvertisementsTests
{
    [TestClass]
    public class AdTests
    {
        [TestMethod]
        public void SearchTest()
        {
            List<Advertisement> list = new List<Advertisement>();
            Advertisements adverse = new Advertisements();
            List<Advertisement> expectedAdvert = new List<Advertisement>
            { new Advertisement("Education", "English", "Lessions", 1000, "+380635699990", "Julia", "Matveeva", "qweqweqw@gmail.com")};
            List<Advertisement> expectedList = new List<Advertisement>();

            expectedList.Add(new Advertisement("Education", "English", "Lessions", 2000, "+380638699990", "Dmytro", "Maryshchuk", "pidmogylnyi@gmail.com"));
            expectedList.Add(new Advertisement("Education", "English", "Lessions", 2000, "+380635799100", "Dmytro", "Maryshchuk", "pidmogylnyi@gmail.com"));

            list.Add(new Advertisement("Education", "English", "Lessions", 2000, "+380638699990", "Dmytro", "Maryshchuk", "pidmogylnyi@gmail.com")); 
            list.Add(new Advertisement("Education", "English", "Lessions", 1000, "+380635699990", "Julia", "Matveeva", "qweqweqw@gmail.com"));
            list.Add(new Advertisement("Education", "English", "Lessions", 2000, "+380635799100", "Dmytro", "Maryshchuk", "pidmogylnyi@gmail.com"));

            CollectionAssert.Equals(expectedAdvert, adverse.Search(list, "+380635699990"));
            CollectionAssert.Equals(expectedList, adverse.Search(list, 2000));
            expectedList.Clear();
            CollectionAssert.Equals(expectedList, adverse.Search(list, "Jora"));
        }

        [TestMethod]
        public void SortTest()
        {
            List<Advertisement> list = new List<Advertisement>();
            List<Advertisement> excpectedList = new List<Advertisement>();
            Advertisements adverse = new Advertisements();

            list.Add(new Advertisement("Education", "English", "Lessions", 1000, "+380635699990", "Dmytro"));
            list.Add(new Advertisement("Education", "English", "Lessions", 3000, "+380635699990", "Dmytro"));
            list.Add(new Advertisement("Education", "English", "Lessions", 500, "+380635699990", "Dmytro"));

            excpectedList.Add(new Advertisement("Education", "English", "Lessions", 500, "+380635699990", "Dmytro"));
            excpectedList.Add(new Advertisement("Education", "English", "Lessions", 1000, "+380635699990", "Dmytro"));
            excpectedList.Add(new Advertisement("Education", "English", "Lessions", 3000, "+380635699990", "Dmytro"));

            CollectionAssert.Equals(excpectedList, adverse.Sort(list, 3));
            

        }

        [TestMethod]
        public void DeserializeXmlTest()
        {
            List<Advertisement> list = new List<Advertisement>();
            list.Add(new Advertisement("Education", "English", "Lessions", 1000, "+380635699990", "Dmytro"));
            list.Add(new Advertisement("Education", "English", "Lessions", 3000, "+380635699990", "Dmytro"));
            list.Add(new Advertisement("Education", "English", "Lessions", 500, "+380635699990", "Dmytro"));

            List<Advertisement> excpectedList = list;
            Advertisements adverse = new Advertisements();

            XmlSerializer formatter = new XmlSerializer(typeof(List<Advertisement>));
            try
            {
                using (FileStream fs = new FileStream("Test.xml", FileMode.Create))
                {
                    formatter.Serialize(fs, list);
                    fs.Close();
                }
                list = null;
                list = new List<Advertisement>();
                using (FileStream fs = new FileStream("Test.xml", FileMode.Open))
                {
                    list = (List<Advertisement>)formatter.Deserialize(fs);
                    fs.Close();
                }


            }

            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                CollectionAssert.Equals(excpectedList, list);
            }

        }

        [TestMethod]
        public void DeserializeLsonTest()
        {
            List<Advertisement> list = new List<Advertisement>();
            list.Add(new Advertisement("Education", "English", "Lessions", 1000, "+380635699990", "Dmytro"));
            list.Add(new Advertisement("Education", "English", "Lessions", 3000, "+380635699990", "Dmytro"));
            list.Add(new Advertisement("Education", "English", "Lessions", 500, "+380635699990", "Dmytro"));

            List<Advertisement> excpectedList = list;
            Advertisements adverse = new Advertisements();

            XmlSerializer formatter = new XmlSerializer(typeof(List<Advertisement>));
            try
            {
                using (StreamWriter file = File.CreateText("Test.json"))
                {
                    string json = JsonConvert.SerializeObject(list);
                    file.WriteLine(json);
                    file.Close();
                }
                list = null;
                list = new List<Advertisement>();
                JsonSerializer se = new JsonSerializer();
                StreamReader re = new StreamReader("Test.json");
                JsonTextReader reader = new JsonTextReader(re);
                list = se.Deserialize<List<Advertisement>>(reader);


            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                CollectionAssert.Equals(excpectedList, list);
            }

        }
        [TestMethod]
        public void ValidatorTest()
        {
            List<Advertisement> list = new List<Advertisement>();
            List<Advertisement> excpectedList = new List<Advertisement>();
            Validation Valid = new Validation();

            excpectedList.Add(new Advertisement("Education", "English", "Lessions", 1000, "+380635699990", "Dmytro"));
            excpectedList.Add(new Advertisement("Education", "English", "Lessions", 3000, "0635699990", "Dmytro"));
            excpectedList.Add(new Advertisement("Education", "English", "Lessions", 500, "+380635699990", "Dmytro"));
            foreach (Advertisement ad in excpectedList)
            {
                if (Valid.IsValidType(ad.Type)
                   && Valid.IsValidName(ad.Name)
                   && Valid.IsValidPrice(ad.Price.ToString())
                   && Valid.IsValidPhone(ad.Owner.PhoneNumber)
                   && Valid.IsValidMainPerson(ad.Owner.Name))
                {
                    list.Add(ad);
                }
            }

            CollectionAssert.Equals(excpectedList, list);


        }
    }


}
