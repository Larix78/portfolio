using System.Collections;
using UnityEngine;

public class IntroSceneController : MonoBehaviour
{
    [SerializeField] private AudioClip _theSoundOfArrival;
    [SerializeField] private AudioClip _windSound;

    [SerializeField] private AudioSource _audioSource;

    [SerializeField] private GameObject _inputText;
    [SerializeField] private GameObject _blackScreen;

    [SerializeField] bool _isSceneStarted = false;

    private void Start()
    {
        StartCoroutine(PlayIntroSounds());
    }
    private IEnumerator PlayIntroSounds() 
    {
        _audioSource.PlayOneShot(_theSoundOfArrival);
        yield return new WaitForSeconds(_theSoundOfArrival.length);

        _audioSource.clip = _windSound;
        _audioSource.loop = true;
        _audioSource.Play();

        _isSceneStarted = true;
        _inputText.SetActive(true);
    }

    private void Update()
    {
        if (_isSceneStarted && Input.anyKeyDown) 
        {
            StartPlay();
        }
    }

    private void StartPlay() 
    {
        _blackScreen.SetActive(false);
        _inputText.SetActive(false);
    }
}
