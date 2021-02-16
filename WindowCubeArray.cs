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
  public class WindowCubeArray : GameWindow 
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
    private Dictionary<string, Shader> _shaderDict = new Dictionary<string, Shader>();
    private Dictionary<string, Texture> _textureDict = new Dictionary<string, Texture>();
    private List<Renderable> _renderObjects;

    public WindowCubeArray() : base(Util.GetGameWindowSettings(), Util.GetNativeWindowSettings())
    {

    }
    protected override void OnLoad()
    {
      CreateProjection();
      GL.Viewport(0,0,Size.X, Size.Y);
      this.CursorVisible = false;
      this.CursorGrabbed = true; 
      _shaderDict.Add("MainShader", new Shader());
      _shaderDict["MainShader"].AddShader(ShaderType.VertexShader, @"Shaders/VertexMain.hlsl");
      _shaderDict["MainShader"].AddShader(ShaderType.FragmentShader, @"Shaders/FragmentMain.hlsl");
      _shaderDict["MainShader"].Link();
      _textureDict.Add("1", new Texture(@"Textures/bitmapa_1.bmp"));
      _textureDict.Add("2", new Texture(@"Textures/bitmapa_2.bmp"));
      _textureDict.Add("3", new Texture(@"Textures/bitmapa_3.bmp"));
      _textureDict.Add("4", new Texture(@"Textures/bitmapa_4.bmp"));
      _textureDict.Add("5", new Texture(@"Textures/bitmapa_5.bmp"));
      _textureDict.Add("6", new Texture(@"Textures/bitmapa_6.bmp"));
      _renderObjects = new List<Renderable>();
      List<Vector3> normals = new List<Vector3>();
      normals.AddRange(Util.leftNormal);
      normals.AddRange(Util.rightNormal);
      normals.AddRange(Util.bottomNormal);
      normals.AddRange(Util.upNormal);
      normals.AddRange(Util.backNormal);
      normals.AddRange(Util.frontNormal);
      _renderObjects.Add(new RenderObject(Util.CubeVertices, normals.ToArray(), _shaderDict["MainShader"].Id, "cube"));
      
      _camera = new Camera();
      
      GL.PointSize(13.0f);
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
      if(renderMethod==1)
      {
        rotTime += e.Time;
      }
      if(renderMethod==2)
        setRotTime += e.Time;
      if(renderMethod ==3)
        translationOffset += e.Time;
      DrawScene();
      
      
      SwapBuffers();
    }
    public void DrawScene()
    {
      
      GL.Enable(EnableCap.DepthTest);
      CreateProjection();
      
      RenderObject cube = (RenderObject) _renderObjects.Where(x=> x.Name == "cube").First();
      for (int i = 0; i < size; i++)
      {
        for (int j = 0; j < size; j++)
        {
          for (int k = 0; k < size; k++)
          {
            DrawCube(cube, Matrix4.Identity, new Vector3(i-size/2,j-size/2,k-size/2));
          }
        }
      }
      
      
      
      

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
    public void DrawCube(RenderObject cube, Matrix4 Rotation, Vector3 Translation)
    {
      Matrix4 model;
      Matrix4 View = _camera.View();
      cube.Bind();
     
      model = returnModelMatrix(Translation); 
      GL.UniformMatrix4(20, false, ref _projectionMatrix);
      GL.UniformMatrix4(21, false, ref model);
      GL.UniformMatrix4(22, false,  ref View);
      var tvec = new Vector3(0, 2, 0);
      GL.Uniform3(23, ref tvec);
      GL.Uniform3(24, ref _camera.Position);
      cube.Render();  
    }
    public Matrix4 model = Matrix4.Identity;
    public Matrix4 translation = Matrix4.Identity;
    public Matrix4 individualRotation = Matrix4.Identity;
    public Matrix4 setRotation = Matrix4.Identity;
    public Matrix4 returnModelMatrix(Vector3 Translation)
    {
      model = Matrix4.Identity;
      switch(renderMethod)
      {
        case(1):
          individualRotation = Matrix4.CreateRotationY((float)rotTime);
          translation = Matrix4.CreateTranslation(Translation*(MathF.Sin((float)translationOffset)+2));
          model *= individualRotation;
          model *= translation;
          model *= setRotation;
          return model;
        case(2):
          translation = Matrix4.CreateTranslation(Translation*(MathF.Sin((float)translationOffset)+2));
          setRotation = Matrix4.CreateRotationY((float)(setRotTime));
          model *= individualRotation;
          model *= translation;
          model *= setRotation;
          return model;
        case(3):
          model*=individualRotation;
          translation = Matrix4.CreateTranslation(Translation*(MathF.Sin((float)translationOffset)+2));
          model *= translation;
          model *= setRotation;
          return model;
        case(5):

          translation = Matrix4.CreateTranslation(Translation*(MathF.Sin((float)translationOffset)+2));
          model *= individualRotation;
          model*=translation;
          model *= setRotation;
          return model;
        default:
          return model;
        
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
      var mousestate = MouseState;
      if(mousestate.ScrollDelta.Y>0)
      {
        size+=1;
      }else 
      if(mousestate.ScrollDelta.Y<0)
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
      if(keyState.IsKeyDown(Keys.D1))
        renderMethod = 1;
      if(keyState.IsKeyDown(Keys.D2))
        renderMethod = 2;
      if(keyState.IsKeyDown(Keys.D3))
        renderMethod = 3;
      if(keyState.IsKeyDown(Keys.D4))
        renderMethod = 4;
      if(keyState.IsKeyDown(Keys.D5))
        renderMethod = 5;

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