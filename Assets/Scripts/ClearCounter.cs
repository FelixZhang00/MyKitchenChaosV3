using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearCounter : MonoBehaviour
{

    [SerializeField] private KitchenObjectSO kitchenObjectSO;
    [SerializeField] private Transform counterTopPoint;

    private KitchenObject kitchenObject;

    public void Interact() {
        if (kitchenObject == null) {
            Transform kitchenObjectTransform = Instantiate(kitchenObjectSO.perfab, counterTopPoint);
            kitchenObjectTransform.localPosition = Vector3.zero;
            kitchenObject = kitchenObjectTransform.GetComponent<KitchenObject>();
            kitchenObject.SetClearCounter(this);
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
}
