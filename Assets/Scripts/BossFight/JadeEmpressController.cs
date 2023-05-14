using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

public enum BossState {
    BEFORE_BATTLE,
    REGULAR_STAGE,
    ATTACKING,
    OFFSCREEN_STAGE,
    DEAD
}
public class JadeEmpressController : MonoBehaviour {
    [SerializeField] private Transform offscreenPosition;
    [SerializeField] private GameObject singleHitProjectilePrefab;
    [SerializeField] private GameObject multiHitProjectilePrefab;
    [SerializeField] private BossFightManager bossFightManager;
    [SerializeField] private float startBattleDistance;
    
    private BossState state;
    private PlayerController player;
    private float lastAttackTime;
    private Material material;
    private AttributesManager atm;
    
    private int numStages = 5;
    private float healthPerStage;
    private float nextStageTransitionHealth;
    private GameObject projectile;
    
    // per-level singleton
    private static JadeEmpressController _instance;
    public static JadeEmpressController Instance => _instance;
    
    private void Awake() {
        if (_instance != null && _instance != this) {
            Destroy(gameObject);
        } else {
            _instance = this;
        }
    }

    private void Start() {
        player = PlayerController.Instance;
        atm = GetComponent<AttributesManager>();
        healthPerStage = atm.maxHealth / numStages;
        nextStageTransitionHealth = atm.maxHealth - healthPerStage;
        state = BossState.BEFORE_BATTLE;
        
        // set attributes 
        transform.localScale = transform.localScale * atm.sizeMultiplier;
        
    }

    private void Update() {
        // debug cheat code
        if (Input.GetKeyDown(KeyCode.K)) {
            Debug.Log("here");
            atm.health = 0;
        }

        if (state == BossState.BEFORE_BATTLE) {
            if (Vector3.Distance(transform.position, player.transform.position) > startBattleDistance) {
                return;
            }
            Debug.Log("Start Boss Battle");
            state = BossState.REGULAR_STAGE;
        }
        
        if (state == BossState.ATTACKING) {
            return;
        }

        if (atm.health <= 0) {
            state = BossState.DEAD;
            Destroy(gameObject);
            GameManager.Instance.LoadNextLevel();
        }
        

        // state transitions
        if (atm.health <= nextStageTransitionHealth) {
            ToggleState();
            nextStageTransitionHealth -= healthPerStage;
        }
        
        if (state == BossState.REGULAR_STAGE) {
            FacePlayer();
            StartCoroutine(RegularAttack());
        } else if (state == BossState.OFFSCREEN_STAGE) {
            
            FacePlayer();
            if (!projectile && Vector3.Distance(transform.position, offscreenPosition.position) <= 0.01f) {
                Vector3 projectileSpawnPosition = transform.position + Vector3.up + transform.rotation * Vector3.right;
                projectile = Instantiate(multiHitProjectilePrefab, projectileSpawnPosition, Quaternion.identity);
            }
        }
    }

    public void OnMagpieHit() {
        if (projectile) {
            Destroy(projectile);
        }
        atm.TakeDamage(20, transform.forward, 0);
        ToggleState();
    }

    private void ToggleState() {
        if (state == BossState.REGULAR_STAGE) {
            state = BossState.OFFSCREEN_STAGE;
            StartCoroutine(MoveToOffscreenPosition());
        } else if (state == BossState.OFFSCREEN_STAGE) {
            state = BossState.REGULAR_STAGE;
        }
    }

    /* State update functions/routines, override these in subclass for different behaviors */
    
    private IEnumerator RegularAttack() {
        state = BossState.ATTACKING;
        while (Vector3.Distance(transform.position, player.transform.position) > 5) {
            FacePlayer();
            MoveForward();
            yield return null;
        }
        
        Vector3 projectileSpawnPosition = transform.position + Vector3.up + transform.rotation * Vector3.right;
        projectile = Instantiate(singleHitProjectilePrefab, projectileSpawnPosition, Quaternion.identity);
        
        // if (hitbox.PlayerInRange) { 
        //     PlayerController.Instance.GetHit(atm.attack, gameObject);
        // }
        // lastAttackTime = Time.time;
        while (projectile != null) {
            yield return null;
        }
        state = BossState.REGULAR_STAGE;
    }

    private IEnumerator MoveToOffscreenPosition() {
        while (Vector3.Distance(transform.position, offscreenPosition.position) > 0.01f) {
            transform.position =
                Vector3.MoveTowards(transform.position, offscreenPosition.position, atm.moveSpeed * Time.deltaTime);
            yield return null;
        }
        if (DialogueManagerBoss.Instance.triggeredTeachJadeEmpressAttackDialogue == false)
        {
            yield return new WaitForSeconds(3);
            DialogueManagerBoss.Instance.StartTeachJadeEmpressAttackDialogue();
        }
        bossFightManager.ActivateNextBridge();
    }
    
    /* Smaller helper functions */

    private void FacePlayer() {
        Vector3 direction = player.transform.position - transform.position;
        direction = new Vector3(direction.x, 0, direction.z);
        Quaternion targetRotation = Quaternion.LookRotation(direction, Vector3.up);
        float rotationMultiplier = Quaternion.Angle(transform.rotation, targetRotation);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, atm.rotationSpeed * rotationMultiplier * Time.deltaTime);
    }

    private void MoveForward() {
        Vector3 localForward = transform.TransformDirection(Vector3.forward);
        transform.position += localForward * atm.moveSpeed * Time.deltaTime;
    }
}
