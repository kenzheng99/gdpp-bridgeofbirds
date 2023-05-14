using UnityEngine;
using UnityEngine.UI;

public class BossHealthBar : MonoBehaviour {
    [SerializeField] private Image fill;
    [SerializeField] private AttributesManager attributes;

    void Update() {
        fill.fillAmount = attributes.health / attributes.maxHealth;
    }
}
