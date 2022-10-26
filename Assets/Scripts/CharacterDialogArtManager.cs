using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CharacterArtManagerScriptableObject", menuName = "ScriptableObjects/CharacterArtManager")]
public class CharacterDialogArtManager : ScriptableObject
{

    public List<CharacterDialogArt> participants;
   
}
