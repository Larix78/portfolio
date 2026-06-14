using UnityEngine;

public class TransitionSpawnManager : MonoBehaviour
{
    public static TransitionSpawnManager Instance { get; private set; }
    public bool WasLoadedFromTransition { get; private set; } // Был ли переход?

    [SerializeField] private TransitionData _transitionData;
    private string _savedDoorId;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            LoadLastDoor();
            WasLoadedFromTransition = false; // По умолчанию - false
        }
        else
        {
            Destroy(gameObject);
        }

    }

    private void LoadLastDoor()
    {
        _savedDoorId = PlayerPrefs.GetString("LastDoorID", null);
        Debug.Log($"Loaded last door ID: {_savedDoorId}");
    }

    public void SaveTransition(string doorId)
    {
        WasLoadedFromTransition = true; // Теперь это ПЕРЕХОД, а не новый запуск
        _savedDoorId = doorId;
        PlayerPrefs.SetString("LastDoorID", doorId);
        PlayerPrefs.Save();
        Debug.Log($"Saved door ID: {doorId}");
    }

    public Vector3? GetSpawnPosition()
    {
        if (string.IsNullOrEmpty(_savedDoorId))
        {
            Debug.LogWarning("No door ID saved");
            return null;
        }

        if (_transitionData == null)
        {
            Debug.LogError("TransitionData not assigned!");
            return null;
        }

        foreach (var transition in _transitionData.TransitionList)
        {
            if (transition.EnterDoorId == _savedDoorId)
            {
                Debug.Log($"Found spawn position for {_savedDoorId}: {transition.SpawnPosition}");
                return transition.SpawnPosition;
            }
        }

        Debug.LogWarning($"No transition found for door: {_savedDoorId}");
        return null;
    }

}