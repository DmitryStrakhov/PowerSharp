using System;
using JetBrains.Annotations;
using JetBrains.ReSharper.Psi.CSharp;
using JetBrains.ReSharper.Psi.CSharp.Tree;

namespace PowerSharp.Builders {
    /// <summary>
    /// 
    /// Generates constructors and provides entry point for code-generation inside them.
    /// API is designed in a fluent style
    /// 
    /// </summary>
    public class ConstructorBuilder {
        readonly IConstructorDeclaration constructor;

        public ConstructorBuilder([NotNull] IConstructorDeclaration constructor) {
            Guard.IsNotNull(constructor, nameof(constructor));
            this.constructor = constructor;
        }
        [NotNull]
        public ICSharpStatement WithBody([NotNull] Func<CodeBuilder, ICSharpStatement> getStatement) {
            Guard.IsNotNull(getStatement, nameof(getStatement));

            CodeBuilder codeBuilder = new(CSharpElementFactory.GetInstance(constructor));
            return constructor.Body.AddStatementAfter(getStatement(codeBuilder), null);
        }
        [NotNull]
        public ConstructorBuilder SetStatic() {
            constructor.SetStatic(true);
            return this;
        }
        [NotNull]
        public IConstructorDeclaration Unwrap() {
            return constructor;
        }
    }
}