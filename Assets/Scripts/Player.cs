using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour,IKitchenObjectParent
{
    
    public static Player Instance { get; private set; }

    [SerializeField] private GameInput gameInput;
    [SerializeField] private float moveSpeed = 7.0f;
    [SerializeField] private LayerMask counterLayerMask;
    [SerializeField] private Transform kitchenObjectHoldPiont;


    private bool isWalking = false;
    private Vector3 lastInteractDir;
    private BaseCounter selectedBaseCounter;
    private KitchenObject kitchenObject;

    public event EventHandler<SelectCounterEventArgs> OnSelectCounterHandler;
    public class SelectCounterEventArgs: EventArgs {
       public BaseCounter selectCounter;
    }

    private void Awake()
    {
        if (Instance != null) {
            Debug.Log("Player Instance is not null!");
        }
        Instance = this;
    }

    private void Start()
    {
        gameInput.OnInteractHandler += GameInput_OnInteractHandler;
    }

    private void GameInput_OnInteractHandler(object sender, EventArgs e)
    {
        selectedBaseCounter?.Interact(this);
    }

    // Update is called once per frame
    void Update()
    {
        HandleMovement();
        HandleInteractions();

    }

    private void HandleInteractions()
    {
        Vector2 inputVector = gameInput.GetPlayerMoveInput();
        Vector3 moveDir = new Vector3(inputVector.x, 0, inputVector.y);

        if (moveDir != Vector3.zero) {
            lastInteractDir = moveDir;
        }

        float interactDistance = 2f;
        if (Physics.Raycast(transform.position, lastInteractDir, out RaycastHit raycastHit, interactDistance, counterLayerMask))
        {
            if (raycastHit.transform.TryGetComponent<BaseCounter>(out BaseCounter baseCounter))
            {
                if (selectedBaseCounter != baseCounter) {
                    SetSelectedCounter(baseCounter);
                }
            }
            else {
                SetSelectedCounter(null);
            }
        }
        else
        {
            SetSelectedCounter(null);
        }
    }

    private void HandleMovement()
    {
        Vector2 inputVector = gameInput.GetPlayerMoveInput();
        Vector3 moveDir = new Vector3(inputVector.x, 0, inputVector.y);
        isWalking = moveDir != Vector3.zero;
        float moveDistance = moveSpeed * Time.deltaTime;

        float playerRadius = .7f;
        float playerHeight = 2f;
        bool canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDir, moveDistance);

        if (canMove)
        {
            transform.position += moveDir * moveDistance;
        }

        float rotateSpeed = 10f;
        if (isWalking)
        {
            transform.forward = Vector3.Slerp(transform.forward, moveDir, Time.deltaTime * rotateSpeed);
        }
    }

    public bool IsWalking()
    {
        return isWalking;
    }

    private void SetSelectedCounter(BaseCounter baseCounter) {
        selectedBaseCounter = baseCounter;

        OnSelectCounterHandler?.Invoke(this, new SelectCounterEventArgs
        {
            selectCounter = selectedBaseCounter
        });
    }

    public Transform GetKitchenObjectFollowTransform()
    {
        return kitchenObjectHoldPiont;
    }

    public KitchenObject GetKitchenObject()
    {
        return kitchenObject;
    }

    public void SetKitchenObject(KitchenObject kitchenObject)
    {
        this.kitchenObject = kitchenObject;
    }

    public void ClearKitchenObject()
    {
        this.kitchenObject = null;
    }

    public bool HasKitchenObject()
    {
        return kitchenObject != null;
    }
}
