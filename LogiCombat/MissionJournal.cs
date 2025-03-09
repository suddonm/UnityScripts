// Mission Journal to Track Player Missions
using System.Collections.Generic;
using UnityEngine;

public class MissionJournal : MonoBehaviour
{
    public List<Mission> ActiveMissions;
    public List<Mission> CompletedMissions;

    public void AddMission(Mission mission)
    {
        if (!ActiveMissions.Contains(mission))
        {
            ActiveMissions.Add(mission);
            Debug.Log($"Mission Added: {mission.MissionName}");
        }
    }

    public void CompleteMission(Mission mission)
    {
        if (ActiveMissions.Contains(mission))
        {
            mission.CompleteMission();
            ActiveMissions.Remove(mission);
            CompletedMissions.Add(mission);
        }
    }

    public void FailMission(Mission mission)
    {
        if (ActiveMissions.Contains(mission))
        {
            mission.FailMission();
            ActiveMissions.Remove(mission);
        }
    }
}