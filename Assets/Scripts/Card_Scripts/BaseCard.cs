using TMPro;
using Unity.Netcode;
using UnityEditor;
using UnityEngine;

public class BaseCard : NetworkBehaviour
{
    public Card_Scriptable cardData;

    [Header("UI References")]
    public TMP_Text nameText;
    public TMP_Text descriptionText;

    public SpriteRenderer artworkImage;

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
            SafeUpdateCardVisuals();
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

    private void InitCard()
    {
        nameText.text = cardData.cardName;
        descriptionText.text = "Ability: /n" + cardData.cardDescription;
        artworkImage.sprite = cardData.cardSprite;
    }

    // for editor updates   
    private void OnValidate()
    {
//#if UNITY_EDITOR
//        if (!Application.isPlaying)
//        {
//            EditorApplication.delayCall += () =>
//            {
//                if (this == null) return; // object might have been destroyed
//                SafeUpdateCardVisuals();
//            };
//            return;
//        }
//#endif
//        SafeUpdateCardVisuals();
    }

    public virtual void SafeUpdateCardVisuals()
    {
        if (cardData == null)
        {
            if (nameText) nameText.text = "";
            if (descriptionText) descriptionText.text = "";
            if (artworkImage) artworkImage.sprite = null;
        }
        else
        {
            if (nameText) nameText.text = cardData.cardName;
            if (descriptionText) descriptionText.text = "Ability: \n"  + cardData.cardDescription;
            if (artworkImage) artworkImage.sprite = cardData.cardSprite;
        }
    }

    public void UpdateCardVisuals()
    {

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
