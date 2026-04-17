using UnityEngine;

public class Fire : MonoBehaviour
{
    [Header("Fire")]
    public ParticleSystem[] fireParticles;
    public Light fireLight;

    [Header("Settings")]
    public float extinguishSpeed = 0.1f;
    private float _currentIntensity = 1f;

    private bool _isExtinguished = false;

    void Start()
    {
        _currentIntensity = 1f;
        _isExtinguished = false;
        ShowFire(true);
    }

    void Update()
    {
        if (_isExtinguished) return;

        foreach (var p in fireParticles)
        {
            var emission = p.emission;
            emission.rateOverTime = _currentIntensity * 10;
        }

        if (fireLight != null)
            fireLight.intensity = _currentIntensity;

        if (_currentIntensity <= 0)
            Extinguish();
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

    // 🔥 这个方法保证重启后火焰100%重新燃烧
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

    // 给外部调用的固定方法（不报错）
    public bool IsExtinguished() => _isExtinguished;
    public float GetCurrentIntensity() => _currentIntensity;
    public void SetIntensity(float val) => _currentIntensity = Mathf.Clamp01(val);
}