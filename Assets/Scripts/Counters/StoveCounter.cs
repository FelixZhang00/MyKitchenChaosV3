using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoveCounter : BaseCounter
{
    [SerializeField] private FryingRecipeSO[] fryingRecipeSOArray;
    [SerializeField] private BurnedRecipeSO[] burnedRecipeSOArray;
    private FryingRecipeSO fryingRecipeSO;
    private BurnedRecipeSO burnedRecipeSO;

    public enum State
    {
        Idle,
        Frying,
        Fired,
        Burned,
    }

    private State state;
    private float fryingTime;
    private float burnTime;

    private void Start()
    {
        state = State.Idle;
    }

    private void Update()
    {
        if (HasKitchenObject())
        {
            switch (state)
            {
                case State.Idle:
                    break;
                case State.Frying:
                    fryingTime += Time.deltaTime;
                  
                    if (fryingTime > fryingRecipeSO.fryingTimerMax) {


                        GetKitchenObject().DestorySelf();
                        KitchenObject.SpawnKitchenObject(fryingRecipeSO.output, this);
                        burnedRecipeSO = GetBurnedRecipeSO(fryingRecipeSO.output);

                        fryingTime = 0f;
                        state = State.Fired;
                    }
                    break;
                case State.Fired:
                    burnTime += Time.deltaTime;
                    if (burnTime > burnedRecipeSO.burnedTimerMax) {
                        GetKitchenObject().DestorySelf();
                        KitchenObject.SpawnKitchenObject(burnedRecipeSO.output, this);

                        burnTime = 0f;
                        state = State.Burned;
                    }
                    break;
                case State.Burned:
                    break;
            }
        }

    }

    public override void Interact(Player player)
    {
        if (!HasKitchenObject())
        {
            if (player.HasKitchenObject())
            {
                if (HasRecipeWithInput(player.GetKitchenObject().GetKitchenObjectSO()))
                {
                    fryingRecipeSO = GetFryingRecipeSO(player.GetKitchenObject().GetKitchenObjectSO());
                    player.GetKitchenObject().SetKitchenObjectParent(this);
                    fryingTime = 0f;
                    state = State.Frying;
                }
            }
            else
            {

            }
        }
        else
        {
            if (player.HasKitchenObject())
            {

            }
            else
            {
                GetKitchenObject().SetKitchenObjectParent(player);

                state = State.Idle;
            }
        }
    }

    private bool HasRecipeWithInput(KitchenObjectSO inputKitchenObjectSO)
    {
        foreach (FryingRecipeSO fryingRecipeSO in fryingRecipeSOArray)
        {
            if (fryingRecipeSO.input == inputKitchenObjectSO)
            {
                return true;
            }
        }
        return false;
    }


    private KitchenObjectSO GetOutputForInput(KitchenObjectSO inputKitchenObjectSO)
    {
        foreach (FryingRecipeSO fryingRecipeSO in fryingRecipeSOArray)
        {
            if (fryingRecipeSO.input == inputKitchenObjectSO)
            {
                return fryingRecipeSO.output;
            }
        }
        return null;
    }

    private FryingRecipeSO GetFryingRecipeSO(KitchenObjectSO inputKitchenObjectSO)
    {
        foreach (FryingRecipeSO fryingRecipeSO in fryingRecipeSOArray)
        {
            if (fryingRecipeSO.input == inputKitchenObjectSO)
            {
                return fryingRecipeSO;
            }
        }
        return null;
    }

    private BurnedRecipeSO GetBurnedRecipeSO(KitchenObjectSO inputKitchenObjectSO)
    {
        foreach (BurnedRecipeSO burnedRecipeSO in burnedRecipeSOArray)
        {
            if (burnedRecipeSO.input == inputKitchenObjectSO)
            {
                return burnedRecipeSO;
            }
        }
        return null;
    }
}
