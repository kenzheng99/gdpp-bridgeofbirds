using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JadeEmpressProjectile : MonoBehaviour {
    [SerializeField] private float moveSpeed;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float windupTime;
    [SerializeField] private float numAttacks;
    [SerializeField] private float overshoot;
    [SerializeField] private float damage;
    [SerializeField] private GameObject warningLine;
    [SerializeField] private EnemyAtkHitbox hitbox;

    private PlayerController player;
    private AttributesManager atm;
    private bool attacking = false;
    void Start() {
        player = PlayerController.Instance;
        hitbox = GetComponent<EnemyAtkHitbox>();
        atm = GameObject.Find("JadeEmpress").GetComponent<AttributesManager>();
    }

    void Update() {
        if (attacking) {
            return;
        }
        
        if (numAttacks > 0) {
            StartCoroutine(Attack());
            attacking = true;
            numAttacks -= 1;
        } else {
            Destroy(gameObject);
        }
    }

    private IEnumerator Attack() {
        float startTime = Time.time;
        Vector3 direction;
        warningLine.SetActive(true);
        while (Time.time - startTime < windupTime) {
            direction = player.transform.position - transform.position;
            direction = new Vector3(direction.x, 0, direction.z).normalized;
            transform.rotation = Quaternion.LookRotation(direction, Vector3.up);
            yield return null;
        }
        warningLine.SetActive(false);
        
        direction = player.transform.position - transform.position;
        direction = new Vector3(direction.x, 0, direction.z).normalized;
        Quaternion targetRotation = Quaternion.LookRotation(direction, Vector3.up);
        transform.rotation = targetRotation;
        
        Vector3 targetPosition = player.transform.position + direction.normalized * overshoot;
        bool playerHit = false;
        while (Vector3.Distance(targetPosition, transform.position) > 0.01f) {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
            if (!playerHit && hitbox.PlayerInRange) { 
                player.GetComponent<PlayerController>().GetHit(damage, transform.forward, atm.knockBackStrength);
                playerHit = true;
            }
            yield return null;
        }

        attacking = false;
    }
}
