﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MinionData", menuName = "Data/MinionData")]
public class MinionData : ScriptableObject
{
    public int Health;
    public int Actions;
    public int Damage;
    public int Steps;
    public Vector2 ColliderSize;
    public Sprite Sprite;
    public Vector3 HPandDMGposition;
}
