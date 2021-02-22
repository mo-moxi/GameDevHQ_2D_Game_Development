using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Mine_Burst : MonoBehaviour
{
    [SerializeField]
    private float _mineRangeX = 11f;
    private float _mineRangeY  = 5f;
    private float _vectorSpeed = 0.3f;
    [SerializeField]
    private int _mineID;
    [SerializeField]
    private GameObject _explosion;
    
    private void OnEnable()
    {
        _vectorSpeed = _vectorSpeed * Time.deltaTime;
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
        if (transform.position.y < -_mineRangeY || transform.position.x < -_mineRangeX || transform.position.x > _mineRangeX)
        {
            DestroyMine();
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
          //Damamge player on collision
        if (other.tag == "Player")
        {
            other.GetComponent<Player>().Damage();
            Destroy(this.gameObject);
        }
        // Player has 50/50 chance of destroying mine.
        if (other.tag == "Laser" && Random.value > 0.5f || other.tag == "Missile")
        {
            Destroy(other.gameObject);
            DestroyMine();
            AudioManager.Instance.PlayExplosion();
        }
    }
    void DestroyMine()
    {
        Instantiate(_explosion, transform.position, Quaternion.identity);
        AudioManager.Instance.PlayExplosion();
        Destroy(this.gameObject);
    }
}