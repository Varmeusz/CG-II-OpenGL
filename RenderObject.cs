using System;
using System.Drawing;
using System.Drawing.Imaging;
using OpenTK;
using OpenTK.Mathematics;
using OpenTK.Graphics.OpenGL4;
using PixelFormat = OpenTK.Graphics.OpenGL4.PixelFormat;

namespace CG_II_OpenGL
{
  public class RenderObject : Renderable
  {
    private int _texture;
    
    public RenderObject(Vector3[] vertices, Vector3[] normals, int program, string name)
        : base(program, vertices.Length, 0, name)
    {
      //Bind the VAO
      GL.BindVertexArray(VAO);
      //Bind the VBO containing positions
      GL.BindBuffer(BufferTarget.ArrayBuffer, VBOPositions);
      GL.BufferData<Vector3>(BufferTarget.ArrayBuffer, (IntPtr) (vertices.Length * 3*sizeof(float)), vertices, BufferUsageHint.StaticDraw);
      GL.VertexAttribPointer(0,3,VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
      GL.EnableVertexArrayAttrib(VAO, 0);
      //Bind the VBO containing normals
      GL.BindBuffer(BufferTarget.ArrayBuffer, VBONormals);
      GL.BufferData<Vector3>(BufferTarget.ArrayBuffer, (IntPtr) (normals.Length * 3*sizeof(float)), normals, BufferUsageHint.StaticDraw);
      GL.VertexAttribPointer(1,3,VertexAttribPointerType.Float, true, 3 * sizeof(float), 0);
      GL.EnableVertexArrayAttrib(VAO, 1);
    }
    public RenderObject(Vector3[] vertices, Vector2[] texcoords, Vector3[] normals, int program, int texID, string name)
        : base(program, vertices.Length, 0,  name)
    {
      _texture = texID;
      //Bind the VAO
      GL.BindVertexArray(VAO);
      //Bind the VBO containing positions
      GL.BindBuffer(BufferTarget.ArrayBuffer, VBOPositions);
      GL.BufferData<Vector3>(BufferTarget.ArrayBuffer, (IntPtr) (vertices.Length * 3*sizeof(float)), vertices, BufferUsageHint.StaticDraw);
      GL.VertexAttribPointer(0,3,VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
      GL.EnableVertexArrayAttrib(VAO, 0);
      //Bind the VBO containing tex coords
      GL.BindBuffer(BufferTarget.ArrayBuffer, VBOTexCoords);
      GL.BufferData<Vector2>(BufferTarget.ArrayBuffer, (IntPtr) (texcoords.Length * 2*sizeof(float)), texcoords, BufferUsageHint.StaticDraw);
      GL.VertexAttribPointer(1,2,VertexAttribPointerType.Float, true, 2 * sizeof(float), 0);
      GL.EnableVertexArrayAttrib(VAO, 1);
      //Bind the VBO containing normals
      GL.BindBuffer(BufferTarget.ArrayBuffer, VBONormals);
      GL.BufferData<Vector3>(BufferTarget.ArrayBuffer, (IntPtr) (normals.Length * 3*sizeof(float)), normals, BufferUsageHint.StaticDraw);
      GL.VertexAttribPointer(2,3,VertexAttribPointerType.Float, true, 3 * sizeof(float), 0);
      GL.EnableVertexArrayAttrib(VAO, 2);
    }
    public RenderObject(Vector3[] vertices, Vector2[] texcoords, Vector3[] normals, int[] indices, int program, int texID, string name)
        : base(program, vertices.Length, indices.Length,  name)
    {
      _texture = texID;
      //Bind the VAO
      GL.BindVertexArray(VAO);
      //Bind the VBO containing positions
      GL.BindBuffer(BufferTarget.ArrayBuffer, VBOPositions);
      GL.BufferData<Vector3>(BufferTarget.ArrayBuffer, (IntPtr) (vertices.Length * 3*sizeof(float)), vertices, BufferUsageHint.StaticDraw);
      GL.VertexAttribPointer(0,3,VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
      GL.EnableVertexArrayAttrib(VAO, 0);
      //Bind the VBO containing tex coords
      GL.BindBuffer(BufferTarget.ArrayBuffer, VBOTexCoords);
      GL.BufferData<Vector2>(BufferTarget.ArrayBuffer, (IntPtr) (texcoords.Length * 2*sizeof(float)), texcoords, BufferUsageHint.StaticDraw);
      GL.VertexAttribPointer(1,2,VertexAttribPointerType.Float, true, 2 * sizeof(float), 0);
      GL.EnableVertexArrayAttrib(VAO, 1);
      //Bind the VBO containing normals
      GL.BindBuffer(BufferTarget.ArrayBuffer, VBONormals);
      GL.BufferData<Vector3>(BufferTarget.ArrayBuffer, (IntPtr) (normals.Length * 3*sizeof(float)), normals, BufferUsageHint.StaticDraw);
      GL.VertexAttribPointer(2,3,VertexAttribPointerType.Float, true, 3 * sizeof(float), 0);
      GL.EnableVertexArrayAttrib(VAO, 2);
      //Bind the EBO
      GL.BindBuffer(BufferTarget.ElementArrayBuffer, EBO);
      GL.BufferData(BufferTarget.ElementArrayBuffer, indices.Length * sizeof(int), indices, BufferUsageHint.StaticDraw);
    }

    public override void Bind()
    {
      base.Bind();
      if(_texture>0)
      GL.BindTextureUnit(0, _texture);

    }
    public override void Render()
    {
      GL.Enable(EnableCap.DepthTest);
      base.Render();
      GL.Disable(EnableCap.DepthTest);
    }
    protected override void Dispose(bool disposing)
    {
      if (disposing)
      {
        GL.DeleteTexture(_texture);
      }
      base.Dispose(disposing);
    }
  }
}
