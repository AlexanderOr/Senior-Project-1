using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DodgeCoolDown : MonoBehaviour
{

    public PlayerController playerController;

    public Image imageCD;
    public TMP_Text textCD;

    private bool Cooldown = false;
    private float CooldownTimer = 0.0f;


    // Start is called before the first frame update
    void Start()
    {
        textCD.gameObject.SetActive(false);
        imageCD.fillAmount = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        if(playerController.isPaused == false)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                UseDodge();
            }
            if (Cooldown)
            {
                ApplyCD();
            }
        }
        
    }

    void ApplyCD()
    {
        CooldownTimer -= Time.deltaTime;

        if(CooldownTimer < 0.0f )
        {
            Cooldown = false;
            textCD.gameObject.SetActive(false);
            imageCD.fillAmount = 0.0f;
        }
        else
        {
            textCD.text = Mathf.RoundToInt(CooldownTimer).ToString();
            imageCD.fillAmount = CooldownTimer / playerController.Player_DodgeCD;
        }
    }

    public void UseDodge()
    {
        if (Cooldown)
        {
            //user tried to roll while on CD
        }
        else
        {
            Cooldown = true;
            textCD.gameObject.SetActive(true);
            CooldownTimer = playerController.Player_DodgeCD;
        }
    }
}
