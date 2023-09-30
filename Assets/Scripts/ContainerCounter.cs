using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContainerCounter : BaseCounter
{



    public override void Interact(Player player)
    {
            Transform kitchenObjectTransform = Instantiate(kitchenObjectSO.perfab);
            kitchenObjectTransform.GetComponent<KitchenObject>().SetKitchenObjectParent(player);
    }



}