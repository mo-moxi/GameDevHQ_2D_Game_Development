using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : MonoBehaviour
{
    [SerializeField]
    private float _speed = 12f;
    private float _range = 8f;
    private Transform _target;

    private void Start()
    {
        GameObject[] gameObjects = GameObject.FindGameObjectsWithTag("Enemy");
        if (gameObjects.Length == 0) return;
        var t = Random.Range(0, gameObjects.Length);
        _target = gameObjects[t].transform;
    }
    private void Update()
    {
        if (_target != null)                            // aim for target
        {
            RotateTowards(_target.position);
            MoveTowards(_target.position);
        }
        else                                            // miss-fire: move up
        {
            transform.Translate(Vector3.up * Time.deltaTime * _speed );
            if (transform.position.y > _range || 
            transform.position.x > _range || transform.position.x < -_range)
            Destroy(this.gameObject); 
        }
    }
    private void MoveTowards(Vector2 _target)
    {
        transform.position = Vector2.MoveTowards(transform.position, _target, _speed * Time.deltaTime);
    }
    private void RotateTowards(Vector2 _target)
    {
        float offset = -90f;
        var direction = _target - (Vector2)transform.position;
        direction.Normalize();
        var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion targetLocation = Quaternion.Euler(Vector3.forward * (angle + offset));
        transform.rotation = Quaternion.Slerp(transform.rotation, targetLocation, 0.5f);
    }
}