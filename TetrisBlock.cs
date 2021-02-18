using System;
using System.Collections.Generic;
using OpenTK.Mathematics;

namespace CG_II_OpenGL
{
  public class TetrisBlock 
  {
    public float Rotation=0;
    public enum BlockType {LinePiece=0, LBlock = 1, ReverseLBlock = 2, Square = 3, TBlock = 4, Squiggly = 5, ReverseSquiggly = 6};
    
    public static Vector3[] Positions(int piece)
    {
      Vector3[] positions = new Vector3[4];
      switch(piece)
      {
        case(0):
          positions[0] = new Vector3(0,-1.5f,0);
          positions[1] = new Vector3(0,-.5f,0);
          positions[2] = new Vector3(0, 1.5f,0);
          positions[3] = new Vector3(0, .5f,0);
          break;
        case(1):
          positions[0] = new Vector3(-1f, 0,0);
          positions[1] = new Vector3( 1f, 0,0);
          positions[2] = new Vector3( 0 , 0,0);
          positions[3] = new Vector3(-1f, 1f,0);
          break;
        case(2):
          positions[0] = new Vector3(-1f, 0,0);
          positions[1] = new Vector3( 1f, 0,0);
          positions[2] = new Vector3( 0   , 0,0);
          positions[3] = new Vector3(1f,  1,0);
          break;
        case(3):
          positions[0] = new Vector3(-0.5f, -0.5f,0);
          positions[1] = new Vector3(-0.5f,  0.5f,0);
          positions[2] = new Vector3(0.5f,   0.5f,0);
          positions[3] = new Vector3(0.5f,  -0.5f,0);
          break;
        case(4):
          positions[0] = new Vector3(0,0,0);
          positions[1] = new Vector3(-1f,0,0);
          positions[2] = new Vector3(1f,0,0);
          positions[3] = new Vector3(0,1f,0);
          break;
        case(5):
          positions[0] = new Vector3(0,0,0);
          positions[1] = new Vector3(1,0,0);
          positions[2] = new Vector3(0,-1,0);
          positions[3] = new Vector3(-1,-1,0);
          break;
        case(6):
          positions[0] = new Vector3(0,0,0);
          positions[1] = new Vector3(1,0,0);
          positions[2] = new Vector3(0,1,0);
          positions[3] = new Vector3(-1,1,0);
          break;
        case(7):
          positions[0] = new Vector3(1,0,0);
          positions[1] = new Vector3(2,0,0);
          positions[2] = new Vector3(2,1,0);
          positions[3] = new Vector3(2,2,0);
          break;
      }
      return positions;
    }
  }
}