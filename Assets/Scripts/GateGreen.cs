using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateGreen : MonoBehaviour
{
    public int requiredCollectables = 3;
    private bool gateOpened = false;

    public GreenGem gem;

    private void Update()
    {
        if (!gateOpened && BucketGreen.collectedCount >= requiredCollectables)
        {
            gateOpened = true;
            OpenGate();
        }
    }

    private void OpenGate()
    {
        Debug.Log("Gate opened!");
        Destroy(gameObject);

        // Activate the gem so it can now be collected
        if (gem != null)
            gem.Activate();
    }
}
