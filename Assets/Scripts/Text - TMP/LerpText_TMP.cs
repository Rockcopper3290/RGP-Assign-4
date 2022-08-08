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
    public float duration = 5f;

    // use this for initialization
    private void Start()
    {
        minScale = transform.localScale;

    }
    private void Update()
    {
        //textMesh.ForceMeshUpdate();
        //mesh = textMesh.mesh;
        //vertices = mesh.vertices;

        //Lerp up scale
        repeatLerp(minScale, maxScale, duration);

        //lerp down scale
        repeatLerp(maxScale, minScale, duration);

    }


    public void repeatLerp(Vector3 startVector, Vector3 newVector, float time)
    {
        float objectScaledSize = 0.0f;
        float rate = (1.0f / time) * speed;
        while (objectScaledSize < 1.0f)
        {
            objectScaledSize += Time.deltaTime * rate;
            gameObject.transform.localScale = Vector3.Lerp(startVector, newVector, time);
        }

    }
}
