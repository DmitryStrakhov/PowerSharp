using System;
using JetBrains.Annotations;
using JetBrains.ReSharper.Psi.CSharp.Tree;

namespace PowerSharp.Builders {
    public class ConstructorBuilder {
        readonly IConstructorDeclaration constructor;

        public ConstructorBuilder([NotNull] IConstructorDeclaration constructor) {
            Guard.IsNotNull(constructor, nameof(constructor));
            this.constructor = constructor;
        }
        [NotNull]
        public ConstructorBuilder WithBody([NotNull] Func<CodeBuilder, ICSharpStatement> getStatement) {
            constructor.Body.AddStatementAfter(getStatement(new CodeBuilder(constructor)), null);
            return this;
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