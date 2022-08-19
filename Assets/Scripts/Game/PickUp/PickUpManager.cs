using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpManager : MonoBehaviour
{
    // PickUp Infomation
    [Header("PickUp Prefabs")]
    [SerializeField] private GameObject coinPrefab;
    [SerializeField] private GameObject shieldPrefab;
    [SerializeField] private GameObject invinciblePrefab;
    [Space(10)]

    // PickUp Behavour
    [Header("Pickup Behavour")]
    [SerializeField] private float probInvincible = 0.05f;
    [SerializeField] private float probShield = 0.05f;
    [SerializeField] private float minTimeBetweenPickUps = 1.0f;
    [SerializeField] private float maxTimeBetweenPickUps = 2.0f;
    [SerializeField] private int maxNumberOfPickUps = 2;
    [Space(10)]

    // Game Manager
    private GameManager gameManager;
    private DifficultyManager difficulty;
    private SpikeManager spikeManager;
    private Tutorial tutorial;

    // PickUp Management
    private float timeSinceLastPickUp;
    private float timeTillNextPickUp;
    List<GameObject> pickups = new List<GameObject>();

    public void SetGameManager(GameManager gameManager)
    {
        this.gameManager = gameManager;
        this.difficulty = gameManager.GetDifficultyManager();
        this.spikeManager = gameManager.GetSpikeManager();
        this.tutorial = gameManager.GetTutorial();
    }

    public List<GameObject> PickUps()
    {
        return this.pickups;
    }

    private bool TooCloseToSpike(Vector3 pickUpPosition)
    {
        float minDistanceFromSpike = this.difficulty.MinimumDistanceFromSpike();

        foreach (GameObject spike in this.spikeManager.Spikes())
        {
            if (Vector3.Distance(pickUpPosition, spike.transform.position) < minDistanceFromSpike)
                return true;
        }

        return false;
    }

    private void CreatePickUp(string pickUpType, float xPosition, bool checkSpikeDistance = true)
    {
        GameObject pickUp;

        // PickUp position
        Vector3 position = new Vector3(xPosition, 15.0f, 0.0f);

        // If the PickUp is too close to a Spike, abort this attempt.
        if (checkSpikeDistance && TooCloseToSpike(position))
            return;

        if (pickUpType == "Coin")
            pickUp = Instantiate(this.coinPrefab);
        else if (pickUpType == "Invincible")
            pickUp = Instantiate(this.invinciblePrefab);
        else if (pickUpType == "Shield")
            pickUp = Instantiate(this.shieldPrefab);
        else
            return;

        // Set the Position
        pickUp.transform.position = position;

        // Get the PickUp Script from Game Object
        PickUp pickupScript = pickUp.GetComponent<PickUp>();

        // Set the PickUp's reference to the Game Manager
        pickupScript.SetGameManager(this.gameManager);

        // Add the PickUp to the coins List 
        this.pickups.Add(pickUp);
    }

    private void GeneratePickUp()
    {
        string pickUpType;
        float xPosition;

        // Randomly place the PickUp across the X-axis
        xPosition = Random.Range(-2.0f, 2.0f);

        // Do we create an Invincible, Sheild or Coin
        float prob = Random.Range(0.0f, 1.0f);
        if (prob <= this.probInvincible)
            pickUpType = "Invincible";
        else if (prob <= (this.probInvincible + this.probShield))
            pickUpType = "Shield";
        else
            pickUpType = "Coin";

        CreatePickUp(pickUpType, xPosition);

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

        if (this.gameManager.TutorialRunning())
        {
            TEPickUp pickUpEvent = this.tutorial.GetPickUpEvent();

            if (pickUpEvent != null)
                CreatePickUp(pickUpEvent.pickUpType, pickUpEvent.xPosition, false);

            return;
        }
        
        // Increment the Time Since Last PickUp
        this.timeSinceLastPickUp += Time.deltaTime;

        // Is it time for another PickUp ?
        if ((this.timeSinceLastPickUp > this.timeTillNextPickUp) && (this.pickups.Count < this.maxNumberOfPickUps))
        {
            GeneratePickUp();
        }
    }
}