using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MenuFontSizer : MonoBehaviour
{
    public TextMeshProUGUI[] myTexts;

    void OptimiseTextSizes()
    {

        int maxLength = myTexts[0].text.Length;
        float size = 0;
        foreach (TextMeshProUGUI t in myTexts)
        {
            if (t.text.Length > maxLength)
            {
                maxLength = t.text.Length;
                size = t.fontSize;
            }
        }

        foreach (TextMeshProUGUI t in myTexts)
        {
            t.fontSize = size;
        }

    }
    // Start is called before the first frame update
    void Start()
    {
        OptimiseTextSizes();
    }

    // Update is called once per frame
    void Update()
    {
        OptimiseTextSizes();
    }
}
