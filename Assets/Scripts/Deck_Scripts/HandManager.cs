using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandManager : MonoBehaviour
{
    public static HandManager Instance;

    public Transform playerHandAnchor;
    public Transform opponentHandTransform;
    public float cardSpacing = 2.0f;
    public float fanAngle = 10.0f;

    public List<GameObject> playerHandCards = new List<GameObject>();
    public List<GameObject> opponentHandCards = new List<GameObject>();

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

    public void AddCardToHand(GameObject newCard)
    {
        StartCoroutine(WaitAndReposition(newCard));     
    }

    IEnumerator WaitAndReposition(GameObject newCard)
    {
        yield return new WaitForSeconds(1);

        newCard.transform.position = playerHandAnchor.position;           

        playerHandCards.Add(newCard);

        RepositionHand(playerHandAnchor, playerHandCards);
    }

    public void RepositionHand(Transform handTransform, List<GameObject> hand)
    {
        float centerOffset = (hand.Count - 1) * 0.5f * cardSpacing;

        for (int i = 0; i < hand.Count; i++)
        {
            GameObject card = hand[i];

            Vector3 targetPosition = handTransform.position + handTransform.right * (i * cardSpacing - centerOffset);

            //Quaternion targetRotation = Quaternion.Euler(0, 0, (i - (handCards.Count - 1) / 2.0f) * fanAngle);

            card.transform.localPosition = handTransform.InverseTransformPoint(targetPosition);
            //card.transform.localRotation = targetRotation;
        }
    }
        
}
