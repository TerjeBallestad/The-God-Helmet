using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public enum Turn
    {
        Player,
        Opponent,
    }
    public Turn turn;
    public Minion selectedMinion;
    private bool followSet = false;
    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        turn = Turn.Player;
    }


    private void Update()
    {
        if (!followSet)
        {
            if (Camera.main.GetComponent<Cinemachine.CinemachineBrain>().ActiveVirtualCamera != null)
            {
                Camera.main.GetComponent<Cinemachine.CinemachineBrain>().ActiveVirtualCamera.Follow = MinionManager.Instance.baseBuilding.transform;
                followSet = true;
            }
        }
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (hit)
            {
                Minion minion = hit.transform.GetComponent<Minion>();

                if (minion)
                {
                    selectedMinion = minion;
                    Camera.main.GetComponent<Cinemachine.CinemachineBrain>().ActiveVirtualCamera.Follow = selectedMinion.transform;
                }
            }
        }
    }

}
