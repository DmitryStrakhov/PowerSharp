using System;
using NUnit.Framework;
using JetBrains.ReSharper.TestFramework;

namespace PowerSharp.Tests {
     [TestFixture]
     public class EditPropertySetterActionTests : ExecuteActionTestBase {
         protected override string ActionId {
             get { return "EditPropertySetter"; }
         }
         protected override string RelativeTestDataPath {
             get { return "EditPropertySetterAction"; }
         }

         [Test] public void Test01() { DoNamedTest(); }
         [Test] public void Test02() { DoNamedTest(); }
         [Test] public void Test03() { DoNamedTest(); }
         [Test] public void Test04() { DoNamedTest(); }
         [Test] public void Test05() { DoNamedTest(); }
         [Test] public void Test06() { DoNamedTest(); }
     }
}
