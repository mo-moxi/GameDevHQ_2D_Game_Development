using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{
    [SerializeField]
    private float _speed = 3.0f;
    [SerializeField]
    private int _powerupID;
    [SerializeField]
    private int _shieldUpdate = 3;
    [SerializeField]
    private int _laserRefill = 7;
    private bool _recallActive;
    [SerializeField]
    private Transform _playerTransform;

    private void Start()
    {
    _playerTransform = GameObject.Find("Player").GetComponent<Transform>(); 
    if (_playerTransform == null)
        Debug.Log("Audio Manager is null!");
    }
    private void Update()
    {
        if(_playerTransform == null)
        {
            return;
        }
        if (Input.GetKeyDown(KeyCode.C) && _recallActive != true)
        {
            _recallActive = true;
            StartCoroutine(AutoCollect());
        }
        if (_recallActive == false)
        {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);
        }
        if (transform.position.y < -5.0f)
        {
            Destroy(gameObject);
        } 
    }
    IEnumerator AutoCollect() 
    {
        while(_recallActive == true)
        { 
            if(_playerTransform == null)
        {
        break;
        }
        transform.position = Vector2.MoveTowards(transform.position, _playerTransform.position, _speed * Time.deltaTime);
        yield return null;
        }
    }
    private void OnTriggerEnter2D(Collider2D other) 
    {       if(other.tag == "Enemy_Laser")
                {
                    Destroy(this.gameObject);
                }
            if (other.tag == "Player")
            {
            Player player = other.transform.GetComponent<Player>();
            if(player !=null)                                       // check player is active
            {    
                if (_powerupID == 6)
                {
                    AudioManager.Instance.PlayPowerDown();
                }
                else
                {
                    AudioManager.Instance.PlayPowerUp();
                }
                switch(_powerupID)
                {
                    case 0:
                    player.TripleShotActive();
                    break;
                    case 1:
                    player.SpeedBoostActive();
                    break;
                    case 2:
                    player.ShieldActive(_shieldUpdate);
                    break;
                    case 3:
                    player.Lives(1);
                    break;
                    case 4:
                    player.LaserRefill(_laserRefill);
                    break;
                    case 5:
                    player.LaserBurst();
                    break;
                    case 6:
                    player.LaserRefill(_laserRefill);
                    player.Damage();
                    break;
                    case 7:
                    player.MissileCount();
                    break;
                    default:
                    break;
                }
            }
            Destroy(this.gameObject);
        }
    }
}
