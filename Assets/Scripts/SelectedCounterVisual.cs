    using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectedCounterVisual : MonoBehaviour
{

    [SerializeField] private GameObject[] visualGameObjectArray;
    [SerializeField] private BaseCounter baseCounter;

    void Start()
    {
        Player.Instance.OnSelectCounterHandler += Player_OnSelectCounterHandler;
    }

    private void Player_OnSelectCounterHandler(object sender, Player.SelectCounterEventArgs e)
    {
        if (e.selectCounter == baseCounter)
        {
            Show();
        }
        else {
            Hide();
        }
    }

    private void Hide() { 
        foreach (var item in visualGameObjectArray)
        {
            item.SetActive(false);
        }

    }

    private void Show() {
        foreach (var item in visualGameObjectArray)
        {
            item.SetActive(true);
        }
    }

}
