# PowerSharp

PowerSharp is a Resharper plugin which provides nice features I miss in out of the box. I started this project to make my own experience with R# even more better, but, I'd be happy if the plugin becomes useful for the others in community.

You can find a ready-to-install plugin package here: [PowerSharp->NuGet](https://github.com/DmitryStrakhov/PowerSharp/tree/main/Nuget)


## Feature List

### 1. 'Create instance' quick fix

Sometimes you have a field or a property in you class and you just want to instantiate it in the class' constructor (means aggregation relationship between two classes). For this purpose, a quick fix 'Create instance' was introduced. Look at the pictures below:

![Create-Instance-01](https://github.com/DmitryStrakhov/PowerSharp/blob/main/ReadMe-Images/Create-Instance-01.png)
![Create-Instance-02](https://github.com/DmitryStrakhov/PowerSharp/blob/main/ReadMe-Images/Create-Instance-02.png)

We support instance and static fields and properties.

### 2. 'Add favorite dependency' action.

I guess every developer has its own set of favorite dependencies he uses very often. My absolute favorites are NUnit and Fluent.Assertions. A quick action 'Add favorite dependency' allows to add your favorite dependencies in the project in two clicks. Look at the picture below.

![Add-Favorite-Dependency](https://github.com/DmitryStrakhov/PowerSharp/blob/main/ReadMe-Images/Add-Favorite-Dependency.png)

In this version however, the set of dependencies is fixed and contains only NUnit and Fluent.Assertions. I'm going to make the dependency set customizable through a settings page in the future.

### 3. 'Create tests' action.

Let's imagine you decided to develop some tests for you class (really?!). You have a Tests-project in your solution. Next, you usually need to add some unit-testing framework dependency in your test-project, then add a reference to the project which contains a class you want to test into the test-project, and write some boiler-plate code which looks like this:

```csharp
using NUnit.Framework;

namespace Project.Tests {
    [TestFixture]
    public class MyClassTests {
        [SetUp]
        public void SetUp() {
        }
        [TearDown]
        public void TearDown() {
        }
    }
}
```
Very boring!

The 'Create tests' quick fix allows finishing it in a few clicks. Look at the picture:

![Create-Tests](https://github.com/DmitryStrakhov/PowerSharp/blob/main/ReadMe-Images/Create-Tests.png)

So, the action generates a boiler-plate code for you and automatically adds required project-dependencies.

Please note however, that we support NUnit framework only at the moment.
The quick fix is only available if the test-project already has NUnit-dependency. You can add it through 'Add favorite dependency' action by the way.

New ideas and proposal are very welcome!
