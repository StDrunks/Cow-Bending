using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class Fusion : MonoBehaviour
{

    public static Fusion singleton;
    
    [SerializeField] private GameObject hider;
    [SerializeField] private GameObject reflection;
    [DoNotSerialize] public Transform subRig;

    public UnityEvent onFused = new UnityEvent();

    public delegate void FusionEvent();
    public FusionEvent OnFused;
    public FusionEvent OnPreFusion;
    
    [HideInInspector] public float horizon = -1.4f;
    
    private void Awake()
    {
        singleton = this;
    }
    
    private void Update()
    {
        //print(transform.position.y - horizon);
        
        if (!Certificate.singleton.levelComplete && !Cursor.singleton.isLeft && (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space)))
        {
            Fuse();
        }
    }

    private void Fuse()
    {
        print("fused");
        OnPreFusion.Invoke();
        CopyRenderers();
        CopyFeatures();
        subRig.localPosition += Vector3.forward;
        onFused.Invoke();
        OnFused.Invoke();
    }

    private void CopyFeatures()
    {
        Feature[] features = subRig.parent.GetComponentsInChildren<Feature>( false);
        foreach (Feature feature in features)
        {
            Transform featureTransform = feature.transform;
            
            float depth = featureTransform.position.y - horizon;

            if (depth > featureTransform.localScale.x/2)
            {
                GameObject newFeatureObject = Instantiate(feature.gameObject,subRig.parent);
                MirrorTransform(newFeatureObject.transform);
            } 
            else if (Math.Abs(depth)<= featureTransform.localScale.x/2)
            {
                featureTransform.localScale = Vector3.one * ((depth * 1.5f) + featureTransform.localScale.x);
                featureTransform.position = new Vector3(featureTransform.position.x, horizon, featureTransform.position.z);
            }
            else
            {
                //print("should destroy");
                //feature.gameObject.SetActive(false);
                feature.gameObject.transform.SetParent(null);
                Destroy(feature.gameObject);
            }
        }
    }
    
    private void CopyRenderers()
    {
        Instantiate(hider, hider.transform.position, hider.transform.rotation, subRig);
        
        GameObject reflectionInstance = Instantiate(reflection, 
            reflection.transform.position, reflection.transform.rotation, subRig);
        
        reflectionInstance.layer = LayerMask.NameToLayer("Capturable");
        
        Renderer reflectionRenderer = reflectionInstance.GetComponent<Renderer>();

        Material snapshotMaterial = new Material(reflectionRenderer.sharedMaterial);
        Texture renderTexture = snapshotMaterial.mainTexture;
        Texture snapshotTexture = new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.RGBA32, false);
        
        Graphics.CopyTexture(renderTexture, snapshotTexture);

        snapshotMaterial.SetTexture("_MainTex", snapshotTexture);

        reflectionRenderer.sharedMaterial = snapshotMaterial;
    }
    
    public void MirrorTransform(Transform targetTransform)
    {
        Vector3 newPosition = targetTransform.position;
        newPosition.y = 2 * horizon - newPosition.y;
        targetTransform.position = newPosition;

        Vector3 newScale = targetTransform.localScale;
        newScale.y *= -1f;
        targetTransform.localScale = newScale;

        // Vector3 newRotation = targetTransform.rotation.eulerAngles;
        // newRotation.z *= -1f;
        // targetTransform.rotation = Quaternion.Euler(newRotation);
    }
}
