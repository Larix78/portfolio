using UnityEngine;

public class PlayerSpawnController : MonoBehaviour
{
    [SerializeField] private CharacterController _characterController; // Ссылка на CharacterController
    [SerializeField] private float _spawnDelay = 0.1f; // Короткая задержка



    private void Start()
    {
        // Если мы в редакторе и только что вышли из Play Mode - сбросить позицию
        if (UnityEditor.EditorApplication.isPlayingOrWillChangePlaymode == false)
        {
            transform.position = Vector3.zero; // Или дефолтная точка спавна
            Debug.Log("Режим редактора: позиция игрока сброшена");
            return;
        }


        if (_characterController == null)
            _characterController = GetComponent<CharacterController>();

        Invoke("DelayedSpawn", _spawnDelay); // Добавляем небольшую задержку
    }

    private void DelayedSpawn()
    {
        if (TransitionSpawnManager.Instance == null)
        {
            Debug.LogWarning("No TransitionSpawnManager found");
            return;
        }

        var spawnPosition = TransitionSpawnManager.Instance.GetSpawnPosition();
        if (spawnPosition.HasValue)
        {
            // Отключаем CharacterController перед перемещением
            if (_characterController != null)
                _characterController.enabled = false;

            transform.position = spawnPosition.Value;
            Debug.Log($"Player spawned at: {spawnPosition.Value}");

            // Включаем обратно после перемещения
            if (_characterController != null)
                _characterController.enabled = true;
        }
        else
        {
            Debug.Log("Using default spawn position");
        }
    }
}