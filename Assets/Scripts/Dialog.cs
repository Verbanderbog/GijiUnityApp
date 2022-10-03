using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Dialog
{

    [SerializeField] List<LineInfo> lines;
    [SerializeField] List<CharacterArt> participants;
    public List<LineInfo> Lines { get { return lines; } }
    public List<CharacterArt> Participants { get { return participants; } }


}