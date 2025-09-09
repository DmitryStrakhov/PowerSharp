using System;
using JetBrains.Annotations;
using JetBrains.ReSharper.Psi.CSharp;
using JetBrains.ReSharper.Psi.CSharp.Tree;

namespace PowerSharp.Builders {
    /// <summary>
    /// 
    /// Generates low-level code (expressions, statement etc).
    /// API is designed in a fluent style
    /// 
    /// </summary>
    public class CodeBuilder([NotNull] CSharpElementFactory factory) {
        [NotNull] readonly CSharpElementFactory factory = factory;

        [NotNull]
        public IExpressionStatement CreateObjectInstantiationStatement([NotNull] IMemberToInstantiate member) {
            Guard.IsNotNull(member, nameof(member));

            IObjectCreationExpression objectCreationExpr = (IObjectCreationExpression)factory.CreateExpression("new $0()", member.TypeUsage());
            if(objectCreationExpr.IsCSharp9Supported())
                objectCreationExpr.SetTypeUsage(null);
            
            return (IExpressionStatement)factory.CreateStatement("$0 = $1;", member.Name(), objectCreationExpr);
        }
    }
}