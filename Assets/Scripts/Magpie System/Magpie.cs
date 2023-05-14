using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magpie : MonoBehaviour {
    [SerializeField] private FloatingAnimation floatingAnimation;
    [SerializeField] private float moveSpeed = 10;
    [SerializeField] private AudioSource magpieAudioSource;
    [SerializeField] private AudioClip magpieChirp1;
    [SerializeField] private AudioClip magpieChirp2;
    [SerializeField] private AudioClip magpieChirp3;

    public Vector3 targetPosition;

    private void Start() {
        targetPosition = transform.position;
    }
    
    private void Update() {
        if (Vector3.Distance(targetPosition, transform.position) > 0.01f) {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
        } 
    }
    public void StartFloating() {
        floatingAnimation.StartFloating();
        FlyToPosition(new Vector3(transform.position.x, transform.position.y + 1.5f, transform.position.z));
    }

    public void FlyToPosition(Vector3 position)
    {
        PlayMagpieChirp(Random.Range(1, 4));
        targetPosition = position;
    }

    public void PlayMagpieChirp(int x)
    {
        switch (x)
        {
            case 1:
                magpieAudioSource.clip = magpieChirp1;
                break;
            case 2:
                magpieAudioSource.clip = magpieChirp2;
                break;
            case 3:
                magpieAudioSource.clip = magpieChirp3;
                break;
               
        }
        magpieAudioSource.Play();
        
    }
}
