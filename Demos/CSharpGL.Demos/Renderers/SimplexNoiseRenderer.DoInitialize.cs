﻿using System;
using System.Drawing;

namespace CSharpGL.Demos
{
    [DemoRenderer]
    partial class SimplexNoiseRenderer
    {
        protected override void DoInitialize()
        {
            base.DoInitialize();

            lastTime = DateTime.Now;

            var bitmap = new Bitmap(@"Textures\sunColor.png");
            var texture = new Texture(BindTextureTarget.Texture1D, bitmap, new SamplerParameters());
            texture.Initialize();
            bitmap.Dispose();
            this.SetUniform("sunColor", texture.ToSamplerValue());
        }
    }
}