using static System.Console;

public class DevTeamUI
{
    private readonly DevTeamRepository _devTeamRepo;
    private readonly DeveloperRepository _devRepo;

    private bool isRunningDevTeamUI;

    public DevTeamUI(DeveloperRepository devRepo)
    {
        _devRepo = devRepo;
        _devTeamRepo = new DevTeamRepository(_devRepo);
    }

    public void Run()
    {
        SeedDevTeam();
        RunApplication();
    }

    private void RunApplication()
    {
        isRunningDevTeamUI = true;
        while (isRunningDevTeamUI)
        {
            WriteLine("DeveloperTeams UI\n" +
                "Please Make a Selection:\n" +
                "1. Add A Developer Team\n" +
                "2. View All Developer Teams\n" +
                "3. View Developer Team By ID\n" +
                "4. Update Developer Team\n" +
                "5. Delete Developer Team\n" +
                "6. Add Multiple Developers To A Team.\n" +
                "7. Open Main Menu\n" +
                "8. Close Application\n");

            string userInputMenuSelection = ReadLine();

            switch (userInputMenuSelection)
            {
                case "1":
                    AddDevTeam();
                    break;
                case "2":
                    ViewAllDevTeams();
                    break;
                case "3":
                    ViewDevTeamByID();
                    break;
                case "4":
                    UpdateDevTeam();
                    break;
                case "5":
                    DeleteDevTeam();
                    break;
                case "6":
                    AddMultiDevsToTeam();
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

    private void AddDevTeam()
    {
        Clear();

        DevTeam devTeam = AddNewDevTeamInformation();
        if (_devTeamRepo.AddDevTeam(devTeam))
        {
            WriteLine($"{devTeam.TeamName} was successfully added to the Database!");
        }
        else
        {
            System.Console.WriteLine("Sorry, the developer team was not added to the database. Please try again.");
        }
    }

    private DevTeam AddNewDevTeamInformation()
    {
        DevTeam devTeam = new DevTeam();

        WriteLine("Please enter the Developer Team Name:");
        devTeam.TeamName = Console.ReadLine();

        return devTeam;
    }

    private void ViewAllDevTeams()
    {
        Console.Clear();
        List<DevTeam> listOfDevTeams = _devTeamRepo.GetAllDevTeams();

        if (listOfDevTeams.Count > 0)
        {
            foreach (var devTeam in listOfDevTeams)
            {
                ShowDevTeamInformation(devTeam);
            }
        }
        else
        {
            System.Console.WriteLine("Sorry there are no Developer Teams in the database.");
        }
        Console.ReadKey();
    }

    private void ShowDevTeamInformation(DevTeam devTeam)
    {
        string developers = "";
        foreach (var developer in devTeam.DevTeamMembers)
        {
            developers += developer.FullName + ", ";
        }

        Console.WriteLine($"Team ID: {devTeam.TeamId}\n" +
                        $"Team Name: {devTeam.TeamName}\n" +
                        $"Team Members: {developers}\n" +
                        "-----------------------------------\n");
    }

    private void ViewDevTeamByID()
    {
        Console.Clear();
        System.Console.WriteLine("Please enter a developer team ID to search");
        int userInput = int.Parse(ReadLine());

        DevTeam devTeam = _devTeamRepo.GetDevTeamById(userInput);

        if (devTeam != null)
        {
            ShowDevTeamInformation(devTeam);
        }
        else
        {
            System.Console.WriteLine("Sorry, invalid developer team ID. Press enter to return to Developer Team Menu.");
        }
        Console.ReadKey();
    }

    private void UpdateDevTeam()
    {
        Console.Clear();

        System.Console.WriteLine("Please enter a Developer ID to update their information.");

        try
        {
            int userInputUpdate = int.Parse(ReadLine());
            DevTeam devTeamToUpdate = _devTeamRepo.GetDevTeamById(userInputUpdate);
            Console.Clear();
            if (devTeamToUpdate != null)
            {
                DevTeam newTeam = new DevTeam();

                System.Console.WriteLine("Please enter the Developer Team Name: ");
                newTeam.TeamName = Console.ReadLine();

                bool updateResult = _devTeamRepo.UpdateDevTeamInfo(userInputUpdate, newTeam);

                if (updateResult)
                {
                    Console.Clear();
                    System.Console.WriteLine("The developer team was successfully updated.");
                }
                else
                {
                    Console.Clear();
                    System.Console.WriteLine("Sorry, there was an issue updating the developer team. Please try again.");
                }
            }
            else
            {
                System.Console.WriteLine("Sorry, there is no developer team with that ID.");
            }
        }
        catch (Exception e) { System.Console.WriteLine("Sorry, there is no developer team with that ID."); }
    }

    private void DeleteDevTeam()
    {
        Console.Clear();
        System.Console.WriteLine("Which Developer Team do you want to remove?");

        List<DevTeam> devTeamList = _devTeamRepo.GetAllDevTeams();

        if (devTeamList.Count() > 0)
        {
            int count = 0;

            foreach (DevTeam devTeam in devTeamList)
            {
                count++;
                System.Console.WriteLine($"{count}. {devTeam.TeamName}");
            }

            int targetDeveloperId = int.Parse(Console.ReadLine());
            int targetIndex = targetDeveloperId - 1;

            if (targetIndex >= 0 && targetIndex < devTeamList.Count)
            {
                DevTeam desiredDevTeamCount = devTeamList[targetIndex];

                if (_devTeamRepo.DeleteDevTeam(desiredDevTeamCount))
                {
                    System.Console.WriteLine($"{desiredDevTeamCount.TeamName} was successfully deleted.");
                }
                else
                {
                    System.Console.WriteLine($"{desiredDevTeamCount.TeamName} failed to be deleted.");
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

    private void AddMultiDevsToTeam()
    {
        Clear();
        List<DevTeam> devTeam = _devTeamRepo.GetAllDevTeams();

        if (devTeam.Count() > 0)
        {
            WriteLine("== Developer Team Listing ==");
            ViewAllDevTeams();
            WriteLine("Please Select a DevTeam by Id for Multi Dev Addition.");
            int userInputDevTeamId = Convert.ToInt32(ReadLine());
            DevTeam selectedDevTeam = _devTeamRepo.GetDevTeamById(userInputDevTeamId);

            List<Developer> developers = _devRepo.GetAllDevelopers();
            List<Developer> devsToAdd = new List<Developer>();

            if (selectedDevTeam != null)
            {
                ViewAllDevelopers();
                WriteLine("Please add Developers by ID. Seperate with a comma to add multiple");
                string userInputAddDeveloperIds = ReadLine();

                List<int> developerIdsToAdd = userInputAddDeveloperIds.Split(',').Select(int.Parse).ToList();

                foreach (int id in developerIdsToAdd)
                {
                    var dev = _devRepo.GetDeveloperById(id);
                    devsToAdd.Add(dev);
                }
                _devTeamRepo.AddMultiDevsToTeam(userInputDevTeamId, devsToAdd);
            }
        }
        else
        {
            WriteLine("That ID Doesn't match any current Dev Teams.");
        }
        ReadKey();
    }

    private void ReturnToMainMenu()
    {
        Console.Clear();
        isRunningDevTeamUI = false;
    }

    private void ExitApplication()
    {
        isRunningDevTeamUI = false;
        DTUtils.isRunning = false;
        DTUtils.PressAnyKey();
    }

    private void DisplayDevelopersInDB(List<Developer> auxDevelopers)
    {
        foreach (var dev in auxDevelopers)
        {
            System.Console.WriteLine(dev);
        }
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
        { System.Console.WriteLine("Sorry there is no developers in the database."); }
        Console.ReadKey();
    }

    private void ShowDeveloperInformation(Developer developer)
    {
        Console.WriteLine($"ID: {developer.Id}\n" +
        $"Name: {developer.FullName}\n" +
        $"Has Pluralsight license: {developer.HasPluralsight}\n" +
        "-----------------------------------\n");
    }

    private void SeedDeveloper()
    {
        Developer dev1 = new Developer(1, "Naruto", "Uzumaki", false);
        Developer dev2 = new Developer(2, "Sasuke", "Uchiha", true);
        Developer dev3 = new Developer(3, "Itachi", "Uchiha", true);
        Developer dev4 = new Developer(4, "Kakashi", "Hatake", true);
        Developer dev5 = new Developer(5, "Shikamaru", "Nara", false);
        Developer dev6 = new Developer(6, "Konohamaru", "Sarutobi", false);
        Developer dev7 = new Developer(7, "Minato", "Namikaze", false);
        Developer dev8 = new Developer(8, "Hinata", "Hyuga", true);
        Developer dev9 = new Developer(9, "Madara", "Uchiha", true);
        Developer dev10 = new Developer(10, "Hashirama", "Senju", false);

        _devRepo.AddDeveloperToDb(dev1);
        _devRepo.AddDeveloperToDb(dev2);
        _devRepo.AddDeveloperToDb(dev3);
        _devRepo.AddDeveloperToDb(dev4);
        _devRepo.AddDeveloperToDb(dev5);
        _devRepo.AddDeveloperToDb(dev6);
        _devRepo.AddDeveloperToDb(dev7);
        _devRepo.AddDeveloperToDb(dev8);
        _devRepo.AddDeveloperToDb(dev9);
        _devRepo.AddDeveloperToDb(dev10);
    }

    private void SeedDevTeam()
    {
        SeedDeveloper();

        var teamOneMembers = _devRepo.GetDevelopersById(new List<int>() { 10, 3, 6, 7 });
        var teamTwoMembers = _devRepo.GetDevelopersById(new List<int> { 9, 4, 5, 8 });
        var teamThreeMembers = _devRepo.GetDevelopersById(new List<int> { 1, 2 });

        DevTeam teamOne = new DevTeam(1, "Front-Line", teamOneMembers);
        DevTeam teamTwo = new DevTeam(2, "Back-Line", teamTwoMembers);
        DevTeam teamThree = new DevTeam(3, "Generals", teamThreeMembers);

        _devTeamRepo.AddDevTeam(teamOne);
        _devTeamRepo.AddDevTeam(teamTwo);
        _devTeamRepo.AddDevTeam(teamThree);
    }
}
