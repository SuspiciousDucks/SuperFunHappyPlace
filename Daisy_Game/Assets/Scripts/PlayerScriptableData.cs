using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerScriptableData", menuName = "Player/List", order = 1)]
public class PlayerScriptableData : ScriptableObject
{
    public Color PlayerColor;
    public string PlayerName;

    public int Score;
    public int PlayerId;
}
