using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ImageGedeKecil : MonoBehaviour
{
    public RectTransform target; // Drag UI Image kamu ke sini lewat Inspector
    public float scaleAmount = 1.2f;
    public float animationDuration = 0.5f;
    public float interval = 7f;

    void Start()
    {
        StartPulseLoop();
    }

    void StartPulseLoop()
    {
        Sequence seq = DOTween.Sequence();

        seq.Append(target.DOScale(scaleAmount, animationDuration).SetEase(Ease.InOutSine));
        seq.AppendInterval(0.2f); // jeda kecil antara besar ke kecil
        seq.Append(target.DOScale(1f, animationDuration).SetEase(Ease.InOutSine));
        seq.AppendInterval(interval); // tunggu 7 detik sebelum ulang lagi

        seq.SetLoops(-1); // loop tak terbatas
    }
}