using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player instance;
    private void Awake()
    {
        if(instance == null){
            instance = this;
        } else
        {
            Destroy(gameObject);
        }
    }
    public PlayerStats stats;
    [SerializeField] private SumoCard currentSumoCard;
    
}
