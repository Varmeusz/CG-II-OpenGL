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
using OpenTK.Audio;

namespace CG_II_OpenGL
{
    public class WindowBlocks : GameWindow
    {
        private Camera _camera;
        private Matrix4 _projectionMatrix;
        private float _fov = 45f;
        private double _time;
        public int size = 3;
        public double rotTime = 0;
        public double setRotTime = 0;
        public double translationOffset = 1;
        public int renderMethod = 5;
        private Dictionary<string, Shader> _shaderDict = new Dictionary<string, Shader>();
        private Dictionary<string, Texture> _textureDict = new Dictionary<string, Texture>();
        private List<Renderable> _renderObjects;

        public WindowBlocks() : base(Util.GetGameWindowSettings(), Util.GetNativeWindowSettings())
        {

        }
        protected override void OnLoad()
        {
            CreateProjection();
            GL.Viewport(0, 0, Size.X, Size.Y);
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
            _camera.Position=new Vector3(0,0,12);
            GL.PointSize(13.0f);
            GL.Disable(EnableCap.CullFace);

        }
        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            HandleInput(e);
            if (CursorVisible == false)
            {
                var m = MouseState.Position;
                _camera.MoveAround(m.X, m.Y);
            }

        }
        public static Random myRand = new Random();
        protected override void OnRenderFrame(FrameEventArgs e)
        {
            _time += e.Time;
            var x = _camera.Position;
            Title = MathF.Round((float)x.X, 2).ToString() + " " + MathF.Round((float)x.Y, 2).ToString() + " " + MathF.Round((float)x.Z, 2).ToString();
            GL.ClearColor(Color4.Black);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            RenderObject cube = (RenderObject)_renderObjects.Where(x => x.Name == "cube").First();
            yPos -= (float)e.Time;
            if (yPos < -12)
            {
                yPos = 11f;
                chosenPiece = myRand.Next(0, 8);
            }
            if (currentRot < userRot)
            {
                currentRot += (float)e.Time * 10;
            }
            else if (currentRot > MathF.PI * 2)
            {
                currentRot = 0;
                userRot = 0;
            }
            var rememberRot = currentRot;
            var pos = TetrisBlock.Positions(chosenPiece);
            DrawPiece(cube, new Vector3(0, 0, 0));
            DrawPiece(cube, pos[0]);
            DrawPiece(cube, pos[1]);
            DrawPiece(cube, pos[2]);
            DrawPiece(cube, pos[3]);
            SwapBuffers();
        }
        public static float[] Rotations = { 0, MathF.PI / 2, MathF.PI, MathF.PI * 3 / 2 };
        public float yPos = 11f;
        public float userRot = 0;
        public float currentRot = 0;
        public int chosenPiece = 3;

        public void DrawPiece(RenderObject cube, Vector3 Translation)
        {
            Matrix4 model;
            Matrix4 View = _camera.View();
            cube.Bind();

            model = Matrix4.Identity * Matrix4.CreateTranslation(Translation);
            model *= Matrix4.CreateRotationZ((float)currentRot);
            model *= Matrix4.CreateTranslation(0, yPos, 0);
            GL.UniformMatrix4(20, false, ref _projectionMatrix);
            GL.UniformMatrix4(21, false, ref model);
            GL.UniformMatrix4(22, false, ref View);
            var tvec = new Vector3(0, 2, 0);
            GL.Uniform3(23, ref tvec);
            GL.Uniform3(24, ref _camera.Position);
            cube.Render();
        }
        protected override void OnResize(ResizeEventArgs e)
        {
            base.OnResize(e);
            GL.Viewport(0, 0, Size.X, Size.Y);
            CreateProjection();
        }
        private void HandleInput(FrameEventArgs e)
        {
            var keyState = KeyboardState;
            var mousestate = MouseState;
            if (mousestate.ScrollDelta.Y > 0)
            {
                size += 1;
            }
            else
            if (mousestate.ScrollDelta.Y < 0)
                size -= 1;
            if (keyState.IsKeyDown(Keys.Escape))
                Close();
            var camSpeed = CursorVisible == false ? _camera.Speed * (float)e.Time : 0;
            if (keyState.IsKeyDown(Keys.Up))
                _camera.Position += camSpeed * _camera.Target;
            if (keyState.IsKeyDown(Keys.Down))
                _camera.Position -= camSpeed * _camera.Target;


            if (keyState.IsKeyDown(Keys.Left))
                _camera.Position -= Vector3.Normalize(Vector3.Cross(_camera.Target, _camera.upVector)) * (camSpeed);
            if (keyState.IsKeyDown(Keys.Right))
                _camera.Position += Vector3.Normalize(Vector3.Cross(_camera.Target, _camera.upVector)) * (camSpeed);
            if (keyState.IsKeyDown(Keys.Space))
                _camera.Position += camSpeed * new Vector3(0, 0.5f, 0);
            if (keyState.IsKeyDown(Keys.LeftShift) || keyState.IsKeyDown(Keys.RightShift))
                _camera.Position += camSpeed * new Vector3(0, -0.5f, 0);
            if (keyState.IsKeyPressed(Keys.D1))
                userRot += MathF.PI / 2;
            if (keyState.IsKeyPressed(Keys.D5))
                chosenPiece = myRand.Next(0, 7);
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
            var aspectRatio = (float)(Size.X / Size.Y);
            // _projectionMatrix = Matrix4.CreateOrthographic(10,10, 0, 300);
            _projectionMatrix = Matrix4.CreatePerspectiveFieldOfView(
                _fov * ((float)Math.PI / 180f),
                aspectRatio,
                0.1f,
                1000f);
        }
    }
}