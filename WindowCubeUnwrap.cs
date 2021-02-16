using System;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using OpenTK.Windowing.Common;

namespace CG_II_OpenGL
{
  public class WindowCubeUnwrap : GameWindow 
  {
    private Camera _camera;
    private Matrix4 _projectionMatrix;
    private float _fov = 45f;
    private double _time;
    private float[] rotateValues = {0, 0, 0, 0};
    private float rotateSpeed = 0.005f;
    private float _cameraRot = 0f;
    private Dictionary<string, Shader> _shaderDict = new Dictionary<string, Shader>();
    private Dictionary<string, Texture> _textureDict = new Dictionary<string, Texture>();
    private List<Renderable> _renderObjects;

    public WindowCubeUnwrap() : base(Util.GetGameWindowSettings(), Util.GetNativeWindowSettings())
    {

    }
    protected override void OnLoad()
    {
      CreateProjection();
      GL.Viewport(0,0,Size.X, Size.Y);
      this.CursorVisible = false;
      this.CursorGrabbed = true; 
      _shaderDict.Add("MainShader", new Shader());
      _shaderDict["MainShader"].AddShader(ShaderType.VertexShader, @"Shaders/VertexMainTexture.hlsl");
      _shaderDict["MainShader"].AddShader(ShaderType.FragmentShader, @"Shaders/FragmentMainTexture.hlsl");
      _shaderDict["MainShader"].Link();
      




      _textureDict.Add("1", new Texture(@"Textures/bitmapa_1.bmp"));
      _textureDict.Add("2", new Texture(@"Textures/bitmapa_2.bmp"));
      _textureDict.Add("3", new Texture(@"Textures/bitmapa_3.bmp"));
      _textureDict.Add("4", new Texture(@"Textures/bitmapa_4.bmp"));
      _textureDict.Add("5", new Texture(@"Textures/bitmapa_5.bmp"));
      _textureDict.Add("6", new Texture(@"Textures/bitmapa_6.bmp"));
      _renderObjects = new List<Renderable>();

      _renderObjects.Add(new RenderObject(Util.CubeVertices.Take(6).ToArray(), Util.PlaneTexCoordswh.Take(6).ToArray(),Util.leftNormal, _shaderDict["MainShader"].Id, _textureDict["1"].Id, "1"));
      _renderObjects.Add(new RenderObject(Util.CubeVertices.Skip(6).Take(6).ToArray(), Util.PlaneTexCoordswh.Skip(6).Take(6).ToArray(),Util.rightNormal, _shaderDict["MainShader"].Id, _textureDict["2"].Id, "2"));
      _renderObjects.Add(new RenderObject(Util.CubeVertices.Skip(12).Take(6).ToArray(), Util.PlaneTexCoordswh.Skip(12).Take(6).ToArray(), Util.bottomNormal, _shaderDict["MainShader"].Id, _textureDict["3"].Id, "3"));
      _renderObjects.Add(new RenderObject(Util.CubeVertices.Skip(18).Take(6).ToArray(), Util.PlaneTexCoordswh.Skip(18).Take(6).ToArray(), Util.upNormal, _shaderDict["MainShader"].Id, _textureDict["4"].Id, "4"));
      _renderObjects.Add(new RenderObject(Util.CubeVertices.Skip(24).Take(6).ToArray(), Util.PlaneTexCoordswh.Skip(24).Take(6).ToArray(), Util.backNormal, _shaderDict["MainShader"].Id, _textureDict["5"].Id, "5"));
      _renderObjects.Add(new RenderObject(Util.CubeVertices.Skip(30).Take(6).ToArray(), Util.PlaneTexCoordswh.Skip(30).Take(6).ToArray(), Util.frontNormal, _shaderDict["MainShader"].Id, _textureDict["6"].Id, "6"));

      _camera = new Camera();
      
      GL.PointSize(13.0f);
      GL.PatchParameter(PatchParameterInt.PatchVertices, 13);
      GL.Disable(EnableCap.CullFace);
      
    }
    protected override void OnUpdateFrame(FrameEventArgs e)
    {
      HandleInput(e);
      if(CursorVisible == false)
      {
        var m = MouseState.Position;
        _camera.MoveAround(m.X, m.Y);
      }
      
    }
    protected override void OnRenderFrame(FrameEventArgs e)
    {
      _time += e.Time;
      var x =_camera.Position;
      Title = MathF.Round((float)x.X,2).ToString()+" " + MathF.Round((float)x.Y,2).ToString() + " "+ MathF.Round((float)x.Z,2).ToString();

      GL.ClearColor(Color4.Black);
      GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
      DrawScene();
      
      
      SwapBuffers();
    }
    public void DrawScene()
    {
      
      GL.Enable(EnableCap.DepthTest);
      CreateProjection();
      Matrix4 model;
      Matrix4 View = _camera.View();
            
      for(int i = 0; i < 5; i++)
      {
        foreach (var item in _renderObjects)
        {
          model = Matrix4.Identity;
          item.Bind();
          if(item.Name == "1")
          {
            model *= Matrix4.CreateTranslation(0.5f, 0.5f,0);
            model *= Matrix4.CreateRotationZ(rotateValues[0]);
            model *= Matrix4.CreateTranslation(-0.5f, -0.5f,0);
          }
          if(item.Name == "2")
          {
            model *= Matrix4.CreateTranslation(-0.5f,.5f,0);
            model *= Matrix4.CreateRotationZ(-rotateValues[1]);
            model *= Matrix4.CreateTranslation(0.5f,-0.5f,0);
          }
          if(item.Name == "4")
          {
            float s = MathF.Sin((float)-rotateValues[1]);
            float c = MathF.Cos((float)-rotateValues[1]);
          
            model *= Matrix4.CreateTranslation(-0.5f,-0.5f,0);
            model *= Matrix4.CreateRotationZ((float)-2*rotateValues[1]);

            model *= Matrix4.CreateTranslation(0.5f,+0.5f,0);
            model *= Matrix4.CreateTranslation(0,-1,0);
            
            model *= Matrix4.CreateTranslation(-s,c,0);
          }
          if(item.Name == "5")
          {
            model *= Matrix4.CreateTranslation(-0.5f,.5f,+0.5f);
            model *= Matrix4.CreateRotationX(-rotateValues[2]);
            model *= Matrix4.CreateTranslation(0.5f,-.5f,-0.5f);
          }
          if(item.Name == "6")
          {
            model *= Matrix4.CreateTranslation(-0.5f, .5f, -0.5f);
            model *= Matrix4.CreateRotationX(rotateValues[3]);
            model *= Matrix4.CreateTranslation(0.5f, -.5f, 0.5f);
          }
          model *= Matrix4.CreateTranslation(i*2,0,-i*2);
          GL.UniformMatrix4(20, false, ref _projectionMatrix);
          GL.UniformMatrix4(21, false, ref model);
          GL.UniformMatrix4(22, false,  ref View);
          var tvec = new Vector3(0, 2, 0);
          GL.Uniform3(23, ref tvec);
          GL.Uniform3(24, ref _camera.Position);
          item.Render();

        }
      }
      
      

    }

