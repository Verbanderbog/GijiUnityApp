using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MovementPath
{
    public bool loop;
    public bool blockedByDialog;
    public List<PathAction> actions;
    private int currentPathAction;

    public bool hasNext()
    {
        return (loop && actions.Count > 0) || currentPathAction < actions.Count ;
    }
    public PathAction next()
    {
        var retAction = actions[currentPathAction];
        currentPathAction++;
        if (loop && currentPathAction >= actions.Count)
            currentPathAction = 0;
        return retAction;
    }
}