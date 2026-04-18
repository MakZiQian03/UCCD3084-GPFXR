using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class ExtinguisherPin : MonoBehaviour
{
    public XRGrabInteractable grabInteractable;
    public Transform lockedPosition;
    public float unlockDistance = 0.1f;
    public bool isUnlocked = false;
    public AudioSource pinAudio;
    public AudioClip pinPullSound;

    private bool hasPlayedSound = false;
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
        if (!isUnlocked && lockedPosition != null)
        {
            float distance = Vector3.Distance(transform.position, lockedPosition.position);

            if (distance > unlockDistance)
            {
                Unlock();
            }
        }
    }

    void Unlock()
    {
        isUnlocked = true;

        if (rb != null)
            rb.isKinematic = false;

        if (!hasPlayedSound && pinAudio != null && pinPullSound != null)
        {
            pinAudio.PlayOneShot(pinPullSound);
            hasPlayedSound = true;
        }
    }

    public void ResetPin()
    {
        isUnlocked = false;
        hasPlayedSound = false;
        transform.localPosition = originLocalPos;
        transform.localRotation = originLocalRot;

        if (rb != null)
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.isKinematic = true;
        }
    }
}