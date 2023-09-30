    using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectedCounterVisual : MonoBehaviour
{

    [SerializeField] private GameObject visualGameObject;
    [SerializeField] private ClearCounter clearCounter;

    void Start()
    {
        Player.Instance.OnSelectCounterHandler += Player_OnSelectCounterHandler;
    }

    private void Player_OnSelectCounterHandler(object sender, Player.SelectCounterEventArgs e)
    {
        if (e.selectCounter == clearCounter)
        {
            Show();
        }
        else {
            Hide();
        }
    }

    private void Hide() { 
        visualGameObject.SetActive(false);
    }

    private void Show() {
        visualGameObject.SetActive(true);
    }

}
