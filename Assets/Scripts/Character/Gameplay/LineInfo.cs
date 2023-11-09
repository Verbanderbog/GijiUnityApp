using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class LineInfo 
{
    [SerializeField] string line;
    [SerializeField] int participantIndex = -1;
    [SerializeField] Reaction reaction;
    [SerializeField] List<CutsceneEventSet> eventSets;
    [SerializeField] bool hideButtons;
    List<CutsceneEvent> events;

    public string Line { get { return line; } }
    public int ParticipantIndex { get { return participantIndex; } }
    public Reaction Reaction { get { return reaction; } }

    public bool HideButtons { get => hideButtons;  }
    public List<CutsceneEvent> Events { 
        get
        {
            if (events == null)
                events = new();
            if (events.Count == 0 && eventSets.Count !=0)
            {
                if (eventSets[0].events.Count !=0)
                {
                    foreach (CutsceneEventSet set in eventSets)
                    {
                        events.AddRange(set.events);
                    }
                }
            }

            return events;
        }  
    }

    public LineInfo(string line,int participantIndex, Reaction reaction)
    {
        this.line = line;
        this.participantIndex = participantIndex;
        this.reaction = reaction;
    }
}


