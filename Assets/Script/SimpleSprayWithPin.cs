using UnityEngine;
using UnityEngine.InputSystem;

public class SimpleSprayWithPin : MonoBehaviour
{
    public ParticleSystem spray;
    public PutOutFire putOutFire;
    public FireExtinguisherPinInputSystem pinSystem;
    public AudioSource spraySound;

    void Update()
    {
        if (pinSystem == null || spray == null) return;

        if (!pinSystem.isPinPulled)
        {
            if (spray.isPlaying)
            {
                spray.Stop();
                if (putOutFire != null) putOutFire.StopSpray();
            }

            if (spraySound != null && spraySound.isPlaying)
            {
                spraySound.Stop();
            }
            return;
        }

        bool spraying = Keyboard.current != null && Keyboard.current.spaceKey.isPressed;

        if (spraying)
        {
            if (!spray.isPlaying)
            {
                spray.Play();
                if (putOutFire != null) putOutFire.StartSpray();
            }

            if (spraySound != null && !spraySound.isPlaying)
            {
                spraySound.Play();
            }
        }
        else
        {
            if (spray.isPlaying)
            {
                spray.Stop();
                if (putOutFire != null) putOutFire.StopSpray();
            }

            if (spraySound != null && spraySound.isPlaying)
            {
                spraySound.Stop();
            }
        }
    }
}