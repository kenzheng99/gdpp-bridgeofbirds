using System.Collections.Generic;
using UnityEngine;

public class BossFightManager : MonoBehaviour {
    [SerializeField] private List<CloudBridge> bridges;

    private int index = 0;

    public void ActivateNextBridge() {
        CloudBridge bridge = bridges[index];
        bridge.ActivateBridge();
        index++;
    }
}
