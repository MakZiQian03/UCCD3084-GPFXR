using UnityEngine;

public class HoseAimController : MonoBehaviour
{
    public Transform joint3;
    public Transform aimTarget;

    public float speed = 6f;

    void Update()
    {
        if (joint3 == null || aimTarget == null) return;

        Vector3 dir = aimTarget.position - joint3.position;

        if (dir.sqrMagnitude < 0.001f) return;

        Quaternion targetRot = Quaternion.LookRotation(dir);

        joint3.rotation = Quaternion.Slerp(
            joint3.rotation,
            targetRot,
            speed * Time.deltaTime
        );
    }
}