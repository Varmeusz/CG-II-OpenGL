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
  public class WindowFxy : GameWindow 
  {
    private Camera _camera;
    private Matrix4 _projectionMatrix;
    private float _fov = 45f;
    private double _time;
    public  int size = 3;
    public double rotTime = 0;
    public double setRotTime = 0;
    public double translationOffset = 1;
    public int renderMethod = 5;
    private string equation;
    private Dictionary<string, Shader> _shaderDict = new Dictionary<string, Shader>();
    private Dictionary<string, Texture> _textureDict = new Dictionary<string, Texture>();
    private List<Renderable> _renderObjects;
    private FunMesh myFun;
    
    private float heightMax; 
    private float heightMin;
    public WindowFxy(string eq) : base(Util.GetGameWindowSettings(), Util.GetNativeWindowSettings())
    {
      equation = eq;
    }
    protected override void OnLoad()
    {
      CreateProjection();
      GL.Viewport(0,0,Size.X, Size.Y);
      this.CursorVisible = false;
      this.CursorGrabbed = true; 
      _shaderDict.Add("MainShader", new Shader());
      _shaderDict["MainShader"].AddShader(ShaderType.VertexShader, @"Shaders/VertexMain.hlsl");
      _shaderDict["MainShader"].AddShader(ShaderType.FragmentShader, @"Shaders/FragmentMain-fxy.hlsl");
      _shaderDict["MainShader"].Link();
      _renderObjects = new List<Renderable>();
      myFun = new FunMesh();
      myFun.Calculator = new RPN(equation);
      if (!myFun.Calculator.properEquation()) 
      {
        Close();
      }
      try
      {
        myFun.Calculator.generateInfixTokens();
      }
      catch(Exception ex)
      {
        Close();
      }
      
      if (myFun.Calculator.invalidTokens)
      {
        Close();
      }
      myFun.Calculator.generatePostfixTokens();
      try
      {
        myFun.calcMesh(10);
      }
      catch(Exception ex)
      {
        Console.WriteLine(ex.Message);
        Close();
      }
      heightMin = myFun.Vertices.OrderByDescending(x=>x.Y).Last().Y;
      heightMax = myFun.Vertices.OrderByDescending(x=>x.Y).First().Y;
      _renderObjects.Add(new RenderObject(myFun.Vertices.ToArray(), myFun.Normals.ToArray(), _shaderDict["MainShader"].Id, "mesh"));

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
      var mesh = _renderObjects.Where(x=>x.Name=="mesh").First();
      mesh.Bind();
      Matrix4 model = Matrix4.Identity;
      Matrix4 View = _camera.View();
      GL.UniformMatrix4(20, false, ref _projectionMatrix);
      GL.UniformMatrix4(21, false, ref model);
      GL.UniformMatrix4(22, false,  ref View);
      var lightPos = new Vector3(0, 5, 0);
      GL.Uniform3(23, ref lightPos);
      GL.Uniform3(24, ref _camera.Position);
      GL.Uniform1(25, heightMin);
      GL.Uniform1(26, heightMax);
      mesh.Render();
      SwapBuffers();
    }
    public float[]  values = {0,1,-1};
    public static Vector3[] translations =
    {
      new Vector3(0,0,0),
      new Vector3(1,0,0),
      new Vector3(1,1,0),
      new Vector3(1,1,1),
      new Vector3(0,1,0),
      new Vector3(0,1,1),
      new Vector3(0,0,1),
      new Vector3(1,0,1)
    };
    protected override void OnResize(ResizeEventArgs e)
    {
      base.OnResize(e);
      GL.Viewport(0,0,Size.X, Size.Y);
      CreateProjection();
    }
    private void HandleInput(FrameEventArgs e)
    {
      var keyState = KeyboardState;
      var mousestate = MouseState;
      if(mousestate.ScrollDelta.Y>0)
        size+=1;
      else if(mousestate.ScrollDelta.Y<0)
        size-=1;
      if(keyState.IsKeyDown(Keys.Escape))
        Close();
      var camSpeed = CursorVisible == false ? _camera.Speed * (float) e.Time : 0;
      if(keyState.IsKeyDown(Keys.Up))
        _camera.Position += camSpeed * _camera.Target;
      if(keyState.IsKeyDown(Keys.Down))
        _camera.Position -= camSpeed * _camera.Target;
      

      if(keyState.IsKeyDown(Keys.Left))
        _camera.Position -= Vector3.Normalize(Vector3.Cross(_camera.Target, _camera.upVector)) * (camSpeed );
      if(keyState.IsKeyDown(Keys.Right))
        _camera.Position += Vector3.Normalize(Vector3.Cross(_camera.Target, _camera.upVector)) * (camSpeed );
      if(keyState.IsKeyDown(Keys.Space))
        _camera.Position += camSpeed * new Vector3(0,0.5f,0);
      if(keyState.IsKeyDown(Keys.LeftShift) || keyState.IsKeyDown(Keys.RightShift))
        _camera.Position += camSpeed * new Vector3(0,-0.5f,0);
      if(keyState.IsKeyDown(Keys.Comma))
        GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Line);
      if(keyState.IsKeyDown(Keys.Period))
        GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Fill);
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