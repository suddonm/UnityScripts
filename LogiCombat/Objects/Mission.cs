using System;
using System.Collections.Generic;
using UnityEditor.Experimental.Rendering;
using UnityEngine;

[CreateAssetMenu(fileName = "NewMission", menuName = "Missions/Mission")]
public class Mission : ScriptableObject
{
    public string MissionName;
    public string Description;
    public bool IsStarted;
    public bool IsCompleted;
    public bool IsFailed;
    public List<string> Objectives;
    public string Location;
    public Action OnMissionCompleted;
    public Action OnMissionFailed;

    public void StartMission()
    {
        IsStarted = true;
        IsCompleted = false;
        IsFailed = false;
    }

    public void CompleteMission()
    {
        IsCompleted = true;
        OnMissionCompleted?.Invoke();
    }

    public void FailMission()
    {
        IsFailed = true;
        OnMissionFailed?.Invoke();
    }
}
