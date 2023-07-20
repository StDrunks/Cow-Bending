using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using DG;
using DG.Tweening;

public class FeatureCounter : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textMesh;
    private Feature.Type type;
    [SerializeField] public int targetCount = 2;

    [SerializeField] private SpriteRenderer icon;
    [SerializeField] private GameObject tick;
    
    [SerializeField] private Sprite headIcon;
    [SerializeField] private Sprite udderIcon;
    [SerializeField] private Sprite legIcon;
    [SerializeField] private Sprite tailIcon;
    [SerializeField] private Sprite manHeadIcon;
    [SerializeField] private Sprite manLegIcon;
    [SerializeField] private Sprite manArmIcon;

    [HideInInspector] public bool isComplete = false;

    private AudioSource audioSource;
    
    public Feature.Type Type
    {
        get => type;
        set
        {
            type = value;
            switch (value)
            {
                case Feature.Type.Head:
                    icon.sprite = headIcon;
                    break;
                case Feature.Type.Udder:
                    icon.sprite = udderIcon;
                    break;
                case Feature.Type.Leg:
                    icon.sprite = legIcon;
                    break;
                case Feature.Type.Tail:
                    icon.sprite = tailIcon;
                    break;
                case Feature.Type.ManHead:
                    icon.sprite = manHeadIcon;
                    break;
                case Feature.Type.ManLeg:
                    icon.sprite = manLegIcon;
                    break;
                case Feature.Type.ManArm:
                    icon.sprite = manArmIcon;
                    break;
            }
        }
    }

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        Fusion.singleton.OnFused += RequestTextUpdate;
        SaveStateManager.singleton.OnRewind += RequestTextUpdate;

        textMesh = GetComponentInChildren<TextMeshProUGUI>();
        UpdateText();
    }

    public void SetIcon(Sprite sprite)
    {
        icon.sprite = sprite;
    }
    
    private void RequestTextUpdate()
    {
        Invoke("UpdateText", 0.5f + 0.2f * transform.GetSiblingIndex());
    }

    void UpdateText()
    {
        textMesh.text = FeatureCount().ToString() + "/" + targetCount.ToString();
        textMesh.transform.DOPunchScale(Vector3.one * 0.5f, 0.2f);
        if (transform.GetSiblingIndex() == transform.parent.childCount - 1)
            Certificate.singleton.CheckForWin();
    }
        
    void OnDestroy()
    {
        Fusion.singleton.OnFused -= RequestTextUpdate;
        SaveStateManager.singleton.OnRewind -= RequestTextUpdate;
    }

    int FeatureCount()
    {
        int result = 0;
        foreach (Feature feature in Fusion.singleton.gameObject.GetComponentsInChildren<Feature>())
        {
            if (feature.type == type)
               result++;
        }

        isComplete = (result == targetCount); 
        tick.SetActive(result == targetCount);
        if (isComplete)
        {
            if (audioSource.isPlaying)
                audioSource.Stop();
            audioSource.Play();
        }

        return result;
    }
}
