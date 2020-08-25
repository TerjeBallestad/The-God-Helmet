using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager Instance { get; private set; }

    private Queue<Plan> evilPlans;
    private List<EvilMinon> minions;

    private void Awake()
    {
        Instance = this;
    }
    public void ExecuteEvilPlans()
    {
        StartCoroutine(EvilPlans());
    }

    public void CalculateEvilPlans()
    {
        evilPlans.Clear();

        foreach (EvilMinon minon in minions)
        {
            evilPlans.Enqueue(new Plan(minon));
        }
    }

    IEnumerator EvilPlans()
    {
        Debug.Log("executing evil plans");
        yield return new WaitForSeconds(2);
        Debug.Log("lol, haha, look at all that evil we just did");
        GameManager.Instance.SetPlayerTurn();
    }

}

public class Plan
{
    EvilMinon minion;

    public Plan(EvilMinon minion)
    {
        this.minion = minion;
    }
}
