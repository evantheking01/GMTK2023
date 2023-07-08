using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace ArtStyle
{
    public class DitherRenderFeature : ScriptableRendererFeature
    {
        [System.Serializable]
        public class CustomPassSettings
        {
            public RenderPassEvent renderPassEvent = RenderPassEvent.BeforeRenderingPostProcessing;

            [Range(0.0f, 1.0f)] public float spread = 0.5f;
            [Range(1, 255)] public int redColorCount = 2;
            [Range(1, 255)] public int greenColorCount = 2;
            [Range(1, 255)] public int blueColorCount = 2;
            [Range(0, 2)] public int bayerLevel = 0;
        }

        [SerializeField] private CustomPassSettings settings;
        private DitherRenderPass _customPass;

        public override void Create()
        {
            _customPass = new DitherRenderPass(settings);
        }
        public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
        {

// #if UNITY_EDITOR
//             if (renderingData.cameraData.isSceneViewCamera) return;
// #endif
            renderer.EnqueuePass(_customPass);
        }
    }
}