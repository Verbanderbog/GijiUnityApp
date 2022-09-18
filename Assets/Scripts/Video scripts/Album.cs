using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "AlbumScriptableObject", menuName = "ScriptableObjects/Album")]
public class Album : ScriptableObject
{
    public int releaseMonth;
    public int releaseDay;
    public int releaseYear;
    public DateTime releaseDate;
    public Sprite albumArt;

    private void OnValidate()
    {
        releaseYear = (releaseYear < 0) ? 0 : releaseYear;
        releaseDay = (releaseDay < 1 || releaseDay > 31) ? 1 : releaseDay;
        releaseMonth = (releaseMonth < 1 || releaseMonth > 12) ? 1 : releaseMonth;
        releaseDate = new DateTime(releaseYear, releaseMonth, releaseDay);
    }
}
