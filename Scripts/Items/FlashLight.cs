using UnityEngine;

public class FlashLight : MonoBehaviour
{
    private KeyCode toggleKey = KeyCode.F; // Кнопка переключения
    [SerializeField] private Light flashlightLight; // Для 3D
    private bool _isOn = false; // Текущее состояние

    void Start()
    {
        // Изначально выключаем фонарик
        if (flashlightLight != null)
            flashlightLight.enabled = _isOn;
    }

    void Update()
    {
        // Срабатывает только при нажатии, а не удержании
        if (Input.GetKeyDown(toggleKey))
        {
            _isOn = !_isOn; // Меняем состояние на противоположное

            if (flashlightLight != null)
                flashlightLight.enabled = _isOn;
        }
    }
}
