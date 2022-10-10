using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaskDamage : MonoBehaviour
{
    [SerializeField] private float damage;

    private void OnCollisionEnter(Collision other) 
    {
        if(other.gameObject.CompareTag("Enemy") && transform.parent == null)
        {
            if(other.gameObject.TryGetComponent<Health>(out Health enemyHeatlh))
            {
                enemyHeatlh.UpdateHealth(damage);
            }
            Debug.Log("BODY?? " + (other.gameObject.GetComponent<Rigidbody>() != null));
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
