using static System.Console;

public class ProgramUI
{
    private DeveloperUI _devUI;
    private DevTeamUI _devTeamUI;
    private DeveloperRepository _devRepo;
    public ProgramUI()
    {
        _devRepo = new DeveloperRepository();
        _devUI = new DeveloperUI();
        _devTeamUI = new DevTeamUI(_devRepo);
    }

    public void Run()
    {
        RunApplication();
    }

    private void RunApplication()
    {

        while (DTUtils.isRunning)
        {
            WriteLine("Welcome to Komodo Insurance Developer Team Management\n" +
                "Please Make a Selection:\n" +
                "1. Developer Menu\n" +
                "2. Developer Teams Menu\n" +
                "3. Exit the Application\n");

            string userInputMenuSelection = ReadLine();
            switch (userInputMenuSelection)
            {
                case "1":
                    DeveloperMenu();
                    break;
                case "2":
                    DevTeamsMenu();
                    break;
                case "3":
                    DTUtils.isRunning = ExitApp();
                    break;
                default:
                    WriteLine("Invalid Selection");
                    DTUtils.PressAnyKey();
                    break;
            }
        }
    }
    private void DeveloperMenu()
    {
        Clear();
        _devUI.Run();
    }

    private void DevTeamsMenu()
    {
        Clear();
        _devTeamUI.Run();
    }

    private bool ExitApp()
    {
        WriteLine("Thanks, for using Komodo Dev Teams.");
        DTUtils.PressAnyKey();
        return false;
    }

}