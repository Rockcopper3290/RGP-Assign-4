using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class TweenyScore : MonoBehaviour
{
    // How much should text grow when it starts boinging (30=30%)
    public float boingStartScaleUp = 30.0f;
    // How long does the text take to elastic-tween back to starting size
    public float boingDuration = 0.7f;
    Vector3 minScale;
    private Vector3 boingStartScale;        // thing pops to this scale at start of boing
    private Vector3 tweenedScale;   // vector being updated by DoTween
    private bool isBoinging = false;
    
    // Start is called before the first frame update
    void Start()
    {
        // Get the scale of the score when first loaded
        minScale = transform.localScale;
        // How much will the text scale up before elastic tweening back to minscale.
        boingStartScale = minScale + new Vector3(boingStartScaleUp, boingStartScaleUp, boingStartScaleUp);
    }

    // Update is called once per frame
    void Update()
    {
        // Testing boing by running it whenever jump is clicked.
        if (isBoinging)
        {
            // get the current value set by the tween engine and apply it to the text
            transform.localScale = tweenedScale; 
        } else
        {
            if (Input.GetButtonDown("Jump"))
            {
                isBoinging = true;
                BoingScore();
            }
        }
    }

    // Called when the tween ends
    private void EndBoingScore()
    {
        Debug.Log("Boing over.");
        isBoinging = false;
    }

    // Kick off the tweening of the "tweenedScale" vector from scaled up (boing start) value
    // back down to minscale. 
    public void BoingScore()
    {
        Debug.Log("Score goes boiiing");
        tweenedScale = new Vector3(boingStartScale.x, boingStartScale.y, boingStartScale.z);
        DOTween.To(() => tweenedScale, x => tweenedScale = x, minScale, boingDuration)
        .SetEase(Ease.OutElastic)
        .OnComplete(EndBoingScore);   
    }

}
