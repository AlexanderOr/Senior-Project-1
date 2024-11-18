using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BossEnemy : MonoBehaviour
{
    public Transform player;             // Reference to the player
    public float stopDistance = 5f;      // Distance to stop from the player
    public float speed = 1f;             // Movement speed
    public GameObject projectilePrefab;  // Projectile to shoot at the player
    public float shootInterval = 5f;     // Time between shots
    public Transform[] firePoint;          // Position to shoot from
    public float EnemyHP;
    public float EnemyMaxHP;
    public bool isBleeding;
    public GameObject EXPPrefab;

    [SerializeField] EnemyHPBar healthBar;
    [SerializeField] PlayerController playerController;
    public GameObject PlayerGO;
    private float shootTimer;

    public float coneAngle = 45f;
    public float coneDistance = 10f;
    public float AOEInterval = 10f;
    public int aoeDamage = 20;
    public int contactDamage = 25;

    public EnemyBehavior enemyBehavior;

    private void Start()
    {
        healthBar = GetComponentInChildren<EnemyHPBar>();
        StartCoroutine(ConeAOEAttack());
    }

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        PlayerGO = GameObject.FindGameObjectWithTag("Player");
        playerController = PlayerGO.GetComponent<PlayerController>();
    }


    void Update()
    {
        if (playerController.isPaused == false)
        {
            MoveTowardsPlayer();
            ShootAtPlayer();
        }


        if (EnemyHP <= 0)
        {
            Destroy(gameObject);
            //win game
            SceneManager.LoadScene("GameWon");
        }

        if (isBleeding == true)
        {
            StartCoroutine(Bleeding());
        }
    }

    void MoveTowardsPlayer()
    {
        // Calculate distance to the player
        float distance = Vector2.Distance(transform.position, player.position);

        // If the distance is greater than the stopping distance, move towards the player
        if (distance > stopDistance)
        {
            Vector2 direction = (player.position - transform.position).normalized;
            transform.position = Vector2.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
        }
    }

    void ShootAtPlayer()
    {
        // Calculate distance to the player
        float distance = Vector2.Distance(transform.position, player.position);

        // Only shoot if within range and time to shoot
        shootTimer += Time.deltaTime;
        if (shootTimer >= shootInterval)
        {
            shootTimer = 0f;
            ShootProjectile();
        }

    }

    void ShootProjectile()
    {
        foreach (Transform spawnPoint in firePoint)
        {
            // Instantiate a projectile and set its direction towards the player
            GameObject projectile = Instantiate(projectilePrefab, spawnPoint.position, Quaternion.identity);
            Vector2 direction = (player.position - spawnPoint.position).normalized;
            projectile.GetComponent<Rigidbody2D>().velocity = direction * 7f; // Adjust speed as necessary
        }
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            collision.gameObject.GetComponent<PlayerController>().TakeDamage(contactDamage);
        }

        if (collision.tag == "Spells")
        {

            Spellinstance spellInstance = collision.GetComponent<Spellinstance>();
            if (spellInstance != null)
            {
                Spells spellData = spellInstance.spellData;

                int finalDamage = spellData.Damage + (5 * (spellData.Level - 1));
                // Apply damage based on the spell's damage property
                StartCoroutine(Damage(finalDamage));

                // Apply status effects based on the spell's properties
                if (spellData.isBleeding)
                {
                    isBleeding = true;
                }
                if (spellData.isStunned)
                {
                    //isStunned = true;
                }
                if (spellData.speedReduction > 0f)
                {
                    speed = Mathf.Max(2f, speed - spellData.speedReduction);
                }

                // Destroy the spell object after applying its effects
                if (enemyBehavior.damageSounds.Length > 0)
                {
                    int randomIndex = Random.Range(0, enemyBehavior.damageSounds.Length);
                    enemyBehavior.audioSource.PlayOneShot(enemyBehavior.damageSounds[randomIndex]);

                }
                Destroy(collision.gameObject);

            }


        }

    }

    IEnumerator Damage(int DamageAmount)
    {
        EnemyHP -= DamageAmount;
        healthBar.UpdateHealthBar(EnemyHP, EnemyMaxHP);
        Debug.Log(EnemyHP);
        yield return new WaitForSeconds(0.5f);
    }

    IEnumerator Bleeding()
    {
        EnemyHP -= 5;
        yield return new WaitForSeconds(1f);
        Debug.Log(EnemyHP);
        EnemyHP -= 5;
        Debug.Log(EnemyHP);
        isBleeding = false;
        yield return null;
        Debug.Log(EnemyHP);
    }

    private IEnumerator ConeAOEAttack()
    {
        while (true)
        {
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, coneDistance);
            foreach (var hitCollider in hitColliders)
            {
                Vector3 directionToTarget = (hitCollider.transform.position - transform.position).normalized;
                float angle = Vector3.Angle(transform.forward, directionToTarget);

                if (angle < coneAngle / 2 && hitCollider.CompareTag("Player"))
                {
                    hitCollider.GetComponent<PlayerController>().TakeDamage(aoeDamage);
                }
            }
            yield return new WaitForSeconds(AOEInterval);
        }
    }
}

