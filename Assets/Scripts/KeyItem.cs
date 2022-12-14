using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Colectables
{
    Onca,
    Heliconia,
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

    protected override void Start()
    {
        base.Start();
        if(GameState.KeyItemAlreadyColected(itemType)) gameObject.SetActive(false);
    }

    public override void CollectItem(Collider info)
    {
        base.CollectItem(info);
        if(info.gameObject.CompareTag("Player"))
        {
            Debug.Log("Colected: " + itemType + " " + itemEnum);
            GameState.ItemColected(itemEnum, itemType);
            DestroyItem();
        }
    }

}
