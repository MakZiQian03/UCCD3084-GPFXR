using UnityEngine;

public class SimpleAimLine : MonoBehaviour
{
    [Header("Line Settings")]
    public Transform nozzleTransform;
    public LineRenderer lineRenderer;
    public float fixedLineLength = 5f; // The line will always be this long

    [Header("Aim Detection")]
    public string fireTagName = "Fire";
    public float requiredHoldTime = 1.5f;
    public float aimDetectionRange = 10f; // How far the aim check goes

    [Header("References")]
    public TrainingStepUI trainingStepUI;
    public ExtinguisherPin pinSystem;

    private float _holdTimer = 0f;
    private bool _aimCompleted = false;

    void Start()
    {
        // Hide the line at the start
        if (lineRenderer != null)
        {
            lineRenderer.enabled = false;
            lineRenderer.useWorldSpace = true;
        }
    }

    void Update()
    {
        // Safety checks
        if (nozzleTransform == null || lineRenderer == null || pinSystem == null)
            return;

        // Hide line if pin is not pulled
        if (!pinSystem.isUnlocked)
        {
            lineRenderer.enabled = false;
            _holdTimer = 0f;
            return;
        }

        // Show the line once pin is pulled
        lineRenderer.enabled = true;

        // Stop updating if aim is already completed
        if (_aimCompleted)
            return;

        // --------------------------
        // 1. ALWAYS draw a fixed-length line (NO raycast here!)
        // --------------------------
        Vector3 lineStart = nozzleTransform.position;
        Vector3 lineEnd = lineStart + nozzleTransform.forward * fixedLineLength;
        lineRenderer.SetPosition(0, lineStart);
        lineRenderer.SetPosition(1, lineEnd);

        // --------------------------
        // 2. Use a SEPARATE raycast ONLY for detecting fire
        // --------------------------
        RaycastHit hit;
        bool isAimingAtFire = false;
        if (Physics.Raycast(lineStart, nozzleTransform.forward, out hit, aimDetectionRange))
        {
            if (hit.collider.CompareTag(fireTagName))
            {
                isAimingAtFire = true;
            }
        }

        // --------------------------
        // 3. Handle aim state and timing
        // --------------------------
        if (isAimingAtFire)
        {
            _holdTimer += Time.deltaTime;
            lineRenderer.startColor = Color.green;
            lineRenderer.endColor = Color.green;

            if (_holdTimer >= requiredHoldTime)
            {
                _aimCompleted = true;
                trainingStepUI.SetAimCorrect(true);
                lineRenderer.startColor = Color.blue;
                lineRenderer.endColor = Color.blue;
            }
        }
        else
        {
            _holdTimer = 0f;
            lineRenderer.startColor = Color.red;
            lineRenderer.endColor = Color.red;
        }
    }
}