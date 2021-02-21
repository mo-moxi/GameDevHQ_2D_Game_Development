using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float _speed = 4.0f;
    private float _canFire;
    [SerializeField]
    private float _fireRate = 2f;
    private float _distanceToPlayer;
    [SerializeField]
    private float _xRange = 8f;
    private float _yRange = 6f;
    private float _destinationX;
    private float _proximityDistance = 4f;
    [SerializeField]
    private int _enemyID;
    private int _enemyWave;
    private bool _dynamicMovement;
    private bool _proximityAlert;
    private bool _playerProximity;
    private bool _enemyShieldActive;
    private Vector3 _startPosition;
    private Vector2 _laserPosition;
    private Vector2 _targetDestination;
    [SerializeField]
    private GameObject _turret;
    [SerializeField]
    private GameObject _enemyShield;
    [SerializeField]
    private GameObject _enemyLaser;
    [SerializeField]
    private GameObject _explosion;
    private AudioManager _audioManager;                 // audio manager
    private UIManager _uiManager;                       // UI Manager
    private Player _player;                             // Player
    private Transform _playerTransform;
    private SpawnManager _spawnManager;
    [SerializeField]
    private AudioClip _laser_shot;
    private AudioSource _audio;

    private void Start()
    {
        _audioManager = GameObject.Find("Audio_Manager").GetComponent<AudioManager>();
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        _playerTransform = GameObject.Find("Player").GetComponent<Transform>();
        _audio = GetComponent<AudioSource>();
        _enemyWave = _spawnManager._enemyWave;
        _destinationX = Random.Range(-_xRange, _xRange);
        if (_spawnManager == null)
            Debug.LogError("Spawn Manager is null");
        if (_audioManager == null)
            Debug.LogError("Audio Manager is null");
        if (_uiManager == null)
            Debug.LogError("UI Manager is null");
        if (_playerTransform == null)
            Debug.LogError("Player is null");
        if (_enemyWave > 2)                         // reset enemy wave to 0 if > 2
        {
            _enemyWave -=3;
        }
        switch (_enemyWave)                         // set enemy preferences
        { 
            case 0:
            _enemyShield.SetActive(false);          // no shields
            _enemyShieldActive = false;
            break;
            case 1:
            _enemyShield.SetActive(true);           // shields
            _enemyShieldActive = true;
            break;
            case 2:
            _dynamicMovement = true;                // change movement type to diagonal
            _enemyShield.SetActive(true);           // shields
            _enemyShieldActive = true;
            _turret.SetActive(true);                // activate turret
            break;
            default:
            _enemyWave = 0;
            break;
        }
    }
    private void Update()
    {  
        if(_spawnManager._enemyWave == 3 && _playerTransform != null)
        {
            PlayerPosition();
        }
        CalculateMovement();
        FireLaser();
    }
    private void CalculateMovement()
    {   
        if (_dynamicMovement == true || _playerProximity == true || _proximityAlert == true)
        {   
            Destination();
            RotateTowards(_targetDestination);
            MoveTowards(_targetDestination);
            RespawnPosition();
        }
        else
        {
            transform.Translate(Vector3.down * _speed * Time.deltaTime);
            RespawnPosition();
        }
    }
    private void PlayerPosition()
    {   
        if (_playerTransform == null)
        {
            return;
        }
        var distanceToPlayer = Vector2.Distance(_playerTransform.position, this.transform.position);
        if(distanceToPlayer <= _proximityDistance)
        {
            _playerProximity = true;
        }
        else
        {
            _playerProximity = false;
        }
    }
    private void Destination()
    { 
        if (_dynamicMovement == true)                    // move enemy across the screen
        {
            _targetDestination = new Vector2(_destinationX, -_yRange);
            return;    
        }
        if (_playerProximity == true && _enemyID == 1 && _playerTransform !=null)  // move the enemy to ram the player
        {
            _targetDestination = new Vector2(_playerTransform.position.x, _playerTransform.position.y);
            return;
        }
        if(_proximityAlert == true)                     // move the enemy to avoid a laser
        {
            var offset = Random.Range(-10f, 10f);
            _targetDestination = new Vector2(transform.position.x + offset, -_yRange);
            _proximityAlert = false;
            return;
        }
    }
    private void FireLaser()
    {   
        if (_dynamicMovement == true || _playerTransform == null)  // disable lasers in dynamic and player death
            return;
        if (Time.time > _canFire)
        {
            _fireRate = Random.Range(0.85f, _fireRate);
            _canFire = Time.time + _fireRate;
            GameObject enemyLaser = Instantiate(_enemyLaser, transform.position, Quaternion.identity);
            Laser[] lasers = enemyLaser.GetComponentsInChildren<Laser>();
            for (int i = 0; i < lasers.Length; i++)
            {
                lasers[i].AssignEnemyLaser();
            }
            _audio.PlayOneShot(_laser_shot, 0.7f);
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {   
        if (other.tag == "Player" || other.tag == "Laser" || other.tag == "Missile")
        {
            if (_enemyShieldActive == true)
            {
                _enemyShield.SetActive(false);
                _enemyShieldActive = false;
                if (other.tag != "Player")
                {
                    Destroy(other.gameObject);
                }
                Explosion();
                return;
            }
            if (other.tag == "Player")
            {
                other.GetComponent<Player>().Damage();
            }
            else if (other.tag != "Player")
            {
                Destroy(other.gameObject);
            }
                Explosion();
                UpdatePlayerScore();
                OnEnemyDeath();
        }
    }
    private void Explosion()
    {
        Instantiate(_explosion, transform.position, Quaternion.identity);
        _audioManager.PlayExplosion();
    }
    private void OnEnemyDeath()
    {   
        Explosion();
        foreach (Transform child in transform)
        {
            GameObject.Destroy(child.gameObject);
        }
        Destroy(this.gameObject);
    }
    private void UpdatePlayerScore()
    {
    var playerScore = Random.Range(10, 40);
    _uiManager.UpdateScore(playerScore);
    }
    private void MoveTowards(Vector2 _target)
    {   
        transform.position = Vector2.MoveTowards(transform.position, _target, _speed * Time.deltaTime);
    }
    private void RotateTowards(Vector2 _target)
    {
        var offset = 90f;
        Vector2 direction = _targetDestination - (Vector2)transform.position;
        direction.Normalize();
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion targetLocation = Quaternion.Euler(Vector3.forward * (angle + offset));
        transform.rotation = Quaternion.Slerp(transform.rotation, targetLocation, 0.5f);
    }
    private void RespawnPosition()
    {      
        if(transform.position.y <= -_yRange || transform.position.x >= _xRange || transform.position.x <= -_xRange)
        {
           _playerProximity = false;
           _proximityAlert = false;
           float startX = Random.Range(-_xRange, _xRange);
           transform.position = new Vector3(startX, _yRange, 0);
           transform.rotation = Quaternion.Euler(Vector3.forward);
        }
    }
    public void ProximityPosition()
    {
        _proximityAlert = true;
    }
}