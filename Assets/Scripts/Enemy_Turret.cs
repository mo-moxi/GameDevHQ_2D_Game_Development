using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Turret : MonoBehaviour
{
    [SerializeField]
    private float _proximityDistance = 6f;
    private float _canFire;
    [SerializeField]
    private float _fireDelay = 1.5f;
    [SerializeField]
    private GameObject _homingMissile;
    private Transform _player;
    [SerializeField]
    private AudioClip _missileShot;
    private AudioSource _audio;

    private void Start()
    {
        _audio = GetComponent<AudioSource>();
        _player = GameObject.Find("Player").GetComponent<Transform>();
        if(_player == null)
            Debug.LogError("Player is null");
    }
    private void Update()
    {
    if (_player != null)
    {
        RotateTowards(_player.position);
        var distanceToPlayer = Vector3.Distance(_player.position, this.transform.position);
        if (distanceToPlayer <= _proximityDistance)
        {
            FireRocket();
        }
    }
    }
    private void RotateTowards(Vector2 target)
    {
        float offset = 90f;
        var direction = target - (Vector2)transform.position;
        direction.Normalize();
        var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(Vector3.forward * (angle + offset));
    }
    private void FireRocket()
    {
        if(Time.time > _canFire)
        {
        _canFire = Time.time + _fireDelay;
        Instantiate(_homingMissile, transform.position, transform.rotation);
        _audio.PlayOneShot(_missileShot, 1.4f);
        }
    }
}
