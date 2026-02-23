using System.Collections;
using TMPro;
using Unity.Netcode;
using UnityEngine;

public class BaseCard : NetworkBehaviour
{
    public Card_Scriptable cardData;

    [Header("UI References")]
    public TMP_Text nameText;
    public TMP_Text descriptionText;

    public SpriteRenderer artworkImage;
    public SpriteRenderer cardBackgroundImage;

    public bool isInHand = false;
    public bool isSelected = false;

    public System.Action<BaseCard> OnCardHover;
    public System.Action<BaseCard> OnCardHoverExit;

    private Vector3 originalPosition;

    public NetworkVariable<int> CardOwnerId = new NetworkVariable<int>(-1);
    public NetworkVariable<int> CardID = new NetworkVariable<int>(-1);

    [SerializeField] private CardCatalog_Scriptable cardCatalog;

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        if (cardCatalog == null)
        {
            Debug.Log("CardCatalog not found");
            return;
        }

        if (cardData == null && CardID.Value != -1)
        {
            cardData = cardCatalog.GetCard(CardID.Value);
        }

        CardOwnerId.OnValueChanged += OnCardOwnerIdChanged;

        if (CardOwnerId.Value != -1)
        {
            StartCoroutine(RegisterCardWithHandManager());
        }
    }

    private void OnCardOwnerIdChanged(int oldValue, int newValue)
    {
        if (newValue != -1)
        {
            isInHand = false;
            HandManager.Instance.RemoveCardFromHand(this);
            StartCoroutine(RegisterCardWithHandManager());
            
        }
    }

    public override void OnNetworkDespawn()
    {
        base.OnNetworkDespawn();
        CardOwnerId.OnValueChanged -= OnCardOwnerIdChanged;
        isInHand = false;
        HandManager.Instance.RemoveCardFromHand(this);
    }

    private IEnumerator RegisterCardWithHandManager()
    {
        int maxWaitFrames = 10;
        int framesWaited = 0;

        while (framesWaited < maxWaitFrames)
        {
            // Check if everything is ready
            if (HandManager.Instance != null &&
                CardOwnerId.Value != -1 &&
                Player.localInstance != null &&
                Player.localInstance.GetConnectionHandler() != null)
            {
                // Double-check we haven't already been added
                if (!isInHand)
                {
                    HandManager.Instance.AddCardToHand(this);
                    isInHand = true;
                    //UpdateCardVisuals();
                }
                yield break;
            }

            yield return null;
            framesWaited++;
        }

        // If we got here, something might be wrong
        if (HandManager.Instance == null)
        {
            Debug.LogWarning($"[BaseCard] HandManager.Instance is null after waiting. CardID: {CardID.Value}");
        }
        else if (CardOwnerId.Value == -1)
        {
            Debug.LogWarning($"[BaseCard] CardOwnerId is still -1 after waiting. CardID: {CardID.Value}");
        }
    }

    public bool IsOwnedByLocalPlayer()
    {
        if (Player.localInstance == null ||
            Player.localInstance.GetConnectionHandler() == null)
            return false;

        return CardOwnerId.Value == Player.localInstance.GetConnectionHandler().playerId.Value;
    }

    public bool IsOwnedByOpponent()
    {
        return CardOwnerId.Value != -1 && !IsOwnedByLocalPlayer();
    }

    private void Awake()
    {
        //OnCardHover += SetOnHoverPos;
        //OnCardHoverExit += SetOffHoverPos;
    }
 
    
    public virtual void UpdateCardVisuals()
    {
        if(cardData == null)
            cardData = cardCatalog.GetCard(CardID.Value);

        if(IsOwnedByLocalPlayer())
            ShowCardFace();
        else
            ShowCardBack();
    }

    private void ShowCardFace()
    {
        if (cardData == null && cardCatalog != null && CardID.Value != -1)
            cardData = cardCatalog.GetCard(CardID.Value);

        if (cardData == null)
            return;

        if (nameText) nameText.text = cardData.cardName;
        if (descriptionText) descriptionText.text = "Ability: \n" + cardData.cardDescription;
        if (artworkImage) artworkImage.sprite = cardData.cardArtWork;
        if (cardBackgroundImage) cardBackgroundImage.sprite = cardData.cardFaceSprite;
    }

    private void ShowCardBack()
    {
        if (cardData == null && cardCatalog != null && CardID.Value != -1)
            cardData = cardCatalog.GetCard(CardID.Value);

        if (cardData == null)
            return;

        if (nameText) nameText.text = "";
        if (descriptionText) descriptionText.text = "";
        if (artworkImage) artworkImage.sprite = null;
        if (cardBackgroundImage)
            cardBackgroundImage.sprite = cardData.cardBackSprite;
    }

    public virtual void PlayCard()
    {
        isInHand = false;
        HandManager.Instance.RemoveCardFromHand(this);
        CardOwnerId.Value = -1; // Clear ownership when played

        Debug.Log($"Playing card: {cardData.cardName}");
    }

    //private void SetOnHoverPos(BaseCard baseCard)
    //{
    //    if(!isInHand)
    //        return;        

    //    Debug.Log("Mouse over card");
    //}

    //private void SetOffHoverPos(BaseCard baseCard)
    //{
    //    if (!isInHand)
    //        return;        

    //    Debug.Log("Mouse exit card");
    //}

}
