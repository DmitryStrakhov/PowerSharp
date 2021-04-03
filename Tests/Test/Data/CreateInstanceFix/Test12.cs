using System;

namespace ClassLibrary
{
  public class OneClass
  {
    public SomeClass() : base()
    {
    }

    SomeClass field{caret};
  }

  public class SomeClass
  {
  }
}