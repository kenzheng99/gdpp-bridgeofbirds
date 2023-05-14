using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinAnimation : MonoBehaviour {
    [SerializeField] private float spinSpeed = 1.0f;

    void Update() {
        transform.Rotate(0, spinSpeed * Time.deltaTime, 0);
    }
}
