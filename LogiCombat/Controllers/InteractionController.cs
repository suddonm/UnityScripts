using UnityEngine;

public class InteractionController : MonoBehaviour
{
    private Camera mainCamera;
    private InteractionMenuManager menuManager;

    private void Start()
    {
        mainCamera = Camera.main;
        menuManager = FindObjectOfType<InteractionMenuManager>();
    }

    private void Update()
    {
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
                else
                {
                    // Hide menu if clicked on a non-interactable object
                    menuManager.HideMenu();
                }
            }
        }
    }
}
