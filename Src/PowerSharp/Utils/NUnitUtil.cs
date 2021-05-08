namespace PowerSharp.Utils {
    public static class NUnitUtil {
        public const string NUnitRootNamespace = "NUnit.Framework";

        public const string TestFixtureAttributeClrName = NUnitRootNamespace + ".TestFixtureAttribute";
        public const string SetUpAttributeClrName = NUnitRootNamespace + ".SetUpAttribute";
        public const string TearDownAttributeClrName = NUnitRootNamespace + ".TearDownAttribute";
        public const string OneTimeSetUpAttributeClrName = NUnitRootNamespace + ".OneTimeSetUpAttribute";
        public const string OneTimeTearDownAttributeClrName = NUnitRootNamespace + ".OneTimeTearDownAttribute";
    }
}