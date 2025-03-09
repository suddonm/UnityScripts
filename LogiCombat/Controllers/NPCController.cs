using BehaviorTree;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(UnityEngine.AI.NavMeshAgent))]
public class NPCController : MonoBehaviour, IInteractable
{
    public string npcName;
    public int trustLevel = 0;
    public int trustThreshold = 50;

    private bool isRecruited;

    private BehaviorTree.BehaviorTree NPCBehaviorTree;

    public DialogueNode initialDialogue;
    private DialogueUIManager dialogueUI;

    public LocationController home;
    public LocationController hangoutSpot;

    private LocationController currentLocation;

    // Reference to the time system
    private TimeManager timeManager;

    public DialogueGraph dialogueGraph;
    public Workplace Workplace;
    public Transform WorkplaceLocation;

    public Mission AssignedMission;
    private InventoryController inventory;

    public NavMeshAgent NavMeshAgent { get; private set; }

    public bool IsQuestGiver => AssignedMission != null;

    private void Start()
    {
        //Globals
        dialogueUI = FindObjectOfType<DialogueUIManager>();
        
        //Locals
        inventory = GetComponent<InventoryController>();
        NavMeshAgent = GetComponent<NavMeshAgent>();
        NPCBehaviorTree = ScriptableObject.CreateInstance<NPCTree>().Init(this);

        isRecruited = false;
    }

    private void Update()
    {
        NPCBehaviorTree.Update();

        /*
        UpdateLocation();
        MoveToCurrentLocation();
        */
    }

    public List<string> GetInteractions()
    {
        List<string> interactions = new List<string> { "Talk", "Mission" };

        return interactions;
    }

    public void PerformInteraction(string interaction)
    {
        switch (interaction)
        {
            case "Talk":
                StartDialogue();
                break;
            case "Mission":
                if (IsQuestGiver)
                {
                    AssignedMission.StartMission();
                }
                break;
            default:
                Debug.Log("Invalid interaction.");
                break;
        }
    }

    private void StartDialogue()
    {
        dialogueUI.DisplayDialogue(dialogueGraph);
        DialogueManager.Instance.StartDialogue();
    }

    public void EndInteraction()
    {
        DialogueManager.Instance.EndDialogue();
        dialogueUI.HideDialogue();
    }

    private void MoveToCurrentLocation()
    {
        if (currentLocation != null && currentLocation.transform.position != null)
        {
            // Move the NPC to their current location
            GetComponent<UnityEngine.AI.NavMeshAgent>().SetDestination(currentLocation.GetComponentInChildren<SpawnPoint>().transform.position);
            GetComponent<UnityEngine.AI.NavMeshAgent>().isStopped = false;
        }
        else
        {
            GetComponent<UnityEngine.AI.NavMeshAgent>().isStopped = true;
        }
    }

    public void PickUpItem(IItem item)
    {
        inventory.AddItem(item);
        Destroy(item);
        Debug.Log(gameObject.name + " picked up " + item.Name);
    }

    public void DropItem(IItem item)
    {
        inventory.RemoveItem(item);
        Instantiate(item, transform.position, Quaternion.identity);
        Debug.Log(gameObject.name + " dropped " + item.Name);
    }
}
