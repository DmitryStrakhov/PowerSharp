using System;
using JetBrains.Annotations;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Util;

namespace PowerSharp.Builders {
    public class PsiFileBuilder {
        [NotNull] readonly CSharpElementFactory factory;
        [NotNull] readonly ICSharpFile psiFile;

        public PsiFileBuilder([NotNull] ICSharpFile psiFile) {
            Guard.IsNotNull(psiFile, nameof(psiFile));
            this.psiFile = psiFile;

#pragma warning disable 618
            this.factory = CSharpElementFactory.GetInstance(psiFile);
#pragma warning restore 618
        }
        
        [NotNull]
        public PsiFileBuilder AddUsingDirective(string reference) {
            if(string.IsNullOrEmpty(reference)) {
                throw new ArgumentException(nameof(reference));
            }

            psiFile.AddImport(factory.CreateUsingDirective("$0", reference));
            return this;
        }
        [NotNull]
        public TypeHolderBuilder AddExpectedNamespace() {
            ICSharpNamespaceDeclaration declaration = factory.CreateNamespaceDeclaration(CalculateExpectedNamespace());
            ICSharpNamespaceDeclaration typeHolder = psiFile.AddNamespaceDeclarationAfter(declaration, null);
            return new TypeHolderBuilder(typeHolder);
        }
        private string CalculateExpectedNamespace() {
            IPsiSourceFile psiSourceFile = psiFile.GetSourceFile();
            if(psiSourceFile == null) {
                throw new InvalidOperationException();
            }

            IProjectItem projectItem = psiSourceFile.ToProjectFile();
            if(projectItem == null) {
                throw new InvalidOperationException();
            }
            return projectItem.CalculateExpectedNamespace(psiSourceFile.PrimaryPsiLanguage);
        }
    }
}