using UnityEngine;
using TMPro;

public class TrainingStepUI : MonoBehaviour
{
    [Header("UI")]
    public TextMeshProUGUI instructionText;
    public TextMeshProUGUI feedbackText;

    [Header("References")]
    public ExtinguisherPin pinSystem;
    public ParticleSystem spray;

    [Header("Training State")]
    public bool isAimingCorrectly = false;
    public bool isSweeping = false;
    public bool fireExtinguished = false;

    void Update()
    {
        if (instructionText == null || feedbackText == null)
            return;

        // Step 1: Pull pin
        if (pinSystem == null || !pinSystem.isUnlocked)
        {
            instructionText.text = "Step 1: Pull the pin";
            feedbackText.text = "Grab and remove the safety pin";
            return;
        }

        // Step 2: Aim
        if (!isAimingCorrectly)
        {
            instructionText.text = "Step 2: Aim at the base of the fire";
            feedbackText.text = "Point the nozzle lower at the fire base";
            return;
        }

        // ------------------------------
        // Once aim is done = Step 3 passed forever
        // ------------------------------

        // Step 4: Sweep
        if (!isSweeping)
        {
            instructionText.text = "Step 4: Sweep side to side";
            feedbackText.text = "Move the nozzle left and right while spraying";
            return;
        }

        // Final success
        if (!fireExtinguished)
        {
            instructionText.text = "Good job! Keep suppressing the fire";
            feedbackText.text = "PASS steps are being applied correctly";
        }
        else
        {
            instructionText.text = "Success!";
            feedbackText.text = "Fire extinguished successfully";
        }
    }

    public void SetAimCorrect(bool value)
    {
        isAimingCorrectly = value;
    }

    public void SetSweeping(bool value)
    {
        isSweeping = value;
    }

    public void SetFireExtinguished(bool value)
    {
        fireExtinguished = value;
    }
}