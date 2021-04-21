using System;
using System.Text;
using JetBrains.Annotations;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.ReSharper.Psi.CSharp.Tree;

namespace PowerSharp.Builders {
    public class CodeBuilder {
        [NotNull] readonly CSharpElementFactory factory;
        [NotNull] readonly IDeclaration declaration;

        public CodeBuilder([NotNull] IDeclaration declaration) {
            this.declaration = declaration;
            this.factory = CSharpElementFactory.GetInstance(declaration);
        }
        [NotNull]
        public ICSharpStatement CreateObjectInstantiationStatement([NotNull] string memberName, [NotNull] IType memberType, bool useThisQualifier) {
            StringBuilder statementPatternBuilder = new StringBuilder(32);

            if(useThisQualifier) {
                statementPatternBuilder.Append("this.");
            }
            statementPatternBuilder.Append("$0 = $1;");

            ICSharpExpression expression = factory.CreateExpression("new $0()", memberType);
            return factory.CreateStatement(statementPatternBuilder.ToString(), memberName, expression);
        }
    }
}