using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    [SerializeField]
    private float _rotateSpeed = 20.0f;
    [SerializeField]
    private GameObject _explosion;
    private SpawnManager _spawnManager;

    private void Start()
    {
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();

        if (_spawnManager == null)
        {
            Debug.LogError("The Spawn Manager is null");
        }
    }
    private void Update()
    {
        transform.Rotate(Vector3.forward * _rotateSpeed * Time.deltaTime); // rotate asteroid
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Laser")
        {
            Destroy(other.gameObject);
            Instantiate(_explosion, transform.position, Quaternion.identity);
            AudioManager.Instance.PlayExplosion();
            _spawnManager.StartSpawning();
            Destroy(this.gameObject);
        }
    }
}
