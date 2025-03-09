using System;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance;

    private bool isDialogueActive = false;

    private void Awake()
    {
        // Singleton setup
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void StartDialogue()
    {
        if (isDialogueActive) return;

        isDialogueActive = true;        
        GameManager.Instance.PauseGame();
    }

    public void EndDialogue()
    {
        if (!isDialogueActive) return;

        isDialogueActive = false;
        GameManager.Instance.ResumeGame();
    }

    public bool IsDialogueActive()
    {
        return isDialogueActive;
    }
}
