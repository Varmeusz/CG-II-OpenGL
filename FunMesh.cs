using System.Collections.Generic;
using OpenTK.Mathematics;

namespace CG_II_OpenGL
{
  public class FunMesh
  {
    public List<Vector3> Vertices = new List<Vector3>();
    public List<Vector3> Normals= new List<Vector3>();
    public RPN Calculator;
    public float calcVertex(float x, float y)
    {
      return (float) Calculator.evaluatePostfix(x, y);
    }
    public Vector3 calcNormal(Vector3 x0, Vector3 x1,Vector3 x2)
    {
      Vector3 v0 = x1 - x0;
      Vector3 v1 = x2 - x1;
      Vector3 n = Vector3.Cross(v0,v1);
      return (n);
    }
    private static float step = 0.1f;
    public void calcMesh(int size)
    {
      for(float i = -size; i < size; i+=step)
      {
        for (float j = -size; j < size; j+=step)
        {
            float z1 = calcVertex(i,j);
            float z2 = calcVertex(i,j+step);
            float z3 = calcVertex(i+step,j);
            float z4 = calcVertex(i+step,j+step);
            Vector3 v1 = new Vector3(i,z1,j);
            Vector3 v2 = new Vector3(i,z2,j+step);
            Vector3 v3 = new Vector3(i+step,z3,j);
            Vector3 v4 = new Vector3(i+step,z3,j);
            Vector3 v5 = new Vector3(i,z2,j+step);
            Vector3 v6 = new Vector3(i+step,z4,j+step);
            Vertices.AddRange(new List<Vector3>{v1,v2,v3,v4,v5,v6});
            Vector3 n1 = calcNormal(v1,v2,v3);
            Vector3 n2 = calcNormal(v4,v5,v6);
            Normals.AddRange(new List<Vector3>{n1,n1,n1});
            Normals.AddRange(new List<Vector3>{n2,n2,n2});
        }
      }
    }
  }
}