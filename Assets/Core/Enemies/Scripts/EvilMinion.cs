using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvilMinon : MonoBehaviour
{
    public EvilMinonData data;
    private SpriteRenderer spriteRenderer;
    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = data.Sprite;
        name = data.name;
    }
}
