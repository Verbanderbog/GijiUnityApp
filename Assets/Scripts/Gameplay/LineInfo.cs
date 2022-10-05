using UnityEngine;


[System.Serializable]
public class LineInfo
{
    [SerializeField] string line;
    [SerializeField] int participantIndex = -1;
    [SerializeField] Reaction reaction;

    public string Line { get { return line; } }
    public int ParticipantIndex { get { return participantIndex; } }
    public Reaction Reaction { get { return reaction; } }
}


