using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvilMinon : MonoBehaviour
{
    public EvilMinonData data;
    public bool active = false;
    private SpriteRenderer spriteRenderer;
    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = data.Sprite;
        name = data.name;
    }
    public void ActivateMinion()
    {
        active = true;
        Debug.Log("I'm active " + gameObject.name);
    }
}
