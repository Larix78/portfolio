using UnityEngine;

public class DoorOpen : MonoBehaviour
{
    private Animator _anim;

    private void Start()
    {
        _anim = GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<DoorOpener>() != null) 
        {
            _anim.SetTrigger("DoorOpen");
        }
    }
}
