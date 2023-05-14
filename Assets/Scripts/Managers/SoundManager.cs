using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource UISFXSource;
    [SerializeField] private AudioSource worldSFXSource;
    [SerializeField] private AudioSource playerSFXSource;
    [SerializeField] private AudioSource ambienceSource;
    
    //music 
    [SerializeField] private AudioClip mainMusic;
    [SerializeField] private AudioClip hubMusic;
    
    
    //world
    [SerializeField] private AudioClip enemyHitSFX;
    [SerializeField] private AudioClip enemyDeathSFX;
    [SerializeField] private AudioClip unlockedMagpieCageSFX;
    
    //ui
    [SerializeField] private AudioClip buttonSelectUISFX;
    [SerializeField] private AudioClip dialogueNextUISFX;

    //player
    [SerializeField] private AudioClip playerMeleeAtkSFX;
    [SerializeField] private AudioClip playerRangedAtkSFX;
    [SerializeField] private AudioClip playerDashSFX;
    [SerializeField] private AudioClip playerAttackLanded1;
    [SerializeField] private AudioClip playerAttackLanded2;
    [SerializeField] private AudioClip placedHealingGourdSFX;
    [SerializeField] private AudioClip usedUltimateSFX;
    [SerializeField] private AudioClip playerHitSFX;
    
    
    //ambience
    public AudioClip mainAmbience;
    public AudioClip hubAmbience;
    
    private void Start()
    {
    
    }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlayMainMusic()
    {
        musicSource.clip = mainMusic;
        musicSource.Play();
    }

    public void StopMusic()
    {
        musicSource.Stop();
    }
    public void ApplyDeathFilter()
    {
        musicSource.GetComponent<AudioReverbFilter>().enabled = true;
        musicSource.GetComponent<AudioLowPassFilter>().enabled = true;
        musicSource.pitch = 0.85f;
        musicSource.volume = 0.425f;
        ambienceSource.GetComponent<AudioReverbFilter>().enabled = true;
        ambienceSource.GetComponent<AudioLowPassFilter>().enabled = true;
    }

    public void LowerAmbienceVolume()
    {
        ambienceSource.volume = .535f;
    }
    
    public void RemoveDeathFilter()
    {
        musicSource.GetComponent<AudioReverbFilter>().enabled = false;
        musicSource.GetComponent<AudioLowPassFilter>().enabled = false;
        musicSource.pitch = 1;
        musicSource.volume = 0.5f;
        
        ambienceSource.GetComponent<AudioReverbFilter>().enabled = false;
        ambienceSource.GetComponent<AudioLowPassFilter>().enabled = false;
    }

   public void PlayButtonSelectSound()
    {
        UISFXSource.clip = buttonSelectUISFX;
        UISFXSource.Play();
    }

   public void PlayDialogueStartSound()
   {
       UISFXSource.PlayOneShot(buttonSelectUISFX);
   }

   public void PlayPlayerMeleeAtkSound()
   {
       playerSFXSource.clip = playerMeleeAtkSFX;
       playerSFXSource.PlayOneShot(playerMeleeAtkSFX);
   }
   public void PlayPlayerRangedAtkSound()
   {
       playerSFXSource.clip = playerRangedAtkSFX;
       playerSFXSource.PlayOneShot(playerRangedAtkSFX);
   }

   public void PlayPlayerDashSound()
   {
        playerSFXSource.PlayOneShot(playerDashSFX);
   }
    
   public void PlayPlayerHitSound()
   {
       playerSFXSource.clip = playerHitSFX;
       playerSFXSource.PlayOneShot(playerHitSFX);
   }

   public void PlayPlacedHealingGourdSFX()
   {
       playerSFXSource.PlayOneShot(placedHealingGourdSFX);
   }

   public void PlayUsedUltimateSFX()
   {
       playerSFXSource.clip = usedUltimateSFX;
       playerSFXSource.PlayOneShot(usedUltimateSFX);
   }
   public void PlayPlayerAttackLandedSFX(int x)
   {
        switch (x)
        {
            case 1:
                playerSFXSource.PlayOneShot(playerAttackLanded1);
                break;
            case 2:
                playerSFXSource.PlayOneShot(playerAttackLanded2);
                break;

        }
    }
   public void PlayEnemyHitSFX()
   {
       worldSFXSource.clip = enemyHitSFX;
       worldSFXSource.PlayOneShot(enemyHitSFX);
   }
   public void PlayUnlockedMagpieCageSFX()
   {
       playerSFXSource.clip = unlockedMagpieCageSFX;
       playerSFXSource.PlayOneShot(unlockedMagpieCageSFX);
   }
   public void PlayEnemyDeathSFX()
   {
       worldSFXSource.clip = enemyDeathSFX;
       worldSFXSource.PlayOneShot(enemyDeathSFX);
   }
    public static class FadeAudioSource {
        public static IEnumerator StartFade(AudioSource audioSource, float duration, float targetVolume)
        {
            float currentTime = 0;
            float start = audioSource.volume;
            while (currentTime < duration)
            {
                currentTime += Time.deltaTime;
                audioSource.volume = Mathf.Lerp(start, targetVolume, currentTime / duration);
                yield return null;
            }
            yield break;
        }
    }
    public void FadeSounds()
    {
        StartCoroutine(FadeAudioSource.StartFade(ambienceSource, 3.5f,  0));
    }
    
    
    
}
