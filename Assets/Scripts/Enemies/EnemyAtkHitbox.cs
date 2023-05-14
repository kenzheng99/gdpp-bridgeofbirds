using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAtkHitbox : MonoBehaviour
{
    public bool PlayerInRange { get; private set; }

    private void OnTriggerEnter(Collider other) {
        var obj = other.gameObject;
        if (obj.CompareTag("Player")) {
            PlayerInRange = true;
        }
    }
    
    private void OnTriggerExit(Collider other) {
        var obj = other.gameObject;
        if (obj.CompareTag("Player")) {
            PlayerInRange = false;
        }
    }
}
