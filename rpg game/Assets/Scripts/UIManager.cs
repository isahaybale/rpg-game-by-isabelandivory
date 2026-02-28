using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [Header("Save UI")]
    [SerializeField] private GameObject savePanel;

    [Header("Sleep Dialog")]
    [SerializeField] private GameObject sleepDialogUI;
    [SerializeField] private Button yesButton;
    [SerializeField] private Button noButton;

    [Header("Fade")]
    [SerializeField] private ScreenFader fader;
    [SerializeField] private float fadeDuration = 0.5f;
    [SerializeField] private float blackHoldSeconds = 1.0f;

    private Action onSleepConfirmed;
    private Action onSleepCancelled;
    private bool dialogOpen;
    private bool sleepSequenceRunning;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        // If your Canvas/FadeOverlay is separate object, also DontDestroy it (see Step 3)
    }

    private void Start()
    {
        savePanel.SetActive(false);
        sleepDialogUI.SetActive(false);

        yesButton.onClick.RemoveAllListeners();
        noButton.onClick.RemoveAllListeners();

        yesButton.onClick.AddListener(() => ConfirmSleep());
        noButton.onClick.AddListener(() => CancelSleep());
    }

    public void ShowSleepDialog(Action onConfirm, Action onCancel = null)
    {
        if (sleepSequenceRunning) return;

        onSleepConfirmed = onConfirm;
        onSleepCancelled = onCancel;

        GameState.CanPlayerMove = false;   // 🔒 LOCK movement

        dialogOpen = true;
        sleepDialogUI.SetActive(true);
    }

    public void HideSleepDialog()
    {
        dialogOpen = false;
        sleepDialogUI.SetActive(false);

        GameState.CanPlayerMove = true;   // 🔓 UNLOCK movement
    }

    private void ConfirmSleep()
    {
        if (!dialogOpen || sleepSequenceRunning) return;
        HideSleepDialog();
        onSleepConfirmed?.Invoke();
    }

    private void CancelSleep()
    {
        if (!dialogOpen || sleepSequenceRunning) return;

        HideSleepDialog();
        onSleepCancelled?.Invoke();

        GameState.CanPlayerMove = true;   // 🔓 UNLOCK movement
    }

    public void RunSleepFade(Action duringBlack)
    {
        if (sleepSequenceRunning) return;
        StartCoroutine(SleepFadeRoutine(duringBlack));
    }

    private IEnumerator SleepFadeRoutine(Action duringBlack)
    {
        sleepSequenceRunning = true;
        GameState.CanPlayerMove = false;

        yield return fader.FadeTo(1f, fadeDuration);
        yield return new WaitForSecondsRealtime(blackHoldSeconds);

        duringBlack?.Invoke();

        yield return fader.FadeTo(0f, fadeDuration);

        sleepSequenceRunning = false;
        GameState.CanPlayerMove = true;
    }

    public void ShowSavePanel()
{
    GameState.CanPlayerMove = false;
    savePanel.SetActive(true);
}

public void HideSavePanel()
{
    savePanel.SetActive(false);
    GameState.CanPlayerMove = true;
}

public void OnSaveYes()
{
    SaveGame();
    HideSavePanel();
}

public void OnSaveNo()
{
    HideSavePanel();
}

private void SaveGame()
{
    PlayerPrefs.SetInt("Saved", 1);
    PlayerPrefs.Save();
    Debug.Log("Game Saved!");
}
}