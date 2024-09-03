using NUnit.Framework;
using JetBrains.ReSharper.TestFramework;

namespace PowerSharp.Tests {
    [TestFixture]
    public class ForceMethodCallActionTests : ExecuteActionTestBase {
        protected override string ActionId {
            get { return "ForceMethodCall"; }
        }
        protected override string RelativeTestDataPath {
            get { return "ForceMethodCallAction"; }
        }

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
