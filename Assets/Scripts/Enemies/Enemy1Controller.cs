using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class Enemy1Controller : MonoBehaviour {
    // This class is deprecated, use EnemyController derived classes instead
    private PlayerController playerController;
    private AttributesManager atm;
    
    private EnemyAtkHitbox hitbox;

    [SerializeField] private float atkCoolDown = 3;
    private Material material;
    private Color color;
    
    // Start is called before the first frame update
    private void Start() {
        playerController = GameObject.Find("Player").GetComponent<PlayerController>();
        atm = transform.GetComponent<AttributesManager>();
        hitbox = transform.GetChild(1).GetComponent<EnemyAtkHitbox>();
        
        material = transform.GetChild(0).GetComponent<Renderer>().material;
        color = material.color;

        StartCoroutine(Attack());
    }

    private void Update() {
        // placeholder code to get health UI
        var text = GameObject.Find("EnemyHealth").GetComponent<Text>();
        text.text = "Enemy Health: " + atm.health;
    }

    public void GetHit(float amount, GameObject source) {
        // atm.TakeDamage(amount, source);
        // StopCoroutine(ChangeColor());
        // StartCoroutine(ChangeColor());
    }

    // private IEnumerator ChangeColor() {
    //     material.color = Color.red;
    //     yield return new WaitForSeconds(.1f);
    //     material.color = color;
    // }

    private IEnumerator Attack() {
        for (;;) {
            if (hitbox.PlayerInRange) {
                // playerController.GetHit(atm.attack, gameObject);
            }
            yield return new WaitForSeconds(atkCoolDown);
        }
    }
}
