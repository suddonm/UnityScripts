// Quest Giver NPC
using UnityEngine;

public class QuestGiver : MonoBehaviour
{
    public Mission AssignedMission;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GiveMission(other.gameObject);
        }
    }

    private void GiveMission(GameObject player)
    {
        var missionJournal = player.GetComponent<MissionJournal>();
        if (missionJournal != null && AssignedMission != null)
        {
            missionJournal.AddMission(AssignedMission);
            Debug.Log($"Mission Given: {AssignedMission.MissionName}");
        }
    }
}