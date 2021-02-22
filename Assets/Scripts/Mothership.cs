using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mothership : MonoBehaviour
{

    [SerializeField]
    private float _speed = 0.5f;
    [SerializeField]
    private float _retreatSpeed = 3.5f;
    private float _destinationY;
    private float _yRange = 4f;
    private float retreatPosition = 12f;
    [SerializeField]
    private int _mothershipLives = 25;
    [SerializeField]
    private int _enemyShieldCount = 50;
    private bool _explodeSequence;
    private bool _moveDown = true;
    private bool _mothershipInDanger;
    [SerializeField]
    private GameObject _explosion;
    [SerializeField]
    private GameObject _enemyShield;
    private UIManager _uiManager;
    private SpawnManager _spawnManager;
    private Vector3 _startPosition;
    private CircleCollider2D _shieldCollider;

    private void Start()
    {
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        _shieldCollider = gameObject.GetComponent<CircleCollider2D>();
        _destinationY = Random.Range(0, _yRange);
        _startPosition = transform.position;

        if (_spawnManager == null)
            Debug.LogError("Audio Manager is null!");
        if (_uiManager == null)
            Debug.LogError("UI Manager is null!");
        if(_shieldCollider !=null)
            Debug.LogError("Shield Collider null!");
    }
    private void Update()
    {
        ShipMovement();                                  // to do: set as coroutine
    }
    private void EndGame()
    {
        _uiManager.PlayerWins();                        // change scene to end title
        _spawnManager.OnPlayerDeath();                  // stop spawning
        this.gameObject.SetActive(false);               // deactivate this object
    }
    private void ShipMovement ()                        // move the mothership up/down ths screen
    {
        if (_mothershipInDanger)                        // test for low life level
        {
            _moveDown = false;                          // cancel moving down
            _startPosition.y = retreatPosition;         // set destination out of fire range
            _speed = _retreatSpeed;                     // retreat speed
        }
        if (transform.position.y >= _destinationY && _moveDown)
        {
            transform.Translate(Vector3.down * _speed * Time.deltaTime);
        }
        if (transform.position.y <= _destinationY && _moveDown)
        {
            _moveDown = false;
        }
        if (_moveDown == false && transform.position.y <= _startPosition.y)
        {
            transform.Translate(Vector3.up * _speed * Time.deltaTime);
        }
        if (transform.position.y >= _startPosition.y && _mothershipInDanger == false)
        {
            _moveDown = true;
        }
        if (transform.position.y >= _startPosition.y && _mothershipInDanger)
        {
            EndGame();
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player" || other.tag == "Laser" || other.tag == "Missile")
        {
            _enemyShieldCount -= 1;
            if (_enemyShieldCount < 1)
            {
                _enemyShield.SetActive(false);
                _shieldCollider.radius = 1f;        // reduce collider to ship body
                DamageMothership();
            }
            if (other.tag == "Player")
                {
                other.GetComponent<Player>().Damage();
                }
            if (other.tag != "Player")
            {
                Destroy(other.gameObject);
            }
                var _explodePoint = other.transform.position;               // get collision point
                Instantiate(_explosion, _explodePoint, Quaternion.identity);
                AudioManager.Instance.PlayExplosion();
        }
    }
    private void UpdatePlayerScore()
    {
        var playerScore = Random.Range(10, 40);
        _uiManager.UpdateScore(playerScore);                    // update UI player score
    }
    private void DamageMothership()
    {
        UpdatePlayerScore();
        _mothershipLives -=1;
        if (_mothershipLives < 15)                              // test for retreat condition
            _mothershipInDanger = true;
        if (_mothershipLives < 1 && _explodeSequence == false)  // call explosion sequence
        {
            _explodeSequence = true;
            StartCoroutine(ExplodeSequence());
        }
    }
    IEnumerator ExplodeSequence()                                // mothership explosion sequence
        {
        AudioManager.Instance.PlayExplosion();
        for (int i = 0; i < 15; i++)
        {
            var xOffset = Random.Range(-2f, 2f);
            var yOffset = Random.Range(-2f, 2f);
            var _explodePoint = new Vector3(transform.position.x + xOffset, transform.position.y + yOffset, 0); 
            Instantiate(_explosion, _explodePoint, Quaternion.identity);
            yield return new WaitForSeconds(.2f);
        }
            EndGame();
        }
}