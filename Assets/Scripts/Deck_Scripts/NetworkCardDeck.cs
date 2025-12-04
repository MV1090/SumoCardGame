using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class NetworkCardDeck : NetworkBehaviour
{
    [Header("Deck Data")]
    public Deck_Scriptable deck;

    [Header("Card Prefabs")]
    public GameObject wrestlingCardPrefab;
    public GameObject utilityCardPrefab;

    private Queue<int> deckQueue;
    [SerializeField] private CardCatalog_Scriptable cardCatalog;


    public override void OnNetworkSpawn()
    {
        //cardCatalog = Resources.Load<CardCatalog_Scriptable>("CardCatalog_Scriptable");

        if (cardCatalog == null)
        {
            Debug.Log("CardCatalog not found");
            return;
        }

        if (IsServer)
        {
            BuildDeck();
        }

        Player.localInstance.InitializeDeck(this);
    }

    private void BuildDeck()
    {
        List<Card_Scriptable> shuffleDeck = deck.BuildShuffledDeck();

        deckQueue = new Queue<int>(shuffleDeck.Count);

        foreach (Card_Scriptable card in shuffleDeck)
        {
            int typeId = cardCatalog.GetCardID(card);
            if (typeId == -1)
            {
                Debug.LogError($"Card {card.name} is missing from CardCatalog!");
                continue;
            }
            
            deckQueue.Enqueue(typeId);            
        }

        Debug.Log($"[Server] Deck built with {deckQueue.Count} cards.");
    }

    private int ServerDrawCard()
    {
        if (deckQueue == null || deckQueue.Count == 0)
        {
            Debug.LogWarning("Deck is empty!");
            return -1;
        }

        int drawnCardTypeId = deckQueue.Dequeue();
        Debug.Log($"[Server] Drew card with Type ID: {drawnCardTypeId}. Cards left in deck: {deckQueue.Count}");
        return drawnCardTypeId;
    }

    [ServerRpc(RequireOwnership = false)]
    public void RequestDrawCardServerRpc(ServerRpcParams rpcParams = default)
    {
        ulong requester = rpcParams.Receive.SenderClientId;

        int cardTypeId = ServerDrawCard();
        if (cardTypeId == -1)
        {
            return;
        }

        SpawnDrawnCardClientRpc(cardTypeId, new ClientRpcParams
        {
            Send = new ClientRpcSendParams
            {
                TargetClientIds = new ulong[] { requester }
            }
        });

        return;
    }

    [ClientRpc]
    private void SpawnDrawnCardClientRpc(int cardTypeId, ClientRpcParams rpcParams = default)
    {
        Card_Scriptable cardData = cardCatalog.GetCard(cardTypeId);
        if(cardData == null)
        {
            Debug.LogError($"Client received invalid cardTypeId {cardTypeId}");
            return ;
        }
        GameObject drawnCard = CreateLocalCard(cardData);
        
        if (drawnCard != null)
        HandManager.Instance.AddCardToHand(drawnCard);

        return; 
    }

    private GameObject CreateLocalCard(Card_Scriptable cardData)
    {
        GameObject cardObject = null;

        if (cardData is WrestlingCard_Scriptable)
        {
            cardObject = Instantiate(wrestlingCardPrefab);
        }
        else if (cardData is UtilityCard_Scriptable)
        {
            cardObject = Instantiate(utilityCardPrefab);
        }

        if (cardObject != null)
        {
            BaseCard cardDisplay = cardObject.GetComponent<BaseCard>();
            if (cardDisplay != null)
            {
                cardDisplay.cardData = cardData;
                cardDisplay.SafeUpdateCardVisuals();
            }
        }

        return cardObject;
    }    
}
