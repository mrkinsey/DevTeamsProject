using static System.Console;

public class DevTeamUI
{
    private readonly DevTeamRepository _devTeamRepo;
    private readonly DeveloperRepository _devRepo;

    private bool isRunningDevTeamUI;

    public DevTeamUI(DeveloperRepository devRepo)
    {
        _devRepo = devRepo;
        _devTeamRepo = new DevTeamRepository();
    }

    public void Run()
    {
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
        Console.WriteLine($"Team ID: {devTeam.TeamId}\n" +
                        $"Team Name: {devTeam.TeamName}\n" +
                        $"Team Members: {devTeam.DevTeamMembers}\n" +
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
        catch (Exception e) { System.Console.WriteLine("Sorry, there is no developer with that ID."); }
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
        WriteLine("== Developer Team Listing ==");
        ViewAllDevTeams();
        List<DevTeam> devTeam = _devTeamRepo.GetAllDevTeams();
        if (devTeam.Count() > 0)
        {
            WriteLine("Please Select a DevTeam by Id for Multi Dev Addition.");
            int userInputDevTeamId = Convert.ToInt32(ReadLine());
            DevTeam team = _devTeamRepo.GetDevTeamById(userInputDevTeamId);

            //UI update stuff....
            List<Developer> auxDevInDb = _devRepo.GetAllDevelopers();

            //what the user selects
            List<Developer> devsToAdd = new List<Developer>();

            if (team != null)
            {
                bool hasFilledPositions = false;
                while (!hasFilledPositions)
                {
                    if (auxDevInDb.Count() > 0)
                    {
                        DisplayDevelopersInDB(auxDevInDb);
                        WriteLine("Do you want to add a Developer y/n?");
                        var userInputAnyDevs = ReadLine();
                        if (userInputAnyDevs == "Y".ToLower())
                        {
                            WriteLine("Please Choose Dev by Id:");
                            int userInputDevId = int.Parse(ReadLine());
                            Developer dev = _devRepo.GetDeveloperById(userInputDevId);
                            if (dev != null)
                            {
                                devsToAdd.Add(dev);
                                auxDevInDb.Remove(dev);
                            }
                            else
                            {
                                WriteLine($"Sorry, the Dev with the Id: {userInputDevId} doesn't Exist.");
                                WriteLine("Press Any Key to continue.");
                                ReadKey();
                            }
                        }
                        else
                        {
                            hasFilledPositions = true;
                        }
                    }
                    else
                    {
                        WriteLine("There are no Developers in the Database.");
                        ReadKey();
                        break;
                    }
                }

                // if (_devTeamRepo.AddMultiDevsToTeam(team.TeamId, devsToAdd))
                // {
                //     WriteLine("Success");
                // }
                // else
                // {
                //     WriteLine("Failure");
                // }
            }


        }
        else
        {
            WriteLine("There aren't any available Developer Teams to Delete.");
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

    private void SeedDevTeam()
    {
    }
}
