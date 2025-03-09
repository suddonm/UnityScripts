using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;

public class DialogueUIManager : MonoBehaviour
{
    public GameObject dialoguePanel;

    public TMP_Text npcDialogueText; // The dialogue text for the NPC
    public GameObject choicesContainer; // The parent object for the choice buttons
    public Button choiceButtonPrefab; // Prefab for individual choice buttons

    private DialogueNode currentNode;

    public void Start()
    {
        dialoguePanel.SetActive(false);
    }

    public void DisplayDialogue(DialogueGraph dialogueGraph)
    {
        dialoguePanel.SetActive(true);
        currentNode = dialogueGraph.startingNode;
        DisplayCurrentNode();
    }

    public void DisplayCurrentNode()
    {
        if (currentNode == null) return;

        // Set NPC dialogue text
        npcDialogueText.text = currentNode.dialogueText;

        // Clear existing choice buttons
        foreach (Transform child in choicesContainer.transform)
        {
            Destroy(child.gameObject);
        }

        // Create choice buttons
        foreach (DialogueChoice choice in currentNode.choices)
        {
            Button button = Instantiate(choiceButtonPrefab, choicesContainer.transform);
            button.GetComponentInChildren<Text>().text = choice.choiceText;

            // Add listener to button
            button.onClick.AddListener(() => OnChoiceSelected(choice));
        }
    }

    private void OnChoiceSelected(DialogueChoice choice)
    {
        // Move to the next node
        currentNode = choice.nextNode;

        if (currentNode != null)
        {
            DisplayCurrentNode();
        }
        else
        {
            EndDialogue();
        }
    }

    public void EndDialogue()
    {
        npcDialogueText.text = "";
        foreach (Transform child in choicesContainer.transform)
        {
            Destroy(child.gameObject);
        }
        Time.timeScale = 1f; // Resume the game if it was paused
    }

    public void HideDialogue()
    {
        dialoguePanel.SetActive(false);
    }
}
