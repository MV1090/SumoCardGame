using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

public class CardSelect : NetworkBehaviour
{  
    public BaseCard lastHovered;
    public BaseCard currentHovered;
    public BaseCard selectedCard;

    void Update()
    {
        if (!IsOwner)
            return;

        HoverOverCard();
        SelectCard();
    }

    private void HoverOverCard()
    {
        if (Mouse.current == null) return;

        Vector2 mouseScreenPos = Mouse.current.position.ReadValue();
        Vector2 mouseWorldPos =
            Camera.main.ScreenToWorldPoint(mouseScreenPos);

        Collider2D hit = Physics2D.OverlapPoint(mouseWorldPos);

        if (hit != null && hit.CompareTag("Card"))
        {
            BaseCard card = hit.GetComponent<BaseCard>();

            if (card == currentHovered)
                return;

            currentHovered = card;

            if (card != null && card != lastHovered && card.isInHand)
            {
                if (lastHovered != null)
                    lastHovered.OnCardHoverExit?.Invoke(lastHovered);

                card.OnCardHover?.Invoke(card);
                lastHovered = card;
            }
        }
        else if (lastHovered != null)
        {
            lastHovered.OnCardHoverExit?.Invoke(lastHovered);
            lastHovered = null;
            currentHovered = null;
        }
    }

    private void SelectCard()
    {
        if(Input.GetMouseButtonDown(0))
        {
            if(currentHovered == null)
                return;

            currentHovered.isSelected = !currentHovered.isSelected;

            if (selectedCard != null && selectedCard != currentHovered)
            {
                selectedCard.isSelected = false;
            }

            selectedCard = currentHovered.isSelected ? currentHovered : null;
        }
    }
}
