using JetBrains.ProjectModel;
using JetBrains.ReSharper.Psi.Tree;

namespace PowerSharp.Refactorings.CreateTests {
    public sealed class CreateTestsDataModel {
        public CreateTestsDataModel() {
        }
        public string TestClassName { get; set; }
        public bool SetUpMethod { get; set; }
        public bool TearDownMethod { get; set; }
        public bool OneTimeSetUpMethod { get; set; }
        public bool OneTimeTearDownMethod { get; set; }

        public IDeclaration Declaration { get; set; }
        public IProjectFile TestClassFile { get; set; }
    }
}