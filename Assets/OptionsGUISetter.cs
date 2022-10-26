using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionsGUISetter : MonoBehaviour
{
    private void OnEnable()
    {
        OptionsController.i.LoadOptions();
    }
}
