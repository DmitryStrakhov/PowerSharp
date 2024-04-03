using System;
using NUnit.Framework;
using PowerSharp.ContextActions;
using JetBrains.ReSharper.FeaturesTestFramework.Intentions;

namespace PowerSharp.Tests {
     [TestFixture]
     public class EditPropertySetterActionTests : CSharpContextActionExecuteTestBase<EditPropertySetterContextAction> {
         protected override string RelativeTestDataPath {
             get { return "EditPropertySetterAction"; }
         }
         protected override string ExtraPath { get { return string.Empty; } }

         [Test] public void Test01() { DoNamedTest(); }
         [Test] public void Test02() { DoNamedTest(); }
         [Test] public void Test03() { DoNamedTest(); }
         [Test] public void Test04() { DoNamedTest(); }
         [Test] public void Test05() { DoNamedTest(); }
         [Test] public void Test06() { DoNamedTest(); }
     }
}
