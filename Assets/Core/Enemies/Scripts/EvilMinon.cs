using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvilMinon : MonoBehaviour
{
    public EvilMinonData data;
    public bool active = false;
    private SpriteRenderer spriteRenderer;
    ArcLineCalculator projectileShooter;
    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        projectileShooter = GetComponent<ArcLineCalculator>();
        spriteRenderer.sprite = data.Sprite;
        name = data.name;
    }
    public void ActivateMinion()
    {
        active = true;
        if (MinionManager.Instance.activeMinion != null)
        {
            projectileShooter.ShootProjectile(MinionManager.Instance.activeMinion.transform.position);
        }
        Debug.Log("I'm active " + gameObject.name);
    }
}
