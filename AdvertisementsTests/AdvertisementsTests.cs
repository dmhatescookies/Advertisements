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
        public void Are_Results_Of_Searh_Equel()
        {
            List<Advertisements.Ad> list = new List<Advertisements.Ad>();
            Advertisements adverse = new Advertisements();

            list.Add(new Advertisements.Ad("Education", "English", "Lessions", 1000, "+380635699990", "Dmytro"));
            list.Add(new Advertisements.Ad("Education", "English", "Lessions", 1000, "+380635699780", "Dmytro"));
            list.Add(new Advertisements.Ad("Education", "English", "Lessions", 1000, "+380635799100", "Dmytro"));

            Assert.AreEqual("+380635799100", adverse.Search(list, "+380635799100").PhoneNumber); 
        }

        [TestMethod]
        public void Are_Results_Of_Sort_Equel()
        {
            List<Advertisements.Ad> list = new List<Advertisements.Ad>();
            List<Advertisements.Ad> excpectedList = new List<Advertisements.Ad>();
            Advertisements adverse = new Advertisements();

            list.Add(new Advertisements.Ad("Education", "English", "Lessions", 1000, "+380635699990", "Dmytro"));
            list.Add(new Advertisements.Ad("Education", "English", "Lessions", 3000, "+380635699990", "Dmytro"));
            list.Add(new Advertisements.Ad("Education", "English", "Lessions", 500, "+380635699990", "Dmytro"));

            excpectedList.Add(new Advertisements.Ad("Education", "English", "Lessions", 500, "+380635699990", "Dmytro"));
            excpectedList.Add(new Advertisements.Ad("Education", "English", "Lessions", 1000, "+380635699990", "Dmytro"));
            excpectedList.Add(new Advertisements.Ad("Education", "English", "Lessions", 3000, "+380635699990", "Dmytro"));

            CollectionAssert.Equals(excpectedList, adverse.Sort(list, 3));
            

        }

        [TestMethod]
        public void Is_It_Possiblie_To_Deserialize_From_Xml()
        {
            List<Advertisements.Ad> list = new List<Advertisements.Ad>();
            list.Add(new Advertisements.Ad("Education", "English", "Lessions", 1000, "+380635699990", "Dmytro"));
            list.Add(new Advertisements.Ad("Education", "English", "Lessions", 3000, "+380635699990", "Dmytro"));
            list.Add(new Advertisements.Ad("Education", "English", "Lessions", 500, "+380635699990", "Dmytro"));

            List<Advertisements.Ad> excpectedList = list;
            Advertisements adverse = new Advertisements();

            XmlSerializer formatter = new XmlSerializer(typeof(List<Advertisements.Ad>));
            try
            {
                using (FileStream fs = new FileStream("Test.xml", FileMode.Create))
                {
                    formatter.Serialize(fs, list);
                    fs.Close();
                }
                list = null;
                list = new List<Advertisements.Ad>();
                using (FileStream fs = new FileStream("Test.xml", FileMode.Open))
                {
                    list = (List<Advertisements.Ad>)formatter.Deserialize(fs);
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
        public void Is_It_Possible_To_Deserialize_From_Json()
        {
            List<Advertisements.Ad> list = new List<Advertisements.Ad>();
            list.Add(new Advertisements.Ad("Education", "English", "Lessions", 1000, "+380635699990", "Dmytro"));
            list.Add(new Advertisements.Ad("Education", "English", "Lessions", 3000, "+380635699990", "Dmytro"));
            list.Add(new Advertisements.Ad("Education", "English", "Lessions", 500, "+380635699990", "Dmytro"));

            List<Advertisements.Ad> excpectedList = list;
            Advertisements adverse = new Advertisements();

            XmlSerializer formatter = new XmlSerializer(typeof(List<Advertisements.Ad>));
            try
            {
                using (StreamWriter file = File.CreateText("Test.json"))
                {
                    string json = JsonConvert.SerializeObject(list);
                    file.WriteLine(json);
                    file.Close();
                }
                list = null;
                list = new List<Advertisements.Ad>();
                JsonSerializer se = new JsonSerializer();
                StreamReader re = new StreamReader("Test.json");
                JsonTextReader reader = new JsonTextReader(re);
                list = se.Deserialize<List<Advertisements.Ad>>(reader);


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
        public void Is_Validator_Work_Correctly()
        {
            List<Advertisements.Ad> list = new List<Advertisements.Ad>();
            List<Advertisements.Ad> excpectedList = new List<Advertisements.Ad>();
            Validation Valid = new Validation();

            excpectedList.Add(new Advertisements.Ad("Education", "English", "Lessions", 1000, "+380635699990", "Dmytro"));
            excpectedList.Add(new Advertisements.Ad("Education", "English", "Lessions", 3000, "0635699990", "Dmytro"));
            excpectedList.Add(new Advertisements.Ad("Education", "English", "Lessions", 500, "+380635699990", "Dmytro"));
            foreach (Advertisements.Ad ad in excpectedList)
            {
                if (Valid.IsValidType(ad.Type)
                   && Valid.IsValidName(ad.Name)
                   && Valid.IsValidPrice(ad.Price.ToString())
                   && Valid.IsValidPhone(ad.PhoneNumber)
                   && Valid.IsValidMainPerson(ad.MainPerson))
                {
                    list.Add(ad);
                }
            }

            CollectionAssert.Equals(excpectedList, list);


        }
    }


}
