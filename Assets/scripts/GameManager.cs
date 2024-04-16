using UnityEngine;
using UnityEngine.UI;
using TMPro; // Make sure to have the TextMeshPro package installed

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public int currentWave = 1;
    public bool isRoundActive = false;
    public int totalWaves; // Total number of waves
    public GameObject inbetweenRoundUI;
    public GameObject gameUI;
    public TextMeshProUGUI infoText;
    public AudioSource audioSource;
    public AudioClip idleMusic;
    public Spawner spawner;
    public Button startButton;
    public TextMeshProUGUI startButtonText;
    public bool isGameOver = false;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    private void Start()
    {
        SetGameState(false);
        UpdateUI("It's halloween. The worst time of the year...\n\n" +
            "The chubby city kids have found their way onto your farm and will soon be begging for candy to clog their arteries.\n\n" +
            " Let's show them what good some veggies could do for them.");
        PlayMusic(idleMusic);

        spawner = FindObjectOfType<Spawner>();
    }

    private void setBetweenUI(bool isActive)
    {
        inbetweenRoundUI.gameObject.SetActive(isActive);
        gameUI.gameObject.SetActive(!isActive);
    }

    public void StartRound()
    {
        if (isGameOver) {
            currentWave = 1;
            isGameOver = false;
            CoinManager.Instance.resetCoins();
            Barn barn = FindObjectOfType<Barn>();
            barn.health = barn.maxHealth;
            barn.TakeDamage(0);
            Debug.Log(barn.healthBar.transform.localScale);

            isRoundActive = true;
            spawner.currentEnemyIndex = 0; // Reset enemy index at the start of each round
            UpdateUI($"Round {currentWave} has started!");
            setBetweenUI(false);
            startButtonText.text = "PLAY!";

            spawner.enemiesAlive = 0;
        }
        else if (currentWave <= totalWaves)
        {
            isRoundActive = true;
            spawner.currentEnemyIndex = 0; // Reset enemy index at the start of each round
            UpdateUI($"Round {currentWave} has started!");
            setBetweenUI(false);
            startButtonText.text = "PLAY!";
        }
    }

    public void EndRound()
    {
        isRoundActive = false;
        if (currentWave == 1)
        {
            UpdateUI(" Phew, night 1 is over.  Press PLAY for next night.");
        }
        else if (currentWave == 2)
        {
            UpdateUI($"Phew, night 2 is over.  Press PLAY for next night.");
        }
        setBetweenUI(true);

        // Find all instances of Potato and destroy them
        Potato[] potatoes = FindObjectsOfType<Potato>();
        foreach (Potato potato in potatoes)
        {
            Destroy(potato.gameObject);
        }

        // Find all instances of Batman and destroy
        Batman[] batmen = FindObjectsOfType<Batman>();
        foreach (Batman batman in batmen)
        {
            Destroy(batman.gameObject);
        }

        currentWave++;

        if (currentWave > totalWaves)
        {
            GameWin();
        }

        startButtonText.text = "PLAY!";
    }

    public void GameOver()
    {
        isGameOver = true;
        UpdateUI(" Those fat kids ate all your candy!\n\n Game Over.");
        isRoundActive = false;
        setBetweenUI(true);

        // Find all instances of Potato and destroy them
        Potato[] potatoes = FindObjectsOfType<Potato>();
        foreach (Potato potato in potatoes)
        {
            Destroy(potato.gameObject);
        }

        // Find all instances of Batman and destroy
        Batman[] batmen = FindObjectsOfType<Batman>();
        foreach (Batman batman in batmen)
        {
            Destroy(batman.gameObject);
        }


        // startButton.gameObject.SetActive(false);
        startButtonText.text = "Again?";
    }

    public void GameWin()
    {
        UpdateUI(" Congrats! You fended of all kids." + "\n" +
                    " At least for this hallloween...");
        SetGameState(false);

        // startButton.gameObject.SetActive(false);
        startButtonText.text = "Again?";
    }
    private void UpdateUI(string text)
    {
        infoText.text = text;
    }

    private void PlayMusic(AudioClip clip)
    {
        audioSource.clip = clip;
        audioSource.Play();
    }

    private void SetGameState(bool isActive)
    {
        isRoundActive = isActive;
        setBetweenUI(!isActive);
    }
}
