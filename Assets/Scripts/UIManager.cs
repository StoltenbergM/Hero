using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;  // Simple Singleton
    public ConfirmUI confirmUIPrefab;  // Prefab reference (or scene reference)
    private ConfirmUI activeConfirmUI;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void ShowConfirm(string message, System.Action onYes, System.Action onNo)
    {
        // If one already exists, reuse it
        if (activeConfirmUI == null)
        {
            activeConfirmUI = Instantiate(confirmUIPrefab, FindFirstObjectByType<Canvas>().transform);
        }

        activeConfirmUI.Show(message, onYes, onNo);
    }
}
