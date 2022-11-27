using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaskDamage : MonoBehaviour
{
    [SerializeField] private float damage;
    [SerializeField] private AudioSource audioSource;
    public Sound[] hitSounds;
    public Sound[] hitSounds2;

    private void OnCollisionEnter(Collision other) 
    {
        if(other.gameObject.CompareTag("Enemy") && transform.parent == null)
        {
            if(other.gameObject.TryGetComponent<Health>(out Health enemyHeatlh))
            {
                var index = Random.Range(0, hitSounds.Length);
                hitSounds[index].PlayOn(audioSource);

                enemyHeatlh.UpdateHealth(damage);
            }
            if(other.gameObject.TryGetComponent<EnemyFireGenericIA>(out EnemyFireGenericIA enemy))
            {
                enemy.KnockBack(this.transform.position);
            }
        }
    }
    
    private void OnValidate() 
    {
        if(damage > 0) damage = -damage;
    }
}
