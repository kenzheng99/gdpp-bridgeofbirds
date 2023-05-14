using System.Collections;
using UnityEngine;

public class RangeHundunController: EnemyController {
    
    [SerializeField] private float attackSpeed;
    [SerializeField] private float attackWindupTime;
    [SerializeField] private float projectileDistance = 5.5f;
    [SerializeField] private float projectileSpeed = 20;

    [SerializeField] private GameObject projPrefab;
    public GameObject projObj;
    protected override void Idle() {
        FacePlayer();
    }
    
    protected override void Alerted() {
        FacePlayer();
        MoveForward();
    }

    protected override IEnumerator Attack() {
        state = EnemyState.ATTACKING;
        // snap rotation
        Vector3 direction = player.transform.position - transform.position;
        direction = new Vector3(direction.x, 0, direction.z);
        Quaternion targetRotation = Quaternion.LookRotation(direction, Vector3.up);
        transform.rotation = targetRotation;
        
        // pick target position
        Vector3 targetPosition = new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z); 
        targetPosition += transform.rotation * Vector3.forward * 0.5f; // slightly overshoot player
        
        // windup (move back a bit)
        float windupStartTime = Time.time;
        while (Time.time - windupStartTime < attackWindupTime) {
            transform.position += transform.rotation * Vector3.back * 0.5f * Time.deltaTime;
            yield return null;
        }
        
        // attack (lunge towards player), check hitbox
        projObj = Instantiate(projPrefab, transform.position, transform.rotation);
        projObj.GetComponent<EnemyRangeAtkHitbox>().atm = atm;
        Vector3 projectileDir = transform.forward;
        Vector3 projSpawnPos = projObj.transform.position;
        for (;;) {
            if (projectileDistance > Vector3.Distance(projSpawnPos, projObj.transform.position)) {
                projObj.transform.position += Time.deltaTime * projectileSpeed * projectileDir;
            }
            else {
                Destroy(projObj);
                break;
            }
            yield return null;
        }

        // cooldown
        yield return new WaitForSeconds(1f);
        
        state = EnemyState.IDLE;
        lastAttackTime = Time.time;
    }
}
