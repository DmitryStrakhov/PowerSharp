using System;
using NUnit.Framework;
using PowerSharp.QuickFixes;
using JetBrains.ReSharper.FeaturesTestFramework.Intentions;

namespace PowerSharp.Tests.Test.Data {
    [TestFixture]
    public class CreateInstanceFixTests : CSharpQuickFixTestBase<CreateInstanceFix> {
        protected override string RelativeTestDataPath { get { return "CreateInstanceFix"; } }

        [Test] public void TestSimple() { DoNamedTest2(); }
        [Test] public void Test01() { DoNamedTest(); }
        [Test] public void Test02() { DoNamedTest(); }
        [Test] public void Test03() { DoNamedTest(); }
        [Test] public void Test04() { DoNamedTest(); }
        [Test] public void Test05() { DoNamedTest(); }
        [Test] public void Test06() { DoNamedTest(); }
        [Test] public void Test07() { DoNamedTest(); }
        [Test] public void Test08() { DoNamedTest(); }
    }
}