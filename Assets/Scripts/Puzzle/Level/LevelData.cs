using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = nameof(LevelData), menuName = "Data/" + nameof(LevelData))]
public class LevelData : ScriptableObject
{
    public string sceneName;
    public float timerA, timerB, timerC;
}
