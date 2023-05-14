using System.Collections;
using UnityEngine;

public class HundunEnemyController: EnemyController {
    
    [SerializeField] private float attackSpeed;
    [SerializeField] private float attackWindupTime;

    // private AttributesManager atm;
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
        bool playerHit = false;
        while (Vector3.Distance(transform.position, targetPosition) > 0.01f) {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, attackSpeed * Time.deltaTime);
            if (!playerHit && hitbox.PlayerInRange) { 
                // PlayerController.Instance.GetHit(20);
                
                PlayerController.Instance.GetComponent<PlayerController>().GetHit(atm.attack, transform.forward, atm.knockBackStrength);
                playerHit = true;
            }
            yield return null;
        }
        // cooldown
        yield return new WaitForSeconds(1f);
        
        state = EnemyState.IDLE;
        lastAttackTime = Time.time;
    }
}
