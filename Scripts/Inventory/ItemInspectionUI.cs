using UnityEngine;
using TMPro;

public class ItemInspectionUI : MonoBehaviour
{
    public static ItemInspectionUI Instance;

    [SerializeField] private GameObject _inspectionPanel;
    [SerializeField] private TextMeshProUGUI _descriptionText;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void ShowDescription(string text) 
    {
        _inspectionPanel.SetActive(true);
        _descriptionText.text = text;
    }

    public void HideDescription() => _inspectionPanel.SetActive(false);
            
}
