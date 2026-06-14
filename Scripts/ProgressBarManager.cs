using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBarManager : MonoBehaviour
{
    public static ProgressBarManager Instance;

    [Header("Настройки")]
    public Image progressCircle;
    public float currentProgress;
    public bool isActive;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        progressCircle.fillAmount = 0;
        progressCircle.gameObject.SetActive(false);
    }

    public void UpdateProgress(float progress)
    {
        currentProgress = progress;
        progressCircle.fillAmount = progress;
        progressCircle.gameObject.SetActive(true);
    }

    public void ResetProgress()
    {
        currentProgress = 0f;
        progressCircle.fillAmount = 0f;
        progressCircle.gameObject.SetActive(false);
    }
}
