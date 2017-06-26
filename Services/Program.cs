using System;

namespace Ad
{


    class Program
    {

        public static void Main(string[] args)
        {
            Advertisements Advertise = new Advertisements();
            Validation Valid = new Validation();
            bool TimeToExit = false;
            bool NotValid = true;
            int choice = -1;
            int index;
            while (!TimeToExit)
            {
                PrintMenuInfo();
                choice = Convert.ToInt32(Console.ReadLine());
                switch (choice)
                {

                    case 1:  //Show all ads
                        if (Advertise.Array != null)
                        {
                            Advertise.Deserialize();
                            Advertise.PrintAll(Advertise.Array);
                        }
                        else
                            Console.WriteLine("The List is empty");
                        break;

                    case 2:  //Add new ad
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
                        Console.WriteLine("Enter phone number:");
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
                        Console.WriteLine("Enter your name:");
                        string mainPerson = "";
                        NotValid = true;
                        while (NotValid)
                        {
                            mainPerson = Console.ReadLine();
                            if (Valid.IsValidMainPerson(mainPerson))
                                NotValid = false;
                            else
                                Console.WriteLine("Invalid Name. Try again");
                        }
                        try
                        {
                            Advertise.Deserialize();
                            Advertise.Add(new Advertisements.Ad(Advertise.Types[int.Parse(indexOfType) - 1], name, description, int.Parse(price), phoneNumber, mainPerson));
                            Console.WriteLine("Added");
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine("ERROR");
                        }
                        break;

                    case 3:  //Delete certain ad                                          
                        Console.WriteLine("Remove: \n Enter ID of service:");
                        if (Advertise.Remove(Advertise.Array[Convert.ToInt32(Console.ReadLine())]))
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
                        Advertise.PrintAll(Advertise.Sort(Advertise.Array, Convert.ToInt32(Console.ReadLine())));
                        break;

                    case 6:  //Search by...
                        Console.WriteLine("Search: \nEnter keyword:");
                        Console.WriteLine((Advertise.Search(Advertise.Array, Console.ReadLine())).Info());
                        break;

                    case 7:  //Save to...
                        Console.WriteLine("Saving: \nChoose file format: press 1 for XML / 2 for JSON");
                        int k = int.Parse(Console.ReadLine());
                        if (k == 1)  //XML
                            Advertise.Serialize();
                        if (k == 2)  //JSON
                            Advertise.Serialize("jSon");
                        break;

                    case 8:   //Load from file
                        Console.WriteLine("Loading: \nChoose file format: press 1 for XML / 2 for JSON");
                        int l = int.Parse(Console.ReadLine());
                        if (l == 1)  //XML
                        {
                            Advertise.Deserialize();
                            Advertise.PrintAll(Advertise.Array);
                        }
                        if (l == 2)  //JSON
                        {
                            Advertise.Deserialize("jSon");
                            Advertise.PrintAll(Advertise.Array);
                        }
                        break;

                    case 9:  //Show certain type
                        Console.WriteLine("Choose type: press 1 for EDUCATION / 2 for REPAIR");
                        int type = int.Parse(Console.ReadLine());
                        index = 0;
                        foreach (Advertisements.Ad ser in Advertise.Array)
                            if (ser.Type == Advertise.Types[type - 1])
                            {
                                Console.WriteLine("ID:{0}, Type: {1},  Name: {2}, Description: {3}, Price: {4}, Phone: {5}, Person: {6}", index++, ser.Type, ser.Name, ser.Description, ser.Price, ser.PhoneNumber, ser.MainPerson);
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

        public static void PrintMenuInfo()
        {
            Console.WriteLine("///////////////////M E N U///////////////////////////////////////////////////////////");
            Console.WriteLine("Press number of action, that you want to do:");
            Console.WriteLine("1: Show all services");
            Console.WriteLine("2: Add new service");
            Console.WriteLine("3: Delete certain service");
            Console.WriteLine("4: Delete all services");
            Console.WriteLine("5: Sort by...");
            Console.WriteLine("6: Search by...");
            Console.WriteLine("7: Save to...");
            Console.WriteLine("8: Load from file");
            Console.WriteLine("9: Show certain type");
            Console.WriteLine("0: Exit");
        }
    }
}