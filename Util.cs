using System;
using OpenTK.Windowing.Desktop;
using OpenTK.Mathematics;
using OpenTK.Graphics.OpenGL4;
using System.Diagnostics;

namespace CG_II_OpenGL
{
  public static class Util
  {
    public static GameWindowSettings GetGameWindowSettings()
    {
      var settings = new GameWindowSettings();
      return settings;
    }
    public static NativeWindowSettings GetNativeWindowSettings()
    {
      var settings = new NativeWindowSettings();
      settings.Size = new Vector2i(800, 800);
      return settings;
    }
    public static readonly Vector3[] PlaneNormals =
    {
      new Vector3(-1,0,0),
      new Vector3(-1,0,0),
      new Vector3(-1,0,0),
      new Vector3(-1,0,0),
      new Vector3(0,0,-1),
      new Vector3(0,0,-1),
      new Vector3(0,0,-1),
      new Vector3(0,0,-1),
      new Vector3(1,0,0),
      new Vector3(1,0,0),
      new Vector3(1,0,0),
      new Vector3(1,0,0),
      new Vector3(0,1,0),
      new Vector3(0,1,0),
      new Vector3(0,1,0),
      new Vector3(0,1,0),
      new Vector3(0,0,1),
      new Vector3(0,0,1),
      new Vector3(0,0,1),
      new Vector3(0,0,1),
      new Vector3(0,0,0),
      new Vector3(0,0,0),
      new Vector3(0,0,0),
      new Vector3(0,0,0)
    };
    public static Vector3[] leftNormal =
    {
      new Vector3(-1,0,0),
      new Vector3(-1,0,0),
      new Vector3(-1,0,0),
      new Vector3(-1,0,0),
      new Vector3(-1,0,0),
      new Vector3(-1,0,0)
    };
    public static Vector3[] rightNormal =
    {
      new Vector3(1,0,0),
      new Vector3(1,0,0),
      new Vector3(1,0,0),
      new Vector3(1,0,0),
      new Vector3(1,0,0),
      new Vector3(1,0,0)
    };
    public static Vector3[] bottomNormal =
    {
      new Vector3(0,-1,0),
      new Vector3(0,-1,0),
      new Vector3(0,-1,0),
      new Vector3(0,-1,0),
      new Vector3(0,-1,0),
      new Vector3(0,-1,0)
    };
    public static Vector3[] upNormal =
    {
      new Vector3(0,1,0),
      new Vector3(0,1,0),
      new Vector3(0,1,0),
      new Vector3(0,1,0),
      new Vector3(0,1,0),
      new Vector3(0,1,0)
    };
    public static Vector3[] backNormal =
    {
      new Vector3(0,0,-1),
      new Vector3(0,0,-1),
      new Vector3(0,0,-1),
      new Vector3(0,0,-1),
      new Vector3(0,0,-1),
      new Vector3(0,0,-1)
    };
    public static Vector3[] frontNormal =
    {
      new Vector3(0,0,1),
      new Vector3(0,0,1),
      new Vector3(0,0,1),
      new Vector3(0,0,1),
      new Vector3(0,0,1),
      new Vector3(0,0,1)
    };

    public static readonly Vector3[] CubeColors =
    {
      new Vector3(0,0,0),
      new Vector3(1,0,0),
      new Vector3(1,0,1),
      new Vector3(0,0,1),
      new Vector3(0,1,0),
      new Vector3(1,1,0),
      new Vector3(1,1,1),
      new Vector3(0,1,1),
    };
    public static readonly Vector2[] TextureCoords =
    {
            //left
            new Vector2(0.0f, 0.0f),
            new Vector2(-1.0f, 1.0f),
            new Vector2(-1.0f, 0.0f),
            new Vector2(0.0f, 1.0f),
 
            // back
            new Vector2(0.0f, 0.0f),
            new Vector2(0.0f, 1.0f),
            new Vector2(-1.0f, 1.0f),
            new Vector2(-1.0f, 0.0f),
 
            // right
            new Vector2(-1.0f, 0.0f),
            new Vector2(0.0f, 0.0f),
            new Vector2(0.0f, 1.0f),
            new Vector2(-1.0f, 1.0f),
 
            // top
            new Vector2(0.0f, 0.0f),
            new Vector2(0.0f, 1.0f),
            new Vector2(-1.0f, 0.0f),
            new Vector2(-1.0f, 1.0f),
 
            // front
            new Vector2(0.0f, 0.0f),
            new Vector2(1.0f, 1.0f),
            new Vector2(0.0f, 1.0f),
            new Vector2(1.0f, 0.0f),
 
            // bottom
            new Vector2(0.0f, 0.0f),
            new Vector2(0.0f, 1.0f),
            new Vector2(-1.0f, 1.0f),
            new Vector2(-1.0f, 0.0f)





    };
    public static readonly Vector3[] CubeColors2 =
    {
      new Vector3(0,1,0),
      new Vector3(1,1,0),
      new Vector3(1,1,1),
      new Vector3(0,1,1),
       new Vector3(0,1,0),
      new Vector3(1,1,0),
      new Vector3(1,1,1),
      new Vector3(0,1,1),
    };
    public static readonly float[] frontFace = { 0, 0, 1, 1, 0, 1, 1, 1, 1, 0, 1, 1 };
    public static readonly float[] backFace = { 1, 0, 0, 0, 0, 0, 0, 1, 0, 1, 1, 0 };
    public static readonly float[] leftFace = { 0, 0, 0, 0, 0, 1, 0, 1, 1, 0, 1, 0 };
    public static readonly float[] rightFace = { 1, 0, 1, 1, 0, 0, 1, 1, 0, 1, 1, 1 };
    public static readonly float[] topFace = { 0, 1, 1, 1, 1, 1, 1, 1, 0, 0, 1, 0 };
    public static readonly float[] bottomFace = { 0, 0, 0, 1, 0, 0, 1, 0, 1, 0, 0, 1 };


