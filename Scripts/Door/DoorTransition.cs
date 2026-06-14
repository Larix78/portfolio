using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(PlayerInput))]
public class DoorTransition : MonoBehaviour
{
    [Header("Settings")]
    public string DoorId;
    public string TargetScene;
    public float HoldDuration = 3f;
    public GameObject EnterText;

    private bool _isPlayerNear;
    private float _currentHoldTime;

    private void Update()
    {
        if (!_isPlayerNear) return;

        if (Mouse.current.rightButton.isPressed)
        {
            _currentHoldTime += Time.deltaTime;

            if (ProgressBarManager.Instance != null)
                ProgressBarManager.Instance.UpdateProgress(_currentHoldTime / HoldDuration);

            if (_currentHoldTime >= HoldDuration)
                ExecuteTransition();
        }
        else
        {
            _currentHoldTime = 0f;
            if (ProgressBarManager.Instance != null)
                ProgressBarManager.Instance.ResetProgress();
        }
    }

    private void ExecuteTransition()
    {
        // Сохраняем переход
        TransitionSpawnManager.Instance?.SaveTransition(DoorId);

        // Сохраняем инвентарь
        if (InventoryManager.Instance != null)
            InventoryManager.Instance.SaveInventory();

        // Загружаем сцену
        SceneManager.LoadScene(TargetScene);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        _isPlayerNear = true;
        if (EnterText != null)
            EnterText.SetActive(true);
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        _isPlayerNear = false;
        if (EnterText != null)
            EnterText.SetActive(false);

        if (ProgressBarManager.Instance != null)
            ProgressBarManager.Instance.ResetProgress();
    }
}