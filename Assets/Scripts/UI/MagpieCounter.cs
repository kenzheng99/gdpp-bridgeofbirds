using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MagpieCounter : MonoBehaviour {
    [SerializeField] private Text text;
    [SerializeField] private int totalMagpies;
    [SerializeField] private MagpieController magpieController;
    
    void Update() {
        string str = magpieController.GetMagpieCount() + "/" + totalMagpies;
        Debug.Log(str);
        text.text = str;
    }
}
