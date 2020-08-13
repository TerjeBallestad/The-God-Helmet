using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand
{
    private Queue<Card> cards;

    public void DrawCard(Card card)
    {
        cards.Enqueue(card);
    }

}
