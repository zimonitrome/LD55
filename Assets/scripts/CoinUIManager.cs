using UnityEngine;
using TMPro; // Add this namespace to access TextMeshPro classes

public class CoinUIManager : MonoBehaviour
{
    public static CoinUIManager Instance { get; private set; } // Singleton for easy access
    public TextMeshProUGUI coinText; // Change Text to TextMeshProUGUI

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void UpdateUI(int coinCount)
    {
        coinText.text = coinCount.ToString(); // Update the text field
    }
}
