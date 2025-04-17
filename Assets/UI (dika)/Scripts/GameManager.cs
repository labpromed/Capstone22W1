using UnityEngine;
using TMPro;
using DG.Tweening;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    public TextMeshProUGUI youWinText;
    public TextMeshProUGUI timeText;
    public TextMeshProUGUI highScoreText;
    public TextMeshProUGUI pressXText;

    private bool gameRunning = true;
    private bool gameWon = false;
    private bool canReturnToMenu = false;


    private float timer = 0f;

    void Start()
    {
        //PlayerPrefs.DeleteKey("HighScore");//sementara, hapus highscore

        youWinText.gameObject.SetActive(false);
        timeText.gameObject.SetActive(false);
        highScoreText.gameObject.SetActive(false);
        pressXText.gameObject.SetActive(false);

        youWinText.color = new Color(youWinText.color.r, youWinText.color.g, youWinText.color.b, 1f);
        timeText.color = new Color(timeText.color.r, timeText.color.g, timeText.color.b, 0f);
        highScoreText.color = new Color(highScoreText.color.r, highScoreText.color.g, highScoreText.color.b, 0f);
        pressXText.color = new Color(pressXText.color.r, pressXText.color.g, pressXText.color.b, 0f);
    }

    void Update()
    {
        if (gameRunning)
        {
            timer += Time.deltaTime;
        }

        if (canReturnToMenu && Input.GetKeyDown(KeyCode.X))
        {
            SceneManager.LoadScene("MainMenu");
        }
    }

    public void WinGame()
    {
        if (gameWon) return; // Prevent double trigger
        gameRunning = false;
        gameWon = true;

        if (AudioManager.Instance != null)
            AudioManager.Instance.PlaySFX(AudioManager.Instance.winSound);

        float finalTime = Mathf.Round(timer * 100f) / 100f;
        float bestTime = PlayerPrefs.GetFloat("HighScore", float.MaxValue);

        // Reset UI and initial positions
        youWinText.gameObject.SetActive(true);
        timeText.gameObject.SetActive(false);
        highScoreText.gameObject.SetActive(false);
        pressXText.gameObject.SetActive(false);

        youWinText.transform.localScale = Vector3.zero; // Start from 0 scale

        // Pop-out animation for "YOU WIN"
        youWinText.transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack).OnComplete(() =>
        {
            // After scaling in "YOU WIN", show Timer & Highscore
            timeText.gameObject.SetActive(true);
            highScoreText.gameObject.SetActive(true);
            timeText.DOFade(1, 0.5f);
            highScoreText.DOFade(1, 0.5f);

            timeText.text = "Time: " + finalTime.ToString("F2") + "s";

            if (finalTime < bestTime)
            {
                PlayerPrefs.SetFloat("HighScore", finalTime);
                PlayerPrefs.Save();
                highScoreText.text = "New Highscore!";
            }
            else
            {
                highScoreText.text = "Best Time: " + bestTime.ToString("F2") + "s";
            }

            // After a 1-second delay, show "Press X"
            DOVirtual.DelayedCall(1f, () =>
            {
                pressXText.gameObject.SetActive(true);
                pressXText.DOFade(1, 0.5f).OnComplete(() =>
                {
                    canReturnToMenu = true; // ✅ Izinkan tekan X
                    Cursor.lockState = CursorLockMode.None; // Ubah kursor menjadi tidak terkunci
                });
            });
        });
    }

    public bool IsGameWon()
    {
        return gameWon;
    }
}