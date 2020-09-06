using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager Instance { get; private set; }
    public GameObject evilMinonPrefab;
    private Queue<Plan> evilPlans;
    public EvilMinonAmount[] minionsToSpawn;
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
        foreach (EvilMinon minion in minions)
        {
            if (minion.active)
            {
                Debug.Log(minion.gameObject.name + " executing evil plans");
                minion.ExecutePlan();
                yield return new WaitUntil(() => minion.finishedExecutingPlan);
            }
        }
        Debug.Log("Player turn");
        GameManager.Instance.SetPlayerTurn();
    }

    public void SpawnEvilMinons()
    {
        foreach (EvilMinonAmount minionToSpawn in minionsToSpawn)
        {
            Vector3[] spawnPositions = LevelManager.Instance.tilemap.GetEnemyPositions(minionToSpawn.amount);
            for (int i = 0; i < minionToSpawn.amount; i++)
            {
                Vector3 spawnPosition = new Vector3(spawnPositions[i].x + 0.5f, spawnPositions[i].y);
                GameObject minionObject = Instantiate(evilMinonPrefab, spawnPosition, Quaternion.identity);
                EvilMinon minion = minionObject.GetComponent<EvilMinon>();
                minion.data = minionToSpawn.data;
                minion.tile = LevelManager.Instance.tilemap.grid.GetCellObject(spawnPosition);
                minions.Add(minion);
                minionObject.transform.SetParent(transform);
            }
        }
    }

    [System.Serializable]
    public struct EvilMinonAmount
    {
        public EvilMinonData data;
        public int amount;
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
