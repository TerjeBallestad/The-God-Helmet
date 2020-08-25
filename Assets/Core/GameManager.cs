using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

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
    private Transform camFollow;
    public PolygonCollider2D cameraConfiner;
    private CinemachineVirtualCamera virtualCamera;
    private void Awake()
    {
        Instance = this;
        virtualCamera = FindObjectOfType<CinemachineVirtualCamera>();

    }
    private void Start()
    {
        turn = Turn.Player;
        camFollow = transform;
    }
    public void SetOpponentTurn()
    {
        turn = Turn.Opponent;
        EnemyManager.Instance.ExecuteEvilPlans();
    }
    public void SetPlayerTurn()
    {
        turn = Turn.Player;
    }
    public void SetSelectedMinion(Minion minion)
    {
        selectedMinion = minion;
        SetCameraFollow(minion.transform);
    }
    public void SetCameraFollow(Transform transform)
    {
        camFollow = transform;
        followSet = false;
    }
    private void Update()
    {
        if (!followSet)
        {
            if (Camera.main.GetComponent<Cinemachine.CinemachineBrain>().ActiveVirtualCamera != null)
            {
                virtualCamera.Follow = camFollow;
                CinemachineConfiner confiner = virtualCamera.GetComponent<CinemachineConfiner>();
                confiner.InvalidatePathCache();
                confiner.m_BoundingShape2D = cameraConfiner;
                followSet = true;
            }
        }
        if (turn == Turn.Opponent)
        {


            return;
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
