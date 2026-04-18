using UnityEngine;

public class FreeRoamModeButton : MonoBehaviour
{
    [Header("Player")]
    public Transform xrOrigin;          
    public Transform cameraOffset;      

    [Header("Spawn Point")]
    public Transform freeRoamSpawnPoint;

    [Header("UI To Hide")]
    public GameObject[] uiObjectsToDisable;

    [Header("Movement To Enable")]
    public Behaviour[] componentsToEnableForFreeRoam;

    [Header("Optional Objects To Disable")]
    public GameObject[] objectsToDisableForFreeRoam;

    public void EnterFreeRoamMode()
    {
        // Move player to chosen spawn point
        if (xrOrigin != null && freeRoamSpawnPoint != null)
        {
            Vector3 offset = Vector3.zero;

            // Keeps headset offset correct if camera offset exists
            if (cameraOffset != null)
                offset = xrOrigin.position - cameraOffset.position;

            xrOrigin.position = freeRoamSpawnPoint.position + offset;

            // Optional: match facing direction
            Vector3 currentForward = xrOrigin.forward;
            currentForward.y = 0;

            Vector3 targetForward = freeRoamSpawnPoint.forward;
            targetForward.y = 0;

            if (currentForward != Vector3.zero && targetForward != Vector3.zero)
            {
                float angle = Vector3.SignedAngle(currentForward, targetForward, Vector3.up);
                xrOrigin.Rotate(Vector3.up, angle);
            }
        }

        // Disable UI
        if (uiObjectsToDisable != null)
        {
            foreach (GameObject uiObj in uiObjectsToDisable)
            {
                if (uiObj != null)
                    uiObj.SetActive(false);
            }
        }

        // Enable locomotion/free roam scripts
        if (componentsToEnableForFreeRoam != null)
        {
            foreach (Behaviour comp in componentsToEnableForFreeRoam)
            {
                if (comp != null)
                    comp.enabled = true;
            }
        }

        Debug.Log("Free roam mode activated.");
    }
}