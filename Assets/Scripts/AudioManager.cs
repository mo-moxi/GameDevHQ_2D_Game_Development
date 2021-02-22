using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AudioManager : MonoBehaviour
{
  private static AudioManager _instance;

  public static AudioManager Instance
  {
    get
    {
      if(_instance == null)
      {
        Debug.Log("Audio Manager is Null.");
      }
      return _instance;
    }
  }
  void Awake()
  {
    _instance = this;
  }

  [SerializeField]
  private AudioClip explosion, powerup, powerdown;
  [SerializeField]
  private float audioDuration;
  [SerializeField]
  private AudioSource sfxSource;

  public void PlayExplosion()
    {
      sfxSource.PlayOneShot(explosion, audioDuration);
    }
  public void PlayPowerUp()
    {
      sfxSource.PlayOneShot(powerup, audioDuration);
    }
  public void PlayPowerDown()
    {
      sfxSource.PlayOneShot(powerdown, audioDuration);
    }

  private void Update()
  {
    if(Input.GetKeyDown(KeyCode.A))
      PlayExplosion();
  }
}
