using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minion : MonoBehaviour
{
    public MinionData data;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = data.Sprite;
        name = data.name;
        HPandDMGUI HPUI = GetComponentInChildren<HPandDMGUI>();
        HPUI.UpdateHPandDMG(data.Health, data.Damage);
        HPUI.transform.localPosition = data.HPandDMGposition;
    }


}
