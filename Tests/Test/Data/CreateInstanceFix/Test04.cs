using System;

namespace ClassLibrary
{
  public class OneClass
  {
    static AnotherClass obj{caret};
    static object x;

    static OneClass()
    {
      x = new object();
    }
  }

  public class AnotherClass
  {
    int x;
    int y;
    
    public AnotherClass(int x, int y) {
      this.x = x;
      this.y = y;
    }
  }
}