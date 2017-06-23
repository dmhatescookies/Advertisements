using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.IO;
using System.Linq;
using Newtonsoft.Json;

namespace Ad
{
    [Serializable]
    public class Advertisements
    {

        public List<Ad> Array = new List<Ad>();
        public string[] Types = new string[] { "Education", "Repair" };

        public int Count
        {
            get
            {
                return Array.Count;
            }
        }

        public void Add(Ad item)
        {
            Array.Add(item);
            Serialize();
        }

        public void Clear()
        {
            Array = null;
            Array = new List<Ad>();
            Serialize();
        }

        public bool Remove(Ad item)
        {
            Array.Remove(item);
            Serialize();
            return true;
        }

        public List<Ad> Sort(List<Ad> list, int byWhat)                                       
        {
            List<Ad> sortedList = null;
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
                    sortedList = list.OrderBy(a => a.PhoneNumber).ToList();
                    break;

                case 5: //PersonName
                    sortedList = list.OrderBy(a => a.MainPerson).ToList();
                    break;

                default:
                    break;

            }
            return sortedList;
        }

        public Ad Search(string keyword)
        {
            foreach (Advertisements.Ad ser in Array)
            {
                if (ser.Name == keyword || ser.Description == keyword || ser.Price == Convert.ToInt16(keyword) || ser.PhoneNumber == keyword || ser.MainPerson == keyword)
                    return ser;
            }
            return null;
        }


        public void PrintAll(List<Ad> list)
        {
            int index = 0;
            foreach (Advertisements.Ad ser in list)
            {
                Console.WriteLine("ID:{0}, Type: {1},  Name: {2}, Description: {3}, Price: {4}, Phone: {5}, Person: {6}", index++, ser.Type, ser.Name, ser.Description, ser.Price, ser.PhoneNumber, ser.MainPerson);
            }
        }

        public void Serialize()
        {

            XmlSerializer formatter = new XmlSerializer(typeof(List<Ad>));
            using (FileStream fs = new FileStream("Services.xml", FileMode.Create))
            {
                formatter.Serialize(fs, Array);
            }

        }

        public void Deserialize()
        {
            XmlSerializer formatter = new XmlSerializer(typeof(List<Ad>));
            using (FileStream fs = new FileStream("Services.xml", FileMode.Open))
            {
                Array = (List<Ad>)formatter.Deserialize(fs);
            }
        }

        public void Serialize(string jSonMode)
        {
            using (StreamWriter file = File.CreateText("Services.txt"))
            {
                string json = JsonConvert.SerializeObject(Array);
                file.WriteLine(json);
                file.Close();
            }
        }

        public void Deserialize(string jSonMode)
        {
            JsonSerializer se = new JsonSerializer();
            StreamReader re = new StreamReader("Services.txt");
            JsonTextReader reader = new JsonTextReader(re);
            Array = se.Deserialize<List<Ad>>(reader);
        }


        public class Ad
        {
            public string Type { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }
            public double Price { get; set; }
            public string PhoneNumber { get; set; }
            public string MainPerson { get; set; }

            public Ad() { }

            public Ad(string type, string name, string description, double price, string phoneNumber, string mainPerson)
            {
                this.Type = type;
                this.Name = name;
                this.Description = description;
                this.Price = price;
                this.PhoneNumber = phoneNumber;
                this.MainPerson = mainPerson;
            }

            public string Info()
            {
                return String.Format("ID:{0}, Type: {1},  Name: {2}, Description: {3}, Price: {4}, Phone: {5}, Person: {6}", 0, Type, Name, Description, Price, PhoneNumber, MainPerson);
            }

        }

        
    }
}