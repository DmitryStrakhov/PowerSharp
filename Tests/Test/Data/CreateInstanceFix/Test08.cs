using System;

namespace ClassLibrary
{
  public class OneClass
  {
    public static AnotherClass Property{caret} { get; set }
  }

  public class AnotherClass
  {
  }
}