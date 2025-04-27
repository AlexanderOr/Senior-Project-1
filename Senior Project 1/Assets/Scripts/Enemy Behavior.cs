using JetBrains.Annotations;
using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

public class EnemyBehavior : MonoBehaviour
{
    public float EnemyHP, EnemyMaxHP = 50;
    public float MoveSpeed = 3f;
    private float LookSpeed = 10f;
    public GameObject Player;
    public HitBox HitBox;
    public bool HasTarget = false;
    public Animator Animator;
    public bool isBleeding = false;

    private Rigidbody2D RB;

    private Vector2 DirectionOfPlayer;
    private Vector2 EnemyToPlayer;
    private float EnemyDistance;
    public float EnemyAttackDistance = 1;

    public PlayerController playerController;

    public SpriteRenderer spriteRenderer;
    private Color Red = new Color(255, 0, 0, 255);
    private Color White = new Color(255, 255, 255, 255);

    public GameObject EXPPrefab;
    public GameObject VortexGO;
    public GameObject ChestGO;

    [SerializeField] EnemyHPBar healthBar;

    //SFX
    public AudioClip[] damageSounds; // Array to hold the three sounds
    public AudioClip DeathSound;
    public AudioSource audioSource;

    //int for drops
    public int ChestDropChance;
    public int VortexDropChance;
    public int RandomChestChance;
    public int RandomVortexChance;

    //Health Increase over time
    public float Counter;

    public GameObject spellHolderGO;
    public SpellHolder spellHolder;

    private CircleCollider2D myCircleCollider;

    public ParticleSystem particleSystem;

    // Start is called before the first frame update
    void Start()
    {
        RB = GetComponent<Rigidbody2D>();
        Animator = GetComponent<Animator>();
        Player = GameObject.FindGameObjectWithTag("Player");
        healthBar = GetComponentInChildren<EnemyHPBar>();
        audioSource = GetComponent<AudioSource>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        spellHolderGO = GameObject.FindGameObjectWithTag("Holder");
        spellHolder = spellHolderGO.GetComponent<SpellHolder>();
        myCircleCollider = GetComponent<CircleCollider2D>();
    }

    private void Awake()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        playerController = Player.GetComponent<PlayerController>();

    }

    // Update is called once per frame
    void Update()
    {
        if (playerController.isPaused == true)
        {
            RB.velocity = transform.up * 0;
        }
        else if (playerController.isPaused == false)
        {
            EnemyDistance = Vector2.Distance(Player.transform.position, transform.position);
            
            //HasTarget = HitBox.Colliders.Count > 0;

            UpdateDirection();
        }

        HasTarget = false;


        if (isBleeding == true)
        {
            StartCoroutine(Bleeding());
            particleSystem.enableEmission = true;
            //enable icon for bleeding
        }
        else
        {
            particleSystem.enableEmission = false;
            //disable icon for bleeding
        }

        //Increases add hp every X seconds
        Counter += Time.deltaTime;

        if (Counter > 30f)
        {
            EnemyMaxHP += 10;
            EnemyHP += 10;

            //reset counter
            Counter = 0;
        }

    }

    private void UpdateDirection()
    {
        EnemyToPlayer = Player.transform.position - transform.position;

        DirectionOfPlayer = EnemyToPlayer.normalized;
        RotateToPlayer();
        
    }

    private void RotateToPlayer()
    {
        Quaternion TargetRotation = Quaternion.LookRotation(transform.forward, DirectionOfPlayer);
        Quaternion rotation = Quaternion.RotateTowards(transform.rotation, TargetRotation, LookSpeed);
    
        //RB.SetRotation(rotation);
        if (Player.transform.position.x < this.transform.position.x)
        {
            spriteRenderer.flipX = true;
        }
        else
        {
            spriteRenderer.flipX = false;
        }
        
        Move();
    }

    private void Move()
    {
        if(EnemyHP >= 0 && playerController.isPaused == false)
        {
            if (HasTarget == true)
            {
                /*//attack anim and stuff
                Animator.SetBool("InRange", true);
                RB.velocity = transform.up * 0;
                //do damage to player
                playerController.playerHit();
                return;*/
            }
            else if (HasTarget == false)
            {
                Animator.SetBool("InRange", false);
                //RB.velocity = transform.up * MoveSpeed;
                Vector2 newPos = Vector2.MoveTowards(this.transform.position, Player.transform.position, 2f * Time.deltaTime);
                this.transform.position = newPos;
            }
        }
        else if (EnemyHP <= 0)
        {
            StartCoroutine(EnemyDeath());
        }
        
        
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
                Debug.Log(spellHolder.playerSpells[spellData]);
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
                    MoveSpeed = Mathf.Max(2f, MoveSpeed - spellData.speedReduction);
                }

                if (spellData.isKnockedBack)
                {
                    PushBack(spellData.ForceAmount, collision);
                }

                if (damageSounds.Length > 0)
                {
                    int randomIndex = Random.Range(0, damageSounds.Length);
                    audioSource.PlayOneShot(damageSounds[randomIndex]);

                }

                Debug.Log($"Spell: {spellData.name}, DeleteOnHit: {spellData.DeleteOnHit}");


                // Destroy the spell object after applying its effects
                if (spellData.DeleteOnHit == true)
                {
                    //Debug.Log("Spell was not one of the detected spells");
                    Destroy(collision.gameObject);
                }
                
                StartCoroutine(Damage(finalDamage));
            }
        }
    }

    public IEnumerator Damage(int DamageAmount)
    {
        EnemyHP -= DamageAmount;
        healthBar.UpdateHealthBar(EnemyHP, EnemyMaxHP);
        //Debug.Log(EnemyHP);
        spriteRenderer.color = Color.red;
        //Debug.Log(spriteRenderer.color);
        yield return new WaitForSeconds(0.1f);
        spriteRenderer.color = Color.white;

        yield return new WaitForSeconds(0.5f);
        //Debug.Log(spriteRenderer.color);

    }

    IEnumerator Bleeding()
    {
        int bleedTime = 2;
        int bleedTicks = 0;
        int bleedDamage = 5;

        yield return new WaitForSeconds(1f);
        while(isBleeding == true)
        {
            if (bleedTicks < bleedTime)
            {
                StartCoroutine(Damage(bleedDamage));
                
                bleedTicks++;
            }
            else
            {
                isBleeding = false;
            }
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            //attack anim and stuff
            Animator.SetBool("InRange", true);
            RB.velocity = transform.up * 0;
            //do damage to player
            playerController.playerHit();
            return;
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
        MoveSpeed = 0; // Disable movement
        yield return new WaitForSeconds(duration);
        MoveSpeed = 3f; // Restore movement
    }

    IEnumerator EnemyDeath()
    {
        Animator.SetBool("HasHP", false);
        MoveSpeed = 0;
        audioSource.PlayOneShot(DeathSound);
        myCircleCollider.enabled = false;
        
        yield return new WaitForSeconds(1f);

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
}


