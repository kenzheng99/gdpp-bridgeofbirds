using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AtkHitbox : MonoBehaviour {
    private PlayerAbilities abilities;
    private HashSet<GameObject> enemies;

    private void Start() {
        abilities = GameObject.Find("Player").GetComponent<PlayerAbilities>();
        enemies = abilities.enemies;
    }

    private void OnTriggerEnter(Collider other) {
        // Debug.Log(gameObject.name + " triggered on " + other.gameObject.name);
        
        var obj = other.gameObject;
        if (!enemies.Contains(obj) && obj.CompareTag("Enemy")) {
            enemies.Add(obj);
            if (gameObject.CompareTag("PlayerProjectile")) {
                abilities.DoRangeAtk();
            }
        }
    }

    private void OnTriggerExit(Collider other) {
        enemies.Remove(other.gameObject);
    }

    public IEnumerator DestroyHitBox() {
        // Debug.Log("start destroy");
        var tmpTime = Time.time;
        while (true) {
            // Debug.Log("Moving");
            transform.Translate(0, -100, 0 * 100 * Time.deltaTime);
            yield return new WaitForSeconds(0.1f);
            if (Time.time > tmpTime + 1) {
                break;
            }
        }
        // Debug.Log("Destroy this");
        Destroy(gameObject);
    }
}
