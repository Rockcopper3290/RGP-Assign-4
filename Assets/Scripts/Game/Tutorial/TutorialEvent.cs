using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialEvent
{
    public float gameTime;

    public TutorialEvent(float gameTime)
    {
        this.gameTime = gameTime;
    }

    public override string ToString()
    {
        return base.ToString() + " : gameTime:" + gameTime;
    }
}

public class TESpike : TutorialEvent
{
    public string spikeType;
    public float yOffset;

    public TESpike(float gameTime, string spikeType) : base(gameTime)
    {
        this.spikeType = spikeType;
        this.yOffset = 0.0f;
    }

    public TESpike(float gameTime, string spikeType, float yOffset) : base(gameTime)
    {
        this.spikeType = spikeType;
        this.yOffset = yOffset;
    }

    public override string ToString()
    {
        return base.ToString() + " : " + spikeType + " yOffset: " + yOffset;
    }
}

public class TEPickUp : TutorialEvent
{
    public string pickUpType;
    public float xPosition;

    public TEPickUp(float gameTime, string pickUpType) : base(gameTime)
    {
        this.pickUpType = pickUpType;
        this.xPosition = Random.Range(-2.0f, 2.0f);
    }

    public TEPickUp(float gameTime, string pickUpType, float xPosition) : base(gameTime)
    {
        this.pickUpType = pickUpType;
        this.xPosition = xPosition;
    }

    public override string ToString()
    {
        return base.ToString() + ": " + pickUpType + " xPosition: " + xPosition;
    }
}

public class TEInput : TutorialEvent
{
    public string name;
    public string inputType;
    public string message;

    public TEInput(float gameTime, string name, string inputType, string message) : base(gameTime)
    {
        this.name = name;
        this.inputType = inputType;
        this.message = message;
    }

    public override string ToString()
    {
        return base.ToString() + ": " + name + " inputType: " + inputType + " message: " + message;
    }
}

public class TEMessage : TutorialEvent
{
    public string name;
    public string message;

    public TEMessage(float gameTime, string message) : base(gameTime)
    {
        this.message = message;
    }

    public override string ToString()
    {
        return base.ToString() + ": " + message;
    }
}
