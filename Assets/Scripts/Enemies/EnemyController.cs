using System.Collections;
using UnityEngine;

public enum EnemyState {
    IDLE,
    ALERTED,
    READY_TO_ATTACK,
    ATTACKING,
    DEAD
}
public class EnemyController : MonoBehaviour {
    public EnemyState state;
    protected PlayerController player;
    protected float lastAttackTime;
    protected EnemyAtkHitbox hitbox;
    private Material material;
    public AttributesManager atm;

    private void Start() {
        material = transform.GetChild(0).GetChild(0).GetComponent<Renderer>().material;
        hitbox = transform.GetChild(1).GetComponent<EnemyAtkHitbox>();
        player = PlayerController.Instance;
        atm = GetComponent<AttributesManager>();
        
        // set attributes 
        transform.localScale = transform.localScale * atm.sizeMultiplier;
        
    }

    private void Update() {
        SetCurrentState();
        if (state == EnemyState.IDLE) {
            Idle();
        } else if (state == EnemyState.ALERTED) {
            Alerted();
        } else if (state == EnemyState.READY_TO_ATTACK) {
            StartCoroutine(Attack());
        } else if (state == EnemyState.ATTACKING) {
            // nothing to do except wait for coroutine to finish
        } else if (state == EnemyState.DEAD)
        {
            SoundManager.Instance.PlayEnemyDeathSFX();
            player.GainQi(atm.qiDropAmount);
            var rangeHundun = GetComponent<RangeHundunController>();
            if (rangeHundun is not null) {
                // Debug.Log("range hundun");
                Destroy(rangeHundun.projObj);
            }
            Destroy(gameObject);
        }
        
        // cheat code, TODO delete later
        if (Input.GetKeyDown(KeyCode.K)) {
            state = EnemyState.DEAD;
        }
    }

    private void SetCurrentState() {
        if (state == EnemyState.ATTACKING) {
            return; // wait for attack to finish, attack coroutine will update state
        } else if (state == EnemyState.DEAD) {
            return; // enemy is dead, no further state transitions
        }
        Vector3 playerPositionOnGround = new Vector3(player.transform.position.x, 0, player.transform.position.z);
        Vector3 positionOnGround = new Vector3(transform.position.x, 0, transform.position.z);
        float distance = Vector3.Distance(playerPositionOnGround, positionOnGround);
        
        if (distance > atm.alertDistance) {
            state = EnemyState.IDLE;
        } else if (distance > atm.attackDistance || Time.time - lastAttackTime < atm.attackCooldown) {
            state = EnemyState.ALERTED;
        } else {
            state = EnemyState.READY_TO_ATTACK;
        }
    }

    /* State update functions/routines, override these in subclass for different behaviors */
    protected virtual void Idle() {
        return;
    }

    protected virtual void Alerted() {
        FacePlayer();
    }
    
    protected virtual IEnumerator Attack() {
        state = EnemyState.ATTACKING;
        yield return null;
        if (hitbox.PlayerInRange) { 
            PlayerController.Instance.GetComponent<PlayerController>().GetHit(atm.attack, transform.forward, atm.knockBackStrength);
        }
        state = EnemyState.IDLE;
        lastAttackTime = Time.time;
    }
    
    /* Smaller helper functions */
    protected void FacePlayer() {
        Vector3 direction = player.transform.position - transform.position;
        direction = new Vector3(direction.x, 0, direction.z);
        Quaternion targetRotation = Quaternion.LookRotation(direction, Vector3.up);
        float rotationMultiplier = Quaternion.Angle(transform.rotation, targetRotation);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, atm.rotationSpeed * rotationMultiplier * Time.deltaTime);
    }

    protected void MoveForward() {
        Vector3 localForward = transform.TransformDirection(Vector3.forward);
        transform.position += localForward * atm.moveSpeed * Time.deltaTime;
    }

    private IEnumerator ChangeColor() {
        Color oldColor = material.color;
        material.color = Color.red;
        yield return new WaitForSeconds(.1f);
        material.color = oldColor;
    }

}
