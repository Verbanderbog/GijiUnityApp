using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class LineInfo 
{
    [SerializeField] string line;
    [SerializeField] int participantIndex = -1;
    [SerializeField] Reaction reaction;
    [SerializeField] List<CutsceneAnimSet> animSets;
    [SerializeField] bool hideButtons;
    List<CutsceneAnim> anims = new();

    public string Line { get { return line; } }
    public int ParticipantIndex { get { return participantIndex; } }
    public Reaction Reaction { get { return reaction; } }

    public bool HideButtons { get => hideButtons;  }
    public List<CutsceneAnim> Anims { 
        get
        { 
            if (anims.Count == 0 && animSets.Count !=0)
            {
                if (animSets[0].anims.Count !=0)
                {
                    foreach (CutsceneAnimSet set in animSets)
                    {
                        anims.AddRange(set.anims);
                    }
                }
            }

            return anims;
        }  
    }

    public LineInfo(string line,int participantIndex, Reaction reaction)
    {
        this.line = line;
        this.participantIndex = participantIndex;
        this.reaction = reaction;
    }
}


