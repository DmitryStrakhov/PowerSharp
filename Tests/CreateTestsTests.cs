using System;
using System.Collections.Generic;
using NUnit.Framework;
using JetBrains.TextControl;
using JetBrains.Util;
using JetBrains.DataFlow;
using PowerSharp.Extensions;
using JetBrains.Rider.Model.UIAutomation;
using JetBrains.Application.DataContext;
using JetBrains.ProjectModel.Properties;
using PowerSharp.Refactorings.CreateTests;
using JetBrains.Util.Dotnet.TargetFrameworkIds;
using JetBrains.ProjectModel.Properties.Managed;
using JetBrains.ReSharper.Feature.Services.Refactorings;
using JetBrains.ReSharper.FeaturesTestFramework.Refactorings;

namespace PowerSharp.Tests {
    [TestFixture]
    public class CreateTestsTests : RefactoringsTestWithUI<CreateTestsWorkflow> {
        List<Tuple<Func<CreateTestsPageStartPage, IProperty<bool>>, bool>> propertyList;
        
        public override void SetUp() {
            base.SetUp();
            this.propertyList = new List<Tuple<Func<CreateTestsPageStartPage, IProperty<bool>>, bool>>(8);
        }

        protected override CreateTestsWorkflow CreateWorkflowInstance(IDataContext context, ITextControl control) {
            return new CreateTestsWorkflow(Solution);
        }
        protected override string RelativeTestDataPath { get { return "CreateTestsRefactoring"; } }

        [Test] public void Test01() { DoTest(true, true, true, true); }
        [Test] public void Test02() { DoTest(false, false, false, false); }
        [Test] public void Test03() { DoTest(setUp: true); }
        [Test] public void Test04() { DoTest(tearDown: true); }
        [Test] public void Test05() { DoTest(oneTimeSetUp: true); }
        [Test] public void Test06() { DoTest(oneTimeTearDown: true); }
        [Test] public void Test07() { DoTest(setUp: true, tearDown: true); }

        private void DoTest(bool setUp = false, bool tearDown = false, bool oneTimeSetUp = false, bool oneTimeTearDown = false) {
            propertyList.Add(Tuple.Create<Func<CreateTestsPageStartPage, IProperty<bool>>, bool>(x => x.SetUpMethod, setUp));
            propertyList.Add(Tuple.Create<Func<CreateTestsPageStartPage, IProperty<bool>>, bool>(x => x.TearDownMethod, tearDown));
            propertyList.Add(Tuple.Create<Func<CreateTestsPageStartPage, IProperty<bool>>, bool>(x => x.OneTimeSetUpMethod, oneTimeSetUp));
            propertyList.Add(Tuple.Create<Func<CreateTestsPageStartPage, IProperty<bool>>, bool>(x => x.OneTimeTearDownMethod, oneTimeTearDown));
            DoNamedTest();
        }

        #region UI

        protected override void ProvideUiData(SingleBeRefactoringPage page, ITextControl control, CreateTestsWorkflow w) {
            CreateTestsPageStartPage startPage = (CreateTestsPageStartPage)page;

            foreach((var getProperty, bool value) in propertyList) {
                var property = getProperty(startPage);
                BeCheckbox checkbox = startPage.BeModel.Content.GetBeControlByIdBfs<BeCheckbox>(property.Id);
                Assert.IsTrue(checkbox.Enabled.Value);
                checkbox.Property.Value = value;
            }
        }

        #endregion

        #region Project SetUp

        protected override IProjectProperties FixProjectProperties(IProjectProperties properties, string _) {
            IProjectProperties prop = base.FixProjectProperties(properties, _);

            if(prop.BuildSettings is ManagedProjectBuildSettings buildSettings) {
                buildSettings.DefaultNamespace = "ClassLibrary";
            }
            return prop;
        }
        protected override IEnumerable<string> GetReferencedAssemblies(TargetFrameworkId frameworkId) {
            IEnumerable<string> assemblies = base.GetReferencedAssemblies(frameworkId);
            return CollectionUtil.Concat(assemblies, RequiredAssemblies());
        }
        private string[] RequiredAssemblies() {
            return new[] {
                // nunit.framework
                typeof(TestFixtureAttribute).Assembly.Location
            };
        }

        #endregion
    }
}
