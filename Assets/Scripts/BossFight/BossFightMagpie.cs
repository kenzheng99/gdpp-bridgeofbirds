using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossFightMagpie : MonoBehaviour {
    [SerializeField] private float moveSpeed;
    private bool attacking = false;
    private Vector3 targetPosition;
    void Start() {
    }

    void Update() {
        if (!attacking) {
            return;
        }
        if (Vector3.Distance(transform.position, targetPosition) > 0.01f) {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
        } else {
            JadeEmpressController.Instance.OnMagpieHit();
            Destroy(gameObject);
        }
    }

    public void Attack() {
        targetPosition = JadeEmpressController.Instance.transform.position;
        attacking = true;
    }
}
