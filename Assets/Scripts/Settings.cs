using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
    [SerializeField] GameObject settingsPanel;
    [SerializeField] Slider zoomSensitivitySlider;
    [SerializeField] Slider pointerSensitivityXSlider;
    [SerializeField] Slider pointerSensitivityYSlider;
    [SerializeField] TextMeshProUGUI zoomSensivityValue;
    [SerializeField] TextMeshProUGUI pointerSensitivityXValue;
    [SerializeField] TextMeshProUGUI pointerSensitivityYValue;
    [SerializeField] MoveAroundObject moveAroundObject;

    void Start()
    {
        LoadCurrentSettings();
        UpdateZoomSensivity();
        UpdatePointerSensitivityX();
        UpdatePointerSensitivityY();
    }

    void LoadCurrentSettings()
    {
        zoomSensitivitySlider.value = PlayerPrefs.GetFloat("zoomSensivity", 1);
        pointerSensitivityXSlider.value = PlayerPrefs.GetFloat("pointerSensitivityX", 1);
        pointerSensitivityYSlider.value = PlayerPrefs.GetFloat("pointerSensitivityY", -1);
    }

    public void UpdateZoomSensivity()
    {
        float value = Mathf.Round(zoomSensitivitySlider.value * 100) / 100;
        zoomSensivityValue.text = value.ToString();
        if (moveAroundObject != null) moveAroundObject.zoomSensivity = value;
        PlayerPrefs.SetFloat("zoomSensivity", value);
    }

    public void UpdatePointerSensitivityX()
    {
        float value = Mathf.Round(pointerSensitivityXSlider.value * 100) / 100;
        pointerSensitivityXValue.text = value.ToString();
        if (moveAroundObject != null) moveAroundObject.pointerSensivityX = value;
        PlayerPrefs.SetFloat("pointerSensitivityX", value);
    }

    public void UpdatePointerSensitivityY()
    {
        float value = Mathf.Round(pointerSensitivityYSlider.value * 100) / 100;
        pointerSensitivityYValue.text = value.ToString();
        if (moveAroundObject != null) moveAroundObject.pointerSensivityY = value;
        PlayerPrefs.SetFloat("pointerSensitivityY", value);
    }

    public void ShowSettings()
    {
        settingsPanel.SetActive(true);
    }

    public void HideSettings()
    {
        settingsPanel.SetActive(false);
    }
}
