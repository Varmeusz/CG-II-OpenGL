using OpenTK.Mathematics;
using System;

namespace CG_II_OpenGL
{ 
  public class Camera
  {
    public float Speed = 10f;
    public Vector3 Position;
    public Vector3 Target;
    public Vector3 cameraDirection;  
    public Vector3 upVector;
    public Vector2 lastPosition = new Vector2(400,400);
    public float Pitch=0;
    public float Yaw = -90.0f;
    public float Roll;
    public bool mouseMoved = false;
    public float Sensitivity = 0.1f;
    public Matrix4 LookAt;
    public Camera()
    {
      //view/ lookat vectors
      Position = new Vector3(5f, 5f, 12f);
      Target = new Vector3(0f, 0f, 0f);
      upVector = new Vector3(0.0f, 1.0f, 0.0f);

      cameraDirection = Vector3.Normalize(Position - Target);
      LookAt = View();
      // Vector3 cameraRight = Vector3.Normalize(Vector3.Cross(_upVector, _cameraDirection));
      // _upVector = Vector3.Cross(_cameraDirection, cameraRight);
      // Matrix4 view = Matrix4.LookAt(_cameraPosition, _cameraDirection, _upVector);
    }
    public Matrix4 View()
    {
      return Matrix4.LookAt(Position, Position + Target, upVector);
    }
    public void CalculateYawPitch()
    {
      Vector3 direction;
      direction.X = (float) Math.Cos(MathHelper.DegreesToRadians(Yaw)) * (float) Math.Cos(MathHelper.DegreesToRadians(Pitch));
      direction.Y = (float) Math.Sin(MathHelper.DegreesToRadians(Pitch));
      direction.Z = (float) Math.Sin(MathHelper.DegreesToRadians(Yaw)) * (float) Math.Cos(MathHelper.DegreesToRadians(Pitch));
      Target = Vector3.Normalize(direction);
    }
    public void MoveAround(float xPos, float yPos)
    {
      if(!mouseMoved)
      {
        lastPosition=new Vector2(xPos, yPos);
        mouseMoved=true;
      }else{

        float deltaX = xPos - lastPosition.X;
        float deltaY = yPos - lastPosition.Y;
        lastPosition = new Vector2(xPos, yPos);

        Yaw += deltaX * Sensitivity;
        Pitch -= deltaY * Sensitivity;
        if(Pitch > 89.0f)
          Pitch = 89.0f;
        else if(Pitch < -89.0f)
          Pitch = -89.0f;
      }
      CalculateYawPitch();
    }
    
    
  }
}