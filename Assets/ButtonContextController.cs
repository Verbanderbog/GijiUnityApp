using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonContextController : MonoBehaviour
{
    [SerializeField] List<ButtonSprites> buttonsList;
    [SerializeField] GameObject giButtonObject;
    [SerializeField] GameObject jiButtonObject;
    Dictionary<string, ButtonSprites> buttons;
    Button giButton;
    Button jiButton;
    Image giImage;
    Image jiImage;

    string giContext="";
    string jiContext="";


    public static ButtonContextController i { get; private set; }
    public string GiContext { get => giContext; }
    public string JiContext { get => jiContext; }

    private enum ButtonName { Gi, Ji, None}
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
    // Start is called before the first frame update
    void Start()
    {
        buttons = new Dictionary<string, ButtonSprites>();
        foreach(ButtonSprites set in buttonsList)
        {
            buttons.Add(set.name, set);
        }
        giButton = giButtonObject.GetComponent<Button>();
        jiButton = jiButtonObject.GetComponent<Button>();
        giImage = giButtonObject.GetComponent<Image>();
        jiImage = jiButtonObject.GetComponent<Image>();

        
    }


    private void ApplySprites(ButtonName name, ButtonSprites set)
    {
        if (name == ButtonName.Gi && giImage.sprite != set.unpressedSprite)
        {
            giImage.sprite = set.unpressedSprite;
            SpriteState state = giButton.spriteState;
            state.pressedSprite = set.pressedSprite;
            giButton.spriteState = state;
            giContext = set.name;
        }
        else if (name==ButtonName.Ji && jiImage.sprite != set.unpressedSprite)
        {
            jiImage.sprite = set.unpressedSprite;
            SpriteState state = jiButton.spriteState;
            state.pressedSprite = set.pressedSprite;
            jiButton.spriteState = state;
            jiContext = set.name;
        }



    }



    public void ApplyContext(string context)
    {
        
        ApplySprites(GetContextButton(context), buttons[context]);
    }

    public void UnapplyContext(string context)
    {
        ButtonName button = GetContextButton(context);

        ApplySprites(button, buttons[button.ToString()]);
    }

    private ButtonName GetContextButton(string context)
    {
        return context switch
        {
            "Gi" => ButtonName.Gi,
            "Ji" => ButtonName.Ji,
            "Talk" => ButtonName.Gi,
            "Push" => ButtonName.Gi,
            "Back" => ButtonName.Ji,
            "Inspect" => ButtonName.Gi,
            "Select" => ButtonName.Gi,
            "Skip" => ButtonName.Ji,
            "Next" => ButtonName.Gi,
            _ => ButtonName.None,
        };
    }
}
