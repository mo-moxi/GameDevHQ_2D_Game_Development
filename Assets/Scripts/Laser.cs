using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField]
    private float _speed = 8f;
    private float _yRange = 8f;
    private bool _isEnemyLaser;
    private bool _isAlive;

    private void Update()
    {
        if (_isEnemyLaser == true)
        {
            MoveDown();
            return;
        }
        else
            MoveUp();
    }
    private void MoveUp()       // move up if player laser
    {   
        transform.Translate(Vector3.up * _speed * Time.deltaTime);
        ObjectRange();
    }
    private void MoveDown()     // move down if enemy laser
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);
        ObjectRange();
    }
    private void OnTriggerEnter2D(Collider2D other) // damage player when enemy laser

    {   
        if (other.tag == "Player" && _isEnemyLaser == true) // damage player
        {
            other.GetComponent<Player>().Damage();
        }
        _isAlive = false;
        ObjectRange();
    }
    public void AssignEnemyLaser()
    {
        _isEnemyLaser = true;
    }
    private void ObjectRange()      // Destroy laser when beyond Y range or in colission.
    {
        if (transform.position.y < -_yRange || transform.position.y > _yRange || _isAlive)
        {
            if (transform.parent != null)
            {
            Destroy(transform.parent.gameObject);
            }
            Destroy(this.gameObject);
        }
    }
}
