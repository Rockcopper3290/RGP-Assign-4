using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PulseTMProColor : MonoBehaviour
{
    // Time taken to return to original color
    [SerializeField] private float pulseDuration = 0.8f;

    private TMP_Text text;

    private Color32 originalColor;
    private Color32 pulseColor;    // GameObject changes to this scale at start of pulse

    private bool pulsing = false;
    private float pulseStart;

    // Start is called before the first frame update
    void Start()
    {
        // Reference to the TMPro
        this.text = gameObject.GetComponent<TMP_Text>();

        // Preserve the original color of the TextMeshPro
        this.originalColor = this.text.color;
    }

    // Pulse the Scale of the GameObject
    // Tween the GameObject from pulsedScale back to its originalScale
    public void Pulse(Color32 pulseColor)
    {
        this.pulsing = true;
        this.pulseStart = Time.time;
        this.pulseColor = pulseColor;
    }

    // Update is called once per frame
    void Update()
    {
        // Are we Pulsing ?
        if (this.pulsing)
        {
            if (Time.time > (this.pulseStart + this.pulseDuration))
            {
                this.pulsing = false;
                this.text.color = this.originalColor;
            }
            else
            {
                float percentage = (Time.time - this.pulseStart) / this.pulseDuration;

                this.text.color = Color32.Lerp(this.pulseColor, this.originalColor, percentage);
            } 
        }
    }
}
