# PowerSharp

PowerSharp is a Resharper and Rider compatible plugin which provides nice features I miss out-of-the box. I started this project to make my own experience with Resharper and Rider even more better, but I'd be happy if the plugin becomes useful for a community.

You can find a ready-to-install plugins here: [PowerSharp->Plugins](https://github.com/DmitryStrakhov/PowerSharp/tree/main/Plugins)


## Feature List

### 1. 'Create instance' context action

Sometimes you have a field or a property in your class and you just want to initialize it in the class' constructor. For this purpose, a context action 'Create instance' was introduced. Look at the pictures below:

![Create-Instance-01](https://github.com/DmitryStrakhov/PowerSharp/blob/main/ReadMe-Images/Create-Instance-01.png)
![Create-Instance-02](https://github.com/DmitryStrakhov/PowerSharp/blob/main/ReadMe-Images/Create-Instance-02.png)

Both instance and static fields and properties are supported.

### 2. 'Add favorite dependency' action

I guess every developer has its own set of favorite dependencies they use often. My favorites are NUnit and Fluent.Assertions. An action 'Add favorite dependency' adds the dependencies into the project in two clicks. Look at the picture below.

![Add-Favorite-Dependency](https://github.com/DmitryStrakhov/PowerSharp/blob/main/ReadMe-Images/Add-Favorite-Dependency.png)

In this version however, the set of dependencies is fixed and contains only NUnit and Fluent.Assertions libraries. I'm going to make the dependency set customizable through a settings page in the future.

### 3. 'Create tests' action

Let's imagine you decided to develop some tests for you class. You create a test-project and add it to your solution. Next, you need to add some unit-testing framework dependency to your test-project. Then you need to setup dependencies between your projects (the test-project must reference under-the-test project). And finally, you write some boiler-plate code which could look like this:

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

The 'Create tests' action allows to do this much faster. Look at the picture:

![Create-Tests](https://github.com/DmitryStrakhov/PowerSharp/blob/main/ReadMe-Images/Create-Tests.png)

The action generates a boiler-plate code for you and sets up required between-projects dependencies if that hasn't been done already.

Please note, however, that only NUnit framework is supported at the moment.

The action is available if the test-project already has NUnit-dependency. You can add the dependency with 'Add favorite dependency' action by the way.

### 4. 'Edit' action group

Let's consider the following use case: you have some interface in you code you need to implement. You create a class which implements the interface and ask Resharper (or Rider) to generate a boilerplate code for you. After this, you usually see something like this:

```csharp

using System;

namespace Project {
    interface I {
        void Method1();
        void Method2();
        void Method3();
        int Property1 { get; set; }
    }

    abstract class MyClass : I {
        public void Method1() {
            throw new NotImplementedException();
        }
        public void Method2() {
            throw new NotImplementedException();
        }
        public void Method3() {
            throw new NotImplementedException();
        }

        public int Property1 {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }
    }
}
```

Now you need to navigate through your code and implement the members. But there is a problem (at least for me)): most navigation features of IDE set your caret at the name of target member. That means that to start writing a code, you need to move cursor down, delete generated stub-code (e.g. throw new NotImplementedException()) and set your caret to correct position. So, 3 or 4 actions need to be done for that! And 'Edit' action group is here to start coding faster.

***'Edit Method' action***

If your caret is on the method name, the 'Edit Method' action is available. When it runs, you get the following result:

![Add-Favorite-Dependency](https://github.com/DmitryStrakhov/PowerSharp/blob/main/ReadMe-Images/Edit-Method.png)


***'Edit Property Getter' and 'Edit Property Setter' actions***

If the caret is on the property name, the 'Edit Property Getter' and 'Edit Property Setter' actions are available. This time the result is the following:

![Add-Favorite-Dependency](https://github.com/DmitryStrakhov/PowerSharp/blob/main/ReadMe-Images/Edit-Property-Getter.png)

![Add-Favorite-Dependency](https://github.com/DmitryStrakhov/PowerSharp/blob/main/ReadMe-Images/Edit-Property-Setter.png)

Those actions select a code of a method or a property, so you can start typing immediatelly in the correct position and replace old code at the same time. Of course, you can bind those actions to your favorite shortcuts to get a maximum efficiency.
