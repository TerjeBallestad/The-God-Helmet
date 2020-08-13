using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{
    private CardData data;
    public enum Type
    {
        DirectDamage,
        HealMinion,
        DOT,
        AOE,
        WeaknessDebuff,
        ApplyBlock,
        ApplyArmor,
        SpawnMinion,
        SpawnBase,
        IncreaseMinionHaste,
        IncreaseMinionActions,
        IncreaseMinionHealth,
        IncreaseMinionDamage,
        Tool,
        Weapon,
    }

    public void PlayCard()
    {

    }

}
