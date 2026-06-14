using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float _speed = 50f;
    [SerializeField] private float _maxLifetime = 2f;
    [SerializeField] private LayerMask _collisionMask;
    [SerializeField] private GameObject _impactEffect;

    private float _currentLifetime;

    private void Start()
    {
        _currentLifetime = _maxLifetime;
    }

    private void Update()
    {
        MoveBullet();
        CheckCollision();
        UpdateLifetime();
    }

    private void MoveBullet()
    {
        transform.Translate(Vector3.forward * _speed * Time.deltaTime);
    }

    private void CheckCollision()
    {
        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit,
            _speed * Time.deltaTime + 0.1f, _collisionMask))
        {
            Instantiate(_impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
            Destroy(gameObject);
        }
    }

    private void UpdateLifetime()
    {
        _currentLifetime -= Time.deltaTime;
        if (_currentLifetime <= 0)
            Destroy(gameObject);
    }
}