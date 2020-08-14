using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinionManager : MonoBehaviour
{
    public static MinionManager Instance { get; private set; }

    public GameObject BasePrefab;
    public GameObject MinionPrefab;
    public BaseBuilding baseBuilding;
    private List<Minion> minions = new List<Minion>();
    private void Awake()
    {
        GameObject building = Instantiate(BasePrefab);
        baseBuilding = building.GetComponent<BaseBuilding>();
        building.transform.Translate(Vector3.down * 0.1f);
        Instance = this;
    }
    private void Start()
    {
    }
    public void SpawnMinion(MinionData data)
    {
        Minion minion = Instantiate(MinionPrefab).GetComponent<Minion>();
        minion.data = data;
        minion.gameObject.transform.position = baseBuilding.spawnPosition;
        minions.Add(minion);
    }
}
