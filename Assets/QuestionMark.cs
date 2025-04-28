using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestionMark : MonoBehaviour
{
    public Vector3 rotationSpeed = new Vector3(0f, 50f, 0f);
    public float floatAmplitude = 0.5f; 
    public float floatFrequency = 1f; 
    public Transform targetObject; 

    private Vector3 startPos;

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        transform.RotateAround(targetObject.position, Vector3.up, rotationSpeed.y * Time.deltaTime);

        float newY = startPos.y + Mathf.Sin(Time.time * floatFrequency) * floatAmplitude;
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);
    }
}