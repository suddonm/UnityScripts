using UnityEngine;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour
{
    public Toggle edgePanningToggle;
    public GameObject mainMenu;

    private void Start()
    {
        // Initialize toggle state
        edgePanningToggle.isOn = GameManager.Instance.isEdgePanningEnabled;
    }

    public void ToggleEdgePanning(bool isEnabled)
    {
        GameManager.Instance.isEdgePanningEnabled = isEnabled;
        Debug.Log("Edge Panning toggled to " + (isEnabled ? "Enabled" : "Disabled"));
    }

    public void BackToMainMenu()
    {        
        mainMenu.SetActive(true);
        gameObject.SetActive(false);
    }
}
