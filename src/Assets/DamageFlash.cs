using UnityEngine;
using UnityEngine.InputSystem;

// ダメージを受けたときにキャラを点滅させる
// 使い方：GetComponent<DamageFlash>().Flash();
public class DamageFlash : MonoBehaviour
{
    static readonly int HitTimeId = Shader.PropertyToID("_HitTime");

    Material[] materials;

    void Awake()
    {
        // 子オブジェクトも含めて、全部の Renderer のマテリアルを集めておく
        Renderer[] renderers = GetComponentsInChildren<Renderer>();
        materials = new Material[renderers.Length];
        for (int i = 0; i < renderers.Length; i++)
        {
            // .materialを使うと、このキャラ専用のコピーが作られる
            // （sharedMaterialだと、同じマテリアルを使う全員が点滅してしまう）
            materials[i] = renderers[i].material;
        }
    }

    // ダメージを受けたときに外から呼ぶ
    public void Flash()
    {
        float now = Time.timeSinceLevelLoad;
        foreach (Material material in materials)
        {
            material.SetFloat(HitTimeId, now);
        }
    }

    void Update()
    {
#if UNITY_EDITOR
        if (Keyboard.current != null && Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            Flash();
        }
#endif
    }

    void OnDestroy()
    {
        foreach (Material material in materials)
        {
            Destroy(material);
        }
    }
}