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
    }
}