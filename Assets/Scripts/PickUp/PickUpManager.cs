using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpManager : MonoBehaviour
{
    // PickUp Infomation
    public GameObject coinPrefab;
    public GameObject invinciblePrefab;

    // PickUp Behavour
    public float probInvincible = 0.10f;
    public float minTimeBetweenPickUps = 1.0f;
    public float maxTimeBetweenPickUps = 2.0f;
    public int maxNumberOfPickUps = 2;

    // Position Control
    public float minDistFromSpike = 2.5f;

    // Game Manager
    private GameManager gameManager;
    private SpikeManager spikeManager;

    // PickUp Management
    private float timeSinceLastPickUp;
    private float timeTillNextPickUp;
    List<GameObject> pickups = new List<GameObject>();

    public void SetGameManager(GameManager gameManager)
    {
        this.gameManager = gameManager;
        this.spikeManager = gameManager.GetSpikeManager();
    }

    public List<GameObject> PickUps()
    {
        return this.pickups;
    }

    private bool TooCloseToSpike(Vector3 pickUpPosition)
    {
        foreach (GameObject spike in this.spikeManager.Spikes())
        {
            if (Vector3.Distance(pickUpPosition, spike.transform.position) < this.minDistFromSpike)
                return true;
        }

        return false;
    }

    private void CreatePickUp()
    {
        GameObject pickup;
        Vector3 position;

        // Randomly poistion the PickUp across the X-axis
        // TODO Create Constants for the hard coded numbers, similar with Spike Manager
        position = new Vector3(Random.Range(-2.0f, 2.0f), 15.0f, 0.0f);

        // If the PickUp is too close to a Spike, abort this attempt.
        if (TooCloseToSpike(position))
            return;

        // Do we create a Coin or Invicible ?
        float prob = Random.Range(0.0f, 1.0f);
        if (prob <= this.probInvincible)
            pickup = Instantiate(this.invinciblePrefab);
        else
            pickup = Instantiate(this.coinPrefab);

        // Set the Position
        pickup.transform.position = position;

        // Get the PickUp Script from Game Object
        PickUp pickupScript = pickup.GetComponent<PickUp>();

        // Set the PickUp's reference to the Game Manager
        pickupScript.SetGameManager(this.gameManager);

        // Add the PickUp to the coins List 
        this.pickups.Add(pickup);

        // Schedule the next PickUp
        ScheduleNextPickUp();
    }

    private void ScheduleNextPickUp()
    {
        this.timeSinceLastPickUp = 0.0f;
        this.timeTillNextPickUp = Random.Range(this.minTimeBetweenPickUps, this.maxTimeBetweenPickUps);
    }

    // Destory a specific PickUp
    public void DestroyPickUp(GameObject pickup)
    {
        this.pickups.Remove(pickup);
        Destroy(pickup);
    }

    // Destroy all Coins
    public void DestroyAllPickUps()
    {
        // Iterate backward though the list and Destroy all Spikes
        for (int i = this.pickups.Count - 1; i >= 0; i--)
        {
            GameObject pickup = this.pickups[i];
            this.pickups.RemoveAt(i);
            Destroy(pickup);
        }
    }

    public void GameStart()
    {
        ScheduleNextPickUp();
    }

    public void GameOver()
    {
        DestroyAllPickUps();
    }

    // Update is called once per frame
    void Update()
    {
        if (!this.gameManager.GameRunning())
            return;

        // Increment the Time Since Last PickUp
        this.timeSinceLastPickUp += Time.deltaTime;

        // Is it time for another PickUp ?
        if ((this.timeSinceLastPickUp > this.timeTillNextPickUp) && (this.pickups.Count < this.maxNumberOfPickUps))
        {
            CreatePickUp();
        }
    }
}