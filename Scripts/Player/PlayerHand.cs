using UnityEngine;

public class PlayerHand : MonoBehaviour
{
    private GameObject currentHandItem; // Текущий предмет в руке

    void Start()
    {
        if (InventoryManager.Instance == null)
        {
            Debug.LogError("InventoryManager.Instance is null!");
            return;
        }

        TryRestoreHandItem();
    }

    void Update()
    {
        // Проверяем наличие предмета в инвентаре каждый кадр (можно оптимизировать)
        CheckItemInInventory();
    }

    /// <summary> Проверяет, есть ли предмет в инвентаре </summary>
    private void CheckItemInInventory()
    {
        if (string.IsNullOrEmpty(InventoryManager.Instance.currentHandItemName))
        {
            ClearHand();
            return;
        }

        // Проверяем, есть ли предмет в инвентаре
        bool itemExists = false;
        foreach (var slot in InventoryManager.Instance.inventoryData.slots)
        {
            if (slot.ItemID == GetItemIDByName(InventoryManager.Instance.currentHandItemName))
            {
                itemExists = true;
                break;
            }
        }

        // Если предмета нет в инвентаре - удаляем из руки
        if (!itemExists)
        {
            Debug.Log($"Item {InventoryManager.Instance.currentHandItemName} not found in inventory - removing from hand");
            ClearHand();
            InventoryManager.Instance.currentHandItemName = "";
            InventoryManager.Instance.SaveInventory();
        }
    }

    /// <summary> Получает ID предмета по его имени </summary>
    private int GetItemIDByName(string itemName)
    {
        foreach (Item item in InventoryManager.Instance.allItems)
        {
            if (item.name == itemName)
            {
                return item.ItemID;
            }
        }
        return 0; // 0 = пустой слот
    }

    public void TryRestoreHandItem()
    {
        if (string.IsNullOrEmpty(InventoryManager.Instance.currentHandItemName))
        {
            Debug.Log("No item to restore in hand.");
            return;
        }

        string prefabPath = "HandItems/" + InventoryManager.Instance.currentHandItemName;
        GameObject handPrefab = Resources.Load<GameObject>(prefabPath);

        if (handPrefab == null)
        {
            Debug.LogError($"Failed to load hand item prefab at path: {prefabPath}");
            return;
        }

        ClearHand();
        currentHandItem = Instantiate(handPrefab, transform);
        currentHandItem.transform.localPosition = Vector3.zero;
        currentHandItem.transform.localRotation = Quaternion.identity;
    }

    public void ClearHand()
    {
        if (currentHandItem != null)
        {
            Destroy(currentHandItem);
            currentHandItem = null;
        }
    }
}