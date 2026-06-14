using UnityEngine;

public class InventoryOpen : MonoBehaviour
{
    [SerializeField] private GameObject _inventoryPanel;
    [SerializeField] private PlayerMovement _playerMovement;
    [SerializeField] private PlayerCrossHair _playerCrossHair;

    private PlayerControls _controls;
    private bool _isInventoryOpen;
    [HideInInspector] public bool CanOpen;

    private void Awake()
    {
        _controls = new PlayerControls();
        _controls.Gameplay.Inventory.performed += _ => ToggleInventory(); // Переключаем по нажатию
    }

    private void ToggleInventory()
    {
        if (!CanOpen) return;

        _isInventoryOpen = !_isInventoryOpen;
        _inventoryPanel.SetActive(_isInventoryOpen);

        // Управление курсором
        Cursor.lockState = _isInventoryOpen ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = _isInventoryOpen;

        // Управление движением и камерой
        if (_playerMovement != null)
        {
            _playerMovement.SetMovementLock(_isInventoryOpen);
            _playerMovement.SetCameraLock(_isInventoryOpen);
        }

        // Отключаем обводку при открытии инвентаря
        if (_isInventoryOpen && _playerCrossHair != null)
        {
            _playerCrossHair.ResetOutline();
        }
    }

    private void OnEnable() => _controls.Gameplay.Enable();
    private void OnDisable() => _controls.Gameplay.Disable();
}
