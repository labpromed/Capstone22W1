using UnityEngine.SceneManagement;
using UnityEngine;
using System.Collections;
using DG.Tweening;
using System.Threading.Tasks;
using DG.Tweening.Core.Easing;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] RectTransform pausePanelRect;
    [SerializeField] float topPosY, middlePosY;
    [SerializeField] float tweenDuration;
    [SerializeField] GameObject PanelHitamLegam;
    [SerializeField] CanvasGroup blackPanelGroup;
    [SerializeField] float blackFadeDuration = 0.3f;

    public static PauseMenu Instance { get; set; }
    public GameObject pauseMenuUI;
    public GameObject pauseHintUI;
    public GameObject audioSettingsUI; // 🆕

    private GameManager gameManager;

    public bool isPaused;



    void Start()
    {
        pausePanelRect.anchoredPosition = new Vector2(
        pausePanelRect.anchoredPosition.x,
        topPosY
    );
        gameManager = FindObjectOfType<GameManager>();
        pauseMenuUI.SetActive(false);
        audioSettingsUI.SetActive(false);
        pauseHintUI.SetActive(true);
        PanelHitamLegam.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (gameManager != null && gameManager.IsGameWon()) return;

            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    public void PauseGame()
    {
        PanelHitamLegam.SetActive(true);
        pauseMenuUI.SetActive(true);
        pauseHintUI.SetActive(false);
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        isPaused = true;

        // Jalankan animasi setelah satu frame
        StartCoroutine(DelayedPausePanelIntro());

        // Fade in black panel
        blackPanelGroup.gameObject.SetActive(true);
        blackPanelGroup.DOFade(1f, blackFadeDuration).SetUpdate(true);

    }

    IEnumerator DelayedPausePanelIntro()
    {
        yield return null; // tunggu satu frame
        PausePanelIntro(); // baru jalankan animasi
    }

    public async void ResumeGame()
    {
        await PausePanelOutro(); // tunggu animasi keluar dulu

        PanelHitamLegam.SetActive(false);
        pauseMenuUI.SetActive(false);
        audioSettingsUI.SetActive(false);
        pauseHintUI.SetActive(true);

        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        isPaused = false;

        // Fade out black panel
        blackPanelGroup.DOFade(0f, blackFadeDuration).SetUpdate(true)
        .OnComplete(() => blackPanelGroup.gameObject.SetActive(false));
    }

    public void OpenAudioSettings()
    {
        pauseMenuUI.SetActive(false);
        audioSettingsUI.SetActive(true);
    }

    public void BackToPauseMenu()
    {
        audioSettingsUI.SetActive(false);
        pauseMenuUI.SetActive(true);
    }


    public void GoToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }

    public void BackToOptions()
    {
        audioSettingsUI.SetActive(false);
        pauseMenuUI.SetActive(true);
    }

    void PausePanelIntro()
    {
        AudioManager.Instance.PlaySFX(AudioManager.Instance.pauseSound);
        pausePanelRect.DOAnchorPosY(middlePosY, tweenDuration).SetUpdate(true);
    }

    async Task PausePanelOutro()
    {
        AudioManager.Instance.PlaySFX(AudioManager.Instance.pauseSound);
        await pausePanelRect.DOAnchorPosY(topPosY, tweenDuration).SetUpdate(true).AsyncWaitForCompletion();
    }
}