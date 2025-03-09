using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InteractionMenuManager : MonoBehaviour
{
    public GameObject interactionMenu; // Reference to the interaction menu
    public Transform optionsContainer; // Container for interaction options
    public GameObject optionPrefab; // Prefab for a single option button

    private IInteractable currentInteractable;

    private void Start()
    {
        interactionMenu.SetActive(false);
    }

    public void ShowMenu(IInteractable interactable)
    {
        currentInteractable = interactable;

        // Move menu to the mouse position
        interactionMenu.SetActive(true);

        // Clear previous options
        foreach (Transform child in optionsContainer)
        {
            Destroy(child.gameObject);
        }

        // Populate with interaction options
        foreach (var option in interactable.GetInteractions())
        {
            GameObject optionObject = Instantiate(optionPrefab, optionsContainer);
            TMP_Text optionText = optionObject.GetComponentInChildren<TMP_Text>();
            optionText.text = option;

            Button optionButton = optionObject.GetComponent<Button>();
            optionButton.onClick.AddListener(() => HandleOptionSelected(option));
        }

        //Add the close option
        GameObject closeOptionObject = Instantiate(optionPrefab, optionsContainer);
        TMP_Text closeText = closeOptionObject.GetComponentInChildren<TMP_Text>();
        closeText.text = "Close";

        Button closeOptionButton = closeOptionObject.GetComponent<Button>();
        closeOptionButton.onClick.AddListener(() => HideMenu());
    }

    public void HideMenu()
    {
        interactionMenu.SetActive(false);
        currentInteractable = null;
    }

    private void HandleOptionSelected(string option)
    {
        // Perform the selected interaction
        currentInteractable.PerformInteraction(option);
        HideMenu();
    }
}
