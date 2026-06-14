using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKey(KeyCode.G))
        {
            PlayerPrefs.DeleteKey("InventoryData"); // Удаляет только инвентарь
            PlayerPrefs.Save(); // Применяем изменения
        }
    }
}
