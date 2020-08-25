using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "CardData", menuName = "Data/CardData")]
public class CardData : ScriptableObject
{
    public int Damage;
    public int RopeLength;
    public MinionData Minion;
    public Card.Type type;
}

