using UnityEngine;
using UnityEngine.SceneManagement;

public class StartScreen : MonoBehaviour {
    [SerializeField] private GameObject creditsScreen;

    private void Start() {
        // controlsScreen.SetActive(false);
        creditsScreen.SetActive(false);
    }

    public void StartGame() {
        GameManager.Instance.LoadNextLevel();
    }
    
    // public void ShowControls() {
    //     controlsScreen.SetActive(true);
    //     creditsScreen.SetActive(false);
    // }
    //
    // public void HideControls() {
    //     controlsScreen.SetActive(false);
    // }

    public void ShowCredits() {
        creditsScreen.SetActive(true);
    }

    public void HideCredits() {
        creditsScreen.SetActive(false);
    }
    
    public void Quit() {
        Application.Quit();
    }
}
