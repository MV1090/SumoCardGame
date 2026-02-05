using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class NetworkCardDeck : NetworkBehaviour
{
    [Header("Deck Data")]
    public Deck_Scriptable deck;

    [Header("Card Prefabs")]
    public NetworkObject wrestlingCardPrefab;
    public NetworkObject utilityCardPrefab;

    private Queue<int> deckQueue;
    [SerializeField] private CardCatalog_Scriptable cardCatalog;


    public override void OnNetworkSpawn()
    {       

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

        Card_Scriptable cardData = cardCatalog.GetCard(cardTypeId);

        NetworkObject cardObject = null;

        if (cardData is WrestlingCard_Scriptable)
        {
            cardObject = Instantiate(wrestlingCardPrefab);
        }
        else if (cardData is UtilityCard_Scriptable)
        {
            cardObject = Instantiate(utilityCardPrefab);
        }

        if (cardObject == null)
        {
            Debug.LogError($"Failed to instantiate card prefab for cardTypeId: {cardTypeId}");
            return;
        }

        cardObject.SpawnWithOwnership(requester);

        BaseCard drawnCard = cardObject.GetComponent<BaseCard>();
        if (drawnCard != null)
        {
            drawnCard.cardData = cardData;
            drawnCard.CardID.Value = cardTypeId;
            drawnCard.CardOwnerId.Value = GetPlayerIdByClientId(requester);
        }
        else
        {
            Debug.LogError("Spawned card does not have BaseCard component!");
        }

        return;
    }

    private int GetPlayerIdByClientId(ulong clientId)
    {
        Player player = FindPlayerByClientId(clientId);
        if (player != null && player.GetConnectionHandler() != null)
        {
            return player.GetConnectionHandler().playerId.Value;
        }
        return -1;
    }

    private Player FindPlayerByClientId(ulong clientId)
    {
        Player[] allPlayers = GameObject.FindObjectsOfType<Player>();
        foreach (var player in allPlayers)
        {
            NetworkObject playerNetworkObject = player.GetComponent<NetworkObject>();
            if (playerNetworkObject != null && playerNetworkObject.OwnerClientId == clientId)
            {
                return player;
            }
        }
        return null;
    }

    //[ClientRpc]
    //private void SpawnDrawnCardClientRpc(int cardTypeId, ClientRpcParams rpcParams = default)
    //{
    //    Card_Scriptable cardData = cardCatalog.GetCard(cardTypeId);
    //    if(cardData == null)
    //    {
    //        Debug.LogError($"Client received invalid cardTypeId {cardTypeId}");
    //        return ;
    //    }
    //    GameObject drawnCard = CreateLocalCard(cardData);

    //    if (drawnCard != null)
    //    {            
    //        Debug.Log($"[Client] Spawned drawn card: {cardData.cardName}");
    //        HandManager.Instance.AddCardToHand(drawnCard);
    //    }

    //    BaseCard card = drawnCard.GetComponent<BaseCard>();
    //    card.isInHand = true;

    //    return; 
    //}       
}
