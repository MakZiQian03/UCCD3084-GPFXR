using UnityEngine;

public class Fire : MonoBehaviour
{
    [Header("Fire")]
    public ParticleSystem[] fireParticles;
    public Light fireLight;

    [Header("Settings")]
    public float extinguishSpeed = 0.1f;
    private float _currentIntensity = 2f;
    private float lastHitTime = 0f;
    [SerializeField] private float recoverySpeed = 0.08f; // fire regrows if not sprayed

    private bool _isExtinguished = false;

    void Start()
    {
        _currentIntensity = 2f;
        _isExtinguished = false;
        ShowFire(true);
    }

    void Update()
    {
        if (_isExtinguished) return;

        foreach (var p in fireParticles)
        {
            var emission = p.emission;
            emission.rateOverTime = _currentIntensity * 100;

            var main = p.main;
            main.startSize = _currentIntensity;
        }

        if (fireLight != null)
            fireLight.intensity = _currentIntensity * 2f;

        if (_currentIntensity <= 0.05f)
        {
             _currentIntensity = 0;
             Extinguish();
        }

        if (Time.time - lastHitTime > 0.2f)
        {
            _currentIntensity += recoverySpeed * Time.deltaTime;
            _currentIntensity = Mathf.Clamp01(_currentIntensity);
        }
    }

    public void ReduceIntensity(float amount)
    {
        _currentIntensity -= amount;
        if (_currentIntensity < 0) _currentIntensity = 0;
    }

    void Extinguish()
    {
        _isExtinguished = true;
        ShowFire(false);

        if (UIManager.instance != null)
            UIManager.instance.SetFireExtinguished(true);
    }

    public void ResetFire()
    {
        CancelInvoke();
        StopAllCoroutines();

        _currentIntensity = 1f;
        _isExtinguished = false;

        ShowFire(true);

        foreach (var p in fireParticles)
        {
            p.Stop();
            p.Clear();
            p.Play();
        }

        if (fireLight != null)
        {
            fireLight.enabled = true;
            fireLight.intensity = 1f;
        }
    }

    void ShowFire(bool enable)
    {
        gameObject.SetActive(enable);

        foreach (var p in fireParticles)
            p.gameObject.SetActive(enable);

        if (fireLight != null)
            fireLight.enabled = enable;
    }

    public bool IsExtinguished() => _isExtinguished;
    public float GetCurrentIntensity() => _currentIntensity;
    public void SetIntensity(float val)
{
    _currentIntensity = Mathf.Clamp01(val);
    lastHitTime = Time.time;
}
}