using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class ExtinguisherPin : MonoBehaviour
{
    public XRGrabInteractable grabInteractable;
    public Transform lockedPosition;
    public float unlockDistance = 0.1f;
    public bool isUnlocked = false;

    private Vector3 originLocalPos;
    private Quaternion originLocalRot;
    private Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        originLocalPos = transform.localPosition;
        originLocalRot = transform.localRotation;
    }

    void Update()
    {
        if (!isUnlocked && Vector3.Distance(transform.position, lockedPosition.position) > unlockDistance)
            Unlock();
    }

    void Unlock()
    {
        isUnlocked = true;
        if (rb != null) rb.isKinematic = false;
    }

    public void ResetPin()
    {
        isUnlocked = false;
        transform.localPosition = originLocalPos;
        transform.localRotation = originLocalRot;

        if (rb != null)
        {
            // 只重置状态，不修改 Kinematic 物体的速度
            if (!rb.isKinematic)
            {
                rb.velocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
            }
            rb.isKinematic = true;
        }
    }
}