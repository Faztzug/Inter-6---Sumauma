using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaskDamage : MonoBehaviour
{
    [SerializeField] private float damage;
    [SerializeField] private float knockBackForce;

    private void OnCollisionEnter(Collision other) 
    {
        if(other.gameObject.CompareTag("Enemy"))
        {
            if(other.gameObject.TryGetComponent<Health>(out Health enemyHeatlh))
            {
                enemyHeatlh.UpdateHealth(damage);
            }
            Debug.Log("BODY?? " + (other.gameObject.GetComponent<Rigidbody>() != null));
            if(other.gameObject.TryGetComponent<Rigidbody>(out Rigidbody enemyBody))
            {
                Debug.Log("KNOCK BACK");
                var vectorDistance = transform.position - this.transform.position;
                var knockBackDir = vectorDistance.normalized;
                knockBackDir.y = 10;
                enemyBody.AddForce(knockBackDir * knockBackForce, ForceMode.Impulse);
            }
        }
    }
    
    private void OnValidate() 
    {
        if(damage > 0) damage = -damage;
    }
}
