using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeManager : MonoBehaviour
{
    // Spike Infomation
    [Header("Spike Prefab")]
    [SerializeField] private GameObject spikePrefab;
    [Space(10)]

    // Spike Behavour, 
    [Header("Spike Behavour")]
    [SerializeField] private float minTimeBetweenSpikes = 0.6f;
    [SerializeField] private float maxTimeBetweenSpikes = 1.0f;
    [SerializeField] private float probOfDoubleSpike = 0.1f;
    [SerializeField] private float probOfTripleSpike = 0.1f;
    [SerializeField] private float probOfQuadrupleSpike = 0.1f;
    [SerializeField] private int maxNumberOfSpikes = 10;
    [Space(10)]

    // Game Manager
    private GameManager gameManager;
    private DifficultyManager difficulty;
    private PickUpManager pickUpManager;
    private Tutorial tutorial;

    // Spike Management
    private float timeSinceLastSpike;
    private float timeTillNextSpike;
    List<GameObject> spikes = new List<GameObject>();
    
    public void SetGameManager(GameManager gameManager)
    {
        this.gameManager = gameManager;
        this.difficulty = gameManager.GetDifficultyManager();
        this.pickUpManager = gameManager.GetPickUpManager();
        this.tutorial = gameManager.GetTutorial();
    }

    public List<GameObject> Spikes()
    {
        return this.spikes;
    }

    private bool TooCloseToPickUp(Vector3 spikePosition)
    {
        float minDistanceFromSpike = this.difficulty.MinimumDistanceFromSpike();

        foreach (GameObject pickup in this.pickUpManager.PickUps())
        {
            if (Vector3.Distance(spikePosition, pickup.transform.position) < minDistanceFromSpike)
                return true;
        }

        return false;
    }

    private void CreateSpike(string spikeType, float yOffset, bool checkPickUpDistance = true)
    {
        Vector3 position;

        // Spike Position
        if (spikeType == "Left Spike")
            position = new Vector3(-3.5f, 15.0f + yOffset, 0.2f);
        else if (spikeType == "Right Spike")
            position = new Vector3(3.5f, 15.0f + yOffset, 0.2f);
        else
            return;

        // If the Spike is too close to a PickUp, abort this attempt
        if (checkPickUpDistance && TooCloseToPickUp(position))
            return;

        // Instantiate a Spike from the Prefab
        GameObject spike = Instantiate(spikePrefab);

        // Get the Spike Script from Spike Game Object
        Spike spikeScript = spike.GetComponent<Spike>();

        // Set the Spike's reference to the Game Manager
        spikeScript.SetGameManager(this.gameManager);
        
        // Initalise the Spike
        spikeScript.Initialise(spikeType, position);

        // Add the Spike to the spikes list
        this.spikes.Add(spike);
    }

    private void GenerateSpike()
    {
        string spikeType;

        // Left or Right Spike ?
        if (Random.Range(0.0f, 1.0f) < 0.5f)
            spikeType = "Left Spike";
        else
            spikeType = "Right Spike";

        // How Many Spikes ?
        int numberOfSpikes;
        int possibleNumOfSpikes = this.difficulty.PossibleNumberOfSpikes();

        if      ((possibleNumOfSpikes >= 4) && (Random.Range(0.0f, 1.0f) < this.probOfQuadrupleSpike)) numberOfSpikes = 4;
        else if ((possibleNumOfSpikes >= 3) && (Random.Range(0.0f, 1.0f) < this.probOfTripleSpike))    numberOfSpikes = 3;
        else if ((possibleNumOfSpikes >= 2) && (Random.Range(0.0f, 1.0f) < this.probOfDoubleSpike))    numberOfSpikes = 2;
        else                                                                                           numberOfSpikes = 1;

        // Create Spikes
        if (numberOfSpikes >= 1) CreateSpike(spikeType, 0.0f);
        if (numberOfSpikes >= 2) CreateSpike(spikeType, 1.4f); 
        if (numberOfSpikes >= 3) CreateSpike(spikeType, 2.8f); 
        if (numberOfSpikes >= 4) CreateSpike(spikeType, 4.2f);
        
        // Schedule the next Spike
        ScheduleNextSpike();
    }

    // Calculate when the next Spike will be created
    private void ScheduleNextSpike()
    {
        this.timeSinceLastSpike = 0.0f;
        this.timeTillNextSpike = Random.Range(this.minTimeBetweenSpikes, this.maxTimeBetweenSpikes);
    }

    // Destroy a specific Spike
    public void DestroySpike(GameObject spike)
    {
        // Tag spike as being Destroyed, ensures PlayerManager will not redetect a collision
        spike.tag = "Destroy Spike";
        this.spikes.Remove(spike);
        Destroy(spike);
    }

    // Destory all Spikes
    public void DestroyAllSpikes()
    {
        // Iterate backward though the list and Destroy all Spikes
        for (int i = this.spikes.Count - 1; i >= 0; i--)
        {
            GameObject spike = this.spikes[i];
            this.spikes.RemoveAt(i);
            spike.tag = "Destroy Spike";
            Destroy(spike);
        }
    }

    public void GameStart()
    {
        ScheduleNextSpike();
    }

    public void GameOver()
    {
        DestroyAllSpikes();
    }

    // Update is called once per frame
    void Update()
    {
        if (!this.gameManager.GameRunning())
            return;

        if (this.gameManager.TutorialRunning())
        {
            TESpike spikeEvent = this.tutorial.GetSpikeEvent();

            if (spikeEvent != null)
                CreateSpike(spikeEvent.spikeType, spikeEvent.yOffset, false);

            return;
        }

        // Increment the Time Since Last Spike
        this.timeSinceLastSpike += Time.deltaTime;

        // Is it time for another Spike ?
        if ((this.timeSinceLastSpike > this.timeTillNextSpike) && (this.spikes.Count < maxNumberOfSpikes))
        { 
            GenerateSpike();
        }
    }
}
