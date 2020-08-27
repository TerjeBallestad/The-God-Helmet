using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyActivator : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {

        EvilMinon enemy = other.gameObject.GetComponent<EvilMinon>();

        if (enemy)
            enemy.ActivateMinion();
    }
}
