using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public Item item;
    [SerializeField] private InventoryOpen _inventoryOpen;

    private void Start()
    {
        if (_inventoryOpen == null)
            _inventoryOpen = FindObjectOfType<InventoryOpen>();
    }

    public void Pickup()
    {
        if (item.IsBackpack)
        {
            // Рюкзак - разблокируем инвентарь
            if (_inventoryOpen != null)
            {
                _inventoryOpen.CanOpen = true;
                Debug.Log("Рюкзак подобран! Инвентарь разблокирован.");
                Destroy(gameObject);
            }
        }
        else if (item.Type == Item.ItemType.Note)
        {
            // Записка - только осмотр, не добавляем в инвентарь
            Debug.Log("Это записка, можно только осмотреть");
            // Не уничтожаем объект - он остается в мире
        }
        else
        {
            // Обычные предметы
            if (InventoryManager.Instance.AddItem(item))
            {
                Destroy(gameObject);
            }
        }
    }

    // Новый метод для проверки, можно ли подобрать
    public bool CanBePickedUp()
    {
        return !item.IsNote; // Записки нельзя подобрать
    }
}