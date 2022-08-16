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
    [SerializeField] private float minTimeBetweenSpikes = 0.5f;
    [SerializeField] private float maxTimeBetweenSpikes = 1.0f;
    [SerializeField] private float probOfDoubleSpike = 0.1f;
    [SerializeField] private float probOfTripleSpike = 0.1f;
    [SerializeField] private float probOfQuadrupleSpike = 0.1f;
    [SerializeField] private int maxNumberOfSpikes = 10;
    [Space(10)]

    // Position Control
    [Header("Spike Position Control")]
    [SerializeField] private float minDistFromPickUp = 2.5f;

    // Game Manager
    private GameManager gameManager;
    private PickUpManager pickUpManager;

    // Spike Management
    private float timeSinceLastSpike;
    private float timeTillNextSpike;
    List<GameObject> spikes = new List<GameObject>();
    
    public void SetGameManager(GameManager gameManager)
    {
        this.gameManager = gameManager;
        this.pickUpManager = gameManager.GetPickUpManager();
    }

    public List<GameObject> Spikes()
    {
        return this.spikes;
    }

    private bool TooCloseToPickUp(Vector3 spikePosition)
    {
        foreach (GameObject pickup in this.pickUpManager.PickUps())
        {
            if (Vector3.Distance(spikePosition, pickup.transform.position) < this.minDistFromPickUp)
                return true;
        }

        return false;
    }

    private void CreateSpike(string spikeType, Vector3 position)
    {
        // If the Spike is too close to a PickUp, abort this attempt
        if (TooCloseToPickUp(position))
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
        Vector3 position;

        // Left or Right Spike ?
        if (Random.Range(0.0f, 1.0f) < 0.5f)
        {
            spikeType = "Left Spike";
            position = new Vector3(-3.5f, 15.0f, 0.2f);
        }
        else
        {
            spikeType = "Right Spike";
            position = new Vector3(3.5f, 15.0f, 0.2f);
        }

        // How Many Spikes ?
        int numberOfSpikes;
        int possibleNumOfSpikes = Mathf.Min(4, (this.gameManager.GameScore() / 100) + 1);

        if      ((possibleNumOfSpikes >= 4) && (Random.Range(0.0f, 1.0f) < this.probOfQuadrupleSpike)) numberOfSpikes = 4;
        else if ((possibleNumOfSpikes >= 3) && (Random.Range(0.0f, 1.0f) < this.probOfTripleSpike))    numberOfSpikes = 3;
        else if ((possibleNumOfSpikes >= 2) && (Random.Range(0.0f, 1.0f) < this.probOfDoubleSpike))    numberOfSpikes = 2;
        else                                                                                           numberOfSpikes = 1;

        // Create Spikes
        if (numberOfSpikes >= 1) CreateSpike(spikeType, position);
        if (numberOfSpikes >= 2) CreateSpike(spikeType, position + new Vector3(0.0f, 2.0f, 0.0f));
        if (numberOfSpikes >= 3) CreateSpike(spikeType, position + new Vector3(0.0f, 4.0f, 0.0f));
        if (numberOfSpikes >= 4) CreateSpike(spikeType, position + new Vector3(0.0f, 6.0f, 0.0f));
        
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

        // Increment the Time Since Last Spike
        this.timeSinceLastSpike += Time.deltaTime;

        // Is it time for another Spike ?
        if ((this.timeSinceLastSpike > this.timeTillNextSpike) && (this.spikes.Count < maxNumberOfSpikes))
        { 
            GenerateSpike();
        }
    }
}
