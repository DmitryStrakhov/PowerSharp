using System;

namespace ClassLibrary
{
  public struct MyPoint1{caret}
  {
    public MyPoint(int x, int y)
    {
      X = x;
      Y = y;
    }
    public readonly int X;
    public readonly int Y;
  }
}