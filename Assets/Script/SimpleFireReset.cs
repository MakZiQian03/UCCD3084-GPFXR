using UnityEngine;

public class SimpleFireReset : MonoBehaviour
{
    public ParticleSystem fireParticle;
    public Light fireLight;

    // 这个方法会被 Reset 按钮直接调用
    public void ForceRestartFire()
    {
        // 强制激活火焰物体
        gameObject.SetActive(true);

        // 重置并播放粒子
        if (fireParticle != null)
        {
            fireParticle.Stop();
            fireParticle.Clear();
            fireParticle.Play();
        }

        // 打开灯光
        if (fireLight != null)
        {
            fireLight.enabled = true;
            fireLight.intensity = 1.0f;
        }
    }
}