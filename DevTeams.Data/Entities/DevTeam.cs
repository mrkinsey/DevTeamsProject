
public class DevTeam
{
    public DevTeam() { }

    public DevTeam(int teamId, string teamName, List<Developer> devTeamMembers)
    {
        TeamId = teamId;
        TeamName = teamName;
        DevTeamMembers = devTeamMembers;
    }

    public List<Developer> DevTeamMembers { get; set; } = new List<Developer>();
    public string TeamName { get; set; }
    public int TeamId { get; set; } = 1;
}
