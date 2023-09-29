using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    
    [SerializeField] private GameInput input;
    [SerializeField] private float moveSpeed = 7.0f;

    // Update is called once per frame
    void Update()
    {
        Vector2 inputVector = input.GetPlayerMoveInput();
        Vector3 moveDir = new Vector3(inputVector.x, 0, inputVector.y);

        float moveDistance = moveSpeed * Time.deltaTime;

        transform.position += moveDir * moveDistance;
        float rotateSpeed = 10f;
        transform.forward = Vector3.Slerp(transform.forward, moveDir, Time.deltaTime * rotateSpeed);
    }
}
