using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Card : MonoBehaviour
{
    private CardData data;
    private TextMeshProUGUI text;
    public enum Type
    {
        SpawnMinion,
        Rope,
        DirectDamage,
        HealMinion,
        DOT,
        AOE,
        WeaknessDebuff,
        ApplyBlock,
        ApplyArmor,
        SpawnBase,
        IncreaseMinionHaste,
        IncreaseMinionActions,
        IncreaseMinionHealth,
        IncreaseMinionDamage,
        Tool,
        Weapon,
    }
    private void Awake()
    {
        text = GetComponentInChildren<TextMeshProUGUI>();

    }
    private void Start()
    {
    }

    public void PlayCard()
    {
        switch (data.type)
        {
            case Card.Type.SpawnMinion:
                MinionManager.Instance.SpawnMinion(data.Minion);
                break;
            case Card.Type.Rope:
                Debug.Log(GameManager.Instance.selectedMinion.transform.position);
                Debug.Log(LevelManager.Instance.tilemap.SpawnRope(GameManager.Instance.selectedMinion.transform.position, data.RopeLength));

                break;

            default:
                Debug.Log("playing " + data.name.ToString());
                break;
        }
        CardManager.Instance.hand.RemoveCard(this);
        Destroy(gameObject);

    }
    public void SetData(CardData data)
    {
        this.data = data;
        text.text = data.name;

    }

}
