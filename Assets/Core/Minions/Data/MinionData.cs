using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MinionData", menuName = "Data/MinionData")]
public class MinionData : ScriptableObject
{
    public int Health;
    public int Actions;
    public int Damage;
    public int Steps;
    public Sprite sprite;
    public Vector3 HPandDMGposition;
}
