using System;
using AdvertisementsServer;

namespace Ad
{
    class Program  
    {
        public static void Main(string[] args)
        {
            Advertisements Advertise = new Advertisements();
            People People = new People();
            Person NewUser = new Person();
            Person AutorizedUser = new Person();
            Validation Valid = new Validation();
            DbDatabase db = new DbDatabase();
            
            bool TimeToExit = false;
            bool NotValid = true;
            int choice = -1;

            while (!TimeToExit)
            {
                Console.WriteLine("///////////////////M E N U////////////////////////\n" +
                    "Press number of action, that you want to do:\n" +
                    "1: Show all services\n" +
                    "2: Add new service\n" +
                    "3: Delete certain service\n" +
                    "4: Delete all services\n" +
                    "5: Sort by...\n" +
                    "6: Search by...\n" +
                    "7: Save to...\n" +
                    "8: Load from file\n" +
                    "9: Show certain type\n" +
                    "0: Exit");
                try
                {
                    choice = Convert.ToInt32(Console.ReadLine());
                }
                catch
                {
                    Console.WriteLine("Invalid symbol");
                }
                switch (choice)
                {

                    case 1:  //Show all ads
                            Console.WriteLine(Advertise.ToString());
                        if (Advertise.Count == 0)
                            Console.WriteLine("The List is empty");
                        break;

                    case 2:  //Add new ad
                        Console.WriteLine("Are you registered? Type Y for  YES, N for NO");
                        string YesNo = Console.ReadLine();
                        if (YesNo == "Y" || YesNo == "y")
                        {
                            Console.WriteLine("Authentification:");
                            Console.WriteLine("Enter your email");
                            string email = Console.ReadLine();
                            Console.WriteLine("Enter your password");
                            string pass = Console.ReadLine();
                            Person person = People.Search(email)[0];
                            if (person.Password != pass)
                                Console.WriteLine("Invalid login or password");
                            else
                                AutorizedUser = person;
                        }
                        if (YesNo == "N" || YesNo == "n")
                        {
                            Console.WriteLine("Authorization:");
                            Console.WriteLine("Enter your firstname");
                            string fname = "";
                            NotValid = true;
                            while (NotValid)
                            {
                                fname = Console.ReadLine();
                                if (Valid.IsValidFirstname(fname))
                                    NotValid = false;
                                else
                                    Console.WriteLine("Invalid Name. Try again");
                            }

                            Console.WriteLine("Enter your lastname");
                            string lname = "";
                            NotValid = true;
                            while (NotValid)
                            {
                                lname = Console.ReadLine();
                                if (Valid.IsValidFirstname(lname))
                                    NotValid = false;
                                else
                                    Console.WriteLine("Invalid lastname. Try again");
                            }

                            Console.WriteLine("Enter your phone number");
                            string phoneNumber = "";
                            NotValid = true;
                            while (NotValid)
                            {
                                phoneNumber = Console.ReadLine();
                                if (Valid.IsValidPhone(phoneNumber))
                                    NotValid = false;
                                else
                                    Console.WriteLine("Invalid Phone. Try again");
                            }
                            Console.WriteLine("Enter your email");
                            string email = "";
                            NotValid = true;
                            while (NotValid)
                            {
                                email = Console.ReadLine();
                                if (Valid.IsValidEmail(email))
                                    NotValid = false;
                                else
                                    Console.WriteLine("Invalid email. Try again");
                            }
                            Console.WriteLine("Enter your password");
                            string pass = "";
                            NotValid = true;
                            while (NotValid)
                            {
                                pass = Console.ReadLine();
                                if (Valid.IsValidPassword(pass))
                                    NotValid = false;
                                else
                                    Console.WriteLine("Invalid password. Try again");
                            }
                            People.Add(new Person(fname, lname, phoneNumber, email, pass));
                            AutorizedUser = People.Search(email)[0];

                        }
                        Console.WriteLine("Add new service:");
                        Console.WriteLine("Select type of service: press 1 for EDUCATION or 2 for REPAIR");
                        NotValid = true;
                        string indexOfType = "";
                        while (NotValid)
                        {
                            indexOfType = Console.ReadLine();
                            if (Valid.IsValidType(indexOfType))
                                NotValid = false;
                            else
                                Console.WriteLine("Invalid Type. Try again");
                        }
                        Console.WriteLine("Enter name of service:");
                        string name = "";
                        NotValid = true;
                        while (NotValid)
                        {
                            name = Console.ReadLine();
                            if (Valid.IsValidName(name))
                                NotValid = false;
                            else
                                Console.WriteLine("Invalid Name. Try again");
                        }

                        Console.WriteLine("Enter description of service:");
                        string description = Console.ReadLine();
                        Console.WriteLine("Enter price:");
                        string price = "";
                        NotValid = true;
                        while (NotValid)
                        {
                            price = Console.ReadLine();
                            if (Valid.IsValidPrice(price))
                                NotValid = false;
                            else
                                Console.WriteLine("Invalid Price. Try again");
                        }
                        try
                        {
                            Advertisement adv = new Advertisement(Advertise.Types[int.Parse(indexOfType) - 1], name, description, int.Parse(price), AutorizedUser);
                            Advertise.Add(adv);
                            Console.WriteLine("Added");
                        }
                        catch
                        {
                            Console.WriteLine("ERROR");
                        }
                        break;

                    case 3:  //Delete certain ad                                          
                        Console.WriteLine("Remove: \n Enter ID of service:");
                        if (Advertise.Remove(Advertise.AdvList[Convert.ToInt32(Console.ReadLine()) - 1]))
                            Console.WriteLine("Removed");
                        else
                            Console.WriteLine("Error");
                        break;

                    case 4:  //Delete all ads
                        Console.WriteLine("Are you sure? Press Y for YES / N for NO");
                        if (Console.ReadLine() == "Y")
                        {
                            Advertise.Clear();
                            Console.WriteLine("Removed");
                        }
                        else
                            Console.WriteLine("Canceled");
                        break;

                    case 5:  //Sort by...
                        Console.WriteLine("Sort: \nChoose an option: press 1 for sorting by NAME / 2 for DESCRIPTION / 3 for PRICE / 4 for PHONE / 5 for PERSON:");
                        Console.WriteLine(Advertise.ToString(Advertise.Sort(Advertise.AdvList, Convert.ToInt32(Console.ReadLine()))));
                        break;

                    case 6:  //Search by...
                        Console.WriteLine("Search: \nEnter keyword:");
                        Console.WriteLine(Advertise.ToString(Advertise.Search(Advertise.AdvList, Console.ReadLine())));
                        break;

                    case 7:  //Save to...
                        Console.WriteLine("Saving: \nChoose file format: press 1 for XML / 2 for JSON / 3 for database");
                        int k = int.Parse(Console.ReadLine());
                        if (k == 1)  //XML
                            Advertise.Serialize();
                        if (k == 2)  //JSON
                            Advertise.Serialize("jSon");
                        if (k == 3)  //DB
                        {
                            db.Create(Advertise.AdvList);
                        }
                        break;

                    case 8:   //Load from file
                        Console.WriteLine("Loading: \nChoose file format: press 1 for XML / 2 for JSON");
                        int l = int.Parse(Console.ReadLine());
                        if (l == 1)  //XML
                        {
                            Advertise.Deserialize();
                            Console.WriteLine(Advertise.ToString());
                            if (Advertise.AdvList.Count == 0)
                                Console.WriteLine("File is empty");
                        }
                        if (l == 2)  //JSON
                        {
                            Advertise.Deserialize("jSon");
                            Console.WriteLine(Advertise.ToString());
                            if (Advertise.AdvList.Count == 0)
                                Console.WriteLine("File is empty");
                        }
                        break;

                    case 9:  //Show certain type
                        Console.WriteLine("Choose type: press 1 for EDUCATION / 2 for REPAIR");
                        int type = int.Parse(Console.ReadLine());
                        foreach (Advertisement ser in Advertise.AdvList)
                            if (ser.Type == Advertise.Types[type - 1])
                            {
                                Console.WriteLine(ser.ToString());
                            }
                        break;

                    case 0:  //Exit
                        TimeToExit = true;
                        break;

                    default:
                        Console.WriteLine("Invalid input");
                        break;

                }

            }

        }

    }
}