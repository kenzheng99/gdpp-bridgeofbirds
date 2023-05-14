using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstHunDunBarrier : MonoBehaviour
{

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.killedFirstHunDun)
        {
            Destroy(this.gameObject);
        }
    }
    
}
