using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;

    public static GameManager Instance => _instance;

    public bool hasMaxQi;
    public bool hasQi;
    public bool unlockedAbilityIcons;
    public bool unlockedUltimate;
    public bool completedTutorial;
    public bool killedFirstHunDun;
    public bool dialogueIsPlaying;
    
    private void Awake() {
        if (_instance != null && _instance != this) {
            Destroy(this.gameObject);
        } else {
            _instance = this;
            DontDestroyOnLoad(this);
        }
    }

    private void Start()
    {
        if (SoundManager.Instance && SceneManager.GetActiveScene().name != "StartScreen" && SceneManager.GetActiveScene().name != "IntroCutscenes") {
            SoundManager.Instance.PlayMainMusic();
        }
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            Application.Quit();
        }
    }

    public void OnPlayerDeath() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void UnlockAbilityIcons()
    {
        unlockedAbilityIcons = true;
    }

    public void LoadNextLevel() {
        SceneManager.LoadScene((SceneManager.GetActiveScene().buildIndex + 1) % SceneManager.sceneCountInBuildSettings);
    }
}
