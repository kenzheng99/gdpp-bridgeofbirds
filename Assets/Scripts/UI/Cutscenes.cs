using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cutscenes : MonoBehaviour
{
    [SerializeField] private List<GameObject> cutscenes;
    // [SerializeField] private Overlay fadeOverlay; 

    private int index; 

    void Start() {
        index = 0;
        UpdateCutscenes();
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.E)) {
            SoundManagerCutscenes.Instance.PlayNextSound();
            index++;
            if (index >= cutscenes.Count) {
                SoundManagerCutscenes.Instance.FadeSounds();
                SoundManager.Instance.PlayMainMusic();
                GameManager.Instance.LoadNextLevel();
            } else {
                UpdateCutscenes();
            }
        }
    }

    private void UpdateCutscenes() {
        for (int i = 0; i < cutscenes.Count; i++) {
            GameObject cutscene = cutscenes[i];
            if (i == index) {
                cutscene.SetActive(true);
            } else {
                cutscene.SetActive(false);
            }
        }
    }
}
