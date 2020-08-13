using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Deck : MonoBehaviour
{
    public CardAmount[] build;
    private Stack<CardData> cards = new Stack<CardData>();

    public void CreateDeck()
    {
        foreach (CardAmount card in build)
        {
            for (int i = 0; i < card.amount; i++)
            {
                cards.Push(card.data);
            }
        }
        ShuffleDeck();
    }
    public void ShuffleDeck()
    {
        CardData[] temp = cards.ToArray();
        cards.Clear();
        foreach (CardData data in temp.OrderBy(x => Random.Range(0, temp.Count() - 1)))
        {
            cards.Push(data);
        }
    }
    public CardData DrawCard()
    {
        if (cards.Count > 0)
            return cards.Pop();
        else return null;
    }
    [System.Serializable]
    public struct CardAmount
    {
        public CardData data;
        public int amount;
    }


}
