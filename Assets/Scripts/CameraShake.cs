using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{

    public IEnumerator Shake(float duration=.1f, float magnitude=12f) {
        var elapsed = 0.0f;
        while (elapsed < duration) {
            var pos = new Vector3(Rng(magnitude), Rng(magnitude), Rng(magnitude));

            transform.localPosition = pos;

            elapsed += Time.deltaTime;
            yield return null;
        }
    }
    
    private float Rng(float magnitude) {
        return Random.Range(-1f, 1f) * magnitude * Time.deltaTime;
    }
}
