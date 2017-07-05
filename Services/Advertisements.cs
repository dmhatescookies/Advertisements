using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using AdvertisementsServer;

namespace Ad
{
    [Serializable]
    public class Advertisements
    {

        public List<Advertisement> AdvList;
        public string[] Types = new string[] { "Education", "Repair" };
        private DbDatabase db;

        public Advertisements()
        {
            db = new DbDatabase();
            AdvList = db.GetAllAdvertisements();
        }

        public int Count
        {
            get
            {
                return AdvList.Count;
            }
        }

        public void Add(Advertisement item)
        {
            AdvList.Add(item);
            db.AddAdvert(item);
        }

        public void Clear()
        {
            AdvList = db.GetAllAdvertisements();
            foreach (Advertisement item in AdvList)
                db.RemoveAdvertisement(item.AdvertId);
            AdvList = null;
            AdvList = new List<Advertisement>();
        }

        public bool Remove(Advertisement item)
        {
            try
            {
                AdvList.Remove(item);
                db.RemoveAdvertisement(item.AdvertId);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public List<Advertisement> Sort(List<Advertisement> list, int byWhat)
        {
            List<Advertisement> sortedList = null;
            switch (byWhat)
            {
                case 1: //Name
                    sortedList = list.OrderBy(a => a.Name).ToList();
                    break;

                case 2: //Description
                    sortedList = list.OrderBy(a => a.Description).ToList();
                    break;

                case 3: //Price
                    sortedList = list.OrderBy(a => a.Price).ToList();
                    break;

                case 4: //phone
                    sortedList = list.OrderBy(a => a.Person.PhoneNumber).ToList();
                    break;

                case 5: //PersonName
                    sortedList = list.OrderBy(a => a.Person.PersonLastname).ToList();
                    break;

                default:
                    break;

            }
            return sortedList;
        }

        public List<Advertisement> Search(List<Advertisement> list, string keyword)
        {
            List<Advertisement> resultList = new List<Advertisement>();
            double price;
            foreach (Advertisement ser in list)
            {
                try
                {
                    price = double.Parse(keyword);
                }
                catch (Exception e)
                {
                    price = -99;
                }
                if (ser.Type == keyword || ser.Name == keyword || ser.Description == keyword ||
                    ser.Price == price || ser.Person.PhoneNumber == keyword || ser.Person.PersonFirstname == keyword ||
                    ser.Person.PersonLastname == keyword || ser.Person.Email == keyword)
                    resultList.Add(ser);
            }
            return resultList;
        }

        public override string ToString()
        {
            string result = "";
            foreach (Advertisement ser in AdvList)
            {
                result += ser.ToString();
            }
            return result;
        }

        public string ToString(List<Advertisement> list)
        {
            string result = "";
            foreach (Advertisement ser in list)
            {
                result += ser.ToString();
            }
            return result;
        }

        public void Serialize()
        {

            XmlSerializer formatter = new XmlSerializer(typeof(List<Advertisement>));
            using (FileStream fs = new FileStream("Services.xml", FileMode.Create))
            {
                formatter.Serialize(fs, AdvList);
                fs.Close();
            }

        }

        public void Deserialize()
        {
            XmlSerializer formatter = new XmlSerializer(typeof(List<Advertisement>));
            using (FileStream fs = new FileStream("Services.xml", FileMode.Open))
            {
                AdvList = (List<Advertisement>)formatter.Deserialize(fs);
                fs.Close();
            }
        }

        public void Serialize(string jSonMode)
        {
            using (StreamWriter file = File.CreateText("Services.txt"))
            {
                string json = JsonConvert.SerializeObject(AdvList);
                file.WriteLine(json);
                file.Close();
            }
        }

        public void Deserialize(string jSonMode)
        {
            JsonSerializer se = new JsonSerializer();
            StreamReader re = new StreamReader("Services.txt");
            JsonTextReader reader = new JsonTextReader(re);
            AdvList = se.Deserialize<List<Advertisement>>(reader);
            re.Close();
        }        
    }
}