    protected override void OnResize(ResizeEventArgs e)
    {
      base.OnResize(e);
      GL.Viewport(0,0,Size.X, Size.Y);
      CreateProjection();
    }
    private void HandleInput(FrameEventArgs e)
    {
      var keyState = KeyboardState;
      if(keyState.IsKeyDown(Keys.Escape))
        Close();
      var camSpeed = CursorVisible == false ? _camera.Speed * (float) e.Time : 0;
      if(keyState.IsKeyDown(Keys.Up))
        _camera.Position += camSpeed * _camera.Target;
      if(keyState.IsKeyDown(Keys.Down))
        _camera.Position -= camSpeed * _camera.Target;
      if(keyState.IsKeyDown(Keys.Q))
      {
        rotateValues[0] += rotateSpeed;
        if(rotateValues[0] > MathF.PI/2) rotateValues[0]=MathF.PI/2;
      }
      if(keyState.IsKeyDown(Keys.A))
      {
        rotateValues[0] -= rotateSpeed;
        if(rotateValues[0] < 0) rotateValues[0]=0;
      }
      if(keyState.IsKeyDown(Keys.W))
      {
        rotateValues[1] += rotateSpeed;
        if(rotateValues[1] > MathF.PI/2) rotateValues[1]=MathF.PI/2;
      }
      if(keyState.IsKeyDown(Keys.S))
      {
        rotateValues[1] -= rotateSpeed;
        if(rotateValues[1] < 0) rotateValues[1]=0;
      }
      if(keyState.IsKeyDown(Keys.E))
      {
        rotateValues[2] += rotateSpeed;
        if(rotateValues[2] > MathF.PI/2) rotateValues[2]=MathF.PI/2;
      }
      if(keyState.IsKeyDown(Keys.D))
      {
        rotateValues[2] -= rotateSpeed;
        if(rotateValues[2] < 0) rotateValues[2]=0;
      }
      if(keyState.IsKeyDown(Keys.R))
      {
        rotateValues[3] += rotateSpeed;
        if(rotateValues[3] > MathF.PI/2) rotateValues[3]=MathF.PI/2;
      }
      if(keyState.IsKeyDown(Keys.F))
      {
        rotateValues[3] -= rotateSpeed;
        if(rotateValues[3] < 0) rotateValues[3]=0;
      }

      if(keyState.IsKeyDown(Keys.Left))
      {
        _camera.Position -= Vector3.Normalize(Vector3.Cross(_camera.Target, _camera.upVector)) * (camSpeed );
      }
      if(keyState.IsKeyDown(Keys.Right))
        _camera.Position += Vector3.Normalize(Vector3.Cross(_camera.Target, _camera.upVector)) * (camSpeed );
      if(keyState.IsKeyDown(Keys.Space))
        _camera.Position += camSpeed * new Vector3(0,0.5f,0);
      if(keyState.IsKeyDown(Keys.LeftShift))
        _camera.Position += camSpeed * new Vector3(0,-0.5f,0);

    }

    private void OnClosed(object sender, System.EventArgs e)
    {
      Close();
    }
    public override void Close()
    {
      base.Close();
    }


    private void CreateProjection()
    {
      var aspectRatio = (float) (800/800);
      //_projectionMatrix = Matrix4.CreateOrthographic(Width,Height,1f,3f);
      _projectionMatrix = Matrix4.CreatePerspectiveFieldOfView(
          _fov * ((float)Math.PI / 180f),
          aspectRatio,
          0.1f,
          1000f);
    }
  }
}