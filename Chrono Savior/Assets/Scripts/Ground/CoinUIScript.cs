using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CoinUIScript : MonoBehaviour
{
    [SerializeField] private Text coinText;

    void Start()
    {
        if(Player.Instance != null)
        {
            coinText.text = Player.Instance.GetCoins().ToString();
            Player.Instance.OnCoinGet += OnCoinGet;
        }
        else
        {
            Debug.LogError("Player not found in CoinUIScript.");
        }
    }
    void OnCoinGet()
    {
        if(Player.Instance != null)
        {
            coinText.text = Player.Instance.GetCoins().ToString();
        }
        else
        {
            Debug.LogError("Player not found in CoinUIScript.");
        }
    }

    void OnDisable()
    {
        if(Player.Instance != null)
        {
            Player.Instance.OnCoinGet -= OnCoinGet;
        }
        
    }
}
