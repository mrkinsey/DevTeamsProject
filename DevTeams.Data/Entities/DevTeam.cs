
public class DevTeam
{
    public DevTeam() { }

    public DevTeam(List<Developer> devTeamMembers, string teamName, int teamId)
    {
        DevTeamMembers = devTeamMembers;
        TeamName = teamName;
        TeamId = teamId;
    }

    public List<Developer> DevTeamMembers { get; set; } = new List<Developer>();
    public string TeamName { get; set; }
    public int TeamId { get; set; } = 1;
}
