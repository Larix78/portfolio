using UnityEngine;

public partial class PlayerCrossHair : MonoBehaviour
{
    [Header("���������")]
    [SerializeField] private Camera _playerCamera;
    [SerializeField] private float _maxDistance = 3f;
    [SerializeField] private LayerMask _interactableLayer;
    [SerializeField] private InventoryOpen _inventoryOpen;

    private Outline _lastOutlinedObject;
    [HideInInspector] public ItemInspect LastInspectItem;
    private ItemPickup _lastPickupItem;
    private Backpack _lastBackpack; // Отдельная ссылка на рюкзак

    private void Update()
    {
        Ray ray = _playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

        // ���������� �������, ���� �� � ������ �������
        if (LastInspectItem == null || !LastInspectItem.IsInspecting)
        {
            ResetOutline();
        }

        // ����� �������� ��� ��������������
        if (Physics.Raycast(ray, out hit, _maxDistance, _interactableLayer))
        {
            // ������ ���� �� � ������ �������
            if (LastInspectItem == null || !LastInspectItem.IsInspecting)
            {
                LastInspectItem = hit.collider.GetComponent<ItemInspect>();
                _lastPickupItem = hit.collider.GetComponent<ItemPickup>();

                if (LastInspectItem != null)
                {
                    Outline outline = hit.collider.GetComponent<Outline>();
                    if (outline != null)
                    {
                        outline.OutlineWidth = 10f;
                        _lastOutlinedObject = outline;
                    }
                }
            }
        }

        // ���������� ��������
        if (Input.GetMouseButtonDown(1))
        {
            if (LastInspectItem != null && !LastInspectItem.IsInspecting && Physics.Raycast(ray, out hit, _maxDistance, _interactableLayer))
            {
                // ������ �������
                LastInspectItem.StartInspect();
                ResetOutline();
            }
            else if (_lastPickupItem != null && _lastPickupItem.CanBePickedUp())
            {
                // Подобрать обычный предмет (но не записку)
                _lastPickupItem.Pickup();
                ResetOutline();
            }
            else if (_lastBackpack != null) // ОСОБАЯ ЛОГИКА ДЛЯ РЮКЗАКА
            {
                // Подобрать рюкзак и разблокировать инвентарь
                _inventoryOpen.CanOpen = true;
                Destroy(_lastBackpack.gameObject);
                Debug.Log("Рюкзак подобран! Инвентарь разблокирован.");
                ResetOutline();
            }
        }

        // �������� �� ����� �������
        if (LastInspectItem != null && LastInspectItem.IsInspecting)
        {
            // ������ ��������
            if (Input.GetKeyDown(KeyCode.T) && _lastPickupItem != null)
            {
                _lastPickupItem.Pickup();
                LastInspectItem.EndInspect();
                ResetOutline();
            }
            // ����� �� �������
            else if (Input.GetKeyDown(KeyCode.Q))
            {
                LastInspectItem.EndInspect();
                ResetOutline();
            }
        }
    }

    public void ResetOutline()
    {
        if (_lastOutlinedObject != null)
        {
            _lastOutlinedObject.OutlineWidth = 0f;
            _lastOutlinedObject = null;
        }
    }
}
