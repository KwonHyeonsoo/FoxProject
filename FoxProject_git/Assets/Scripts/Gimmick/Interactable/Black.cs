using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

[System.Serializable]

public class Black : MonoBehaviour
{
    [SerializeField]
    private List<RenderFeatureToggle> renderFeatures = new List<RenderFeatureToggle>();
    [SerializeField]
    private UniversalRenderPipelineAsset pipelineAsset;
    [SerializeField]
    private ScriptableRendererFeature outlineFeature;

    void Update()
    {
        transform.localScale += Vector3.one * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //outline shader off
            outlineFeature.SetActive(false);
            Destroy(gameObject);
        }
    }
}
