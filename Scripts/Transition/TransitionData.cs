using UnityEngine;

[CreateAssetMenu(fileName = "TransitionData", menuName = "Game/TransitionData")]
public class TransitionData : ScriptableObject
{
    [System.Serializable]
    public class TransitionInfo 
    {
        public string EnterDoorId;
        public string TargetDoorId;
        public Vector3 SpawnPosition;
    }

    public TransitionInfo[] TransitionList;
}
