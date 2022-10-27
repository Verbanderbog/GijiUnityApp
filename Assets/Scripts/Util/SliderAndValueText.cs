using TMPro;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class SliderAndValueText
{
    public Slider slider;
    public TextMeshProUGUI text;
    
    public void SetText()
    {
        if (text != null)
        {
            text.SetText(NormalizeAndRound(slider.value).ToString());
        }

        AudioController.i.SetVolume(slider);
    }

    private int NormalizeAndRound(float val)
    {
        return Mathf.RoundToInt(((val - Mathf.Epsilon) / (1 - Mathf.Epsilon))*100f);
    }
}