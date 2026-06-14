using System;
using System.Collections.Generic;

[System.Serializable]
public class InventoryData
{
    public List<SlotData> slots = new List<SlotData>();
}

[System.Serializable]
public class SlotData
{
    public int ItemID;
    public int Amount;

    public SlotData()
    {
        ItemID = 0;
        Amount = 0;
    }
}