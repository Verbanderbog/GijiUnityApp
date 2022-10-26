using System;
using UnityEngine;

[CreateAssetMenu(fileName = "FootstepsScriptableObject", menuName = "ScriptableObjects/Footsteps")]
public class FootstepSounds : ScriptableObject
{
    [SerializeField] private AudioClip[] audioClips = new AudioClip[Enum.GetValues(typeof(TileType)).Length]; 
    public AudioClip GetClip(TileType tileType)
    {
        return audioClips[(int)tileType];
    }
}