using UnityEngine;
using UnityEngine.UI;

public class QiBar: MonoBehaviour {
    [SerializeField] private Image fill;
    private AttributesManager playerAttributes;

    void Start() {
        playerAttributes = PlayerController.Instance.GetComponent<AttributesManager>();
    }

    void Update() {
        fill.fillAmount = playerAttributes.qi / 100.0f;
    }
}
