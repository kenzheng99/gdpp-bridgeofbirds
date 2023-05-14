using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManagerCutscenes : MonoBehaviour
{
    public static SoundManagerCutscenes Instance;
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource UISFXSource;
    
    //music 
    [SerializeField] private AudioClip cutsceneMusic;

    //ui
    [SerializeField] private AudioClip nextUISFX;
    
    
    public void PlayNextSound()
    {
        UISFXSource.PlayOneShot(nextUISFX);
    }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlayMainMusic()
    {
        musicSource.clip = cutsceneMusic;
        musicSource.Play();
    }

    public void StopMusic()
    {
        musicSource.Stop();
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
        StartCoroutine(FadeAudioSource.StartFade(musicSource, 1.5f,  0));
    }
    

}
