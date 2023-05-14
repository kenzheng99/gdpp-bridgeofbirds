using UnityEngine;

public class LevelTransition : MonoBehaviour
{
    private void OnTriggerEnter(Collider collider) {
        if (collider.gameObject.CompareTag("Player")) {
            GameManager.Instance.LoadNextLevel();
        }
    }
}
