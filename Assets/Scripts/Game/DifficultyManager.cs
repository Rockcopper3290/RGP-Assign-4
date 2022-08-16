using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//
// Manages Game Difficulty that alters as the game progresses
//

public class DifficultyManager : MonoBehaviour
{
    // Movement Speed
    [Header("Movement Speed")]
    [SerializeField] private float initialSpeed = 10.0f;
    [SerializeField] private float speedIncreasePerSec = 0.05f;
    [SerializeField] private float maxSpeed = 20.0f;
    [Space(10)]

    // PickUp Position Control
    [Header("Pickup Position Control")]
    [SerializeField] private float easiestMinDistFromSpike = 5.0f;
    [SerializeField] private float hardestMinDistFromSpike = 2.5f;
    [SerializeField] private int   scoreForHardestMinDistFromSpike = 600;
    [Space(10)]

    // Multiple Spikes From
    [Header("Multiple Spikes From")]
    [SerializeField] private int doubleSpikeFromScore = 150;
    [SerializeField] private int tripleSpikeFromScore = 300;
    [SerializeField] private int quadrupleSpikeFromScore = 450;
    [Space(10)]

    // Game Manager
    private GameManager gameManager;

    public void SetGameManager(GameManager gameManager)
    {
        this.gameManager = gameManager;
    }

    public float GameSpeed()
    {
        return (Mathf.Min(this.initialSpeed + (this.gameManager.GameTime() * this.speedIncreasePerSec), this.maxSpeed));
    }

    public float MinimumDistanceFromSpike()
    {
        float minDistanceFromSpike = Mathf.Lerp(
            this.easiestMinDistFromSpike, this.hardestMinDistFromSpike,
            ((float)this.gameManager.GameScore() / (float)this.scoreForHardestMinDistFromSpike));

        return minDistanceFromSpike;
    }

    public int PossibleNumberOfSpikes()
    {
        int gameScore = this.gameManager.GameScore();

        if (gameScore >= this.quadrupleSpikeFromScore) return 4;
        if (gameScore >= this.tripleSpikeFromScore)    return 3;
        if (gameScore >= this.doubleSpikeFromScore)    return 2;

        return 1;
    }

    public void GameStart()
    {

    }

    public void GameOver()
    {

    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
