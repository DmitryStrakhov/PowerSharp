# PowerSharp

PowerSharp is a Resharper plugin which provides nice features I miss out-of-the box. I started this project to make my own experience with Resharper even more better, but, I'd be happy if the plugin becomes useful for the others in a community.

You can find a ready-to-install plugin package here: [PowerSharp->Plugins](https://github.com/DmitryStrakhov/PowerSharp/tree/main/Plugins)


## Feature List

### 1. 'Create instance' quick fix

Sometimes you have a field or a property in your class and you just want to initialize it in the class' constructor (meaning aggregation relationships between two classes). For this purpose, a quick fix 'Create instance' was introduced. Look at the pictures below:

![Create-Instance-01](https://github.com/DmitryStrakhov/PowerSharp/blob/main/ReadMe-Images/Create-Instance-01.png)
![Create-Instance-02](https://github.com/DmitryStrakhov/PowerSharp/blob/main/ReadMe-Images/Create-Instance-02.png)

Instance and static fields and properties are supported.

### 2. 'Add favorite dependency' action

I guess every developer has its own set of favorite dependencies he or she uses very often. My absolute favorites are NUnit and Fluent.Assertions. An action 'Add favorite dependency' allows to add your favorite dependencies into the project in two clicks. Look at the picture below.

![Add-Favorite-Dependency](https://github.com/DmitryStrakhov/PowerSharp/blob/main/ReadMe-Images/Add-Favorite-Dependency.png)

In this version however, the set of dependencies is fixed and contains only NUnit and Fluent.Assertions. I'm going to make the dependency set customizable through a settings page in the future.

### 3. 'Create tests' action

Let's imagine you decided to develop some tests for you class (really?!). You have a test-project in your solution. Next, you usually need to add some unit-testing framework dependency into your test-project, then setup required dependencies between your projects (the test-project must reference under-the-test project), and, finally, write some boiler-plate code which looks like this:

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

The 'Create tests' action allows finishing it in a few clicks. Look at the picture:

![Create-Tests](https://github.com/DmitryStrakhov/PowerSharp/blob/main/ReadMe-Images/Create-Tests.png)

So, the action generates a boiler-plate code for you and sets up required between-projects dependencies.

Please note however, we support NUnit framework only at the moment.

The action is available if the test-project already has NUnit-dependency. You can add it through 'Add favorite dependency' action by the way.

### 4. Edit actions

Let me describe an use case. You have some interface you need to implement. You created an class which implements it and ask R# (or Rider) to generate boilerplate code for you. Usually it creates something like this:

![Add-Favorite-Dependency](https://github.com/DmitryStrakhov/PowerSharp/blob/main/ReadMe-Images/Edit-General.png)

Now you need to navigate through you code and implement those members. But there is a problem (at least for me)): most navigation features move your caret position at the name of target member. That means that to start writing a code, you need to move cursor down, delete generated stub-code (e.g. throw new NotImplementedException()) and (3 or 4 actions!) only after that to start coding. All of that made me nervous. I want to start code immediatelly (maximum what I' ready to do before is to run one action, but not 3 or 4).

So, let me introduce

##### 'Edit Method' action

If your caret is on the method name, the 'Edit Method' action is available. When it runs, you get the following result:

![Add-Favorite-Dependency](https://github.com/DmitryStrakhov/PowerSharp/blob/main/ReadMe-Images/Edit-Method.png)


##### 'Edit Property Getter' and 'Edit Property Setter' actions

If the caret is on the property name, the 'Edit Property Getter' and 'Edit Property Setter' actions are available. The result is the following:

![Add-Favorite-Dependency](https://github.com/DmitryStrakhov/PowerSharp/blob/main/ReadMe-Images/Edit-Property-Getter.png)

![Add-Favorite-Dependency](https://github.com/DmitryStrakhov/PowerSharp/blob/main/ReadMe-Images/Edit-Property-Setter.png)

Of course, you can bind those actions to your favorite shortcuts to get maximum efficiency.

--
#### New ideas and proposals are very welcome!
