using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR;

public class ExtinguisherController : MonoBehaviour
{
    [Header("References")]
    public ParticleSystem spray;
    public PutOutFire putOutFire;
    public AudioSource spraySound;
    public ExtinguisherPin pinSystem;

    private List<UnityEngine.XR.InputDevice> leftHandDevices = new List<UnityEngine.XR.InputDevice>();
    private List<UnityEngine.XR.InputDevice> rightHandDevices = new List<UnityEngine.XR.InputDevice>();

    void Update()
    {
        bool keyboardInput = Keyboard.current != null && Keyboard.current.kKey.isPressed;
        bool vrTriggerInput = IsAnyVRTriggerPressed();

        bool canSpray = true;

        if (pinSystem != null)
            canSpray = pinSystem.isUnlocked;

        if (canSpray && (keyboardInput || vrTriggerInput))
        {
            StartSpray();
        }
        else
        {
            StopSpray();
        }
    }

    bool IsAnyVRTriggerPressed()
    {
        bool leftPressed = false;
        bool rightPressed = false;

        InputDevices.GetDevicesAtXRNode(XRNode.LeftHand, leftHandDevices);
        InputDevices.GetDevicesAtXRNode(XRNode.RightHand, rightHandDevices);

        for (int i = 0; i < leftHandDevices.Count; i++)
        {
            if (leftHandDevices[i].isValid &&
                leftHandDevices[i].TryGetFeatureValue(UnityEngine.XR.CommonUsages.triggerButton, out bool pressed) &&
                pressed)
            {
                leftPressed = true;
                break;
            }
        }

        for (int i = 0; i < rightHandDevices.Count; i++)
        {
            if (rightHandDevices[i].isValid &&
                rightHandDevices[i].TryGetFeatureValue(UnityEngine.XR.CommonUsages.triggerButton, out bool pressed) &&
                pressed)
            {
                rightPressed = true;
                break;
            }
        }

        return leftPressed || rightPressed;
    }

    void StartSpray()
    {
        if (spray != null && !spray.isPlaying)
        {
            spray.Play();

            if (putOutFire != null)
                putOutFire.StartSpray();

            if (spraySound != null && !spraySound.isPlaying)
                spraySound.Play();

            Debug.Log("Spray started");
        }
    }

    void StopSpray()
    {
        if (spray != null && spray.isPlaying)
        {
            spray.Stop();

            if (putOutFire != null)
                putOutFire.StopSpray();

            if (spraySound != null && spraySound.isPlaying)
                spraySound.Stop();

            Debug.Log("Spray stopped");
        }
    }
}