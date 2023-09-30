using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearCounter : MonoBehaviour
{

    [SerializeField] private Transform tomatoPrefab;
    [SerializeField] private Transform tomatoTopPos;

    public void Interact() {
        Debug.Log("interact");
        Transform tomatoTransform =  Instantiate(tomatoPrefab,tomatoTopPos);
        tomatoTransform.localPosition = Vector3.zero;
    }
}
