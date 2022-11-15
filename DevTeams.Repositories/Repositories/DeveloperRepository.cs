// C.R.U.D. - > Create, Read, Update, Delete
public class DeveloperRepository
{
    // Fake database
    private readonly List<Developer> _devsDb = new List<Developer>();
    private int _count;

    //todo: Create: a new developer
    public bool AddDeveloperToDb(Developer developer)
    {
        int initialDevCount = _devsDb.Count;
        _devsDb.Add(developer);

        if (_devsDb.Count > initialDevCount)
        {
            AssignId(developer);
            return true;
        }
        else
        {
            return false;
        }
    }

    //todo: Create: Assign ID to developer (helper method)
    private void AssignId(Developer developer)
    {
        _count++;
        developer.Id = _count;
    }

    //todo: Read: Get all developers
    public List<Developer> GetAllDevelopers()
    {
        return _devsDb;
    }

    //todo Read: Get single developer by ID (helper method)
    public Developer GetDeveloperById(int id)
    {
        foreach (var developer in _devsDb)
        {
            if (developer.Id == id)
            {
                return developer;
            }
        }
        return null;
    }

    //todo Update: update developer information
    public bool UpdateDeveloperInfo(int originalId, Developer updatedDev)
    {
        Developer oldDev = GetDeveloperById(originalId);

        if (oldDev != null)
        {
            oldDev.FirstName = updatedDev.FirstName;
            oldDev.LastName = updatedDev.LastName;
            oldDev.HasPluralsight = updatedDev.HasPluralsight;
            return true;
        }
        else
        {
            return false;
        }
    }

    //todo Delete: Delete a developer
    public bool DeleteDeveloper(Developer existingContent)
    {
        bool deleteResult = _devsDb.Remove(existingContent);
        return deleteResult;
    }

    //todo return a List<Developer> based on whether they have Pluralsight
    public List<Developer> GetDeveloperByPluralsight(Developer hasPluralsight)
    {
        List<Developer> pluralsight = new List<Developer>();

        foreach (Developer developer in _devsDb)
        {
            if (developer.HasPluralsight == false)
            {
                pluralsight.Add(developer);
            }
        }
        return pluralsight;
    }

    public List<Developer> GetDevelopersById(List<int> developerIds)
    {
        List<Developer> developers = new List<Developer>();
        foreach (var developerId in developerIds)
        {
            var developer = GetDeveloperById(developerId);
            developers.Add(developer);
        }
        return developers;
    }
}