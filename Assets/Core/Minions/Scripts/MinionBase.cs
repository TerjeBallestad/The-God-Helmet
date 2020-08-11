using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinionBase : MonoBehaviour
{
    public MinionData data;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = data.sprite;
    }
}
