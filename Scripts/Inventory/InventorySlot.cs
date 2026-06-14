using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventorySlot : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private Image _icon;

    public Transform handTransform;
    [SerializeField] private Item _item;
    private GameObject spawnedItem;

    public void SetItem(Item item, int amount)
    {
        if (_icon == null)
        {
            Debug.LogWarning("Icon reference is missing in slot!");
            return;
        }

        if (item == null)
        {
            ClearSlot();
            return;
        }

        _icon.sprite = item.Icon;
        _icon.enabled = true;
        _item = item;
    }

    public void ClearSlot()
    {
        if (_icon != null)
        {
            _icon.enabled = false;
        }
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if (_item == null || _item.ItemInHandPrefab == null || handTransform == null)
        {
            Debug.LogWarning("Missing references in InventorySlot!");
            return;
        }
        ClearHand();

        GameObject newItem = Instantiate(_item.ItemInHandPrefab, handTransform);
        newItem.transform.localPosition = Vector3.zero;
        newItem.transform.localRotation = Quaternion.identity;

        InventoryManager.Instance.currentHandItemName = _item.name;
        InventoryManager.Instance.SaveInventory(); // Не забываем сохранить!

    }
    private void ClearHand()
    {
        // Удаляем все дочерние объекты
        foreach (Transform child in handTransform)
        {
            Destroy(child.gameObject);
        }
        InventoryManager.Instance.currentHandItemName = "";
    }
}