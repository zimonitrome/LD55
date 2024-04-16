using UnityEngine;

public class CoinManager : MonoBehaviour
{
    public static CoinManager Instance { get; private set; } // Singleton pattern

    [SerializeField] private int initialCoins = 10; // Starting number of coins, adjustable in the editor
    public int Coins { get; private set; } // Property to keep track of coins

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Make this a persistent singleton
        }
        else
        {
            Destroy(gameObject);
        }
        Coins = initialCoins; // Initialize coins with the specified amount
        UpdateCoinUI(); // Update UI at start
    }

    public void AddCoins(int amount)
    {
        Coins += amount;
        UpdateCoinUI();
    }

    public void resetCoins()
    {
        Coins = initialCoins;
        UpdateCoinUI();
    }

    public bool SpendCoins(int amount)
    {
        if (Coins >= amount)
        {
            Coins -= amount;
            UpdateCoinUI();
            return true;
        }
        return false;
    }

    private void UpdateCoinUI()
    {
        if (CoinUIManager.Instance != null)
        {
            CoinUIManager.Instance.UpdateUI(Coins);
        }
    }
}
