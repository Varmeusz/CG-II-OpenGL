using System;
using System.Collections.Generic;
using OpenTK.Graphics.OpenGL4;


namespace CG_II_OpenGL
{
  public class Renderable : IDisposable
  {
    public string Name;
    public readonly int Program;
    protected readonly int VAO;
    protected readonly int VBOPositions;
    protected readonly int VBOTexCoords;
    protected readonly int VBONormals;
    protected readonly int EBO;
    protected readonly int VerticeCount;
    private int _indicesLength;
    protected Renderable(int program, int vertexCount, int indicesLength,  string name)
    {
      Name = name;
      Program = program;
      VerticeCount = vertexCount;
      _indicesLength = 0;
      VAO = GL.GenVertexArray();
      VBOPositions = GL.GenBuffer();
      VBOTexCoords = GL.GenBuffer();
      VBONormals = GL.GenBuffer();
    }
    public virtual void Bind()
    {
      GL.UseProgram(Program);
      GL.BindVertexArray(VAO);
    }
    public virtual void Render()
    {
      if(_indicesLength != 0)
        GL.DrawElements(BeginMode.Triangles, _indicesLength, DrawElementsType.UnsignedInt, 0);
      else
        GL.DrawArrays(PrimitiveType.Triangles, 0, VerticeCount);
      Util.CheckGLError("testrend");

    }
    public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                GL.DeleteVertexArray(VAO);
                GL.DeleteBuffer(VBOPositions);
            }
        }
  }
}