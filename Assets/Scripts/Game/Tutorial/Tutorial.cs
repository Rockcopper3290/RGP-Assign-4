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

        // How to create a Tutorial Sequence
        //     Events do not have to be added in strict time order
        //     Game Times have to be changed if the Game Speed is changed
        //
        // TEInput - Stop game until a specific user input
        //     Game Time   - When the Event Occurs
        //     Name        - Unit Input Manager name of the user Input
        //     Input Type  - "Mouse", "Key", "". Must be from Mouse, Must be from Keyboard, Either
        //     Message     - Message to display to player
        //
        // TESpike - Create a Spike
        //     Game Time   - When the Event Occurs   
        //     Spike Type  - "Right Spike" or "Left Spike"
        //     yOffset     - Optional, Vertical offset, useful to create Double, Triple or Quadruple Spikes 
        //
        // TEPickUp - Create a PickUp
        //     Game Time   - When the Event Occurs 
        //     PickUp Type - "Coin", "Sheild", "Invincible"
        //     yPosition   - Optional, Horizontal possition, -2.0f to 2.0f, Randomly chosen if ommited
        //
        // TEMessage - Display Message to Player
        //     Game Time   - When the Event Occurs 
        //     Message     - Message to display to player


        // In Tutorial, Player will always start on the right 
        time = 0.0f;
        this.events.Add(new TEMessage(time + 0.25f, "Avoid The Spikes"));
        this.events.Add(new TEMessage(time + 1.75f, ""));

        // Right Click To move to opposite wall (Player starts on right, ends on right)
        time = 1.0f;
        this.events.Add(new TESpike  (time + 0.00f, "Right Spike"));
        this.events.Add(new TEInput  (time + 1.25f, "Jump", "", "Swap Sides to avoid the Spike"));
        this.events.Add(new TESpike  (time + 0.80f, "Left Spike"));
        this.events.Add(new TEInput  (time + 2.05f, "Jump", "", "Swap Sides to avoid the Spike"));

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
        time = 7.00f;
        this.events.Add(new TEMessage(time + 0.00f, "Jump over a spike"));
        this.events.Add(new TEMessage(time + 1.50f, ""));
        this.events.Add(new TESpike  (time + 0.50f, "Right Spike"));
        this.events.Add(new TEInput  (time + 1.70f, "Jump", "", "Jump the Spike, start by swapping sides"));
        this.events.Add(new TEInput  (time + 1.85f, "Jump", "", "Press again to complete Jump"));
        this.events.Add(new TEMessage(time + 2.00f, "You must wait between Jumps"));
        this.events.Add(new TEMessage(time + 3.75f, ""));
        this.events.Add(new TESpike  (time + 3.75f, "Left Spike"));
        this.events.Add(new TEPickUp (time + 3.95f, "Coin", -1.00f));
        this.events.Add(new TESpike  (time + 4.00f, "Right Spike"));
        this.events.Add(new TESpike  (time + 4.20f, "Left Spike"));
        this.events.Add(new TESpike  (time + 4.20f, "Left Spike", 1.40f));
        this.events.Add(new TESpike  (time + 4.20f, "Left Spike", 2.80f));
        this.events.Add(new TEInput  (time + 5.20f, "Jump", "", "Jump the Spike and collect Coin"));
        this.events.Add(new TEInput  (time + 5.35f, "Jump", "", "Press again to complete Jump"));

        // Shield (Player starts on right, ends on left)
        time = 13.50f;
        this.events.Add(new TEMessage(time + 0.00f, "Shield protects you"));
        this.events.Add(new TEMessage(time + 1.50f, ""));
        this.events.Add(new TEPickUp (time + 0.50f, "Shield", 0.0f));
        this.events.Add(new TESpike  (time + 2.50f, "Left Spike"));
        this.events.Add(new TEInput  (time + 1.80f, "Jump", "", "Move and collect Shield"));
        this.events.Add(new TEMessage(time + 2.00f, "Shield protects you from one Spike"));
        this.events.Add(new TEMessage(time + 3.50f, ""));

        // Invincible (Player starts on left, ends on right)
        time = 18.50f;
        this.events.Add(new TEMessage(time + 0.00f, "Invincibility!"));
        this.events.Add(new TEMessage(time + 1.50f, ""));
        this.events.Add(new TEPickUp (time + 0.50f, "Invincible", 0.0f));
        this.events.Add(new TESpike  (time + 1.50f, "Right Spike"));
        this.events.Add(new TESpike  (time + 1.50f, "Right Spike", 1.40f));
        this.events.Add(new TESpike  (time + 1.50f, "Right Spike", 2.80f));
        this.events.Add(new TEInput  (time + 1.70f, "Jump", "", "Move and become Invincible"));
        this.events.Add(new TESpike  (time + 2.50f, "Left Spike"));
        this.events.Add(new TESpike  (time + 2.50f, "Left Spike", 1.40f));
        this.events.Add(new TEMessage(time + 2.00f, "1 green spike = 5 points"));
        this.events.Add(new TEMessage(time + 5.00f, "Beware, it wears off!"));
        this.events.Add(new TEMessage(time + 7.00f, ""));

        // End tutorial with a TEMessage sequence, not a TEInput, or you will get stuck
        time = 25.50f;
        this.events.Add(new TEMessage(time + 0.00f, "Good luck!"));
        this.events.Add(new TEMessage(time + 2.00f, ""));
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
