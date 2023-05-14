using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FloatingAnimation : MonoBehaviour {
    [SerializeField] private float floatSpeed = 0.001f;
    [SerializeField] private float floatAmplitude = 0.25f;
    [SerializeField] private bool isFloating = true;

    private float startAltitude;

    // private bool movingUp = true;
    private float targetY;

    void Start() {
        startAltitude = 0;
        targetY = startAltitude + floatAmplitude;
    }

    void Update() {
        if (!isFloating) {
            return;
        }

        Vector3 targetPosition = new Vector3(0, targetY, 0);
        transform.localPosition = Vector3.MoveTowards(transform.localPosition, targetPosition, floatSpeed * Time.deltaTime);

        if (Math.Abs(targetY - transform.localPosition.y) <= 0.01f) {
            if (targetY > startAltitude) {
                targetY = startAltitude - floatAmplitude;
            } else {
                targetY = startAltitude + floatAmplitude;
            }
        }
    }

    public void StartFloating() {
        isFloating = true;
    }

    public void StopFloating() {
        isFloating = false;
    }

    // public void SetFloatingAltitude(float altitude) {
    //     startAltitude = altitude;
    //     if (targetY > startAltitude) {
    //         targetY = startAltitude - floatAmplitude;
    //     } else {
    //         targetY = startAltitude + floatAmplitude;
    //     }
    // }
}