using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance { get; private set; }
    public bool IsChangingScene { get; private set; }
    [Header("Settings")]
    public int slotsCount = 20;
    public List<Item> allItems;

    public InventoryData inventoryData;

    // ������� ��� ���������� UI
    public delegate void InventoryUpdateDelegate(InventoryData data);
    public static event InventoryUpdateDelegate OnInventoryUpdated;
    [Header("Hand Item")]
    public string currentHandItemName; // ������ ��������� ���
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;
            InitializeInventory();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // ���������, ���� �� ������� � ��������� ����� ���������������
        if (!string.IsNullOrEmpty(currentHandItemName) && !HasItem(currentHandItemName))
        {
            currentHandItemName = ""; // ����������, ���� �������� ���
            PlayerPrefs.DeleteKey("CurrentHandItem");
        }

        UpdateInventoryUI();
    }

    public bool AddItem(Item item)
    {
        if (item.IsBackpack)
        {
            Debug.LogError("Нельзя добавить рюкзак в инвентарь! Он обрабатывается отдельно.");
            return false;
        }

        // ��������� ����
        if (item.IsStackable)
        {
            foreach (var slot in inventoryData.slots)
            {
                if (slot.ItemID == item.ItemID && slot.Amount < item.MaxStack)
                {
                    slot.Amount++;
                    SaveInventory();
                    UpdateInventoryUI();
                    return true;
                }
            }
        }

        // ���� ������ ����
        foreach (var slot in inventoryData.slots)
        {
            if (slot.ItemID == 0) // 0 = ������ ����
            {
                slot.ItemID = item.ItemID;
                slot.Amount = 1;
                SaveInventory();
                UpdateInventoryUI();
                return true;
            }
        }

        Debug.Log("Инвентарь полон!");
        return false;
    }

    public void UpdateInventoryUI()
    {
        // �������� ������� ���������� ���������
        OnInventoryUpdated?.Invoke(inventoryData);
    }

    private void InitializeInventory()
    {
        inventoryData = new InventoryData();

        // �������� ����������� ������
        if (PlayerPrefs.HasKey("InventoryData"))
        {
            string json = PlayerPrefs.GetString("InventoryData");
            inventoryData = JsonUtility.FromJson<InventoryData>(json);
        }
        else
        {
            // ������������� ������� ���������
            for (int i = 0; i < slotsCount; i++)
            {
                inventoryData.slots.Add(new SlotData());
            }
        }

        // ��������� ����������� ������� � ����
        if (PlayerPrefs.HasKey("CurrentHandItem"))
        {
            currentHandItemName = PlayerPrefs.GetString("CurrentHandItem");
        }
    }

    public void SaveInventory()
    {
        string json = JsonUtility.ToJson(inventoryData);
        PlayerPrefs.SetString("InventoryData", json);

        // ��������� ������� ������� � ����
        PlayerPrefs.SetString("CurrentHandItem", currentHandItemName);

        PlayerPrefs.Save();
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
    }
    public bool HasItem(string itemName)
    {
        return allItems.Exists(item => item.name == itemName);
    }
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    private IEnumerator ResetSceneFlag()
    {
        yield return new WaitForSeconds(0.1f);
        IsChangingScene = false;
    }
    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}