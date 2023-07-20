using System;
using DG.Tweening;
using UnityEngine;

public class MoveAndRotate : MonoBehaviour
{
    //[SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float rotationSpeed = 200f;
    [SerializeField] private float rotationSpeedForKeys = 200f;
    //[SerializeField] private float horizonY = 0;
    private Vector3 offset = Vector3.zero;
    private SpriteRenderer defaultSpriteRenderer;
    private bool following = true;
    
    private Vector3 defaultPosition;

    private void Start()
    {
        defaultPosition = transform.position;
        defaultSpriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    private void Update()
    {
        float rotation = 0f;

        // Check for rotation input using the mouse scroll wheel
        rotation += Input.GetAxis("Mouse ScrollWheel") * rotationSpeed * Time.deltaTime;

        // Check for rotation input using the A and D keys
        rotation += Input.GetAxis("Horizontal") * rotationSpeedForKeys * Time.deltaTime;

        transform.rotation *= Quaternion.Euler(0, 0, rotation);
        
        //float rotation = Input.GetAxis("Mouse ScrollWheel") * rotationSpeed * Time.deltaTime;
        //transform.rotation*= Quaternion.Euler(0, 0, rotation);
        
        //transform.Rotate(Vector3.forward, rotation);
        
        if(!following)
            return;
        
        float savedZ = transform.position.z;
        // Move the GameObject to the mouse position
        Vector3 targetPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);//+pivot;
        targetPosition.z = savedZ;
        //transform.position = targetPosition;
        transform.position = Vector3.Lerp(transform.position, targetPosition, 10f * Time.deltaTime);
    }

    public void FindPivot()
    {
        Vector3 newGlobalPosition = new Vector3(transform.position.x , Fusion.singleton.horizon, transform.position.z);
        // offset = transform.InverseTransformPoint(newGlobalPosition-transform.position);
            
        offset = (newGlobalPosition-transform.position);
        Transform child = transform.GetChild(transform.childCount-1);
        
        Vector3 originalPosition = child.position;
        child.position -= offset;
        Vector3 savedLocalPosition = child.localPosition;
        child.position = originalPosition;
        child.DOLocalMove(savedLocalPosition,0.25f);
    }

    public void FollowMouse()
    {
        following = true;
        transform.DOKill();
    }
    
    public void UnfollowMouse()
    {
        following = false;
        transform.DOMove(defaultPosition, 0.5f);
        //transform.position = defaultPosition;
    }
}