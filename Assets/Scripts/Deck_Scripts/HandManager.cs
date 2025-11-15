using System.Collections.Generic;
using UnityEngine;

public class HandManager : MonoBehaviour
{
    public static HandManager Instance;

    public Transform handAnchor;    
    public float cardSpacing = 2.0f;
    public float fanAngle = 10.0f;

    public List<GameObject> handCards = new List<GameObject>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public bool IsHandFull()
    {
        return handCards.Count >= Player.instance.stats.currentHandSize;
    }

    public void AddCardToHand(GameObject newCard)
    {
        newCard.transform.SetParent(handAnchor);

        handCards.Add(newCard);

        RepositionHand();
    }

    public void RepositionHand()
    {
        float centerOffset = (handCards.Count - 1) * 0.5f * cardSpacing;

        for (int i = 0; i < handCards.Count; i++)
        {
            GameObject card = handCards[i];

            Vector3 targetPosition = handAnchor.position + handAnchor.right * (i * cardSpacing - centerOffset);

            Quaternion targetRotation = Quaternion.Euler(0, 0, (i - (handCards.Count - 1) / 2.0f) * fanAngle);

            card.transform.localPosition = handAnchor.InverseTransformPoint(targetPosition);
            card.transform.localRotation = targetRotation;
        }
    }

}
