using UnityEngine;

public class ItemInspect : MonoBehaviour
{
   [Header("Настройки")]
    public float rotationSpeed = 3f;
    
    private Vector3 _originalPosition;
    private Quaternion _originalRotation;
    public bool IsInspecting = false;
    private PlayerMovement _playerMovement;

    void Start()
    {
        _originalPosition = transform.position;
        _originalRotation = transform.rotation;
        _playerMovement = FindObjectOfType<PlayerMovement>();
        
        if (!TryGetComponent<Rigidbody>(out var rb))
        {
            rb = gameObject.AddComponent<Rigidbody>();
            rb.isKinematic = true;
        }
    }

    void Update()
    {
        if (!IsInspecting) return;

        // Вращение предмета
        if (Input.GetMouseButton(0))
        {
            float X = Input.GetAxis("Mouse X") * rotationSpeed;
            float Y = Input.GetAxis("Mouse Y") * rotationSpeed;
            transform.Rotate(Vector3.up, -X, Space.World);
            transform.Rotate(Vector3.right, Y, Space.World);
        }
    }

    public void StartInspect()
    {
        IsInspecting = true;
        GetComponent<Rigidbody>().isKinematic = true;

        // Фиксируем позицию и родителя
        _originalPosition = transform.position;
        _originalRotation = transform.rotation;
        transform.position = Camera.main.transform.position + Camera.main.transform.forward * 0.5f;
        transform.SetParent(Camera.main.transform);

        // Блокируем движение игрока
        _playerMovement.SetMovementLock(true);
        _playerMovement.SetCameraLock(true);

        ItemInspectionUI.Instance.ShowDescription(GetComponent<ItemPickup>().item.InspectionDescription);
    }

    public void EndInspect()
    {
        IsInspecting = false;
        transform.SetParent(null);

        GetComponent<Rigidbody>().isKinematic = false;
        transform.position = _originalPosition;
        transform.rotation = _originalRotation;

        // Разблокируем движение
        _playerMovement.SetMovementLock(false);
        _playerMovement.SetCameraLock(false);

        ItemInspectionUI.Instance.HideDescription();
    }
}
