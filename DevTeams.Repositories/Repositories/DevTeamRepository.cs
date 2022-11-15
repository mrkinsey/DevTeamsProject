
public class DevTeamRepository
{
    private readonly List<DevTeam> _devTeamDb = new List<DevTeam>();
    private int _count;

    //todo Create: Create a developer team
    public bool AddDevTeam(DevTeam team)
    {
        int startingCount = _devTeamDb.Count;
        _devTeamDb.Add(team);

        if (_devTeamDb.Count > startingCount)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    //todo: Read: Get all developer teams
    public List<DevTeam> GetAllDevTeams()
    {
        return _devTeamDb;
    }

    //todo Read: Get single developer team by ID (helper method)
    public DevTeam GetDevTeamById(int id)
    {
        foreach (DevTeam devTeam in _devTeamDb)
        {
            if (devTeam.TeamId == id)
            {
                return devTeam;
            }
        }
        return null;
    }

    //todo Update: update dev team information
    public bool UpdateDevTeamInfo(int devTeamId, DevTeam updatedDevTeam)
    {
        DevTeam oldDevTeam = GetDevTeamById(devTeamId);

        if (oldDevTeam != null)
        {
            oldDevTeam.TeamName = updatedDevTeam.TeamName;
            oldDevTeam.DevTeamMembers = updatedDevTeam.DevTeamMembers;
            return true;
        }
        else
        {
            return false;
        }
    }

    //todo Delete: Delete a dev team
    public bool DeleteDevTeam(DevTeam existingTeam)
    {
        bool deleteResult = _devTeamDb.Remove(existingTeam);
        return deleteResult;
    }

    // Challenge
    // We need the teamID and the list<developer> to add
    // public bool AddMultiDevsToTeam(int teamID, List<Developer> devs)
    // {
    //     var teamInDB = GetDevTeamById(GetDevTeamById);

    //     if (teamInDB != null && devs != null)
    //     {
    //         teamInDB.DevTeamMembers.AddRange(devs);
    //         return true;
    //     }
    //     else
    //     {
    //         return false;
    //     }
    // }
}