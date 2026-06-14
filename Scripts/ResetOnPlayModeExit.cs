#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

[InitializeOnLoad]
public static class ResetOnPlayModeExit
{
    static ResetOnPlayModeExit()
    {
        EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
    }

    private static void OnPlayModeStateChanged(PlayModeStateChange state)
    {
        if (state == PlayModeStateChange.ExitingPlayMode)
        {
            PlayerPrefs.DeleteKey("LastDoorID"); // Удали КЛЮЧ, который сохраняет позицию
            PlayerPrefs.DeleteKey("InventoryData"); // Удаляет только инвентарь
            PlayerPrefs.Save();
            Debug.Log("PlayerPrefs очищены при выходе из Play Mode");
        }
    }
}
#endif
