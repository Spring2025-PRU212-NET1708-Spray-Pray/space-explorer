using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    public float enemySpeed;
    public float health;
    public int scoreValue;
    private float minY;
    private Rigidbody2D myBody;
    private float maxHealth;

    [SerializeField] private GameObject explosionEffect;
    private AudioManager audioManager;
    private int isFollowingPlayer;
    [SerializeField] private Plane player;
    public AudioClip blowSound;
    private AudioSource audioSource;

    // HP Bar
    [SerializeField] private GameObject healthBarPrefab;
    private Slider healthBar;
    private Transform healthBarTransform;

    private void Awake()
    {
        myBody = GetComponent<Rigidbody2D>();

        int currentLevel = GameManager.Instance.GetLevel();
        health = UnityEngine.Random.Range(3, 6) + currentLevel * 2; // Random HP if not set in Unity
        maxHealth = health; // Save max HP

        if (!player)
            player = GameObject.FindAnyObjectByType<Plane>();

        if (isFollowingPlayer == 0)
            isFollowingPlayer = UnityEngine.Random.Range(-3, 5);

        Vector3 screenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, 0, 0));
        minY = screenBounds.y;
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    void Start()
    {
        int currentLevel = GameManager.Instance.GetLevel();
        enemySpeed += currentLevel * 0.7f;
        myBody.linearVelocity = new Vector2(0f, -enemySpeed);

        // Create HP Bar inside World Space Canvas
        if (healthBarPrefab != null)
        {
            GameObject hpBar = Instantiate(healthBarPrefab, transform.position + new Vector3(0, -0.5f, 0), Quaternion.identity);
            healthBar = hpBar.GetComponentInChildren<Slider>();
            healthBarTransform = hpBar.transform;

            // Gán HP Bar làm con của Enemy để nó di chuyển theo
            healthBarTransform.SetParent(transform);
            healthBarTransform.localPosition = new Vector3(0, 1f, 0); // Điều chỉnh Y để HP Bar nằm trên đầu Enemy



            // Giữ kích thước nhỏ phù hợp
            healthBarTransform.localScale = Vector3.one * 0.3f;

            FollowEnemy hpBarScript = hpBar.GetComponent<FollowEnemy>();
            if (hpBarScript != null)
            {
                hpBarScript.SetTarget(transform);
            }
        }
    }


    void Update()
    {
        if (isFollowingPlayer > 0 && player != null)
        {
            FollowPlayer();
        }
        if (transform.position.y < minY)
        {
            Destroy(gameObject);
            if (healthBarTransform != null)
            {
                Destroy(healthBarTransform.gameObject);
            }
        }
    }

    public void TakeDamage(float damage)
    {
        health -= damage;

        if (healthBar != null)
        {
            healthBar.value = health / maxHealth; // Normalize value
        }

        if (health <= 0)
        {
            audioManager.PlaySFX(audioManager.explosion);
            Die();
        }
    }

    private void FollowPlayer()
    {
        Vector2 direction = player.transform.position - transform.position;
        Vector2 moveDirection = new Vector2(direction.x, -1).normalized;
        transform.position += (Vector3)moveDirection * enemySpeed * Time.deltaTime;
    }

    void Die()
    {
        if (explosionEffect != null)
        {
            var explosion = Instantiate(explosionEffect, transform.position, Quaternion.identity);
            Destroy(explosion, 1f);
        }

        if (healthBarTransform != null)
        {
            Destroy(healthBarTransform.gameObject);
        }

        Destroy(gameObject);
        GameManager.Instance.AddScore(scoreValue);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ship1") || collision.CompareTag("Ship2"))
        {
            if (explosionEffect != null)
            {
                Instantiate(explosionEffect, transform.position, Quaternion.identity);
            }

            if (blowSound != null)
            {
                StartCoroutine(PlayBlowSoundThenGameOver(collision.gameObject));
                Destroy(collision.gameObject);
            }
            else
            {
                GameOver(collision.gameObject);
            }
        }
    }

    IEnumerator PlayBlowSoundThenGameOver(GameObject playerShip)
    {
        //audioSource.clip = blowSound;
        //audioSource.volume = 3.0f;
        //audioSource.Play();
        audioManager.PlaySFX(audioManager.gameOver);
        yield return new WaitForSeconds(1);
        GameOver(playerShip);
    }

    void GameOver(GameObject playerShip)
    {
        PlayerPrefs.SetString("PlayerScore", GameManager.Instance.scoreText.text);
        SceneManager.LoadScene("GameOverScene");
    }
}
