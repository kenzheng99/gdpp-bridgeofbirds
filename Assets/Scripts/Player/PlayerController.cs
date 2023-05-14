using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {
    [SerializeField] private float moveSpeed = 5;
    [SerializeField] private float turnSpeed = 50;

    [SerializeField] private float dashTime = 1f;
    [SerializeField] private float dashSpeed = 0.05f;
    [SerializeField] private float dashCooldown = 0.1f;
    [SerializeField] private float iframeDuration = .4f;
    [SerializeField] private float atkDuration;
    [SerializeField] private float clickPlaneHeight = 0.5f;
    
    [SerializeField] private Animator anim;
    [SerializeField] private Animator stickAnim;
    
    private Vector3 input;
    private CameraFollow camera;
    private CharacterController cc;
    private PlayerAbilities abilities;
    private AttributesManager atm;
    
    private Coroutine heal;

    private bool isDashing;
    private bool gaveQi;
    
    public Vector3 CursorDir3 { get; private set; }

    // statuses
    public bool NormalAttacking { get; private set; }
    public bool RangeAttacking { get; private set; }

    // timers
    private float startAtkTime;
    private float gotHitTime;
    
    // per-level singleton
    private static PlayerController _instance;
    public static PlayerController Instance => _instance;
    
    private void Awake() {
        if (_instance != null && _instance != this) {
            Destroy(gameObject);
        } else {
            _instance = this;
        }
    }

    private void Start() {
        cc = GetComponent<CharacterController>();
        abilities = gameObject.GetComponent<PlayerAbilities>();
        atm = gameObject.GetComponent<AttributesManager>();
        camera = GameObject.Find("Camera Container").GetComponent<CameraFollow>();

        gaveQi = false;
    }

    private void Update() {
        if (DialogueManager.Instance && GameManager.Instance.dialogueIsPlaying)
        {
            return;
        }
        if (atm.health <= 0) {
            GameManager.Instance.OnPlayerDeath();
        }

        if (!gaveQi && DialogueManager.Instance && DialogueManager.Instance.triggeredTeachUltimateAbilityDialogue)
        {
            atm.qi = atm.maxQi;
            gaveQi = true;
            
        }
        GatherInput();
        Look();
        CheckAttacks();
        Move();
        CheckQi();
        if (!isDashing && Input.GetKeyDown(KeyCode.Space)) {
            StartCoroutine(Dash());
        }
        
        // animation logic
        if (input.magnitude > 0) {
            anim.SetBool("isRunning", true);
        } else {
            anim.SetBool("isRunning", false);
        }
        anim.transform.rotation = transform.rotation; // hacky fix for weird animation rotation bug
        anim.transform.position = transform.position;
    }

    private void GatherInput() {
        input = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        
        if (input.magnitude > 0.2) {
            if (GameManager.Instance.unlockedAbilityIcons)
            {
                if (abilities.healing) {
                    StopCoroutine(heal);
                    abilities.healing = false;
                } 
            }
        }

        if (Input.GetKeyDown(KeyCode.Q)) {
            heal = StartCoroutine(abilities.Heal());
        }

        if (Input.GetKeyDown((KeyCode.R))) {
            if (GameManager.Instance.unlockedUltimate)
            {
                StartCoroutine(abilities.Ultimate(Utils.MouseToGroundPoint(clickPlaneHeight)));
            }
        }
            
        if (Input.GetMouseButtonDown(0)) {
            // detect left click
            NormalAtk();
        } 
        if (Input.GetMouseButtonDown(1)) {
            // detect holding right click
            RangeAttacking = true;
            NormalAttacking = false;
        }
        else if (Input.GetMouseButtonUp(1)) {
            // detect release right click
            RangeAttacking = false;
            StartCoroutine(abilities.FireProjectile());
        }
        
        // cheat: fill qi
        if (Input.GetKeyDown(KeyCode.P)) {
            atm.qi = atm.maxQi;
        }
        
        //tester: get hit
        if (Input.GetKeyDown(KeyCode.T)) {
            GetHit(0, transform.forward, 1);
        }
    }

    #region orientation
    private void Look() {
        if (input == Vector3.zero) return;

        Quaternion newRotation = Quaternion.LookRotation(input.ToIso(), Vector3.up);
        var angleDifference = Quaternion.Angle(transform.rotation, newRotation);
        if (!NormalAttacking && !RangeAttacking) {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, newRotation, turnSpeed * angleDifference * Time.deltaTime);
        }
    }

    private void LookCursor() {
        Quaternion newRotation = Quaternion.LookRotation(CursorDir3, Vector3.up);
        // var angleDiff = Quaternion.Angle(transform.rotation, newRotation);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, newRotation, turnSpeed * 100 * Time.deltaTime);
    }
    
    private void SetCursorDirection() {
        CursorDir3 = Utils.MouseToGroundPoint(clickPlaneHeight) - transform.position;
        CursorDir3 = new Vector3(CursorDir3.x, transform.position.y, CursorDir3.z);
    }

    #endregion
    
    #region movement
    private void Move() {
        Vector3 direction = input.ToIso().normalized;
        float adjustedMoveSpeed = Utils.NormalizeIsoMovementSpeed(moveSpeed, direction);
        if (!atm.isKnocking) {
            cc.Move(direction * adjustedMoveSpeed * Time.deltaTime);
        }
    }

    private IEnumerator Dash() {
        SoundManager.Instance.PlayPlayerDashSound();
        isDashing = true;
        float startTime = Time.time;
        Vector3 direction = input.ToIso().normalized;
        if (direction.magnitude < 0.01f) {
            direction = (transform.rotation * Vector3.forward).normalized;
        }
        float adjustedDashSpeed = Utils.NormalizeIsoMovementSpeed(dashSpeed, direction);
        while (Time.time - startTime < dashTime) {
            cc.Move(direction * adjustedDashSpeed * Time.deltaTime);
            yield return null;
        }

        yield return new WaitForSeconds(dashCooldown);
        isDashing = false;
    }
    #endregion

    #region controlls
    private void CheckAttacks() {
        if (NormalAttacking) {
            // Debug.Log(Time.time + " " + startAtkTime);
            LookCursor();
            if (Time.time > startAtkTime + atkDuration) {
                // end normal attack
                NormalAttacking = false;
            }
        } else if (RangeAttacking) {
            SetCursorDirection();
            LookCursor();
        }
    }

    private void NormalAtk() {
        if (NormalAttacking) {
            return;}
        // start normal attack
        NormalAttacking = true;
        RangeAttacking = false;
        SetCursorDirection();
        startAtkTime = Time.time;
        abilities.DoNormalAtk();
    }
    
    public void GetHit(float amount, Vector3 direction, float strength) {
        if (Time.time < gotHitTime + iframeDuration) {
            return;}

        atm.TakeDamage(amount, direction, strength);
        StartCoroutine(camera.cameraShake.Shake());

        gotHitTime = Time.time;
    }
    
    public void GainQi(float qi) {
        atm.qi += qi;
        // Debug.Log("Gained "+qi+" qi");
        if (atm.qi > atm.maxQi) {
            atm.qi = atm.maxQi;
        }
    }

    private void CheckQi()
    {
        if (atm.qi > 0)
        {
            GameManager.Instance.hasQi = true;
            if (atm.qi >= atm.maxQi)
            {
                GameManager.Instance.hasMaxQi = true;
            }
            else
            {
                GameManager.Instance.hasMaxQi = false;
            }
        }
        else if (atm.qi <= 0)
        {
            GameManager.Instance.hasQi = false;
        }
    }
    #endregion
    
}