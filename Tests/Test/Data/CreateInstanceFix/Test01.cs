using System;

namespace ClassLibrary
{
  public class OneClass
  {
    int x;
    readonly AnotherClass obj{caret};

    public OneClass(int x)
    {
      this.x = x;
    }
  }

  public class AnotherClass
  {
    int value;

    public AnotherClass(int value)
    {
      this.value = value;
    }
  }
}