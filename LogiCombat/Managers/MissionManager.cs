using System.Collections.Generic;

public class MissionManager
{
    private List<Mission> activeMissions = new List<Mission>();
    private List<Mission> completedMissions = new List<Mission>();
    private List<Mission> failedMissions = new List<Mission>();

    public void AddMission(Mission mission)
    {
        activeMissions.Add(mission);
    }

    public void CompleteMission(Mission mission)
    {
        if (activeMissions.Contains(mission))
        {
            activeMissions.Remove(mission);
            mission.CompleteMission();
            completedMissions.Add(mission);
        }
    }

    public void FailMission(Mission mission)
    {
        if (activeMissions.Contains(mission))
        {
            activeMissions.Remove(mission);
            mission.FailMission();
            failedMissions.Add(mission);
        }
    }

    public List<Mission> GetActiveMissions() => activeMissions;
    public List<Mission> GetCompletedMissions() => completedMissions;
    public List<Mission> GetFailedMissions() => failedMissions;
}
