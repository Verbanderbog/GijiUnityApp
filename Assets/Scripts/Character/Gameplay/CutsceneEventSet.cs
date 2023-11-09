using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "CutsceneEventSet", menuName = "ScriptableObjects/CutsceneEventSet")]
public class CutsceneEventSet : ScriptableObject
{
    public List<CutsceneEvent> events;
#if UNITY_EDITOR

    private void OnValidate()
    {
        foreach (CutsceneEvent cutEvent in events)
        {
            switch (cutEvent.cutsceneType)
            {
                case CutsceneEventType.StateChange:
                    cutEvent.animName = null;
                    cutEvent.move = Vector3.zero;
                    cutEvent.duration = 0;
                    cutEvent.rotate = 0;
                    cutEvent.zoom = 0;
                    cutEvent.flagKey = null;
                    cutEvent.flagValue = 0;
                    break;
                case CutsceneEventType.Movement:
                    cutEvent.animName = null;
                    cutEvent.nextAnimName = null;
                    cutEvent.stateIndex = 0;
                    cutEvent.incrementStateFlag = true;
                    cutEvent.duration = 0;
                    cutEvent.rotate = 0;
                    cutEvent.zoom = 0;
                    cutEvent.flagKey = null;
                    cutEvent.flagValue = 0;
                    break;
                case CutsceneEventType.SpecialAnim:
                    cutEvent.stateIndex = 0;
                    cutEvent.incrementStateFlag = true;
                    cutEvent.move = Vector3.zero;
                    cutEvent.duration = 0;
                    cutEvent.rotate = 0;
                    cutEvent.zoom = 0;
                    cutEvent.flagKey = null;
                    cutEvent.flagValue = 0;
                    break;
                case CutsceneEventType.Wait:
                    cutEvent.animName = null;
                    cutEvent.nextAnimName = null;
                    cutEvent.stateIndex = 0;
                    cutEvent.incrementStateFlag = true;
                    cutEvent.move = Vector3.zero;
                    cutEvent.rotate = 0;
                    cutEvent.zoom = 0;
                    cutEvent.flagKey = null;
                    cutEvent.flagValue = 0;
                    cutEvent.duration = (cutEvent.duration < 0) ? 0 : cutEvent.duration;
                    break;
                case CutsceneEventType.Camera:
                    cutEvent.animName = null;
                    cutEvent.nextAnimName = null;
                    cutEvent.stateIndex = 0;
                    cutEvent.incrementStateFlag = true;
                    cutEvent.flagKey = null;
                    cutEvent.flagValue = 0;
                    if (cutEvent.rotate > 180)
                    {
                        cutEvent.rotate = cutEvent.rotate % 360;
                        if (cutEvent.rotate > 180)
                            cutEvent.rotate = cutEvent.rotate - 360;
                    }
                    if (cutEvent.rotate < -180)
                    {
                        cutEvent.rotate = cutEvent.rotate % -360;
                        if (cutEvent.rotate < -180)
                            cutEvent.rotate = cutEvent.rotate + 360;
                    }
                    cutEvent.duration = (cutEvent.duration < 0) ? 0 : cutEvent.duration;
                    break;
                case CutsceneEventType.FlagChange:
                    cutEvent.animName = null;
                    cutEvent.nextAnimName = null;
                    cutEvent.move = Vector3.zero;
                    cutEvent.duration = 0;
                    cutEvent.rotate = 0;
                    cutEvent.zoom = 0;
                    cutEvent.stateIndex = 0;
                    cutEvent.character = null;
                    break;
            }
        }


    }
#endif
}