using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Manages all UI elements for the Harmony in Diversity experience
/// </summary>
public class HarmonyUIManager : MonoBehaviour
{
    public static HarmonyUIManager Instance;

    [Header("UI Canvas")]
    [Tooltip("Main world-space canvas for all UI")]
    public Canvas mainCanvas;

    [Header("Text Elements")]
    [Tooltip("Text display for narration (Scene transitions)")]
    public TextMeshProUGUI narrationText;

    [Tooltip("Text display for culture info (when touching orbs)")]
    public TextMeshProUGUI cultureInfoText;

    [Tooltip("Text display for connection counter")]
    public TextMeshProUGUI connectionCounterText;

    [Header("End Screen")]
    [Tooltip("Panel that appears at the end")]
    public GameObject endScreenPanel;

    [Tooltip("Restart button")]
    public Button restartButton;

    [Tooltip("Exit button (optional)")]
    public Button exitButton;

    void Awake()
    {
        // Singleton pattern
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    void Start()
    {
        // Initialize UI state
        if (narrationText != null)
            narrationText.gameObject.SetActive(false);

        if (cultureInfoText != null)
            cultureInfoText.gameObject.SetActive(false);

        if (connectionCounterText != null)
            connectionCounterText.gameObject.SetActive(false);

        if (endScreenPanel != null)
            endScreenPanel.SetActive(false);

        // Setup button listeners
        if (restartButton != null)
        {
            restartButton.onClick.AddListener(OnRestartClicked);
        }

        if (exitButton != null)
        {
            exitButton.onClick.AddListener(OnExitClicked);
        }
    }

    #region Narration

    public void ShowNarration(string text, float duration)
    {
        if (narrationText != null)
        {
            StartCoroutine(ShowTextTemporary(narrationText, text, duration));
        }
    }

    IEnumerator ShowTextTemporary(TextMeshProUGUI textComponent, string text, float duration)
    {
        textComponent.text = text;
        textComponent.gameObject.SetActive(true);

        // Fade in (optional)
        if (textComponent.canvasRenderer != null)
        {
            textComponent.canvasRenderer.SetAlpha(0f);
            textComponent.CrossFadeAlpha(1f, 0.5f, false);
        }

        yield return new WaitForSeconds(duration);

        // Fade out
        if (textComponent.canvasRenderer != null)
        {
            textComponent.CrossFadeAlpha(0f, 0.5f, false);
            yield return new WaitForSeconds(0.5f);
        }

        textComponent.gameObject.SetActive(false);
    }

    #endregion

    #region Culture Info

    public void ShowCultureInfo(string cultureName, string greeting)
    {
        if (cultureInfoText != null)
        {
            string displayText = $"<b>{cultureName}</b>\n<i>{greeting}</i>";
            StartCoroutine(ShowTextTemporary(cultureInfoText, displayText, 3f));
        }
    }

    #endregion

    #region Connection Counter

    public void ShowConnectionCounter(bool show)
    {
        if (connectionCounterText != null)
        {
            connectionCounterText.gameObject.SetActive(show);

            if (show)
            {
                UpdateConnectionCount(0);
            }
        }
    }

    public void UpdateConnectionCount(int count)
    {
        if (connectionCounterText != null && connectionCounterText.gameObject.activeSelf)
        {
            int required = 7; // Minimum connections needed

            if (ThreadConnectionSystem.Instance != null)
            {
                required = ThreadConnectionSystem.Instance.minimumConnectionsForCompletion;
            }

            connectionCounterText.text = $"Connections: {count} / {required}";

            // Change color when complete
            if (count >= required)
            {
                connectionCounterText.color = Color.green;
            }
            else
            {
                connectionCounterText.color = Color.white;
            }
        }
    }

    #endregion

    #region End Screen

    public void ShowEndScreen()
    {
        if (endScreenPanel != null)
        {
            endScreenPanel.SetActive(true);
        }
    }

    public void HideEndScreen()
    {
        if (endScreenPanel != null)
        {
            endScreenPanel.SetActive(false);
        }
    }

    void OnRestartClicked()
    {
        Debug.Log("Restart button clicked");

        // Hide end screen
        HideEndScreen();

        // Restart experience
        if (HarmonySceneManager.Instance != null)
        {
            HarmonySceneManager.Instance.RestartExperience();
        }
    }

    void OnExitClicked()
    {
        Debug.Log("Exit button clicked");

        // In editor, stop play mode
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #else
        // On device, quit application
        Application.Quit();
        #endif
    }

    #endregion

    void OnDestroy()
    {
        // Clean up button listeners
        if (restartButton != null)
        {
            restartButton.onClick.RemoveListener(OnRestartClicked);
        }

        if (exitButton != null)
        {
            exitButton.onClick.RemoveListener(OnExitClicked);
        }
    }
}
