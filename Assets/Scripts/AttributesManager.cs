using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class AttributesManager : MonoBehaviour {

    #region attributes
    // basics 
    [Header("Basics")]
    [SerializeField] public float health = 100;
    [SerializeField] public float maxHealth = 100;
    [SerializeField] public float attack;
    [SerializeField] public float knockBackStrength = 1;
    [SerializeField] public float knockBackRes = 1;
    [SerializeField] public float maxKnockDistance = 5;

    // enemy specific
    [Header("Enemy Specific")]
    [SerializeField] public float sizeMultiplier = 1;
    [SerializeField] public float rotationSpeed = 3;
    [SerializeField] public float moveSpeed = 1;
    [SerializeField] public float alertDistance = 10;
    [SerializeField] public float attackDistance = 5;
    [SerializeField] public float attackCooldown = 1;
    [SerializeField] public float qiDropAmount = 10;

    // player specific
    [Header("Player Specific")]
    [SerializeField] public float qi = 100;
    [SerializeField] public float maxQi = 100;
    
    [SerializeField] public float ultDurationSecs = 4;
    [SerializeField] public float ultIntervalSecs = .07f;
    [SerializeField] public float ultDmgPerHit = 12;

    #endregion

    #region vars

    private PlayerController pc;
    private TileManager tm;
    
    private bool isPlayer;
    private Material material;
    private Color color;

    private const bool Testing = true;
    private const float KnockBackDuration = 0.05f;
    private const float KnockBackCooldown = 0.15f;

    public bool isKnocking;
    #endregion
    private void Start() {
        tm = TileManager.Instance;
        
        isPlayer = gameObject.name == "Player";
        if (isPlayer) {
            pc = transform.gameObject.GetComponent<PlayerController>();
        }
        material = transform.GetChild(0).GetChild(0).GetComponent<Renderer>().material;
        color = material.color;
        isKnocking = false;
        
        
    }

    public void TakeDamage(float amount, Vector3 pushDir, float strength)
    {
        health -= amount;
        if (!isPlayer)
        {
            SoundManager.Instance.PlayPlayerAttackLandedSFX(UnityEngine.Random.Range(1,3));
        }
        
        StartCoroutine(ChangeColor());
        StartCoroutine(KnockBack(pushDir, strength));
        if (health <= 0)
        {
            health = 0;
            // todo trigger death
            if (!isPlayer)
            {
                if (DialogueManager.Instance && DialogueManager.Instance.triggeredTeachQiHealAbilitiesDialogue == false)
                {
                    DialogueManager.Instance.StartTeachQiAbilitiesDialogue();
                    GameManager.Instance.dialogueIsPlaying = true;
                    DialogueManager.Instance.triggeredTeachQiHealAbilitiesDialogue = true;
                    GameManager.Instance.UnlockAbilityIcons();
                    GameManager.Instance.killedFirstHunDun = true;
                }

                gameObject.GetComponent<EnemyController>().state = EnemyState.DEAD;
            }
        }
        if (DialogueManager.Instance && DialogueManager.Instance.triggeredTeachHealthAndQiDialogue == false)
        {
            DialogueManager.Instance.StartTeachHealthAndQiDialogue();
            GameManager.Instance.dialogueIsPlaying = true;
            DialogueManager.Instance.triggeredTeachHealthAndQiDialogue = true;
        }
    }

    private IEnumerator KnockBack(Vector3 pushDir, float strength) {
        if (isKnocking) {
            yield break;}
        
        isKnocking = true;
        float startTime = Time.time;
        Vector3 direction = pushDir.normalized * strength * (1/knockBackRes) * 40;
        direction.y = 0;
        // Debug.Log("Knock back in dir: " + direction + " | pushDir: " + pushDir);

        var startingPos = transform.position;
        while (Time.time - startTime < KnockBackDuration) {
            
            if (!tm.tileExists(transform.position + direction * Time.deltaTime)) {
                // direction *= -1;
                break;
            }
            transform.Translate(direction * Time.deltaTime, Space.World);
            if (maxKnockDistance < Vector3.Distance(startingPos, transform.position)) {
                break;
            }
            yield return null;
        }
        yield return new WaitForSeconds(KnockBackCooldown);
        isKnocking = false;
    }

    private IEnumerator ChangeColor() {
        material.color = Color.red;
        yield return new WaitForSeconds(.2f);
        material.color = color;
    }
}