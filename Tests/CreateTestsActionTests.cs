using System;
using NUnit.Framework;
using PowerSharp.ContextActions;
using JetBrains.ReSharper.FeaturesTestFramework.Intentions;

namespace PowerSharp.Tests {
     [TestFixture]
     public class CreateTestsActionTests : CSharpContextActionExecuteTestBase<CreateTestsContextAction> {
         protected override string ExtraPath { get { return string.Empty; } }
         protected override string RelativeTestDataPath { get { return "CreateTestsAction"; } }

         [Test] public void Test01() { DoNamedTest(); }
     }
}