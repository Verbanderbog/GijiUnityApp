using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "CutsceneAnimSet", menuName = "ScriptableObjects/CutsceneAnimSet")]
public class CutsceneAnimSet : ScriptableObject
{
    public List<CutsceneAnim> anims;
#if UNITY_EDITOR

    private void OnValidate()
    {
        foreach (CutsceneAnim anim in anims)
        {
            switch (anim.cutsceneType)
            {
                case CutsceneAnimType.StateChange:
                    anim.animName = null;
                    anim.move = Vector3.zero;
                    anim.duration = 0;
                    anim.rotate = 0;
                    anim.zoom = 0;
                    break;
                case CutsceneAnimType.Movement:
                    anim.animName = null;
                    anim.nextAnimName = null;
                    anim.stateIndex = 0;
                    anim.incrementState = true;
                    anim.duration = 0;
                    anim.rotate = 0;
                    anim.zoom = 0;
                    break;
                case CutsceneAnimType.SpecialAnim:
                    anim.stateIndex = 0;
                    anim.incrementState = true;
                    anim.move = Vector3.zero;
                    anim.duration = 0;
                    anim.rotate = 0;
                    anim.zoom = 0;
                    break;
                case CutsceneAnimType.Wait:
                    anim.animName = null;
                    anim.nextAnimName = null;
                    anim.stateIndex = 0;
                    anim.incrementState = true;
                    anim.move = Vector3.zero;
                    anim.rotate = 0;
                    anim.zoom = 0;
                    break;
                case CutsceneAnimType.Camera:
                    anim.animName = null;
                    anim.nextAnimName = null;
                    anim.stateIndex = 0;
                    anim.incrementState = true;
                    break;
            }
        }
        

    }
#endif
}