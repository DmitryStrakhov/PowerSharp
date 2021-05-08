using System;
using JetBrains.ProjectModel;
using System.Collections.Generic;
using JetBrains.Annotations;
using JetBrains.ReSharper.Psi.Tree;

namespace PowerSharp.Refactorings.CreateTests {
    public sealed class CreateTestsDataModel {
        public CreateTestsDataModel() {
        }
        public bool SetUpMethod { get; set; }
        public bool TearDownMethod { get; set; }
        public bool OneTimeSetUpMethod { get; set; }
        public bool OneTimeTearDownMethod { get; set; }

        public string TargetFilePath { get; set; }
        public IProjectFile TestClassFile { get; set; }

        [NotNull]
        public IDeclaration Declaration { get; set; }
        [NotNull]
        public IProjectFile SourceFile { get; set; }
        [NotNull]
        public IProjectFolder DefaultTargetProject { get; set; }

        [NotNull]
        public IList<IProjectFolder> SelectionScope { get; set; }
        [NotNull]
        public Func<IProjectFile, bool> SuggestFilter{ get; set; }

        [NotNull]
        public ISolution GetSolution() {
            return SourceFile.GetSolution();
        }
    }
}