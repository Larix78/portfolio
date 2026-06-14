using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    [SerializeField] private Transform slotsParent;
    private List<InventorySlot> slots = new List<InventorySlot>();

    private void OnEnable()
    {
        // ������������� �� ������� ���������� ���������
        InventoryManager.OnInventoryUpdated += UpdateUI;

        // ������������� ������
        if (slots.Count == 0)
        {
            slots = new List<InventorySlot>(slotsParent.GetComponentsInChildren<InventorySlot>(true));
        }

        // ����������� ���������� ��� ���������
        if (InventoryManager.Instance != null)
        {
            InventoryManager.Instance.UpdateInventoryUI();
        }
    }

    private void OnDisable()
    {
        // ������������ �� �������
        InventoryManager.OnInventoryUpdated -= UpdateUI;
    }

    private void UpdateUI(InventoryData data)
    {
        if (slots == null || slots.Count == 0) return;

        for (int i = 0; i < slots.Count; i++)
        {
            if (i < data.slots.Count && data.slots[i].ItemID != 0)
            {
                Item item = InventoryManager.Instance.allItems.Find(x => x.ItemID == data.slots[i].ItemID);
                if (item != null)
                {
                    slots[i].SetItem(item, data.slots[i].Amount);
                    continue;
                }
            }

            slots[i].ClearSlot();
        }
    }
}