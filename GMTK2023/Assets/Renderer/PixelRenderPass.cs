using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace ArtStyle
{
    public class PixelRenderPass : ScriptableRenderPass
    {
        private PixelRenderFeature.CustomPassSettings _settings;
        
        private RenderTargetIdentifier _colorBuffer;
        private RenderTargetIdentifier _pixelBuffer;

        private int _pixelBufferID;

        //private RenderTargetIdentifier pointBuffer;
        //private int pointBufferID = Shader.PropertyToID("_PointBuffer");

        private Material _material;
        private int _pixelScreenWidth;
        private int _pixelScreenHeight;

        public PixelRenderPass(PixelRenderFeature.CustomPassSettings settings)
        {
            this._settings = settings;
            this.renderPassEvent = settings.renderPassEvent;

            _material = CoreUtils.CreateEngineMaterial("Hidden/PixelPass");
            _pixelBufferID = Shader.PropertyToID("_PixelBuffer");
        }

        public override void OnCameraSetup(CommandBuffer cmd, ref RenderingData renderingData)
        {
            RenderTextureDescriptor descriptor = renderingData.cameraData.cameraTargetDescriptor;
            
            _pixelScreenHeight = _settings.screenHeight;
            _pixelScreenWidth = (int)(_pixelScreenHeight * renderingData.cameraData.camera.aspect + 0.5f);

            _material.SetVector("_BlockCount", new Vector2(_pixelScreenWidth, _pixelScreenHeight));
            _material.SetVector("_BlockSize", new Vector2(1.0f / _pixelScreenWidth, 1.0f / _pixelScreenHeight));
            _material.SetVector("_HalfBlockSize", new Vector2(0.5f / _pixelScreenWidth, 0.5f / _pixelScreenHeight));
            
            cmd.GetTemporaryRT(_pixelBufferID, descriptor, FilterMode.Point);
            
#pragma warning disable 0618
            _colorBuffer = renderingData.cameraData.renderer.cameraColorTarget;
            _pixelBuffer = new RenderTargetIdentifier(_pixelBufferID);
#pragma warning restore 0618
        }

        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
        {
            CommandBuffer cmd = CommandBufferPool.Get("PixelPass");
            using (new ProfilingScope(cmd, new ProfilingSampler("Pixel Pass")))
            {
#pragma warning disable 0618
                cmd.SetRenderTarget(_pixelBuffer);
                cmd.Blit(_colorBuffer, _pixelBuffer, _material);
                cmd.SetRenderTarget(_colorBuffer);
                cmd.Blit(_pixelBuffer, _colorBuffer);
                //Blit(cmd, _colorBuffer, _pixelBuffer, _material);
                //Blit(cmd, _pixelBuffer, _colorBuffer);
#pragma warning restore 0618
            }

            context.ExecuteCommandBuffer(cmd);
            CommandBufferPool.Release(cmd);
        }

        public override void OnCameraCleanup(CommandBuffer cmd)
        {
            cmd.ReleaseTemporaryRT(_pixelBufferID);
        }

    }
}