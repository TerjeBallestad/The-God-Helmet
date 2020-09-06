using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvilMinon : MonoBehaviour
{
    public EvilMinonData data;
    public bool active = false;
    private SpriteRenderer spriteRenderer;
    ArcLineCalculator projectileShooter;
    public GameTile tile;
    int searchRadius = 6;
    public bool finishedExecutingPlan = false;
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

        Debug.Log("I'm active " + gameObject.name);
    }

    public void CalculatePlan()
    {

    }

    public void SearchForClosestMinion()
    {
        for (int i = 0; i < searchRadius; i++)
        {
            // top
            for (int x = tile.x - i; x < tile.x + i; x++)
            {

            }
            for (int y = tile.y - i; y < tile.y + i; y++)
            {

            }
        }
    }

    public void ExecutePlan()
    {
        GameManager.Instance.SetCameraFollow(transform);
        StartCoroutine(Plan());
    }

    IEnumerator Plan()
    {
        finishedExecutingPlan = false;
        yield return new WaitForSeconds(0.5f);
        if (MinionManager.Instance.activeMinion != null)
        {
            Projectile projectile = projectileShooter.ShootProjectile(MinionManager.Instance.activeMinion.transform.position);
            if (projectile)
            {
                GameManager.Instance.SetCameraFollow(projectile.transform);
                yield return new WaitWhile(() => projectile.active);
                GameManager.Instance.SetCameraFollow(transform);
                Destroy(projectile.gameObject);
            }
        }
        yield return new WaitForSeconds(2f);
        finishedExecutingPlan = true;
    }
}
