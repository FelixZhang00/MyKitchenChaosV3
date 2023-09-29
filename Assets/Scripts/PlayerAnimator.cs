using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimotor : MonoBehaviour
{

    [SerializeField] Player player;
    private const string IS_WALKING = "IsWalking";

    private Animator animator;
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }


    // Update is called once per frame
    void Update()
    {
        animator.SetBool(IS_WALKING, player.IsWalking());
    }
}
