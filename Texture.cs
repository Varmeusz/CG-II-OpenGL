using System;
using System.Drawing;
using System.Numerics;
using System.Drawing.Imaging;
using OpenTK.Graphics.OpenGL4;

namespace CG_II_OpenGL
{
  public class Texture
  {
    public int Id;
    public Texture(string filename)
    {
      GL.CreateTextures(TextureTarget.Texture2D, 1, out Id);
      using(Bitmap i = new System.Drawing.Bitmap(filename))
      {
        BitmapData data;
        data = i.LockBits(new System.Drawing.Rectangle(0, 0, i.Width, i.Height), ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppPArgb);
        GL.TextureStorage2D(Id, 1, SizedInternalFormat.Rgba32f, data.Width, data.Height);
        GL.TextureSubImage2D(Id, 0 , 0, 0, data.Width, data.Height, OpenTK.Graphics.OpenGL4.PixelFormat.Bgra, PixelType.UnsignedByte, data.Scan0);
        i.UnlockBits(data);
        GL.GenerateTextureMipmap(Id); 
      }
      
    }
  }
}
