using System.Diagnostics;
using System.IO.Enumeration;
using static System.Console;
public class DeveloperUI
{
    private DeveloperRepository _devRepo;
    private bool isRunningDevUI;
    private DevTeamUI _dtUI;

    public DeveloperUI()
    {
        _devRepo = new DeveloperRepository();
    }

    public void Run()
    {
        SeedDeveloper();
        RunApplication();
    }

    private void RunApplication()
    {
        isRunningDevUI = true;
        while (isRunningDevUI)
        {
            Clear();
            WriteLine("Komodo Insurance Developer UI \n" +
                "Please Make a Selection:\n" +
                "1. Add A Developer\n" +
                "2. View All Developers\n" +
                "3. View Developer By ID\n" +
                "4. Update Existing Developer\n" +
                "5. Delete Existing Developer\n" +
                "6. View All Developers without a Pluralsight license\n" +
                "7. Return To Main Menu\n" +
                "8. Exit the Application\n");

            string userInputMenuSelection = ReadLine();
            switch (userInputMenuSelection)
            {
                case "1":
                    AddDeveloper();
                    break;
                case "2":
                    ViewAllDevelopers();
                    break;
                case "3":
                    ViewDeveloperByID();
                    break;
                case "4":
                    UpdateDeveloper();
                    break;
                case "5":
                    DeleteDeveloper();
                    break;
                case "6":
                    System.Console.WriteLine("Test");
                    ViewDevsWithOutPluralsight();
                    break;
                case "7":
                    ReturnToMainMenu();
                    break;
                case "8":
                    ExitApplication();
                    break;
                default:
                    WriteLine("Invalid Selection");
                    DTUtils.PressAnyKey();
                    break;
            }
        }
    }

    private void AddDeveloper()
    {
        Clear();

        Developer developer = AddNewDeveloperInformation();
        if (_devRepo.AddDeveloperToDb(developer))
        {
            WriteLine($"{developer.FullName} was successfully added to the Database!");
        }
        else
        {
            System.Console.WriteLine("Sorry, developer was not added to the database. Please try again.");
        }

    }

    private Developer AddNewDeveloperInformation()
    {
        Developer developer = new Developer();

        WriteLine("Please enter the new developers First Name:");
        developer.FirstName = Console.ReadLine();

        System.Console.WriteLine("Please enter the new developers Last Name:");
        developer.LastName = Console.ReadLine();

        System.Console.WriteLine("Please enter if the new developer has a Pluralsight license: \n" +
        "1. Yes - The new developer has a Pluralsight license.\n" +
        "2. No - The new developer does not have a Pluralsight license.");

        string pluralsightResponse = Console.ReadLine();
        switch (pluralsightResponse)
        {
            case "1":
                developer.HasPluralsight = true;
                break;
            case "2":
                developer.HasPluralsight = false;
                break;
            default:
                System.Console.WriteLine("Sorry, input was accepted. Please try again.");
                DTUtils.PressAnyKey();
                break;
        }
        return developer;
    }

    private void ViewAllDevelopers()
    {
        Console.Clear();
        List<Developer> listOfDevelopers = _devRepo.GetAllDevelopers();

        if (listOfDevelopers.Count > 0)
        {
            foreach (var developer in listOfDevelopers)
            {
                ShowDeveloperInformation(developer);
            }
        }
        else
        {
            System.Console.WriteLine("Sorry there is no developers in the database.");
        }
        Console.ReadKey();
    }

    private void ShowDeveloperInformation(Developer developer)
    {
        Console.WriteLine($"ID: {developer.Id}\n" +
                        $"Name: {developer.FullName}\n" +
                        $"Has Pluralsight license: {developer.HasPluralsight}\n" +
                        "-----------------------------------\n");
    }


    private void ViewDeveloperByID()
    {
        Console.Clear();
        System.Console.WriteLine("Please enter a developer ID to search");
        int userInput = int.Parse(ReadLine());

        Developer developer = _devRepo.GetDeveloperById(userInput);

        if (developer != null)
        {
            ShowDeveloperInformation(developer);
        }
        else
        {
            System.Console.WriteLine("Sorry, invalid developer ID.");
        }
        Console.ReadKey();
    }

