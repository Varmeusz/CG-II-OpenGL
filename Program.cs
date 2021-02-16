using System;
using OpenTK.Windowing;
using OpenTK.Windowing.Desktop;

namespace CG_II_OpenGL
{
  class Program
  {
    static void Main(string[] args)
    {
      string HelloMsg = "Choose one of the following OpenGL programs:\n"+
      "1 - Opening a cube\n"+
      "2 - Cube Array\n"+
      "3 - Falling cubes\n"+
      "4 - f(x,y)";
      Console.WriteLine(HelloMsg);
      //var option = Console.ReadKey(true);
      //Console.WriteLine(option.KeyChar);
      using(GameWindow app = new WindowCubeArray())
      {
        app.Run();
      }
    }
  }
}
