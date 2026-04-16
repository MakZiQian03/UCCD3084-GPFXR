using UnityEngine;

public class AimDetection : MonoBehaviour
{
    public Transform extinguisherBody;  
    public Transform fireTarget;        
    public TrainingStepUI trainingUI;

    public float maxAngle = 45f;

    void Update()
    {
        if (extinguisherBody == null || fireTarget == null || trainingUI == null)
            return;

        Vector3 dirToFire = fireTarget.position - extinguisherBody.position;

        float angle = Vector3.Angle(extinguisherBody.forward, dirToFire);

        if (angle < maxAngle)
            trainingUI.SetAimCorrect(true);
        else
            trainingUI.SetAimCorrect(false);
    }
}