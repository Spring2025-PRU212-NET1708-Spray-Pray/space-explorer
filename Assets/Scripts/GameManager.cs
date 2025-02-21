using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public GameObject enemyPrefab;
    public GameObject starPrefab;
    public float minInstantiateValue;
    public float maxInstantiateValue;
    public float enemyDestroyTime = 5f;
    public float starDestroyTime = 5f;

    [Header("Asteroid Sprites")]
    public Sprite[] asteroidSprites; // Add this line

    [Header("Partice Effects")]
    public GameObject explosionEffect;
    public GameObject muzzleFlash;

    [Header("Panels")]
    public GameObject StartMenu;
    public GameObject PauseMenu;
    public GameObject GameOverPanel;

    [Header("UI Elements")]
    public Text ScoreText;

    private int score = 0;

    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        StartMenu.SetActive(true);
        PauseMenu.SetActive(false);
        GameOverPanel.SetActive(false);
        ScoreText.text = "Score: 0";
        Time.timeScale = 0f;
        InvokeRepeating("InstantiateEnemy", 1f, 1f);
        InvokeRepeating("InstantiateStar", 1f, 1f);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseGameButton(true);
        }
    }

    void InstantiateEnemy()
    {
        Vector3 enemypros = new Vector3(Random.Range(minInstantiateValue, maxInstantiateValue), 6f);
        GameObject enemy = Instantiate(enemyPrefab, enemypros, Quaternion.Euler(0f, 0f, 180f));

        // Randomly select a sprite from the array and assign it to the enemy
        SpriteRenderer spriteRenderer = enemy.GetComponent<SpriteRenderer>();
        if (spriteRenderer != null && asteroidSprites.Length > 0)
        {
            spriteRenderer.sprite = asteroidSprites[Random.Range(0, asteroidSprites.Length)];
        }

        Destroy(enemy, enemyDestroyTime);
    }

    void InstantiateStar()
    {
        Vector3 enemypros = new Vector3(Random.Range(minInstantiateValue, maxInstantiateValue), 6f);
        GameObject enemy = Instantiate(starPrefab, enemypros, Quaternion.Euler(0f, 0f, 180f));
        Destroy(enemy, starDestroyTime);
    }

    public void StartGameButton()
    {
        StartMenu.SetActive(false);
        Time.timeScale = 1f;
    }
    public void PauseGameButton(bool isPaused)
    {
        if (isPaused == true)
        {
            PauseMenu.SetActive(true);
            Time.timeScale = 0f;
        }
        else
        {
            PauseMenu.SetActive(false);
            Time.timeScale = 1f;
        }
    }
    public void QuitGame()
    {
        UnityEditor.EditorApplication.isPlaying = false;
        Application.Quit();
    }

    public void ShowGameOverPanel()
    {
        GameOverPanel.SetActive(true);
        Time.timeScale = 0f;
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void AddScore(int points)
    {
        score += points;
        ScoreText.text = "Score: " + score;
    }
}
