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
        Debug.Log("executing evil plans");
        yield return new WaitForSeconds(2);
        Debug.Log("lol, haha, look at all that evil we just did");
        GameManager.Instance.SetPlayerTurn();
    }

    public void SpawnEvilMinons()
    {
        foreach (EvilMinonAmount minion in minionsToSpawn)
        {
            Vector3[] spawnPositions = LevelManager.Instance.tilemap.GetEnemyPositions(minion.amount);
            for (int i = 0; i < minion.amount; i++)
            {
                Vector3 spawnPosition = new Vector3(spawnPositions[i].x + 0.5f, spawnPositions[i].y);
                GameObject minonObject = Instantiate(evilMinonPrefab, spawnPosition, Quaternion.identity);
                minonObject.GetComponent<EvilMinon>().data = minion.data;
                minonObject.transform.SetParent(transform);
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
