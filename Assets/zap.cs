using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;

public class zap : MonoBehaviour
{
    private Image image;

    private AudioSource audioSource;
    // Start is called before the first frame update
    void Start()
    {
        image = GetComponent<Image>();
        audioSource = GetComponent<AudioSource>();
        Fusion.singleton.OnFused += Play;
    }

    // Update is called once per frame
    void Play()
    {
        image.enabled = true;
        transform.DOShakePosition(0.25f, 0.5f, 100, 90f, false, true).onComplete += Stop;
        if(audioSource.isPlaying)
            audioSource.Stop();
        audioSource.Play();
    }

    void Stop()
    {
        image.enabled = false;
    }
}
