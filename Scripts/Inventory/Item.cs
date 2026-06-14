using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
public class Item : ScriptableObject
{
    public enum ItemType
    {
        Regular,    // Обычные предметы
        Backpack,   // Рюкзак (особый тип)
        Note        // Записка (только осмотр)
    }

    [Header("Основные настройки")]
    public ItemType Type = ItemType.Regular;
    public string ItemName = "New Item";

    [Header("Для обычных предметов")]
    public Sprite Icon = null;
    public bool IsStackable = false;
    [Range(1, 99)] public int MaxStack = 1;
    public int ItemID;
    public GameObject ItemInHandPrefab;

    [Header("Для всех предметов")]
    [TextArea(3, 10)]
    public string InspectionDescription = "Описание при осмотре";
    public GameObject WorldPrefab;

    // Свойства для удобства
    public bool IsBackpack => Type == ItemType.Backpack;
    public bool IsNote => Type == ItemType.Note;
}
