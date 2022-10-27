using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIControls : MonoBehaviour
{
    [SerializeField] HorizontalLayoutGroup mainControls;
    [SerializeField] HorizontalLayoutGroup topButton;
    [SerializeField] HorizontalLayoutGroup bottomButton;
    [SerializeField] RectTransform startButton;
    [SerializeField] RectTransform startMenu;
    Transform buttons;
    Transform emptyPadding;

    public static UIControls i { get; private set; }

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
        
    }

    private void Start()
    {
        emptyPadding = mainControls.gameObject.transform.GetChild(0);
        buttons = mainControls.gameObject.transform.GetChild(3);
        SetUI();
    }

    public void SetUI()
    {
        var reverse = (PlayerPrefs.GetInt("Southpaw Toggle") != 0);
        var oneHanded = (PlayerPrefs.GetInt("One Hand Toggle") != 0);
        if (!oneHanded)
        {
            emptyPadding.SetAsFirstSibling();
            buttons.SetSiblingIndex(3);
            mainControls.reverseArrangement = reverse;
            topButton.reverseArrangement = reverse;
            bottomButton.reverseArrangement = reverse;
            if (reverse)
            {
                startButton.pivot = new Vector2(1, 1);
                startButton.anchorMin = new Vector2(1, 1);
                startButton.anchorMax = new Vector2(1, 1);

                startMenu.pivot = new Vector2(1, 1);
                startMenu.anchorMin = new Vector2(1, 0);
                startMenu.anchorMax = new Vector2(1, 1);
            }
            else
            {
                startButton.pivot = new Vector2(0, 1);
                startButton.anchorMin = new Vector2(0, 1);
                startButton.anchorMax = new Vector2(0, 1);

                startMenu.pivot = new Vector2(0, 1);
                startMenu.anchorMin = new Vector2(0, 0);
                startMenu.anchorMax = new Vector2(0, 1);
            }
        }
        else
        {
            
            buttons.SetAsFirstSibling();
            emptyPadding.SetSiblingIndex(3);
            mainControls.reverseArrangement = !reverse;
            topButton.reverseArrangement = reverse;
            bottomButton.reverseArrangement = reverse;
            if (!reverse)
            {
                startButton.pivot = new Vector2(1, 1);
                startButton.anchorMin = new Vector2(1, 1);
                startButton.anchorMax = new Vector2(1, 1);

                startMenu.pivot = new Vector2(1, 1);
                startMenu.anchorMin = new Vector2(1, 0);
                startMenu.anchorMax = new Vector2(1, 1);
            }
            else
            {
                startButton.pivot = new Vector2(0, 1);
                startButton.anchorMin = new Vector2(0, 1);
                startButton.anchorMax = new Vector2(0, 1);

                startMenu.pivot = new Vector2(0, 1);
                startMenu.anchorMin = new Vector2(0, 0);
                startMenu.anchorMax = new Vector2(0, 1);
            }
        }
        

    }
}
