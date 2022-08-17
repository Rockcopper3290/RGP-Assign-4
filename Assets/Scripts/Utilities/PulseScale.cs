using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PulseScale : MonoBehaviour
{
    // Maximum Increase in Scale
    [SerializeField] private float maxScaleChange = 0.2f;
    // Time taken to return Scale to original value
    [SerializeField] private float pulseDuration = 0.8f;

    private Vector3 originalScale;
    private Vector3 pulsedScale;    // GameObject changes to this scale at start of pulse

    private Tween tween = null;
    private Vector3 tweenedScale;   // Vector being updated by DoTween

    // Start is called before the first frame update
    void Start()
    {
        // Preserve the original scale of the GameObject
        this.originalScale = transform.localScale;

        // Calculate the pulsed scale for the GameObject
        this.pulsedScale = this.originalScale * (1.0f + maxScaleChange);
    }

    // Called by the Tween when it ends
    private void PulseEnd()
    {
        this.tween = null;
    }

    // Pulse the Scale of the GameObject
    // Tween the GameObject from pulsedScale back to its originalScale
    public void Pulse()
    {
        // If we are already Pulsing, kill it and start a new Pulse
        if (this.tween != null)
            this.tween.Kill();

        this.tweenedScale = this.pulsedScale;

        this.tween = DOTween.To(() => this.tweenedScale, x => this.tweenedScale = x, this.originalScale, this.pulseDuration)
            .SetEase(Ease.OutElastic)
            .OnComplete(PulseEnd); 
    }

    // Update is called once per frame
    void Update()
    {
        // Are we Pulsing ?
        if (this.tween != null)
        {
            // Apply the Tweened Scale to the Game Object
            transform.localScale = tweenedScale; 
        }
    }
}
