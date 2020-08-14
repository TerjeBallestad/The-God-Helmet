using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HPandDMGUI : MonoBehaviour
{
    private SpriteRenderer HPRenderer;
    private SpriteRenderer DMGRenderer;
    public Sprite[] numbers;

    private void Awake()
    {
        HPRenderer = transform.GetChild(0).GetChild(0).GetComponent<SpriteRenderer>();
        DMGRenderer = transform.GetChild(1).GetChild(0).GetComponent<SpriteRenderer>();
    }
    public void UpdateHPandDMG(int HP, int DMG)
    {
        HPRenderer.sprite = numbers[HP];
        DMGRenderer.sprite = numbers[DMG];
    }
}
