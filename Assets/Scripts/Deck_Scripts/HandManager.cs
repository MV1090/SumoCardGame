using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandManager : MonoBehaviour
{
    public static HandManager Instance;

    public Transform playerHandAnchor;
    public Transform opponentHandAnchor;
    public float cardSpacing = 2.0f;
    public float fanAngle = 10.0f;

    public List<BaseCard> playerHandCards = new List<BaseCard>();
    public List<BaseCard> opponentHandCards = new List<BaseCard>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public bool IsHandFull()
    {
        if (Player.localInstance == null || Player.localInstance.Stats == null)
        {
            return false;
        }
        return playerHandCards.Count >= Player.localInstance.Stats.currentHandSize.Value;
    }

    public void AddCardToHand(BaseCard newCard)
    {
        if (playerHandCards.Contains(newCard) || opponentHandCards.Contains(newCard))
            return;

        if (newCard.IsOwnedByLocalPlayer())
        {
            StartCoroutine(WaitAndReposition(newCard, playerHandAnchor, playerHandCards));
        }
        else if (newCard.IsOwnedByOpponent())
        {
            StartCoroutine(WaitAndReposition(newCard, opponentHandAnchor, opponentHandCards));
        }
    }

    IEnumerator WaitAndReposition(BaseCard newCard, Transform handTransform, List<BaseCard> hand)
    {
        if (hand.Contains(newCard))
        {
            yield break;
        }

        yield return new WaitForSeconds(0.5f);

        if (!hand.Contains(newCard))
        {
            hand.Add(newCard);
        }

        RepositionHand(handTransform, hand);
    }

    public void RepositionHand(Transform handTransform, List<BaseCard> hand)
    {
        if (hand.Count == 0 || handTransform == null)
            return;

        float centerOffset = (hand.Count - 1) * 0.5f * cardSpacing;

        for (int i = 0; i < hand.Count; i++)
        {
            BaseCard card = hand[i];
            
            if (card == null)
            {
                hand.RemoveAt(i);
                i--;
                continue;
            }            

            Vector3 targetWorldPosition = handTransform.position + handTransform.right * (i * cardSpacing - centerOffset);

            card.transform.position = targetWorldPosition;
        }
    }
}
