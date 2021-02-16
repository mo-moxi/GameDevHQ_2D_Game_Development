using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Mine_Burst : MonoBehaviour
{
    [SerializeField]
    private float _mineRange = 9f;
    private float _mineRangeY;
    private float _vectorSpeed = 3f;
    [SerializeField]
    private int _mineID;
    [SerializeField]
    private GameObject _explosion;
    private AudioManager _audioManager;

    private void Start()
    {
        _audioManager = GameObject.Find("Audio_Manager").GetComponent<AudioManager>();
        if (_audioManager == null)
            Debug.LogError("Audio Manager is null");
        _vectorSpeed = _vectorSpeed * Time.deltaTime;
        _mineRangeY = Random.Range(1.5f, 5f);
    }
    private void Update()
    {
        switch (_mineID)
        {
            case 0:
                transform.Translate(Vector3.left * _vectorSpeed);
                break;
            case 1:
                transform.Translate(-_vectorSpeed, -_vectorSpeed, 0);
                break;
            case 2:
                transform.Translate(Vector3.down * _vectorSpeed);
                break;
            case 3:
                transform.Translate(_vectorSpeed, -_vectorSpeed, 0);
                break;
            case 4:
                transform.Translate(Vector3.right * _vectorSpeed);
                break;
        }
        if (transform.position.y < -_mineRangeY || transform.position.x < -_mineRange || transform.position.x > _mineRange)
        {
            DestroyMine();
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Hit: " + other.tag);
        //Damamge player on collision
        if (other.tag == "Player")
        {
            other.GetComponent<Player>().Damage();
            Destroy(this.gameObject);
        }
        // Player has 50/50 chance of destroying mine.
        if (other.tag == "Laser" && Random.value > 0.5f || other.tag == "Missile")
        {
            DestroyMine();
        }
    }
    void DestroyMine()
    {
        Instantiate(_explosion, transform.position, Quaternion.identity);
        _audioManager.PlayExplosion();
        Destroy(this.gameObject);
    }
}

