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
        Instance = this;
    }

    public void SpawnBase(Vector3 position)
    {
        GameObject building = Instantiate(BasePrefab, position, Quaternion.identity);
        baseBuilding = building.GetComponent<BaseBuilding>();
        building.transform.Translate(Vector3.down * 0.1f);
        GameManager.Instance.SetCameraFollow(baseBuilding.transform);
    }

    public void SpawnMinion(MinionData data)
    {
        Minion minion = Instantiate(MinionPrefab).GetComponent<Minion>();
        minion.data = data;
        minion.gameObject.transform.position = baseBuilding.spawnPosition;
        GameManager.Instance.SetSelectedMinion(minion);
        minions.Add(minion);
    }
}
