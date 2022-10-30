using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//using UnityEngine.Rendering.PostProcessing;

public class PlayerHealth : Health
{
    //[SerializeField] private Image bar;
    private Image bar => GameState.mainCanvas.healthBar;
    //[SerializeField] private PostProcessVolume damageEffect;
    private float damageTime = 0;
    [SerializeField] float effectTimeMultplier = 10;
    [SerializeField] float effectGainMultplier = 2f;
    [SerializeField] float effectDownMultplier = 0.5f;
    [SerializeField] float fallingDeathHeight = -1000;
    public bool IsMaxHealth => health >= maxHealth;
    public AudioSource audioSource;
    //private GameState state;
    public bool dead;
    

    public override void Start()
    {
        base.Start();
        bar.fillAmount = health / maxHealth;
        UpdateHealth();
        //state = GetComponent<GameState>();
    }

    void Update()
    {
        if(GameState.GodMode) UpdateHealth(maxHealth);

        if(Input.GetButtonDown("GodMode"))
        {
            GameState.ToogleGodMode();
            Debug.Log("GOD MODE: " + GameState.GodMode);
        }

        // if(damageEffect.weight > 0)
        // {
        //     if(damageTime < 0)
        //     {
        //         damageEffect.weight -= 1f * Time.deltaTime * effectDownMultplier;
        //         damageTime = 0;
        //     }
        //     else
        //     {
        //         damageTime -= 1f * Time.deltaTime;
        //     }
        // }

        // if(state.playerDead)
        // {
        //     damageEffect.weight += Time.deltaTime;
        //     gameOver.SetActive(true);
        // } 

        if(transform.position.y < fallingDeathHeight) DestroyCharacter();
        
    }

    public override void UpdateHealth(float value = 0)//, Item item = null)
    {
        //if(health < maxHealth && item != null) item.DestroyItem();
         
        base.UpdateHealth(value);
        bar.fillAmount = health / maxHealth;

        // var hpPorcentage = Mathf.Abs(health / maxHealth);
        // var chgPorcentage = Mathf.Abs(value / maxHealth);

        // if(value < 0)
        // {
        //     damageEffect.weight += chgPorcentage * effectGainMultplier;
        //     damageTime += chgPorcentage * effectTimeMultplier;
        // }
        // else if(value > 0)
        // {
        //     damageEffect.weight -= chgPorcentage * effectGainMultplier;
        //     damageTime -= chgPorcentage * effectTimeMultplier;
        // }
    }

    public override void DestroyCharacter()
    {
        if(GameState.IsPlayerDead) return;
        //anim.SetTrigger("Die");
        
        if(audioSource != null) audioSource?.Play();
        GameState.IsPlayerDead = true;
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.None;
        foreach (var item in GetComponentsInChildren<Collider>())
        {
            if(item is CharacterController) continue;
            item.enabled = false;
        }
        foreach (var item in GetComponentsInChildren<MonoBehaviour>())
        {
            if(item == this || item is Movimento || item is GameState) continue;
            item.enabled = false;
        }
        GameState.mainCanvas.ResumeGame();
        Cursor.lockState = CursorLockMode.None;
        GameState.mainCanvas.gameOver.SetActive(true);
    }
}
