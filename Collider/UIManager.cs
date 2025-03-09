using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; } // Singleton instance

    public Button moveButton;
    public Button attackButton;
    public Button fortifyButton;
    public Button endTurnButton;

    public Button cameraButton;

    private Camera mainCamera;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject); // Destroy extra instances if one already exists
        }
    }

    void Start()
    {
        mainCamera = Camera.main;
        cameraButton.onClick.AddListener(OnCameraButtonClicked);
        endTurnButton.onClick.AddListener(OnEndTurnButtonClicked);

        moveButton.onClick.AddListener(OnMoveButtonClicked);
        attackButton.onClick.AddListener(OnAttackButtonClicked);
        fortifyButton.onClick.AddListener(OnFortifyButtonClicked);
    }

    public void DisplayUnitActions(UnitController unit)
    {
        // Enable buttons based on the selected unit's abilities
        moveButton.interactable = true; // Can always move
        attackButton.interactable = true; // Check if the unit can attack
        fortifyButton.interactable = true; // Can always fortify

        // Add listeners for the buttons
        moveButton.onClick.AddListener(OnMoveButtonClicked);
        attackButton.onClick.AddListener(OnAttackButtonClicked);
        fortifyButton.onClick.AddListener(OnFortifyButtonClicked);       
    }

    public void HideUnitActions()
    {
        moveButton.onClick.RemoveAllListeners();
        attackButton.onClick.RemoveAllListeners();
        fortifyButton.onClick.RemoveAllListeners();        

        moveButton.interactable = false;
        attackButton.interactable = false;
        fortifyButton.interactable = false;
    }

    public void OnMoveButtonClicked()
    {
        GameManager.Instance.SetMoveMode();
    }

    public void OnAttackButtonClicked()
    {
        GameManager.Instance.SetAttackMode();
            
        //ActionComplete();
    }

    public void OnFortifyButtonClicked()
    {
        //selectedUnit.Fortify();
        //ActionComplete();
    }

    public void OnEndTurnButtonClicked()
    {
        GameManager.Instance.EndTurn();
        //HideUnitActions();
    }

    public void OnCameraButtonClicked()
    {
        mainCamera.GetComponent<RTSCameraController>().SwitchCameraMode();
    }

    private void ActionComplete()
    {
        //HideUnitActions();
        GameManager.Instance.OnUnitActionComplete();
    }
}