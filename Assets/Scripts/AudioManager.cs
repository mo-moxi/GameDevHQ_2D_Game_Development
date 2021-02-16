using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{ 
  [SerializeField]
  private float _audioDuration = 2.7f;
  [SerializeField]
  private AudioClip _explosion, _powerup, _powerdown;
  private AudioSource _audio;
 
  private void Start()
  {
    _audio = GetComponent<AudioSource>();
    if(_audio == null)
    {
    Debug.LogError("Audio manager is null");
    }
  }
  public void PlayExplosion()
    {
      _audio.PlayOneShot(_explosion, _audioDuration);
    }
  public void PlayPowerUp()
    {
      _audio.PlayOneShot(_powerup, _audioDuration);
    }
  public void PlayPowerDown()
    {
      _audio.PlayOneShot(_powerdown, _audioDuration);
    }
}
