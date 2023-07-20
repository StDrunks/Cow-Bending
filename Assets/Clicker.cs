using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clicker : MonoBehaviour
{
    public static Clicker singleton;
    private AudioSource audioSource;
    [SerializeField] private AudioClip clip;
    // Start is called before the first frame update
    void Awake()
    {
        singleton = this;
    }

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    
    // Update is called once per frame
    public void PlayClick()
    {
        audioSource.PlayOneShot(clip);
    }
}
