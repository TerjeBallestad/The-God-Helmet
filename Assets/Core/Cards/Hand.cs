using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Hand : MonoBehaviour
{
    public int MaxInHand = 10;
    private List<Card> cards = new List<Card>();
    public float distanceBetweenCards = 1.6f;
    public void PutInHand(Card card)
    {
        if (cards.Count < MaxInHand)
        {
            cards.Add(card);
        }
        else
        {
            Destroy(card.gameObject);
        }
        PutCardsInOrder();
    }
    public void RemoveCard(Card card)
    {
        cards.Remove(card);
        PutCardsInOrder();
    }
    private void PutCardsInOrder()
    {
        for (int i = 0; i < cards.Count; i++)
        {
            cards.ElementAt(i).transform.position = gameObject.transform.position + new Vector3((float)i * distanceBetweenCards, 0);
        }
    }


}
