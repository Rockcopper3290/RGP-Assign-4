using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial
{
    // Game Manager
    GameManager gameManager;

    private List<TutorialEvent> events = new List<TutorialEvent>();

    private void InitaliseTutorialEvents()
    {
        float time;
        this.events.Clear();

        // Help for creating the Tutorial Sequence
        //     Events do not have to be added in strict time order
        //     PickUps and Spikes must be greater than minimum distance appart or they will not show
        //     Times will have to change if the Game Speed is changed
        
        // Click to start. In Tutorial, Player will always start on the right 
        time = 0.0f;
        this.events.Add(new TEInput  (time + 0.00f, "Jump", "", ""));
        this.events.Add(new TEMessage(time + 0.25f, "Avoid The Spikes"));
        this.events.Add(new TEMessage(time + 1.75f, ""));

        // Right Click To move to opposite wall (Player starts on right, ends on right)
        time = 1.0f;
        this.events.Add(new TESpike  (time + 0.00f, "Right Spike"));
        this.events.Add(new TEInput  (time + 1.25f, "Jump", "Mouse", "Left Click and move to opposite wall"));
        this.events.Add(new TESpike  (time + 0.80f, "Left Spike"));
        this.events.Add(new TEInput  (time + 2.05f, "Jump", "Key", "Press Space and move to opposite wall"));

        // Collecting Coins (Player starts on right, ends on right)
        time = 3.5f;
        this.events.Add(new TEMessage(time + 0.00f, "Collect Coins by hitting them"));
        this.events.Add(new TEMessage(time + 1.50f, ""));
        this.events.Add(new TEPickUp (time + 0.50f, "Coin", 0.0f));
        this.events.Add(new TESpike  (time + 0.80f, "Right Spike"));
        this.events.Add(new TEInput  (time + 1.80f, "Jump", "", "Move and collect Coin"));
        this.events.Add(new TEPickUp (time + 1.50f, "Coin", 0.0f));
        this.events.Add(new TESpike  (time + 1.80f, "Left Spike"));
        this.events.Add(new TEInput  (time + 2.80f, "Jump", "", "Move and collect Coin"));

        // Reversing Direction (Player starts on right, ends on right)
        time = 6.90f;
        this.events.Add(new TEMessage(time + 0.00f, "Jump over a spike"));
        this.events.Add(new TEMessage(time + 1.50f, ""));
        this.events.Add(new TESpike  (time + 0.50f, "Right Spike"));
        this.events.Add(new TEInput  (time + 1.70f, "Jump", "", "Move to the opposite wall"));
        this.events.Add(new TEInput  (time + 1.90f, "Jump", "Mouse", "Left Click to reverse"));;
        this.events.Add(new TESpike  (time + 1.50f, "Right Spike"));
        this.events.Add(new TEInput  (time + 2.70f, "Jump", "", "Move to the opposite wall"));
        this.events.Add(new TEInput  (time + 2.90f, "Jump", "Key", "Press Space to reverse"));

        // Shield (Player starts on right, ends on left)
        time = 10.10f;
        this.events.Add(new TEMessage(time + 0.00f, "Shield protects you"));
        this.events.Add(new TEMessage(time + 1.50f, ""));
        this.events.Add(new TEPickUp (time + 0.50f, "Shield", 0.0f));
        this.events.Add(new TESpike  (time + 1.25f, "Left Spike"));
        this.events.Add(new TEInput  (time + 1.80f, "Jump", "", "Move and collect Shield"));
        this.events.Add(new TEMessage(time + 2.00f, "Shield protects you from one Spike"));
        this.events.Add(new TEMessage(time + 3.50f, ""));

        // Invincible (Player starts on left, ends on right)
        time = 13.70f;
        this.events.Add(new TEMessage(time + 0.00f, "Invincibility !"));
        this.events.Add(new TEMessage(time + 1.50f, ""));
        this.events.Add(new TEPickUp (time + 0.50f, "Invincible", 0.0f));
        this.events.Add(new TESpike  (time + 1.50f, "Right Spike"));
        this.events.Add(new TESpike  (time + 1.75f, "Right Spike"));
        this.events.Add(new TESpike  (time + 2.00f, "Right Spike"));
        this.events.Add(new TEInput  (time + 1.80f, "Jump", "", "Move and become Invincible"));
        this.events.Add(new TESpike  (time + 2.50f, "Left Spike"));
        this.events.Add(new TESpike  (time + 3.00f, "Left Spike"));
        this.events.Add(new TEMessage(time + 2.00f, "Beware it wares off !"));
        this.events.Add(new TEMessage(time + 4.00f, ""));

        // End tutorial with a TEMessage sequence, not a TEInput, or you will get stuck
        time = 18.20f;
        this.events.Add(new TEMessage(time + 0.00f, "Game On !"));
        this.events.Add(new TEMessage(time + 1.00f, ""));
        
        // How to Create PickUps, xPosition can be -2.0f to 2.0f
        //this.events.Add(new TEPickUp(time + 3.0f, "Coin", 0.0f));
        //this.events.Add(new TEPickUp(time + 4.0f, "Shield",0.0f));
        //this.events.Add(new TEPickUp(time + 5.0f, "Invincible",0.0f));

        // How to Create Double, Tripe Spike etc
        // this.events.Add(new TESpike (time + 2.0f, "Left Spike"));
        // this.events.Add(new TESpike (time + 2.0f, "Left Spike", 2.0f));
        // this.events.Add(new TESpike (time + 2.0f, "Left Spike", 4.0f));
    }

    public void SetGameManager(GameManager gameManager)
    {
        this.gameManager = gameManager;
    }

    public void TutorialStart()
    {
        InitaliseTutorialEvents();
    }

    public void TutorialOver()
    {
        this.events.Clear();
    }
    
    public TESpike GetSpikeEvent()
    {
        // Is the Tutorial Over ?
        if (this.events.Count <= 0)
            this.gameManager.TutorialOver();

        float gameTime = this.gameManager.GameTime();

        for (int i = 0; i < this.events.Count; i++)
        {
            TutorialEvent tutorialEvent = this.events[i];

            if ((tutorialEvent.gameTime <= gameTime) && (tutorialEvent is TESpike))
            {
                this.events.RemoveAt(i);
                return (TESpike)tutorialEvent; 
            }
        }

        return null;
    }

    public TEPickUp GetPickUpEvent()
    {
        // Is the Tutorial Over ?
        if (this.events.Count <= 0)
            this.gameManager.TutorialOver();

        float gameTime = this.gameManager.GameTime();

        for (int i = 0; i < this.events.Count; i++)
        {
            TutorialEvent tutorialEvent = this.events[i];

            if ((tutorialEvent.gameTime <= gameTime) && (tutorialEvent is TEPickUp))
            {
                this.events.RemoveAt(i);
                return (TEPickUp)tutorialEvent; 
            }
        }

        return null;
    }

    public TEInput GetInputEvent()
    {
        // Is the Tutorial Over ?
        if (this.events.Count <= 0)
            this.gameManager.TutorialOver();

        float gameTime = this.gameManager.GameTime();

        for (int i = 0; i < this.events.Count; i++)
        {
            TutorialEvent tutorialEvent = this.events[i];

            if ((tutorialEvent.gameTime <= gameTime) && (tutorialEvent is TEInput))
            {
                this.events.RemoveAt(i);
                return (TEInput)tutorialEvent; 
            }
        }

        return null;
    }

    public TEMessage GetMessageEvent()
    {
        // Is the Tutorial Over ?
        if (this.events.Count <= 0)
            this.gameManager.TutorialOver();

        float gameTime = this.gameManager.GameTime();

        for (int i = 0; i < this.events.Count; i++)
        {
            TutorialEvent tutorialEvent = this.events[i];

            if ((tutorialEvent.gameTime <= gameTime) && (tutorialEvent is TEMessage))
            {
                this.events.RemoveAt(i);
                return (TEMessage)tutorialEvent; 
            }
        }

        return null;
    }
}
