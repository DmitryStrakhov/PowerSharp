using JetBrains.ReSharper.Feature.Services.Refactorings;

namespace PowerSharp.Refactorings.CreateTests {
    public sealed class CreateTestsHelper : IWorkflowExec {
        public CreateTestsHelper() {
        }
        public bool IsLanguageSupported { get { return false;  } }
    }
}