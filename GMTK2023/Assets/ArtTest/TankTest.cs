using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    public Transform testTransform;

    private float startY = 0.0f;
    public float range = 45.0f;
    public float timescalar = 0.01f;
    public float scalar = 5.0f;

    private float intval;
    
    // Start is called before the first frame update
    void Start()
    {
        startY = testTransform.eulerAngles.y;
    }

    // Update is called once per frame
    void Update()
    {
        float sin = Mathf.Sin(Time.time * timescalar);
        float scaled = sin * scalar;

        float clamped = Mathf.Clamp(scaled, -1.0f, 1.0f);
        float rotation = Mathf.LerpAngle(startY - range, startY + range, (clamped + 1.0f) / 2.0f);

        intval = scaled;
        
        Vector3 eulera = testTransform.eulerAngles;
        eulera.y = rotation;
        testTransform.eulerAngles = eulera;
    }
}
