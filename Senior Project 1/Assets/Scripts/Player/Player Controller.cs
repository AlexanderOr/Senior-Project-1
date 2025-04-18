using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;


public class PlayerController : MonoBehaviour
{
    public static int Player_MaxHP = 100;
    public static int Player_HP = 100;
    public static int Player_MaxEXP = 50;
    public static int Player_EXP = 0;
    public static int Player_Level = 0;
    public bool Invincible;
    float Iframes = 0.5f;
    float IframeTimer;
    public float Player_DodgeCD = 5;
    public float DodgeCDTimer;
    public float RollDist = 10;
    public bool Immune;
    float ImmuneTimer;
    private bool isCasting = false;

    public GameObject Indicator;

    public bool isPaused;

    public EnemyBehavior EnemyBehavior;
    public DodgeCoolDown dodgeCoolDown;
    public CastBarScript castBarScript;
    public SpriteRenderer spriteRenderer;
    public DamageEffect damageEffect;

    private const string Horizontal = "Horizontal";
    private const string Vertical = "Vertical";


    [SerializeField] private float MoveSpeed = 5f;

    private Vector2 Movement;
    private Rigidbody2D RB;
    private Animator animator;


    //spells
    public SpellHolder SpellHolder;
    public int currentSpellIndex = 0;

    //Spell Menu
    public SpellMenuManager SpellMenuManager;
    private Dictionary<int, float> spellCooldowns = new Dictionary<int, float>();

    //Spell UI
    public List<TextMeshProUGUI> spellCooldownTexts = new List<TextMeshProUGUI>(); // 5 cooldown texts

    //SFX
    public AudioClip[] damageSounds; // Array to hold the three sounds
    public AudioClip DeathSound;
    private AudioSource audioSource;
    public AudioClip[] CastSounds;


    //death anim
    public RenderTexture blurTexture;
    public Camera cam1;
    public Camera cam2;
    public GameObject PixelatedPanel;
    private int originalHeight;
    private int originalWidth;

    private void Awake()
    {
        RB = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        //Player = GameObject.FindGameObjectWithTag("Player").transform;
        Player_MaxEXP = 50;
        Player_Level = 0;
        Player_EXP = 0;
        Player_HP = 100;
        Player_MaxHP = 100;
        audioSource = GetComponent<AudioSource>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        ResetRenderTexture();
    }

    private void Update()
    {
        //spells
        HandleSpellSelection();
        HandleSpellCasting();
        UpdateSpellCooldownUI();

        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll < 0 && currentSpellIndex < 4 && !isCasting)
        {
            currentSpellIndex++;
        }

        if(scroll > 0 && currentSpellIndex > 0 && !isCasting)
        {
            currentSpellIndex--;
        }


        if (Player_EXP >= Player_MaxEXP)
        {
            LevelUp();
        }


        //movement
        if(isPaused == false)
        {
            Movement.Set(InputManager.Movement.x, InputManager.Movement.y);

            RB.velocity = Movement * MoveSpeed;

            animator.SetFloat(Horizontal, Movement.x);
            animator.SetFloat(Vertical, Movement.y);

            if(Movement.x < 0)
            {
                spriteRenderer.flipX = true;
            }
            else
            {
                spriteRenderer.flipX = false;

            }

            if (Input.GetKeyDown(KeyCode.Space) && Movement.x != 0 && DodgeCDTimer == 0)
            {
                RB.velocity = new Vector2((Movement.x * MoveSpeed) * RollDist, RB.velocity.y);
                DodgeCDTimer = Player_DodgeCD;
                dodgeCoolDown.UseDodge();
            }
            else if (Input.GetKeyDown(KeyCode.Space) && Movement.y != 0 && DodgeCDTimer == 0)
            {
                RB.velocity = new Vector2(RB.velocity.x, (Movement.y * MoveSpeed) * RollDist);
                DodgeCDTimer = Player_DodgeCD;
                dodgeCoolDown.UseDodge();
            }

            if (DodgeCDTimer > 0)
            {
                DodgeCDTimer -= Time.deltaTime;
            }

            if (DodgeCDTimer < 0)
            {
                DodgeCDTimer = 0;
            }



            if (Player_HP <= 0)
            {
                audioSource.PlayOneShot(DeathSound);
                StartCoroutine(DeathTransition());
                
            }
        }
        else if(isPaused == true)
        {
            RB.velocity = Vector2.zero;
        }
        

