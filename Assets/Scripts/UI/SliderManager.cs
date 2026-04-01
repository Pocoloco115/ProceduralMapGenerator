using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SliderManager : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private TextMeshProUGUI sliderText;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        sliderText.text = slider.value.ToString("0");
        slider.onValueChanged.AddListener((v) => sliderText.text = v.ToString("0"));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
