using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class ConfirmUI : MonoBehaviour
{
    public TMP_Text messageText;
    public Button yesButton;
    public Button noButton;
    public GameObject dimBackground;

    private Action onYes;
    private Action onNo;

    public void Show(string message, System.Action yesAction, System.Action noAction)
    {
        gameObject.SetActive(true);
        if (dimBackground != null) dimBackground.SetActive(true);

        messageText.text = message;
        onYes = yesAction;
        onNo = noAction;

        yesButton.onClick.RemoveAllListeners();
        noButton.onClick.RemoveAllListeners();

        yesButton.onClick.AddListener(() =>
        {
            onYes?.Invoke();
            Close();
        });

        noButton.onClick.AddListener(() =>
        {
            onNo?.Invoke();
            Close();
        });
    }

    private void Close()
    {
        gameObject.SetActive(false);
        if (dimBackground != null) dimBackground.SetActive(false);
    }
}
