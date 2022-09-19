using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Colectables
{
    Animal1,
    Planta1,
    Animal2,
    Planta2,
    Animal3,
    Planta3,
}
public enum ColectableType
{
    Animal,
    Planta,
}
public class KeyItem : Item
{
    [SerializeField] protected Colectables itemEnum;
    [SerializeField] protected ColectableType itemType;



    public override void CollectItem(Collider info)
    {
        base.CollectItem(info);
        if(info.gameObject.CompareTag("Player"))
        {
            Debug.Log("Colected: " + itemType + " " + itemEnum);
            Destroy(this.gameObject);
        }
    }

}
