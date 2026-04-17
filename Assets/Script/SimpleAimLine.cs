using UnityEngine;

public class SimpleAimLine : MonoBehaviour
{
    [Header("Line Settings")]
    public Transform nozzleTransform;
    public LineRenderer lineRenderer;
    public float fixedLineLength = 5f;

    [Header("Aim Detection")]
    public string fireTagName = "Fire";
    public float requiredHoldTime = 1.5f;
    public float aimDetectionRange = 10f;

    [Header("References")]
    public TrainingStepUI trainingStepUI;   // backup old UI
    public ExtinguisherPin pinSystem;

    private float _holdTimer = 0f;
    private bool _aimCompleted = false;

    void Start()
    {
        if (lineRenderer != null)
        {
            lineRenderer.enabled = false;
            lineRenderer.useWorldSpace = true;
        }
    }

    void Update()
    {
        if (nozzleTransform == null || lineRenderer == null || pinSystem == null)
            return;

        if (!pinSystem.isUnlocked)
        {
            lineRenderer.enabled = false;
            _holdTimer = 0f;
            _aimCompleted = false;

            if (trainingStepUI != null)
                trainingStepUI.SetAimCorrect(false);

            if (UIManager.instance != null)
                UIManager.instance.SetAimCorrect(false);

            return;
        }

        lineRenderer.enabled = true;

        Vector3 lineStart = nozzleTransform.position;
        Vector3 lineEnd = lineStart + nozzleTransform.forward * fixedLineLength;
        lineRenderer.SetPosition(0, lineStart);
        lineRenderer.SetPosition(1, lineEnd);

        if (_aimCompleted)
        {
            lineRenderer.startColor = Color.blue;
            lineRenderer.endColor = Color.blue;
            return;
        }

        RaycastHit hit;
        bool isAimingAtFire = false;

        if (Physics.Raycast(lineStart, nozzleTransform.forward, out hit, aimDetectionRange))
        {
            if (hit.collider.CompareTag(fireTagName))
            {
                isAimingAtFire = true;
            }
        }

        if (isAimingAtFire)
        {
            _holdTimer += Time.deltaTime;
            lineRenderer.startColor = Color.green;
            lineRenderer.endColor = Color.green;

            if (_holdTimer >= requiredHoldTime)
            {
                _aimCompleted = true;

                if (trainingStepUI != null)
                    trainingStepUI.SetAimCorrect(true);

                if (UIManager.instance != null)
                    UIManager.instance.SetAimCorrect(true);

                lineRenderer.startColor = Color.blue;
                lineRenderer.endColor = Color.blue;
            }
        }
        else
        {
            _holdTimer = 0f;

            if (trainingStepUI != null)
                trainingStepUI.SetAimCorrect(false);

            if (UIManager.instance != null)
                UIManager.instance.SetAimCorrect(false);

            lineRenderer.startColor = Color.red;
            lineRenderer.endColor = Color.red;
        }
    }
}