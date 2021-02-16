using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Homing_Missile : MonoBehaviour
{
    [SerializeField]
    public float _speed = 6f;
    [SerializeField]
    private float _lifeDuration = 1f;
    private float _timeToLive;
    [SerializeField]
    public GameObject _explosion;
    private Transform _player;
    private AudioManager _audioManager;

    private void Start()
    {
        _audioManager = GameObject.Find("Audio_Manager").GetComponent<AudioManager>();
        _player = GameObject.Find("Player").GetComponent<Transform>();
        if (_player == null)
        {
            Debug.LogError("Player does not exist!");
        }
        if (_audioManager == null)
        {
            Debug.LogError("Audio Manager does not exist!");
        }
        _timeToLive = Time.time + _lifeDuration;        // missile live duration
    }

    private void Update()
    {
        if (_player != null)                            // check for player
        {
            MoveTowards(_player.position);
            RotateTowards(_player.position);
        }
        if (Time.time > _timeToLive)
        {
            OnEnemyDeath();
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {   
        if (other.tag == "Player")
        {
            other.GetComponent<Player>().Damage();
            OnEnemyDeath();
        }
        if (other.tag == "Laser")
        {
            OnEnemyDeath();
        }
    }
    private void MoveTowards(Vector2 target)
    {
        transform.position = Vector2.MoveTowards(transform.position, target, _speed * Time.deltaTime);
    }
    private void RotateTowards(Vector2 target)
    {
        var offset = -90f;
        Vector2 direction = target - (Vector2)transform.position;
        direction.Normalize();
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(Vector3.forward * (angle + offset));
    }
    private void OnEnemyDeath()
    {   
        Instantiate(_explosion, transform.position, Quaternion.identity);
        _audioManager.PlayExplosion();
        Destroy(this.gameObject);
    }
}
