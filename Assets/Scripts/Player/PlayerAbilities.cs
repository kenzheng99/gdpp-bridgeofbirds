using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class PlayerAbilities : MonoBehaviour {
    
    [SerializeField] public float healInterval = .3f;
    [SerializeField] public float healQiRequired = 30f;
    [SerializeField] public float healAmount = 1;
    [SerializeField] public float healDuration = 3;
    [SerializeField] public float healCooldown = 12;
    [SerializeField] public float healRange = 3;
    
    private PlayerController controller;
    private AttributesManager atm;
    
    private GameObject normalAtk;
    public GameObject projPrefab;
    private GameObject projObj;
    public GameObject ultPrefab;
    private GameObject ultObj;
    public GameObject gourdPrefab;
    private GameObject gourdObj;
    private GameObject rangeAtkPreview;
    public HashSet<GameObject> enemies = new HashSet<GameObject>();

    [SerializeField] private float projectileSpeed = 1;
    [SerializeField] private float projectileDistance = 1;
    [SerializeField] private float projectilePauseDuration = .2f;
    private Vector3 projectileDir;
    private Vector3 projSpawnPos;
    private bool projectileOnField;
    private bool projectileReturn;
    private float projSpawnTime;
    private float projTimeOut = 4f;
    
    [SerializeField] private Animator anim;
    [SerializeField] private Animator stickAnim;

    public bool healing;
    private float startHealTime;
    
    // Start is called before the first frame update
    private void Start() {
        controller = gameObject.GetComponent<PlayerController>();
        atm = gameObject.GetComponent<AttributesManager>();
        
        normalAtk = transform.Find("NormalAtkHitbox").gameObject;
        rangeAtkPreview = transform.Find("RangeAtkPreview").gameObject;

        startHealTime = Time.time - healCooldown; // to allow heal at beginning of level
    }

    // Update is called once per frame
    private void Update()
    {
        if (!projectileOnField && controller.RangeAttacking) {
            rangeAtkPreview.SetActive(true);
        }
        else {
            rangeAtkPreview.SetActive(false);
        }

        if (controller.NormalAttacking) {
            normalAtk.transform.Find("NormalAtkVisualizer").gameObject.SetActive(true);
        }
        else {
            normalAtk.transform.Find("NormalAtkVisualizer").gameObject.SetActive(false);
        }

        if (projectileOnField && Time.time > projSpawnTime + projTimeOut) {
            Debug.Log("destroy gameobj");
            despawnProjectile();
            StopCoroutine(FireProjectile());
            StopCoroutine(ReturnProjectile());
        }
    }

    #region attacks
    public void DoNormalAtk() {
        anim.SetTrigger("meleeAttack");
        stickAnim.Play("StickMelee");
        SoundManager.Instance.PlayPlayerMeleeAtkSound();
        
        foreach (var enemy in enemies) {
            if (enemy != null) {
                enemy.GetComponent<AttributesManager>().TakeDamage(atm.attack, transform.forward, atm.knockBackStrength);
                controller.GainQi(3);
            }
        }
    }

    public void DoRangeAtk() {
        foreach (var enemy in enemies) {
            if (enemy != null) {
                var knockDir = projObj.transform.forward;
                if (projectileReturn) { // reverse knock direction on the return
                    knockDir *= -1;
                }
                
                enemy.GetComponent<AttributesManager>().TakeDamage(atm.attack/2, knockDir, atm.knockBackStrength);
                controller.GainQi(3);
            }
        }
    }

    public IEnumerator FireProjectile() {
        if (projectileOnField) {
            Debug.Log("Cannot have 2 projectiles");
            yield break;
        }
        projectileOnField = true;
        SoundManager.Instance.PlayPlayerRangedAtkSound();
        anim.SetTrigger("rangedAttack");
        stickAnim.gameObject.SetActive(false);
        StartCoroutine(HideStick());
        
        projObj = Instantiate(projPrefab, transform.position, transform.rotation);
        projectileDir = controller.CursorDir3.normalized;
        projSpawnPos = projObj.transform.position;
        projSpawnTime = Time.time;
        for (;;) {
            if (projectileDistance > Vector3.Distance(projSpawnPos, projObj.transform.position)) {
                projObj.transform.position += Time.deltaTime * projectileSpeed * projectileDir;
            }
            else {
                break;
            }
            yield return null;
        }
        StartCoroutine(ReturnProjectile());
    }
    private IEnumerator ReturnProjectile() {
        yield return new WaitForSeconds(projectilePauseDuration);
        projectileReturn = true;
        while (projectileOnField) {
            var returnDir = (transform.position - projObj.transform.position).normalized;
            projObj.transform.position += Time.deltaTime * projectileSpeed * returnDir;
                
            // Destroy projectile when back to player
            if (.5f > Vector3.Distance(projObj.transform.position, transform.position)) {
                despawnProjectile();
                yield break;
            }
            yield return null;
        }
    }

    private void despawnProjectile() {
        StartCoroutine(projObj.GetComponent<AtkHitbox>().DestroyHitBox());
        projectileOnField = false;
        projectileReturn = false;
    }
    private IEnumerator HideStick() {
        yield return new WaitForSeconds(0.8f);
        stickAnim.gameObject.SetActive(true);
    }

    #endregion attacks
    

    public IEnumerator Heal() {
        if (atm.qi < healQiRequired || gourdObj is not null || Time.time - startHealTime < healCooldown) {
            Debug.Log("Can't heal right now");
            yield break;
        }

        var pos = transform.position;
        pos.y += 0.5f;

        gourdObj = Instantiate(gourdPrefab, pos, transform.rotation);
        
        startHealTime = Time.time;
        atm.qi -= healQiRequired;
        while (Time.time - startHealTime < healDuration) {
            float playerDistance = Vector3.Distance (transform.position, gourdObj.transform.position);
            if (playerDistance < healRange) {
                atm.health += healAmount;
                yield return new WaitForSeconds(healInterval);
                if (atm.health > atm.maxHealth) {
                    atm.health = atm.maxHealth;
                }
            }
            yield return null;
        }
        Debug.Log("destroy gourd");
        Destroy(gourdObj);
    }

    public IEnumerator Ultimate(Vector3 mousePos) {
        if (atm.qi > atm.maxQi - 0.1) {
            // math to account for floating point loss
            SoundManager.Instance.PlayUsedUltimateSFX();
            atm.qi = 0;
            ultObj = Instantiate(ultPrefab, mousePos, transform.rotation);

            var ultStartTime = Time.time;
            while (true) {
                // do dammage
                IEnumerable<GameObject> nonNullNonEnemies = enemies.Where(e => e != null);
                if (nonNullNonEnemies != null && nonNullNonEnemies.Any()) {
                    var rnd = Random.Range(0, nonNullNonEnemies.Count());
                    var enemy = nonNullNonEnemies.ElementAt(rnd);
                    Debug.Log(rnd + " " + enemy);
                    enemy.GetComponent<AttributesManager>().TakeDamage(atm.ultDmgPerHit, transform.forward, 0);
                }

                // Debug.Log("ulting");
                yield return new WaitForSeconds(atm.ultIntervalSecs);
                if (Time.time > ultStartTime + atm.ultDurationSecs) {
                    break;
                }
            }
            StartCoroutine(ultObj.GetComponent<AtkHitbox>().DestroyHitBox());
        }
    }
}
