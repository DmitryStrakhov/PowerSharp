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
  }
}