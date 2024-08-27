using System;
using NUnit.Framework;
using JetBrains.ReSharper.TestFramework;

namespace PowerSharp.Tests {
     [TestFixture]
     public class EditMethodActionTests : ExecuteActionTestBase {
         protected override string ActionId {
             get { return "EditMethod"; }
         }
         protected override string RelativeTestDataPath {
             get { return "EditMethodAction"; }
         }

         [Test] public void Test01() { DoNamedTest(); }
         [Test] public void Test02() { DoNamedTest(); }
         [Test] public void Test03() { DoNamedTest(); }
         [Test] public void Test04() { DoNamedTest(); }
         [Test] public void Test05() { DoNamedTest(); }
         [Test] public void Test06() { DoNamedTest(); }
         [Test] public void Test07() { DoNamedTest(); }
         [Test] public void Test08() { DoNamedTest(); }
         [Test] public void Test09() { DoNamedTest(); }
         [Test] public void Test10() { DoNamedTest(); }
         [Test] public void Test11() { DoNamedTest(); }
         [Test] public void Test12() { DoNamedTest(); }
     }
}
