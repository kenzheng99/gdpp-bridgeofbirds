using UnityEngine;

public class MagpieCage : MonoBehaviour {
    
    [SerializeField] private float triggerDistance;
    [SerializeField] private GameObject lockedModel;
    [SerializeField] private GameObject unlockedModel;
    [SerializeField] private Magpie magpie;
    private MagpieController magpieController;
    private PlayerController player;
    private bool unlocked;
    void Start() {
        player = PlayerController.Instance;
        magpieController = player.GetComponent<MagpieController>();
        unlocked = false;
        lockedModel.SetActive(true);
        unlockedModel.SetActive(false);
    }

    void Update() {
        if (unlocked) {
            return;
        }
        if (Input.GetKeyDown(KeyCode.E) || Input.GetMouseButtonDown(0)) { // E or left click
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
        magpieController.AddMagpie(magpie);
        magpie.StartFloating();
    }
}
