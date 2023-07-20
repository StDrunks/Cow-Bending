using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

public class Stamp : MonoBehaviour
{
    public UnityEvent onEnable = new UnityEvent();
    // Update is called once per frame
    void OnEnable()
    {
        transform.DOPunchScale(Vector3.one * 0.5f, 0.5f, 0, 1f);
        onEnable.Invoke();
    }

    void Stop()
    {
        gameObject.SetActive(false);
    }
}