        //kill Enemy
        if (Input.GetKeyDown(KeyCode.E))
        {
            EnemyBehavior.EnemyHP = 0;
        }

        if(Invincible == true)
        {
            ApplyIframeCD();
        }

        if(Player_HP > Player_MaxHP)
        {
            Player_HP = Player_MaxHP;
        }

        if (Immune == true)
        {
            ImmuneTimer -= Time.deltaTime;
            Debug.Log(ImmuneTimer);
            Debug.Log(Immune);

            if(ImmuneTimer <= 0) 
            { 
                Immune = false; 
            }
        }

        if (Input.GetKeyDown(KeyCode.F1))
        {
            LevelUp();
        }

    }

    public void playerHit()
    {
        if (Invincible == false && isPaused == false && Immune == false)
        {
            Debug.Log("hit");
            Player_HP = Player_HP - 5;
            IframeCD();
            damageEffect.ShowDamageEffect();
            PlayRandomDamageSound();
            Debug.Log(Player_HP);            
        }        
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "EXP")
        {
            Destroy(collision.gameObject);
            Player_EXP += 5;
        }

        if(collision.tag == "Chest")
        {
            Destroy(collision.gameObject);
            //open spell menu
            SpellHolder.LevelUp();
            //"pause" game
            isPaused = true;
        }

        if(collision.tag == "EnemyBullet" && Immune == false)
        {
            Destroy(collision.gameObject);
            playerHit();
        }

    }

    public void IframeCD()
    {
        IframeTimer = Iframes;
        Invincible = true;
    }

    public void ApplyIframeCD()
    {
        IframeTimer -= Time.deltaTime;

        if( IframeTimer < 0 )
        {
            Invincible = false;
        }
        //Debug.Log(IframeTimer);
    }

    void HandleSpellSelection()
    {
        if(!isCasting)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1)) currentSpellIndex = 0;
            if (Input.GetKeyDown(KeyCode.Alpha2)) currentSpellIndex = 1;
            if (Input.GetKeyDown(KeyCode.Alpha3)) currentSpellIndex = 2;
            if (Input.GetKeyDown(KeyCode.Alpha4)) currentSpellIndex = 3;
            if (Input.GetKeyDown(KeyCode.Alpha5)) currentSpellIndex = 4;
        }
    }

    void HandleSpellCasting()
    {
        if (isPaused == false && !isCasting)
        {
            if (Input.GetMouseButton(0) && SpellHolder.playerSpells.Count > currentSpellIndex)
            {
                
                Spells currentSpell = SpellHolder.playerSpell[currentSpellIndex];

                if(GetSpellCooldown(currentSpellIndex) <= 0)
                {
                    //SpellHolder.playerSpell[currentSpellIndex].CastSpell(mousePos2, playerPosition);
                    StartCoroutine(CastSpell(currentSpell));
                }
                

            }
        }

        // Make a copy of the keys to safely iterate while modifying the dictionary
        List<int> keys = new List<int>(spellCooldowns.Keys);

        // Decrease cooldown timers over time
        foreach (var spell in keys)
        {
            if (spellCooldowns[spell] > 0)
            {
                spellCooldowns[spell] -= Time.deltaTime;

                
                if (spellCooldowns[spell] <= 0)
                {
                    spellCooldowns[spell] = 0;
                }
            }
        }
    }

    IEnumerator CastSpell(Spells spell)
    {
        castBarScript.ActivateBar(spell);
        // Apply speed reduction during casting (25% slower)
        float originalSpeed = MoveSpeed;
        MoveSpeed *= 0.75f; // Reducing player speed by 25%

        // Start the casting animation or effect (if any)
        isCasting = true;

        // Wait for the cast time to finish
        yield return new WaitForSeconds(spell.CastTime);

        // Once casting is done, cast the spell
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 mousePos2 = new Vector2(mousePosition.x, mousePosition.y);
        Debug.Log(mousePosition.x + " " + mousePosition.y);
        Vector2 playerPosition = transform.position;
        spell.CastSpell(mousePos2, playerPosition);

        float pitch = 1;

        pitch = Random.Range(.8f, 1.2f);

        audioSource.pitch = pitch;

        // SFX
        audioSource.PlayOneShot(spell.SpellSound);

        /*if (CastSounds.Length > 0)
        {
           //int randomIndex = Random.Range(0, CastSounds.Length);
            
        }*/


        // After casting is complete, restore player speed
        MoveSpeed = originalSpeed;

        // Set the cooldown for this specific spell
        SetSpellCooldown(currentSpellIndex, spell.CoolDown);

        // End casting
        isCasting = false;
    }

    void LevelUp()
    {
        //Player_Level++;
        Player_MaxEXP += (10 * Player_Level);
        Player_EXP = 0;
        SpellHolder.LevelUp();

    }

    public void Heal()
    {
        if (Player_HP < Player_MaxHP)
        {
            Player_HP += 15;
        }
        
    }

    public void HealOption(int amount)
    {
        if (Player_HP < Player_MaxHP)
        {
            Player_HP += amount;
        }

    }

    public void Teleport(Vector2 mousePos2)
    {
        transform.position = mousePos2;
    }

    public void ImmuneRock(float Duration)
    {
        Debug.Log("Is immune for " + Duration);
        Immune = true;
        ImmuneTimer = Duration;
    }

    public void TakeDamage(int damage)
    {
        if (Invincible == false && isPaused == false && Immune == false)
        {
            damageEffect.ShowDamageEffect();
            Player_HP -= damage;
            PlayRandomDamageSound();
            IframeCD();
        }
    }

    float GetSpellCooldown(int spellIndex)
    {
        if (spellCooldowns.ContainsKey(spellIndex))
        {
            return spellCooldowns[spellIndex];
        }
        return 0; // Default to no cooldown if not found
    }

    void SetSpellCooldown(int spellIndex, float cooldownTime)
    {
        if (spellCooldowns.ContainsKey(spellIndex))
        {
            spellCooldowns[spellIndex] = cooldownTime;
        }
        else
        {
            spellCooldowns.Add(spellIndex, cooldownTime);
        }
    }

    void UpdateSpellCooldownUI()
    {
        for (int i = 0; i < spellCooldownTexts.Count; i++)
        {
            float cooldown = GetSpellCooldown(i);

            if (cooldown > 0)
            {
                spellCooldownTexts[i].gameObject.SetActive(true);  // Enable the cooldown text
                spellCooldownTexts[i].text = Mathf.Ceil(cooldown).ToString();  // Update the cooldown text
            }
            else
            {
                spellCooldownTexts[i].gameObject.SetActive(false);  // Disable the cooldown text
            }
        }
    }

    private void PlayRandomDamageSound()
    {
        // Select a random sound from the array
        if (damageSounds.Length > 0)
        {
            int randomIndex = Random.Range(0, damageSounds.Length);
            audioSource.PlayOneShot(damageSounds[randomIndex]);
        }
    }

    IEnumerator DeathTransition()
    {
        isPaused = true;
        originalWidth = blurTexture.width;
        originalHeight = blurTexture.height;

        PixelatedPanel.SetActive(true);
        cam1.gameObject.SetActive(false);
        cam2.gameObject.SetActive(true);

        int steps = 75;
        float waitTime = 0.04f;

        for (int i = 0; i < steps; i++)
        {
            yield return new WaitForSeconds(waitTime);
            AdjustRenderTextureSize(i, steps);
        }

        SceneManager.LoadScene("GameOver");
    }

    private void AdjustRenderTextureSize(int step, int totalSteps)
    {
        blurTexture.Release();

        int newWidth = Mathf.Max(1, (int)(originalWidth * (totalSteps - step) / totalSteps));
        int newHeight = Mathf.Max(1, (int)(originalHeight * (totalSteps - step) / totalSteps));

        blurTexture.width = newWidth;
        blurTexture.height = newHeight;
        blurTexture.Create();
    }

    private void ResetRenderTexture()
    {
        blurTexture.Release();
        blurTexture.width = 1920;
        blurTexture.height = 1080;
        blurTexture.Create();
    }

    private void OnApplicationQuit()
    {
        ResetRenderTexture();
    }


}
