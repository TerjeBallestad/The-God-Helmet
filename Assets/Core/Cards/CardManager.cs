using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardManager : MonoBehaviour
{
    public static CardManager Instance { get; private set; }

    public GameObject cardPrefab;

    public Hand hand;
    public Deck deck;

    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        deck.CreateDeck();
        DrawCards(12);
    }
    public void DrawCards(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            GameObject card = Instantiate(cardPrefab);
            card.transform.SetParent(hand.transform);
            Card cardComponent = card.GetComponent<Card>();
            CardData data = deck.DrawCard();
            cardComponent.SetData(data);
            hand.PutInHand(cardComponent);
        }
    }
}
