using System;

namespace ClassLibrary
{
  public class OneClass
  {
    int x;
    double y;
    AnotherClass obj{caret};

    public OneClass(int x)
    {
      this.x = x;
    }
    public OneClass(double y)
    {
      this.y = y;
    }
  }

  public class AnotherClass
  {
  }
}