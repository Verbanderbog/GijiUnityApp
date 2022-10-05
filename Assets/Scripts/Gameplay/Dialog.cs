using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Dialog
{

    [SerializeField] List<LineInfo> lines;
    [SerializeField] List<CharacterDialogArt> participants;
    public List<LineInfo> Lines { get { return lines; } }
    public List<CharacterDialogArt> Participants { get { return participants; } }


}