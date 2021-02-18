using OpenTK;
using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace CG_II_OpenGL
{
  public class Shader : IDisposable
  {
    public int Id => _program;
    private readonly int _program;
    private readonly List<int> _shaders = new List<int>();
    
    public Shader()
    {
        _program = GL.CreateProgram();
    }
    public void AddShader(ShaderType type, string path)
    {
      var shader = GL.CreateShader(type);
      var src = File.ReadAllText(path);
      GL.ShaderSource(shader, src);
      GL.CompileShader(shader);
      checkForCompileErrors(shader);
      _shaders.Add(shader);      
    }
    public void Link()
    {
      foreach(var shader in _shaders)
      {
        GL.AttachShader(_program, shader);
      }
      GL.LinkProgram(_program);
      checkForLinkingErrors(_program);
      foreach(var shader in _shaders)
      {
        GL.DetachShader(_program, shader);
        GL.DeleteShader(shader);
      }
    }
    public void checkForCompileErrors(int shader)
    {
      int success;
      GL.GetShader(shader, ShaderParameter.CompileStatus, out success);
      if(success==0){
        var infoLog = GL.GetShaderInfoLog(shader);
        Debug.WriteLine($"GL.CompileShader [vertexShader] had info log {infoLog}");
      }
    }
    public void checkForLinkingErrors(int program)
    {
      int success;
      GL.GetProgram(program, GetProgramParameterName.LinkStatus, out success);
      if(success==0)
      {
        var infoLog = GL.GetProgramInfoLog(program);
        Debug.WriteLine($"GL.LinkProgram {program} log: {infoLog}");
      }
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
        GL.DeleteProgram(_program);
      }
    }
 
  
  }
}