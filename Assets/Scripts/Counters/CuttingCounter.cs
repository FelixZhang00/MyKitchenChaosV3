using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static IHasProgress;

public class CuttingCounter : BaseCounter,IHasProgress
{

    [SerializeField] private CuttingRecipeSO[] CuttingRecipeSOArray;

    private int cuttingProgress;

    public static event EventHandler OnAnyCut;
    public event EventHandler OnCut;
    public event EventHandler<ProgressEventArgs> OnProgressChanged;

    new public static void ResetStaticData()
    {
        OnAnyCut = null;
    }

    public override void Interact(Player player)
    {
        if (!HasKitchenObject())
        {
            if (player.HasKitchenObject())
            {
                if (HasRecipeWithInput(player.GetKitchenObject().GetKitchenObjectSO())) {
                    player.GetKitchenObject().SetKitchenObjectParent(this);

                    cuttingProgress = 0;
                    CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSO(GetKitchenObject().GetKitchenObjectSO());
                    int maxProgress = cuttingRecipeSO?.progressMax ?? 0;
                    OnProgressChanged?.Invoke(this, new ProgressEventArgs()
                    {
                        progressNormalized = (float)cuttingProgress / maxProgress
                    }) ;
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
                // Player is carrying something
                if (player.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject))
                {
                    // Player is holding a Plate
                    if (plateKitchenObject.TryAddIngredient(GetKitchenObject().GetKitchenObjectSO()))
                    {
                        GetKitchenObject().DestorySelf();
                    }
                }
            }
            else
            {
                GetKitchenObject().SetKitchenObjectParent(player);
                OnProgressChanged?.Invoke(this, new ProgressEventArgs()
                {
                    progressNormalized = 0
                });
            }
        }
    }

    public override void InteractAlternate(Player player)
    {
        if (HasKitchenObject() && HasRecipeWithInput(GetKitchenObject().GetKitchenObjectSO())) {
            KitchenObjectSO outputKitchenObjectSO = GetOutputForInput(GetKitchenObject().GetKitchenObjectSO());

            cuttingProgress++;
            CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSO(GetKitchenObject().GetKitchenObjectSO());
            int maxProgress = cuttingRecipeSO?.progressMax ?? 0;
            if (maxProgress>0) {
                OnProgressChanged?.Invoke(this, new ProgressEventArgs()
                {
                    progressNormalized = (float)cuttingProgress / maxProgress
                });
                OnCut?.Invoke(this, EventArgs.Empty);
                OnAnyCut?.Invoke(this, EventArgs.Empty);
            }

            if (cuttingProgress >= maxProgress) {
                GetKitchenObject().DestorySelf();
                if (outputKitchenObjectSO != null)
                {
                    KitchenObject.SpawnKitchenObject(outputKitchenObjectSO, this);
                }
            }
        }
    }


    private bool HasRecipeWithInput(KitchenObjectSO inputKitchenObjectSO) {
        foreach (CuttingRecipeSO cuttingRecipeSO in CuttingRecipeSOArray)
        {
            if (cuttingRecipeSO.input == inputKitchenObjectSO)
            {
                return true;
            }
        }
        return false;
    }


    private KitchenObjectSO GetOutputForInput(KitchenObjectSO inputKitchenObjectSO) {
        foreach (CuttingRecipeSO cuttingRecipeSO in CuttingRecipeSOArray) {
            if (cuttingRecipeSO.input == inputKitchenObjectSO) {
                return cuttingRecipeSO.output;
            }
        }
        return null;
    }

    private CuttingRecipeSO GetCuttingRecipeSO(KitchenObjectSO inputKitchenObjectSO)
    {
        foreach (CuttingRecipeSO cuttingRecipeSO in CuttingRecipeSOArray)
        {
            if (cuttingRecipeSO.input == inputKitchenObjectSO)
            {
                return cuttingRecipeSO;
            }
        }
        return null;
    }

}
