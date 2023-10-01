using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgresBarUI : MonoBehaviour
{
    [SerializeField] private GameObject progressGameObject;
    [SerializeField] private Image image;

    private IHasProgress progress;

    private void Start()
    {
        progress = progressGameObject.GetComponent<IHasProgress>();
        progress.OnProgressChanged += IHasProgress_OnProgressChanged;
        image.fillAmount = 0;
        Hide();
    }

    private void IHasProgress_OnProgressChanged(object sender, IHasProgress.ProgressEventArgs e)
    {
        image.fillAmount = e.progressNormalized;
        if (image.fillAmount >= 1)
        {
            Hide();
        }
        else
        {
            Show();
        }
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }

    private void Show()
    {
        gameObject.SetActive(true);
    }
}
