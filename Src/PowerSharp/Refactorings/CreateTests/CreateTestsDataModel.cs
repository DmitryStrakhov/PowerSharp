namespace PowerSharp.Refactorings.CreateTests {
    public sealed class CreateTestsDataModel {
        public CreateTestsDataModel() {
        }

        public string TestClassName { get; set; }

        public bool AddSetUpMethod { get; set; }
        public bool AddTearDownMethod { get; set; }

        public bool AddNUnitPackage { get; set; }
        public bool AddFluentAssertionsPackage { get; set; }
    }
}