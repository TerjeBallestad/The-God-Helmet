using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Card : MonoBehaviour
{
    private CardData data;
    private TextMeshPro text;
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
    private void Awake()
    {
        text = GetComponentInChildren<TextMeshPro>();
    }

    public void PlayCard()
    {

    }
    public void SetData(CardData data)
    {
        this.data = data;
        text.text = data.name;
    }

}
