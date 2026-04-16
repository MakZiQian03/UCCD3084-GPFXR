using UnityEngine;
using UnityEngine.InputSystem;

public class ExtinguisherController : MonoBehaviour
{
    public ParticleSystem spray;
    public PutOutFire putOutFire;
    public AudioSource spraySound; 

    void Update()
    {
        if (Keyboard.current != null && Keyboard.current.spaceKey.isPressed)
        {
            if (spray != null && !spray.isPlaying)
            {
                spray.Play();

                if (putOutFire != null)
                    putOutFire.StartSpray();

                if (spraySound != null && !spraySound.isPlaying)
                    spraySound.Play();
            }
        }
        else
        {
            if (spray != null && spray.isPlaying)
            {
                spray.Stop();

                if (putOutFire != null)
                    putOutFire.StopSpray();

                if (spraySound != null && spraySound.isPlaying)
                    spraySound.Stop();
            }
        }
    }
}