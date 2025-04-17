using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformMereng : MonoBehaviour
{
    public RectTransform platform;         // Drag image/platform ke sini
    public Vector2 startPosition;          // Posisi awal (x, y) bisa diatur dari Inspector
    public Vector2 endPosition;            // Posisi akhir (x, y) bisa diatur dari Inspector
    public float duration = 3f;            // Lama gerak dari start ke end
    public Ease easeType = Ease.InOutSine; // Jenis easing untuk animasi

    void Start()
    {
        // Set posisi awal saat mulai
        platform.anchoredPosition = startPosition;

        // Buat animasi gerak dari start → end → balik lagi (Yoyo)
        platform.DOAnchorPos(endPosition, duration)
                .SetEase(easeType)
                .SetLoops(-1, LoopType.Yoyo);
    }
}
