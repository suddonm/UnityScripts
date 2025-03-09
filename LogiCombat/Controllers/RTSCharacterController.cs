using UnityEngine;
using UnityEngine.AI;

public class RTSCharacterController : MonoBehaviour
{
    private NavMeshAgent agent; // Reference to the NavMeshAgent component
    private bool isSelected = false; // Whether this character is selected
    private Camera mainCamera;

    private Combat combat;

    private InteractionMenuManager menuManager;

    public bool IsHostile = false;

    private InventoryController inventory;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();        
        combat = GetComponent<Combat>();
        menuManager = FindObjectOfType<InteractionMenuManager>();
        inventory = GetComponent<InventoryController>();

        mainCamera = Camera.main;

        if (agent == null)
        {
            Debug.LogError("NavMeshAgent component is missing on " + gameObject.name);
        }
    }

    void Update()
    {
        // Check if this character is selected
        if (isSelected && Input.GetMouseButtonDown(1)) // Right-click to move
        {
            MoveToCursorPosition();
        }

        if (Input.GetMouseButtonDown(0)) // Left mouse button
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                IInteractable interactable = hit.collider.GetComponent<IInteractable>();
                if (interactable != null)
                {
                    menuManager.ShowMenu(interactable as IInteractable);
                }
            }
        }
    }

    public void SelectCharacter()
    {
        isSelected = true;
        Debug.Log(gameObject.name + " selected.");
    }

    public void DeselectCharacter()
    {
        isSelected = false;
        Debug.Log(gameObject.name + " deselected.");
    }

    private void MoveToCursorPosition()
    {
        // Raycast to determine where the player clicked
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            // Set the NavMeshAgent destination to the hit point
            agent.SetDestination(hit.point);
            Debug.Log(gameObject.name + " moving to " + hit.point);
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
