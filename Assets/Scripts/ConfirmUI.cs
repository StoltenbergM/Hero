using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class ConfirmUI : MonoBehaviour
{
    public TMP_Text messageText;
    public Button yesButton;
    public Button noButton;

    private Action onYes;
    private Action onNo;

    public void Show(string message, Action yesAction, Action noAction)
    {
        messageText.text = message;
        onYes = yesAction;
        onNo = noAction;
        gameObject.SetActive(true);
    }

    public void OnYes()
    {
        gameObject.SetActive(false);
        onYes?.Invoke();
    }

    public void OnNo()
    {
        gameObject.SetActive(false);
        onNo?.Invoke();
    }
}
