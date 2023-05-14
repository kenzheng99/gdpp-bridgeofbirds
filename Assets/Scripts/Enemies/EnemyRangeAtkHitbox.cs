using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRangeAtkHitbox : MonoBehaviour
{
    private PlayerController pc;
    public AttributesManager atm;
    
    // Start is called before the first frame update
    void Start()
    {
        pc = GameObject.Find("Player").GetComponent<PlayerController>();
    }

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player")) {
            pc.GetHit(atm.attack, transform.forward, atm.knockBackStrength);
        }
    }
}
