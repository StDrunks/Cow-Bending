using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class Cursor : MonoBehaviour
{
    public static Cursor singleton;
    private float horizonX;
    private float defaultHorizonX = -2;
    [DoNotSerialize] public bool isLeft = false;
    public UnityEvent OnLeft = new UnityEvent();
    public UnityEvent OnRight = new UnityEvent();

    public delegate void CrusorEvent();
    public CrusorEvent OnLeftEvent;
    public CrusorEvent OnRightEvent;
    
    private void Awake()
    {
        singleton = this;
    }

    public void ForceLeft()
    {
        horizonX = 16;
    }
    public void UnforceLeft()
    {
        horizonX = defaultHorizonX;
    }

    private void Start()
    {
        horizonX = defaultHorizonX;
        if (transform.position.x <= horizonX)
        {
            isLeft = true;
        }
    }

    private void Update()
    { 
        // Move the GameObject to the mouse position
        Vector3 targetPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);//+pivot;
        targetPosition.z = 0;
        transform.position = targetPosition;
        
        if(isLeft && transform.position.x > horizonX)
        {
            isLeft = false;
            OnRight.Invoke();
            OnRightEvent?.Invoke();
            //UnityEngine.Cursor.visible = false;
        }
        else if(!isLeft && transform.position.x <= horizonX)
        {
            isLeft = true;
            OnLeft.Invoke();
            OnLeftEvent?.Invoke();
            //UnityEngine.Cursor.visible = true;
        }
    }
}