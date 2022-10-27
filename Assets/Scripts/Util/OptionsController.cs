using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OptionsController : MonoBehaviour
{
    [SerializeField] private List<SliderAndValueText> sliders;
    [SerializeField] private TMP_Dropdown textspeedDropdown;
    [SerializeField] private Toggle southpawToggle;
    [SerializeField] private Toggle oneHandToggle;


    public static OptionsController i { get; private set; }

    private void Awake()
    {
        if (i == null)
        {
            i = this;
        }
        else
        {
            DestroyImmediate(this);
        }
        AudioController.i.InitializeMixer();
        foreach (SliderAndValueText st in sliders)
        {
            st.slider.value = PlayerPrefs.GetFloat(st.slider.name);
            if (st.text != null)
            {
                st.slider.onValueChanged.AddListener(delegate { st.SetText(); });
                st.SetText();
            }

        }
        textspeedDropdown.value = PlayerPrefs.GetInt(textspeedDropdown.name);
        textspeedDropdown.onValueChanged.AddListener(e => { PlayerPrefs.SetInt(textspeedDropdown.name, TextspeedIndexToSpeed(textspeedDropdown.value)); });
        southpawToggle.isOn = (PlayerPrefs.GetInt(southpawToggle.name) != 0);
        southpawToggle.onValueChanged.AddListener(e =>
            {
                PlayerPrefs.SetInt(southpawToggle.name, (southpawToggle.isOn) ? 1 : 0);
                if (UIControls.i != null)
                {
                    UIControls.i.SetUI();
                }
            }
            );
        oneHandToggle.isOn = (PlayerPrefs.GetInt(oneHandToggle.name) != 0);
        oneHandToggle.onValueChanged.AddListener(e =>
        {
            PlayerPrefs.SetInt(oneHandToggle.name, (oneHandToggle.isOn) ? 1 : 0);
            if (UIControls.i != null)
            {
                UIControls.i.SetUI();
            }
        }
            );
    }

    private void Start()
    {
        AudioController.i.InitializeMixer();
    }

    public void LoadOptions()
    {
        foreach (SliderAndValueText st in sliders)
        {
            st.slider.value = PlayerPrefs.GetFloat(st.slider.name);
            if (st.text != null)
            {
                st.slider.onValueChanged.AddListener(delegate { st.SetText(); });
                st.SetText();
            }

        }
        textspeedDropdown.value = TextspeedSpeedtoIndex(PlayerPrefs.GetInt(textspeedDropdown.name));
        southpawToggle.isOn = (PlayerPrefs.GetInt(southpawToggle.name) != 0);
    }

    private int TextspeedIndexToSpeed(int dropdown)
    {
        if (dropdown == 0)
        {
            return 25;
        }
        else if (dropdown == 2)
        {
            return 75;
        }
        else
        {
            return 50;
        }
    }

    private int TextspeedSpeedtoIndex(int speed)
    {
        if (speed == 25)
        {
            return 0;
        }
        else if (speed == 75)
        {
            return 2;
        }
        else
        {
            return 1;
        }
    }
}
