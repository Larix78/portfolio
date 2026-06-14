using UnityEngine;

public class RadioTuner : MonoBehaviour
{
    [Header("Настройки вращения")]
    [SerializeField] private float _rotationSpeed = 100f; // Скорость в градусах/сек
    [SerializeField] private float _maxAngle = 120f;     // Макс. угол в каждую сторону
    [SerializeField] private KeyCode _rotateRight = KeyCode.C;
    [SerializeField] private KeyCode _rotateLeft = KeyCode.Z;

    [Header("Настройки частот")]
    [SerializeField] private float _minFrequency = 88.0f;  // Минимальная частота (соответствует точке A)
    [SerializeField] private float _maxFrequency = 108.0f; // Максимальная частота (соответствует точке B)
    [SerializeField] private float _currentFrequency = 88.0f; // Текущая частота

    [Header("Точки движения")]
    [SerializeField] private Transform _pointA; // Точка для minFrequency
    [SerializeField] private Transform _pointB; // Точка для maxFrequency
    [SerializeField] private Transform _frequencyIndicator; // Объект-индикатор

    [Header("Ссылки")]
    [SerializeField] private Transform _knobTransform;   // Объект крутилки
    private float _currentRotationZ = 0f; // Текущий угол по Z
    private Quaternion _initialLocalRotation; // Начальное вращение

    void Start()
    {
        // Запоминаем начальное вращение, чтобы избежать дрейфа
        if (_knobTransform != null)
        {
            _initialLocalRotation = _knobTransform.localRotation;
        }
    }

    void Update()
    {
        if (_knobTransform == null) return;

        // Получаем ввод
        float input = 0f;
        if (Input.GetKey(_rotateRight)) input = 1f;
        if (Input.GetKey(_rotateLeft)) input = -1f;

        // Вычисляем новое вращение
        if (input != 0f)
        {
            _currentRotationZ += input * _rotationSpeed * Time.deltaTime;
            _currentRotationZ = Mathf.Clamp(_currentRotationZ, -_maxAngle, _maxAngle);

            // Применяем вращение относительно начального положения
            _knobTransform.localRotation = _initialLocalRotation * Quaternion.Euler(0f, 0f, _currentRotationZ);

            // Обновляем частоту (0-1 диапазон)
            float t = (_currentRotationZ + _maxAngle) / (2f * _maxAngle);
            _currentFrequency = Mathf.Lerp(_minFrequency, _maxFrequency, t);

            MoveIndicator();

            Debug.Log(_currentFrequency);
        }
    }
    void MoveIndicator()
    {
        if (!_pointA || !_pointB || !_frequencyIndicator) return;

        // Вычисляем позицию между точками A и B (0-1 диапазон)
        float t = Mathf.InverseLerp(_minFrequency, _maxFrequency, _currentFrequency);
        _frequencyIndicator.position = Vector3.Lerp(_pointA.position, _pointB.position, t);
    }
}
