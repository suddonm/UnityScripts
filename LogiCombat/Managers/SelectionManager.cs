using UnityEngine;

public class SelectionManager : MonoBehaviour
{
    private RTSCharacterController selectedCharacter;

    void Update()
    {
        // Left-click to select a character
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                // Check if the clicked object has a CharacterController
                RTSCharacterController character = hit.collider.GetComponent<RTSCharacterController>();
                if (character != null)
                {
                    // Deselect the currently selected character
                    if (selectedCharacter != null)
                    {
                        selectedCharacter.DeselectCharacter();
                    }

                    // Select the new character
                    selectedCharacter = character;
                    selectedCharacter.SelectCharacter();
                }
            }
        }
    }
}
