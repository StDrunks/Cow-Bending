using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;
using TMPro;
using Unity.VisualScripting;

public class Certificate : MonoBehaviour
{
    public static Certificate singleton; 
    public bool levelComplete = false;
    //private CertificateData certificateData;
    [SerializeField] GameObject counterPrefab;
    [SerializeField] float spread = 1f;
    [SerializeField] Transform counterParent;
    //private int counterCount = 0;
    private List<FeatureCounter> counters = new List<FeatureCounter>();
    
    [SerializeField] private List<CertificateData> levels = new List<CertificateData>();

    private int currentLevelIndex = -1;
    private Vector3 defaultPosition;
    
    public UnityEvent onWin = new UnityEvent();
    public UnityEvent onClear = new UnityEvent();
    public UnityEvent goCow = new UnityEvent();
    public UnityEvent goGuy = new UnityEvent();

    
    [SerializeField] GameObject cowPrefab;
    [SerializeField] GameObject manPrefab;
    
    
    [SerializeField] private TextMeshProUGUI levelName;
    void Awake()
    {
        singleton = this;
    }
    
    void Start()
    {
        defaultPosition = transform.localPosition;
        
        if(levels.Count > 0)
            LoadNextLevel();
    }

    void TweenOut()
    {
        transform.DOLocalMove(defaultPosition - Vector3.right*6f,0.4f).onComplete+=LoadNextLevel;
    }
    
    void TweenIn()
    {
        transform.DOLocalMove(defaultPosition,0.8f);
    }

    public void ChangeLevel()
    {
        TweenOut();
    }
    
    private void LoadNextLevel()
    {
        ClearLevel();
        TweenIn();
        
        currentLevelIndex++;
        if (currentLevelIndex >= levels.Count)
            currentLevelIndex = 0;
        
        InstantiateCounters();
    }

    void ClearLevel()
    {
        levelComplete = false;
        foreach (var counter in counters)
        {
            Destroy(counter.gameObject);
        }
        counters.Clear();
        onClear.Invoke();
    }
    
    void InstantiateCounters()
    {
        CertificateData certificateData = levels[currentLevelIndex];
        levelName.text = "ORDER #" + (currentLevelIndex+1);

        GameObject prefab;
        if (certificateData.features[0].type == Feature.Type.ManArm
            || certificateData.features[0].type == Feature.Type.ManLeg
            || certificateData.features[0].type == Feature.Type.ManHead)
        {
            prefab = Instantiate(manPrefab, Fusion.singleton.transform);
            goCow.Invoke();
            
        }
        else
        {
            prefab = Instantiate(cowPrefab, Fusion.singleton.transform);
            goGuy.Invoke();
        }


        Fusion.singleton.subRig = prefab.transform.GetChild(0);

            for (int i = 0; i < certificateData.features.Count; i++)
        {
            GameObject counter = Instantiate(counterPrefab, counterParent);
            FeatureCounter featureCounter = counter.GetComponent<FeatureCounter>();
            featureCounter.Type = certificateData.features[i].type;
            featureCounter.targetCount = certificateData.features[i].count;
            counter.transform.localPosition = Vector3.up * -(i*spread);
            counters.Add(featureCounter);
        }
    }

    public void CheckForWin()
    {
        foreach (FeatureCounter counter in counters)
        {
           if(!counter.isComplete)
               return;
        }

        levelComplete = true;
        Invoke("CallWin",0.5f);
    }

    public void CallWin()
    {
        onWin.Invoke();
    }
}
