using UnityEngine;

public class BossFightMagpieCage : MonoBehaviour {
    
    [SerializeField] private float triggerDistance;
    [SerializeField] private GameObject lockedModel;
    [SerializeField] private GameObject unlockedModel;
    [SerializeField] private BossFightMagpie bossFightMagpie;
    private PlayerController player;
    private bool unlocked;
    void Start() {
        player = PlayerController.Instance;
        unlocked = false;
        lockedModel.SetActive(true);
        unlockedModel.SetActive(false);
    }

    void Update() {
        if (unlocked) {
            return;
        }
        if (Input.GetKeyDown(KeyCode.E)) {
            float distance = Vector3.Distance(player.transform.position, transform.position);
            if (distance <= triggerDistance) {
                SoundManager.Instance.PlayUnlockedMagpieCageSFX();
                UnlockCage();
            }
        }
    }

    private void UnlockCage() {
        unlocked = true;
        lockedModel.SetActive(false);
        unlockedModel.SetActive(true);
        bossFightMagpie.Attack();
    }
}