    public static readonly int[] CubeIndices =
    {
      //left
      0,1,2,0,3,1,

      //back
      4,5,6,4,6,7,

      //right
      8,9,10,8,10,11,

      //top
      13,14,12,13,15,14,

      //front
      16,17,18,16,19,17,

      //bottom
      20,21,22,20,22,23
    };
    public static float side = 0.5f;
    public static Vector3[] CubeVertices =
    {
      //up
          new Vector3(-side, -side, -side),
          new Vector3(-side, -side, side),
          new Vector3(-side, side, -side),
          new Vector3(-side, side, -side),
          new Vector3(-side, -side, side),
          new Vector3(-side, side, side),

          new Vector3(side, -side, -side),
          new Vector3(side, side, -side),
          new Vector3(side, -side, side),
          new Vector3(side, -side, side),
          new Vector3(side, side, -side),
          new Vector3(side, side, side),

          new Vector3(-side, -side, -side),
          new Vector3(side, -side, -side),
          new Vector3(-side, -side, side),
          new Vector3(-side, -side, side),
          new Vector3(side, -side, -side),
          new Vector3(side, -side, side),

          new Vector3(-side, side, -side),
          new Vector3(-side, side, side),
          new Vector3(side, side, -side),
          new Vector3(side, side, -side),
          new Vector3(-side, side, side),
          new Vector3(side, side, side),

          new Vector3(-side, -side, -side),
          new Vector3(-side, side, -side),
          new Vector3(side, -side, -side),
          new Vector3(side, -side, -side),
          new Vector3(-side, side, -side),
          new Vector3(side, side, -side),

          new Vector3(-side, -side, side),
          new Vector3(side, -side, side),
          new Vector3(-side, side, side),
          new Vector3(-side, side, side),
          new Vector3(side, -side, side),
          new Vector3(side, side, side),


    };
    public static float w = 1;
    public static float h = 1;
    public static Vector2[] PlaneTexCoordswh =
    {
      new Vector2(0, h),
      new Vector2(w, h),
      new Vector2(0, 0),
      new Vector2(0, 0),
      new Vector2(w, h),
      new Vector2(w, 0),
      new Vector2(w, 0),
      new Vector2(0, 0),
      new Vector2(w, h),
      new Vector2(w, h),
      new Vector2(0, 0),
      new Vector2(0, h),
      new Vector2(w, 0),
      new Vector2(0, 0),
      new Vector2(w, h),
      new Vector2(w, h),
      new Vector2(0, 0),
      new Vector2(0, h),
      new Vector2(w, 0),
      new Vector2(0, 0),
      new Vector2(w, h),
      new Vector2(w, h),
      new Vector2(0, 0),
      new Vector2(0, h),
      new Vector2(0, h),
      new Vector2(w, h),
      new Vector2(0, 0),
      new Vector2(0, 0),
      new Vector2(w, h),
      new Vector2(w, 0),
      new Vector2(0, h),
      new Vector2(w, h),
      new Vector2(0, 0),
      new Vector2(0, 0),
      new Vector2(w, h),
      new Vector2(w, 0),
    };
    public static void CheckGLError(string title)
    {
      var error = GL.GetError();
      if (error != ErrorCode.NoError)
      {
        Debug.Print($"{title}: {error}");
      }
    }
  }

}