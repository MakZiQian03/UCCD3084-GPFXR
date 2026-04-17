using UnityEngine;

public class ExtinguisherResetter : MonoBehaviour
{
    [Header("Extinguisher")]
    public Transform extinguisherTransform;
    public Rigidbody extinguisherRb;

    [Header("Pin")]
    public Transform pinTransform;
    public Rigidbody pinRb;
    public ExtinguisherPin pinScript;

    [Header("Spray")]
    public ParticleSystem spray;
    public AudioSource spraySound;
    public PutOutFire putOutFire;

    private Vector3 extinguisherStartPos;
    private Quaternion extinguisherStartRot;

    private Vector3 pinStartPos;
    private Quaternion pinStartRot;

    void Start()
    {
        if (extinguisherTransform != null)
        {
            extinguisherStartPos = extinguisherTransform.position;
            extinguisherStartRot = extinguisherTransform.rotation;
        }

        if (pinTransform != null)
        {
            pinStartPos = pinTransform.position;
            pinStartRot = pinTransform.rotation;
        }
    }

    public void ResetExtinguisher()
    {
        // Stop spray
        if (spray != null)
            spray.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);

        if (spraySound != null && spraySound.isPlaying)
            spraySound.Stop();

        if (putOutFire != null)
            putOutFire.StopSpray();

        // Reset extinguisher rigidbody
        if (extinguisherRb != null)
        {
            extinguisherRb.velocity = Vector3.zero;
            extinguisherRb.angularVelocity = Vector3.zero;
        }

        if (extinguisherTransform != null)
        {
            extinguisherTransform.position = extinguisherStartPos;
            extinguisherTransform.rotation = extinguisherStartRot;
        }

        // Reset pin rigidbody
        if (pinRb != null)
        {
            pinRb.velocity = Vector3.zero;
            pinRb.angularVelocity = Vector3.zero;
        }

        // Reset pin position
        if (pinTransform != null)
        {
            pinTransform.position = pinStartPos;
            pinTransform.rotation = pinStartRot;
        }

        // Lock pin again
        if (pinScript != null)
        {
            pinScript.isUnlocked = false;
        }
    }
}