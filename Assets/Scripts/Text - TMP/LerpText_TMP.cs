using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LerpText_TMP : MonoBehaviour
{
    //TMP_Text textMesh;
    //Mesh mesh;
    //Vector3[] vertices;



    Vector3 minScale;
    public Vector3 maxScale;
    public bool repeatable;
    public float speed = 2f;
    public float duration = 2f;
    public float elapsedTime;
    bool isGrowing;


    // use this for initialization
    private void Start()
    {
        minScale = transform.localScale;

    }
    private void Update()
    {
        /*
        if (elapsedTime >= 1)
            elapsedTime = 0;

        if (transform.localScale == minScale)
            isGrowing = true;
        else if (transform.localScale == maxScale)
            isGrowing = false;


        if (isGrowing)
        {

            lerpZoom_Score(minScale, maxScale, elapsedTime);

        }
        else if (!isGrowing)
        {
            lerpZoom_Score(maxScale, minScale, elapsedTime);
        }
        /*

        {
            /*
            //textMesh.ForceMeshUpdate();
            //mesh = textMesh.mesh;
            //vertices = mesh.vertices;

            //Lerp up scale
            repeatLerp(minScale, maxScale, duration);

            //lerp down scale
            repeatLerp(maxScale, minScale, duration);
            */
        }

    }

/*

    public void lerpZoom_Score(Vector3 startVector, Vector3 newVector, float elapsedTime)
    {
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float percentageCompleted = elapsedTime / duration;

            transform.localScale = Vector3.Lerp(startVector, newVector, percentageCompleted);
        }
        //====================================================================================


    }
}

*/