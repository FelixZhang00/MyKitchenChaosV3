using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static IHasProgress;
public class StoveCounter : BaseCounter, IHasProgress
{
    [SerializeField] private FryingRecipeSO[] fryingRecipeSOArray;
    [SerializeField] private BurnedRecipeSO[] burnedRecipeSOArray;
    private FryingRecipeSO fryingRecipeSO;
    private BurnedRecipeSO burnedRecipeSO;

    public event EventHandler<StateChangedArgs> OnStateChanged;
    public class StateChangedArgs : EventArgs {
        public State state;
    }

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

    public event EventHandler<ProgressEventArgs> OnProgressChanged;

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
                    OnProgressChanged?.Invoke(this, new ProgressEventArgs()
                    {
                        progressNormalized = fryingTime / fryingRecipeSO.fryingTimerMax
                    });
                    if (fryingTime > fryingRecipeSO.fryingTimerMax) {


                        GetKitchenObject().DestorySelf();
                        KitchenObject.SpawnKitchenObject(fryingRecipeSO.output, this);
                        burnedRecipeSO = GetBurnedRecipeSO(fryingRecipeSO.output);

                        fryingTime = 0f;
                        state = State.Fired;
                        OnStateChanged?.Invoke(this, new StateChangedArgs()
                        {
                            state = state
                        });
                    }
                    break;
                case State.Fired:
                    burnTime += Time.deltaTime;
                    OnProgressChanged?.Invoke(this, new ProgressEventArgs()
                    {
                        progressNormalized = burnTime / burnedRecipeSO.burnedTimerMax
                    });
                    if (burnTime > burnedRecipeSO.burnedTimerMax) {
                        GetKitchenObject().DestorySelf();
                        KitchenObject.SpawnKitchenObject(burnedRecipeSO.output, this);

                        burnTime = 0f;
                        state = State.Burned;
                        OnStateChanged?.Invoke(this, new StateChangedArgs()
                        {
                            state = state
                        });

                        OnProgressChanged?.Invoke(this, new ProgressEventArgs()
                        {
                            progressNormalized = 0
                        });
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
                    OnStateChanged?.Invoke(this, new StateChangedArgs()
                    {
                        state = state
                    });

                    OnProgressChanged?.Invoke(this, new ProgressEventArgs()
                    {
                        progressNormalized = fryingTime / fryingRecipeSO.fryingTimerMax
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

                        state = State.Idle;

                        OnStateChanged?.Invoke(this, new StateChangedArgs
                        {
                            state = state
                        });

                        OnProgressChanged?.Invoke(this, new IHasProgress.ProgressEventArgs
                        {
                            progressNormalized = 0f
                        });
                    }
                }
            }
            else
            {
                GetKitchenObject().SetKitchenObjectParent(player);

                state = State.Idle;
                OnStateChanged?.Invoke(this, new StateChangedArgs()
                {
                    state = state
                });
                OnProgressChanged?.Invoke(this, new ProgressEventArgs()
                {
                    progressNormalized = 0
                });
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

    public bool IsFried()
    {
        return state == State.Fired;
    }
}
