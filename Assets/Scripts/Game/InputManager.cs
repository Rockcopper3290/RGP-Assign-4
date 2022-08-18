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
    [Space(10)]

    [Header("Instruction View Canvas")]
    [SerializeField] private GameObject instructionViewCanvas;
    [SerializeField] private GameObject exitButton;
    
    private GraphicRaycaster gameViewRaycaster;
    private GraphicRaycaster instructionViewRaycaster;
    private EventSystem eventSystem;

    private PointerEventData clickData;
    private List<RaycastResult> clickResults;

    // Game Manager
    private GameManager gameManager;
    private GameScreen gameScreen;
    private Tutorial tutorial;

    // Used by Tutorial. Need to receive a specific input
    private TEInput requiredInput;

    public void SetGameManager(GameManager gameManager)
    {
        this.gameManager = gameManager;
        this.gameScreen = gameManager.GetGameScreen();
        this.tutorial = gameManager.GetTutorial();
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
        this.instructionViewRaycaster = this.instructionViewCanvas.GetComponent<GraphicRaycaster>();
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

    private bool GameObjectWasMouseClicked(GraphicRaycaster raycaster, GameObject gameObject)
    {
        if (!WasMouseClick())
            return false;

        this.clickData.position = Input.mousePosition;
        this.clickResults.Clear();

        raycaster.Raycast(this.clickData, this.clickResults);

        foreach(RaycastResult result in this.clickResults)
        {
            if (result.gameObject.name == gameObject.name)
                return true;
        }

        return false;
    }

    private bool GameViewPauseWasClicked()
    {
        return GameObjectWasMouseClicked(this.gameViewRaycaster, this.pauseButton);
    }

    private bool InstructionViewExitWasClicked()
    {
        return GameObjectWasMouseClicked(this.instructionViewRaycaster, this.exitButton);
    }

    private bool GetRequiredInput(string name)
    {
        if (!this.gameManager.GameRunning() || !this.gameManager.TutorialRunning())
            return Input.GetButtonDown(name);

        if (this.requiredInput != null)
        {
            if (this.requiredInput.name != name)
                return false;

            if (Input.GetButtonDown(name))
            {
                if (this.requiredInput.inputType == "Mouse" && (WasMouseClick() == false))
                    return false;

                if (this.requiredInput.inputType == "Key" && (WasMouseClick() == true))
                    return false;

                // Resume Game
                Time.timeScale = 1.0f;
                // AudioListener.pause = false;
                this.gameScreen.SetTutorialText("");

                this.requiredInput = null;
                return true;
            }
        }

        return false;
    }

    public bool GetButtonDown(string name)
    {
        bool result = GetRequiredInput(name);

        // Do not Jump if Pause Button Was Mouse Clicked
        if (result && (name == "Jump") && GameViewPauseWasClicked())
        {
            this.gameManager.GamePause();
            return false;
        }

        // Go to Splash Screen if Instruction View's Exit Button Was Mouse Clicked
        if (!this.gameManager.GameRunning() &&
            result && (name == "Jump") && InstructionViewExitWasClicked())
        {
            this.gameScreen.MainMenu();
            return false;
        }

        return result;
    }

    // Update is called once per frame
    void Update()
    {
        if (this.gameManager.TutorialRunning())
        {
            TEInput inputEvent = this.tutorial.GetInputEvent();

            if (inputEvent != null)
            {
                this.requiredInput = inputEvent;

                // Halt Game until required input
                Time.timeScale = 0.0f;
                // AudioListener.pause = true;
                this.gameScreen.SetTutorialText(this.requiredInput.message);
            }

            return;
        }
    }
}
