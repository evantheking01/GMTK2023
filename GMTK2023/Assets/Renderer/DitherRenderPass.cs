using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace ArtStyle
{
    public class DitherRenderPass : ScriptableRenderPass
    {
        private DitherRenderFeature.CustomPassSettings _settings;
        
        private Material _material;

        private RenderTargetIdentifier _currentSource;
        private RenderTargetIdentifier _currentDestination;

        public DitherRenderPass(DitherRenderFeature.CustomPassSettings settings)
        {
            this._settings = settings;
            this.renderPassEvent = settings.renderPassEvent;
            
            _material = CoreUtils.CreateEngineMaterial("Hidden/DitherPass");
        }

        public override void OnCameraSetup(CommandBuffer cmd, ref RenderingData renderingData)
        {
            RenderTextureDescriptor descriptor = renderingData.cameraData.cameraTargetDescriptor;
            cmd.GetTemporaryRT(Shader.PropertyToID("_MainTex"), descriptor, FilterMode.Point);
            
            _material.SetFloat("_Spread", _settings.spread);
            _material.SetInt("_RedColorCount", _settings.redColorCount);
            _material.SetInt("_GreenColorCount", _settings.greenColorCount);
            _material.SetInt("_BlueColorCount", _settings.blueColorCount);
            _material.SetInt("_BayerLevel", _settings.bayerLevel);
            
#pragma warning disable 0618
            _currentSource = renderingData.cameraData.renderer.cameraColorTarget;
            _currentDestination = new RenderTargetIdentifier(Shader.PropertyToID("_MainTex"));
#pragma warning restore 0618
        }

        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
        {
            CommandBuffer cmd = CommandBufferPool.Get("DitherPass");
            using (new ProfilingScope(cmd, new ProfilingSampler("Dither Pass")))
            {
#pragma warning disable 0618
                cmd.SetRenderTarget(_currentDestination);
                cmd.Blit(_currentSource, _currentDestination, _material);
                cmd.SetRenderTarget(_currentSource);
                cmd.Blit(_currentDestination, _currentSource);
                //Blit(cmd, _currentSource, _currentDestination, _material, 0);
                //Blit(cmd, _currentDestination, _currentSource);
#pragma warning restore 0618

            }
            
            context.ExecuteCommandBuffer(cmd);
            CommandBufferPool.Release(cmd);
        }

        public override void OnCameraCleanup(CommandBuffer cmd)
        {
            cmd.ReleaseTemporaryRT(Shader.PropertyToID("_MainTex"));
        }
    }
}