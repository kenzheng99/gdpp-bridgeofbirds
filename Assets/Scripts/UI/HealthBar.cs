using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour {
    [SerializeField] private Image fill;
    private AttributesManager playerAttributes;

    void Start() {
        playerAttributes = PlayerController.Instance.GetComponent<AttributesManager>();
    }

    void Update() {
        fill.fillAmount = playerAttributes.health / 100.0f;
    }
}
