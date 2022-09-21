using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SetSliderText : MonoBehaviour
{
    private Slider slider;
    [SerializeField] private TextMeshProUGUI text;
    // Start is called before the first frame update
    void Start()
    {
        slider = GetComponent<Slider>();
        slider.value = PlayerPrefs.GetInt(slider.name);
        slider.onValueChanged.AddListener(this.setText);
        setText(slider.value);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void setText(float value)
    {
        text.SetText(value.ToString());
        PlayerPrefs.SetInt(slider.name,(int)value);
    }
}
