using UnityEngine;
using UnityEngine.Events;

public class SaveStateManager : MonoBehaviour
{
    public static SaveStateManager singleton;
    
    public delegate void SaveStateEvent();

    public event SaveStateEvent OnRewind;
    
    public UnityEvent OnRewindEvent = new UnityEvent();
    // [SerializeField] private GameObject masterRigCow;
    // [SerializeField] private GameObject masterRigGuy;
    // private GameObject currentState;
    
    //public List<GameObject> rigs = new List<GameObject>();
    // Start is called before the first frame update

    private void Awake()
    {
        singleton = this;
    }

    void Start()
    {
        Fusion.singleton.OnPreFusion += SaveCurrentState;
        //SaveCurrentState();
        //Fusion.singleton.subRig = transform.GetChild(transform.childCount-1).GetChild(0);
    }
    
    private void SaveCurrentState()
    {
        GameObject lastState = transform.GetChild(transform.childCount-1).gameObject;
        GameObject newState = Instantiate(lastState, transform);
        lastState.SetActive(false);
        Fusion.singleton.subRig = newState.transform.GetChild(0);
    }
    
    private void Update()
    {
        if (Input.GetMouseButtonDown(1) || Input.GetKeyDown(KeyCode.Backspace) || Input.GetKeyDown(KeyCode.Z) || Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift) )
        {
            Rewind();
        }
    }

    public void Rewind()
    {
        if(Certificate.singleton.levelComplete || transform.childCount <= 1)
            return;
        
        GameObject newState = transform.GetChild(transform.childCount-2).gameObject;
        Destroy(transform.GetChild(transform.childCount-1).gameObject);
        newState.SetActive(true);
        // Fusion.singleton.subRig = newState.transform.GetChild(0);
        OnRewind?.Invoke();
        OnRewindEvent?.Invoke();
        
    }

    public void Revert()
    {
        GameObject newState = transform.GetChild(0).gameObject;
        
        for (int i = transform.childCount-1; i >= 0; i--)
        {
            Destroy(transform.GetChild(i).gameObject);
        }
        newState.SetActive(true);
        Fusion.singleton.subRig = newState.transform.GetChild(0);
    }
    
    private void OnDestroy()
    {
        Fusion.singleton.OnPreFusion -= SaveCurrentState;
    }
}
