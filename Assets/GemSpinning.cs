using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GemSpinning : MonoBehaviour
{
    public Vector3 rotationSpeed = new Vector3(0f, 50f, 0f);

    void Update()
    {
        transform.Rotate(rotationSpeed * Time.deltaTime);
    }
}