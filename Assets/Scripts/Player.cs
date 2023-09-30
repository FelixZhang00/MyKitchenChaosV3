using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    
    public static Player Instance { get; private set; }

    [SerializeField] private GameInput gameInput;
    [SerializeField] private float moveSpeed = 7.0f;
    [SerializeField] private LayerMask counterLayerMask;


    private bool isWalking = false;
    private Vector3 lastInteractDir;
    private ClearCounter selectedClearCounter;

    public event EventHandler<SelectCounterEventArgs> OnSelectCounterHandler;
    public class SelectCounterEventArgs: EventArgs {
       public ClearCounter selectCounter;
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
        selectedClearCounter?.Interact();
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
            if (raycastHit.transform.TryGetComponent<ClearCounter>(out ClearCounter clearCounter))
            {
                if (selectedClearCounter != clearCounter) {
                    SetSelectedCounter(clearCounter);
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

    private void SetSelectedCounter(ClearCounter clearCounter) {
        selectedClearCounter = clearCounter;

        OnSelectCounterHandler?.Invoke(this, new SelectCounterEventArgs
        {
            selectCounter = selectedClearCounter
        });
    }
}
