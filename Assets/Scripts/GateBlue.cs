using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateBlue : MonoBehaviour
{
    public int requiredCollectables = 3;
    private bool gateOpened = false;

    public BlueGem gem; // Drag and drop the Gem object in the Inspector

    private void Update()
    {
        if (!gateOpened && CollectableBlue.collectedCount >= requiredCollectables)
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
