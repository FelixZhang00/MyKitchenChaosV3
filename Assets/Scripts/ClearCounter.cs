using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearCounter : BaseCounter,IKitchenObjectParent
{

    [SerializeField] private KitchenObjectSO kitchenObjectSO;
    [SerializeField] private Transform counterTopPoint;

    private KitchenObject kitchenObject;

    public override void Interact(Player player) {
        if (kitchenObject == null)
        {
            Transform kitchenObjectTransform = Instantiate(kitchenObjectSO.perfab, counterTopPoint);
            kitchenObjectTransform.GetComponent<KitchenObject>().SetKitchenObjectParent(this);
        }
        else {
            kitchenObject.SetKitchenObjectParent(player);
        }

    }

    public Transform GetKitchenObjectFollowTransform()
    {
        return counterTopPoint;
    }

    public KitchenObject GetKitchenObject() {
        return kitchenObject;
    }

    public void SetKitchenObject(KitchenObject kitchenObject) { 
        this.kitchenObject = kitchenObject;
    }

    public void ClearKitchenObject() {
        this.kitchenObject = null;
    }

    public bool HasKitchenObject() {
        return kitchenObject != null;
    }

}
