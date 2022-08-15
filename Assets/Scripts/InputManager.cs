using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InputManager : MonoBehaviour
{
    [Header("Game View Canvas")]
    [SerializeField] private GameObject gameViewCanvas;
    [SerializeField] private GameObject pauseButton;
    
    private GraphicRaycaster gameViewRaycaster;
    private EventSystem eventSystem;

    private PointerEventData clickData;
    private List<RaycastResult> clickResults;

    // Game Manager
    private GameManager gameManager;

    public void SetGameManager(GameManager gameManager)
    {
        this.gameManager = gameManager;
    }

    public void GameStart()
    {

    }

    public void GameOver()
    {

    }

    // Start is called before the first frame update
    private void Start()
    {
        this.gameViewRaycaster = this.gameViewCanvas.GetComponent<GraphicRaycaster>();
        this.eventSystem = EventSystem.current;

        this.clickData = new PointerEventData(this.eventSystem);
        this.clickResults = new List<RaycastResult>();
    }

    private bool WasMouseClick()
    {
        int[] mouseButtons = {0, 1, 2};

        foreach (int button in mouseButtons)
        {
            if (Input.GetMouseButtonDown(button))
                return true;
        }

        return false;
    }

    private bool PauseButtonWasMouseClicked()
    {
        if (!WasMouseClick())
            return false;

        this.clickData.position = Input.mousePosition;
        this.clickResults.Clear();

        this.gameViewRaycaster.Raycast(this.clickData, this.clickResults);

        foreach(RaycastResult result in this.clickResults)
        {
            if (result.gameObject.name == this.pauseButton.name)
                return true;
        }

        return false;
    }

    public bool GetButtonDown(string name)
    {
        bool result = Input.GetButtonDown(name);

        // Do not Jump if Pause Button Was Mouse Clicked
        if (result && (name == "Jump") && PauseButtonWasMouseClicked())
        {
            this.gameManager.GamePause();
            return false;
        }

        return result;
    }
}
