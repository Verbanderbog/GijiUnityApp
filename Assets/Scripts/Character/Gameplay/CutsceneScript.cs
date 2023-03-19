using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


[CreateAssetMenu(fileName = "CutsceneScriptScriptableObject", menuName = "ScriptableObjects/CutsceneScript")]
public class CutsceneScript : ScriptableObject
{

    [SerializeField] List<LineInfo> lines;
    [SerializeField] List<CharacterDialogArt> participants;
    [SerializeField] TextAsset file;
    [SerializeField] CharacterDialogArtManager allParticipants;
    public List<LineInfo> Lines { get { return lines; } }
    public List<CharacterDialogArt> Participants { get { return participants; } }

    private void OnValidate()
    {
        if (file != null && allParticipants !=null)
        {
            var linesOutput = new List<LineInfo>();
            participants = new List<CharacterDialogArt>();
            var fileLines = file.text.Split('\n');
            var dialogStart = false;
            for (int i=0;i<fileLines.Length;i++)
            {
                //Debug.Log(fileLines[i]);
                if (fileLines[i].Trim() == "//DIALOG START//")
                {
                    dialogStart = true;
                    continue;
                }
                    
                if (dialogStart)
                {
                    Debug.Log("dialog");
                    linesOutput.Add(new LineInfo(fileLines[i].Trim(), int.Parse(fileLines[i + 1].Trim()), Enum.Parse<Reaction>(fileLines[i + 2].Trim())));
                    i += 2;
                }
                else
                {
                    foreach (CharacterDialogArt c in allParticipants.participants)
                    {
                        if (c.name.Equals(fileLines[i].Trim()))
                            participants.Add(c);
                    }
                }
            }
            foreach(string i in fileLines)
            {
                
            }
            
            lines = linesOutput;
            //participants = participantsOutput;
            file = null;
            allParticipants = null;
        }
    }
}