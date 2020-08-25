using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndTurn : MonoBehaviour
{
    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(delegate { HandleButton(); });
    }
    public void HandleButton()
    {
        GameManager.Instance.SetOpponentTurn();
    }
}
