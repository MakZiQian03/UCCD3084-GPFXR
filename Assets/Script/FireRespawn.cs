using UnityEngine;

public class FireRespawn : MonoBehaviour
{
    public GameObject realFire;

    void Start()
    {
        realFire.SetActive(true);
    }

    public void ResetTheFire()
    {
        realFire.SetActive(true);  // 强制打开火焰
    }
}