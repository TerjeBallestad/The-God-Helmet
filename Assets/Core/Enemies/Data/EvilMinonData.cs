using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EvilMinonData", menuName = "Data/EvilMinion")]
public class EvilMinonData : ScriptableObject
{
    public int Health;
    public int Actions;
    public int Damage;
    public int Steps;
    public Sprite Sprite;
}

