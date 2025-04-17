using UnityEngine;
using DG.Tweening;

public class AwanMeluncur : MonoBehaviour
{
    public RectTransform cloud;       // UI image awan
    public float moveDuration = 10f;  // durasi awan melayang dari kanan ke kiri
    public float waitTime = 3f;       // jeda sebelum muncul lagi dari kanan
    public float startX = 1000f;      // posisi X awal di kanan layar
    public float endX = -1000f;       // posisi X akhir di kiri layar

    void Start()
    {
        StartCloudLoop();
    }

    void StartCloudLoop()
    {
        cloud.anchoredPosition = new Vector2(startX, cloud.anchoredPosition.y);

        Sequence seq = DOTween.Sequence();
        seq.Append(cloud.DOAnchorPosX(endX, moveDuration).SetEase(Ease.Linear));
        seq.AppendInterval(waitTime);
        seq.AppendCallback(() => {
            // Reset posisi ke kanan
            cloud.anchoredPosition = new Vector2(startX, cloud.anchoredPosition.y);
        });
        seq.SetLoops(-1); // loop terus
    }
}
