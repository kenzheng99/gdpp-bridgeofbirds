using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndingCutscenesManager : MonoBehaviour
{
    void Start()
    {
        if (SoundManager.Instance) {
            SoundManager.Instance.StopMusic();
        }
    }

}
