using UnityEngine;
using DG.Tweening;

public class Leaver : MonoBehaviour
{

    // Update is called once per frame
    public void Kick()
    {
        transform.DOKill();
        transform.DOPunchRotation(Vector3.forward*-90f,1f,0,0f);
    }
}
