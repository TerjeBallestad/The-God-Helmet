using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand : MonoBehaviour
{
    private Queue<Card> cards;
    public void PutInHand(Card card)
    {
        cards.Enqueue(card);
        Debug.Log(card);
    }
}
