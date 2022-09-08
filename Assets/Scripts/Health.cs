using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class Health : MonoBehaviour
{
    public float maxHealth;
    [SerializeField] protected float health;
    //[SerializeField] protected AudioSource source;
    //[SerializeField] protected AudioClip damageSound;
    protected Animator anim;
    public virtual void Start()
    {
        health = maxHealth;
        //anim = GetComponentInChildren<Animator>();
    }

    public virtual void UpdateHealth(float value = 0)//, Item item = null)
    {
        health += value;

        if(health > maxHealth) health = maxHealth;
        if(health <= 0) DestroyCharacter();

        if(value < 0)
        {
            if(anim != null) anim.SetTrigger("Damage");
            //if(source != null) source.PlayOneShot(damageSound);
            //else Debug.LogError("NO AUDIO SOURCE FOR DAMAGE");
        }
    }

    public virtual void DestroyCharacter()
    {
        Destroy(this.gameObject);
    }
}
