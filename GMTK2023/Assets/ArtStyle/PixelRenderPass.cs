using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace ArtStyle
{
    public class PixelRenderPass : ScriptableRenderPass
    {
        private PixelRenderFeature.CustomPassSettings settings;

        private RenderTargetIdentifier _colorBuffer;
        private RenderTargetIdentifier _pixelBuffer;
        
        private int _pixelBufferID = Shader.PropertyToID("_PixelBuffer");

        //private RenderTargetIdentifier pointBuffer;
        //private int pointBufferID = Shader.PropertyToID("_PointBuffer");

        private Material _material;
        private int _pixelScreenWidth;
        private int _pixelScreenHeight;

        public PixelRenderPass(PixelRenderFeature.CustomPassSettings settings)
        {
            this.settings = settings;
            this.renderPassEvent = settings.renderPassEvent;
            
            _material = CoreUtils.CreateEngineMaterial("Hidden/PixelPass");
        }

        public override void OnCameraSetup(CommandBuffer cmd, ref RenderingData renderingData)
        {
            _colorBuffer = renderingData.cameraData.renderer.cameraColorTarget;
            RenderTextureDescriptor descriptor = renderingData.cameraData.cameraTargetDescriptor;

            //cmd.GetTemporaryRT(pointBufferID, descriptor.width, descriptor.height, 0, FilterMode.Point);
            //pointBuffer = new RenderTargetIdentifier(pointBufferID);

            _pixelScreenHeight = settings.screenHeight;
            _pixelScreenWidth = (int)(_pixelScreenHeight * renderingData.cameraData.camera.aspect + 0.5f);

            _material.SetVector("_BlockCount", new Vector2(_pixelScreenWidth, _pixelScreenHeight));
            _material.SetVector("_BlockSize", new Vector2(1.0f / _pixelScreenWidth, 1.0f / _pixelScreenHeight));
            _material.SetVector("_HalfBlockSize", new Vector2(0.5f / _pixelScreenWidth, 0.5f / _pixelScreenHeight));

            descriptor.height = _pixelScreenHeight;
            descriptor.width = _pixelScreenWidth;

            cmd.GetTemporaryRT(_pixelBufferID, descriptor, FilterMode.Point);
            _pixelBuffer = new RenderTargetIdentifier(_pixelBufferID);
        }

        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
        {
            CommandBuffer cmd = CommandBufferPool.Get();
            using (new ProfilingScope(cmd, new ProfilingSampler("PixelPass")))
            {
                // No-shader variant
                //Blit(cmd, colorBuffer, pointBuffer);
                //Blit(cmd, pointBuffer, pixelBuffer);
                //Blit(cmd, pixelBuffer, colorBuffer);

                Blit(cmd, _colorBuffer, _pixelBuffer, _material);
                Blit(cmd, _pixelBuffer, _colorBuffer);
            }

            context.ExecuteCommandBuffer(cmd);
            CommandBufferPool.Release(cmd);
        }

        public override void OnCameraCleanup(CommandBuffer cmd)
        {
            if (cmd == null) throw new System.ArgumentNullException("cmd");
            cmd.ReleaseTemporaryRT(_pixelBufferID);
            //cmd.ReleaseTemporaryRT(pointBufferID);
        }

    }
}