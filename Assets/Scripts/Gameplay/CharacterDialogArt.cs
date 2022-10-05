using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public enum Reaction
{
    Empty=-1,
    Default,
    Happy,
    Sad,
    Angry
}
[CreateAssetMenu(fileName = "CharacterArtScriptableObject", menuName = "ScriptableObjects/CharacterArt")]
public class CharacterDialogArt : ScriptableObject
{
    public string Name;
    private static int reactions = Enum.GetNames(typeof(Reaction)).Length;
    public Sprite[] images=new Sprite[reactions-1];

    private void OnValidate()
    {
        if (images.Length != reactions)
        {
            images = new Sprite[reactions-1];
        }

    }
}

