using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// https://gamedevbeginner.com/how-to-play-audio-in-unity-with-examples/
/// </summary>
public class GunAudioPlayer : MonoBehaviour
{
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip _clip;
    [SerializeField] private float _volume = 1f;

    public void PlaySound()
    {
        _audioSource.PlayOneShot(_clip, _volume);
    }
}