    private void UpdateDeveloper()
    {
        Console.Clear();

        System.Console.WriteLine("Please enter a Developer ID to update their information.");

        int userInputUpdate = int.Parse(ReadLine());
        Developer developerToUpdate = _devRepo.GetDeveloperById(userInputUpdate);

        Console.Clear();
        if (developerToUpdate != null)
        {
            Developer newDeveloper = new Developer();

            System.Console.WriteLine("Please enter the developers First Name: ");
            newDeveloper.FirstName = Console.ReadLine();

            System.Console.WriteLine("Please enter the developers Last Name: ");
            newDeveloper.LastName = Console.ReadLine();

            System.Console.WriteLine("Please enter if the Developer has a Pluralsight license.\n" +
            "1. Yes" +
            "2. No");

            string pluralsightUpdateResponse = Console.ReadLine();
            switch (pluralsightUpdateResponse)
            {
                case "1":
                    newDeveloper.HasPluralsight = true;
                    break;
                case "2":
                    newDeveloper.HasPluralsight = false;
                    break;
                default:
                    System.Console.WriteLine("Sorry, input was accepted. Please try again.");
                    DTUtils.PressAnyKey();
                    break;
            }

            bool updateResult = _devRepo.UpdateDeveloperInfo(userInputUpdate, newDeveloper);

            if (updateResult)
            {
                Console.Clear();
                System.Console.WriteLine("The developer was successfully updated.");
            }
            else
            {
                Console.Clear();
                System.Console.WriteLine("Sorry, there was an issue updating the developer. Please try again.");
            }
        }
        else
        {
            System.Console.WriteLine("Sorry, there is no developer with that ID.");
        }
    }

    private void DeleteDeveloper()
    {
        Console.Clear();
        System.Console.WriteLine("Which developer do you want to remove?");

        List<Developer> developerList = _devRepo.GetAllDevelopers();

        if (developerList.Count() > 0)
        {
            int count = 0;

            foreach (Developer developer in developerList)
            {
                count++;
                System.Console.WriteLine($"{count}. {developer.FullName}");
            }

            int targetDeveloperId = int.Parse(Console.ReadLine());
            int targetIndex = targetDeveloperId - 1;

            if (targetIndex >= 0 && targetIndex < developerList.Count)
            {
                Developer desiredDeveloperCount = developerList[targetIndex];

                if (_devRepo.DeleteDeveloper(desiredDeveloperCount))
                {
                    System.Console.WriteLine($"{desiredDeveloperCount.FullName} was successfully deleted.");
                }
                else
                {
                    System.Console.WriteLine($"{desiredDeveloperCount.FullName} failed to be deleted.");
                }
            }
            else
            {
                System.Console.WriteLine("Sorry, invalid ID selection. Please try again.");
            }
        }
        else
        {
            System.Console.WriteLine("There is no available developers to delete.");
        }
        Console.ReadKey();
    }

    private void ViewDevsWithOutPluralsight()
    {
        System.Console.WriteLine("Test");
        List<Developer> devsWithOutPS = _devRepo.GetAllDevelopers().Where(x => x.HasPluralsight == true).ToList();

        if (devsWithOutPS.Count() > 0)
        {
            foreach (var developer in devsWithOutPS)
            {
                System.Console.WriteLine(developer);

                ShowDeveloperInformation(developer);
            }
        }
        else
        {
            WriteLine("There Are No Developers at this time with out a Pluralsight Membership.");
        }
    }

    private void ReturnToMainMenu()
    {
        Console.Clear();
        isRunningDevUI = false;
    }

    private void ExitApplication()
    {
        isRunningDevUI = false;
        DTUtils.isRunning = false;
        DTUtils.PressAnyKey();
    }

    private void SeedDeveloper()
    {
        Developer dev1 = new Developer(1, "Naruto", "Uzumaki", false);
        Developer dev2 = new Developer(2, "Sasuke", "Uchiha", true);
        Developer dev3 = new Developer(3, "Itachi", "Uchiha", true);
        Developer dev4 = new Developer(4, "Kakashi", "Hataki", false);
        Developer dev5 = new Developer(5, "Shikamara", "Nara", false);

        _devRepo.AddDeveloperToDb(dev1);
        _devRepo.AddDeveloperToDb(dev2);
        _devRepo.AddDeveloperToDb(dev3);
        _devRepo.AddDeveloperToDb(dev4);
        _devRepo.AddDeveloperToDb(dev5);
    }
}