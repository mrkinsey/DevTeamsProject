// POCO -> Plain old c# object

public class Developer
{
    public Developer() { }

    public Developer(int id, string firstName, string lastName, bool hasPluralsight)
    {
        Id = id;
        FirstName = firstName;
        LastName = lastName;
        HasPluralsight = hasPluralsight;
    }

    public int Id { get; set; } = 1;
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string FullName
    {
        get
        {
            return $"{FirstName} {LastName}";
        }
    }
    public bool HasPluralsight { get; set; }
}