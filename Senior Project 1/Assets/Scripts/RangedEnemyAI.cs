using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedEnemyAI : MonoBehaviour
{
    public Transform player;             // Reference to the player
    public float stopDistance = 5f;      // Distance to stop from the player
    public float speed = 2f;             // Movement speed
    public GameObject projectilePrefab;  // Projectile to shoot at the player
    public float shootInterval = 2f;     // Time between shots
    public Transform firePoint;          // Position to shoot from
    public float EnemyHP;
    public float EnemyMaxHP;
    public bool isBleeding;

    public GameObject EXPPrefab;
    public GameObject VortexGO;
    public GameObject ChestGO;

    [SerializeField] EnemyHPBar healthBar;
    [SerializeField] PlayerController playerController;
    public GameObject PlayerGO;
    private float shootTimer;

    public EnemyBehavior enemyBehavior;

    //SFX
    public AudioClip[] damageSounds; // Array to hold the three sounds
    public AudioClip DeathSound;
    public AudioSource audioSource;

    //int for drops
    public int ChestDropChance;
    public int VortexDropChance;
    public int RandomChestChance;
    public int RandomVortexChance;

    public GameObject spellHolderGO;
    public SpellHolder spellHolder;
    private void Start()
    {
        healthBar = GetComponentInChildren<EnemyHPBar>();
    }

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        PlayerGO = GameObject.FindGameObjectWithTag("Player");
        playerController = PlayerGO.GetComponent<PlayerController>();
        audioSource = GetComponent<AudioSource>();
        spellHolderGO = GameObject.FindGameObjectWithTag("Holder");
        spellHolder = spellHolderGO.GetComponent<SpellHolder>();
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
            Instantiate(EXPPrefab, gameObject.transform.position, Quaternion.Euler(0, 0, 0));
            RandomChestChance = Random.Range(1, 200);
            RandomVortexChance = Random.Range(1, 200);

            if (ChestDropChance >= RandomChestChance)
            {
                Instantiate(ChestGO, gameObject.transform.position, Quaternion.Euler(0, 0, 0));
            }
            else if (VortexDropChance >= RandomVortexChance)
            {
                Instantiate(VortexGO, gameObject.transform.position, Quaternion.Euler(0, 0, 0));
            }

            Destroy(gameObject);
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
        if (distance <= stopDistance)
        {
            shootTimer += Time.deltaTime;
            if (shootTimer >= shootInterval)
            {
                shootTimer = 0f;
                ShootProjectile();
            }
        }
    }

    void ShootProjectile()
    {
        // Instantiate a projectile and set its direction towards the player
        GameObject projectile = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);
        Vector2 direction = (player.position - firePoint.position).normalized;
        projectile.GetComponent<Rigidbody2D>().velocity = direction * 7f; // Adjust speed as necessary
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Spells")
        {

            Spellinstance spellInstance = collision.GetComponent<Spellinstance>();
            if (spellInstance != null)
            {
                Spells spellData = spellInstance.spellData;

                int finalDamage = spellData.Damage + (5 * (spellHolder.playerSpells[spellData] - 1));
                // Apply damage based on the spell's damage property
                

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
                if (spellData.isKnockedBack)
                {
                    PushBack(spellData.ForceAmount, collision);
                }

                // Destroy the spell object after applying its effects
                if (damageSounds.Length > 0)
                {
                    int randomIndex = Random.Range(0, damageSounds.Length);
                    audioSource.PlayOneShot(damageSounds[randomIndex]);

                }

                if (spellData.DeleteOnHit == true)
                {
                    Debug.Log("Spell was not one of the detected spells");
                    Destroy(collision.gameObject);
                }

                StartCoroutine(Damage(finalDamage));

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
        int bleedTime = 2;
        int bleedTicks = 0;
        int bleedDamage = 5;

        yield return new WaitForSeconds(1f);
        while (isBleeding == true)
        {
            if (bleedTicks < bleedTime)
            {
                StartCoroutine(Damage(bleedDamage));
                //yield return new WaitForSeconds(1f);
                bleedTicks++;
            }
            else
            {
                isBleeding = false;
            }
        }
    }

    public void PushBack(float force, Collider2D collision)
    {
        //Vector2 pushDirection = (transform.position - collision.transform.position).normalized;
        //RB.velocity = Vector2.zero; // Reset velocity
        //RB.AddForce(pushDirection * force, ForceMode2D.Impulse);

        StartCoroutine(TemporarilyDisableMovement(0.3f));
        Debug.Log("pushed");
    }

    private IEnumerator TemporarilyDisableMovement(float duration)
    {
        speed = 0; // Disable movement
        yield return new WaitForSeconds(duration);
        speed = 3f; // Restore movement
    }
}
