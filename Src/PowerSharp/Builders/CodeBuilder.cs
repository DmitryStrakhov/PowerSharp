using System.Text;
using JetBrains.Annotations;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Tree;

namespace PowerSharp.Builders {
    public class CodeBuilder {
        readonly CSharpElementFactory factory;

        public CodeBuilder([NotNull] ITreeNode node) {
            this.factory = CSharpElementFactory.GetInstance(node);
        }
        [NotNull]
        public ICSharpStatement CreateObjectInstantiationStatement([NotNull] string memberName, [NotNull] IType memberType, bool useThisQualifier) {
            StringBuilder statementPatternBuilder = new StringBuilder(32);
            if(useThisQualifier) {
                statementPatternBuilder.Append("this.");
            }
            statementPatternBuilder.Append("$0 = $1;");

            ICSharpExpression rightExp = factory.CreateExpression("new $0()", memberType);
            return factory.CreateStatement(statementPatternBuilder.ToString(), memberName, rightExp);
        }
    }
}