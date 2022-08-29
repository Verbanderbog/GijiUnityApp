using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MenuFontSizer : MonoBehaviour
{
    public TextMeshProUGUI autosizeTemplate;
    public TextMeshProUGUI[] myTexts;

    void OptimiseTextSizes()
    {

        foreach (TextMeshProUGUI t in myTexts)
        {
            t.fontSize = autosizeTemplate.fontSize;
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
