using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    [Header("Bullet Settings")]
    [SerializeField] private GameObject _bullet;
    [SerializeField] private Transform _shotPoint;
    [SerializeField] private float _fireRate = 0.2f;
    [SerializeField] private GameObject _fireEffect;

    private PlayerControls _controls;
    private float _nextShotTime;
    private bool _isShooting = false;

    private void Awake()
    {
        _controls = new PlayerControls();

        // Подписка на события управления
        _controls.Gameplay.Shot.performed += _ => StartShooting();
        _controls.Gameplay.Shot.canceled += _ => StopShooting();
    }

    private void OnEnable()
    {
        _controls.Gameplay.Enable();
    }

    private void OnDisable()
    {
        // Отписка от событий для предотвращения утечек памяти
        _controls.Gameplay.Shot.performed -= _ => StartShooting();
        _controls.Gameplay.Shot.canceled -= _ => StopShooting();
        _controls.Gameplay.Disable();
    }

    private void Update()
    {
        if (_isShooting && Time.time >= _nextShotTime)
        {
            Fire();
            _nextShotTime = Time.time + _fireRate;
        }
    }

    private void StartShooting()
    {
        _isShooting = true;
        _nextShotTime = Time.time;
    }

    private void StopShooting()
    {
        _isShooting = false;
    }

    private void Fire()
    {
        // Проверка на null для предотвращения исключений
        if (_bullet == null || _shotPoint == null || _fireEffect == null)
        {
            Debug.LogError("Не все объекты назначены в PlayerShooting!");
            return;
        }

        // Создание пули и эффекта выстрела
        Instantiate(_bullet, _shotPoint.position, _shotPoint.rotation);
        Instantiate(_fireEffect, _shotPoint.position, _shotPoint.rotation);
    }

    // Геттер для безопасного доступа к состоянию стрельбы извне
    public bool IsShooting => _isShooting;
}