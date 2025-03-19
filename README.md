# Runtime Console

Tool for Unity that allows you to view log history, execute commands, and extend functionality by adding custom commands. This tool is particularly useful for debugging and interacting with your application during runtime.

## Features

- **View Log History:** Easily view all logged messages (Info, Warnings, Errors) at runtime.
- **Execute Commands:** Type and execute commands directly in the console during runtime.
- **Custom Command Handling:** Add and handle custom commands to extend the console's functionality.

## Installation

### Unity Package Manager

```
https://github.com/potikot/RuntimeConsole.git?path=RuntimeConsole
```

<details>
  <summary> Details </summary>
  <br>

  1. Open your Unity project.
  2. Navigate to `Window` > `Package Manager`.
  3. Click the `+` button in the top left.
  4. Select `Add package from git URL...`.
  5. Enter following url: `https://github.com/potikot/RuntimeConsole.git?path=RuntimeConsole`.
  6. If you want to install specific version just add `#v1.0.0` to the link (`1.0.0` is version you want).
  7. Click `Add`. The Runtime Console package will be installed.

</details>

### [Version History](https://github.com/potikot/RuntimeConsole/tags)

> [!NOTE]
> It is better to read README for your installed version of package.

## Usage

### Opening the Console

To open the console during runtime, use the defined key binding. By default, this is typically the `Tab` or a specific key set in the package configuration.

### Console Preferences

To open preferences window, navigate to `Tools` > `PotikotTools` > `Console Preferences`. There you can configure package preferences

### Logging

You can log messages to the console using provided methods:

```csharp
Console.Log("This is an info message.");
Console.LogWarning("This is a warning message.");
Console.LogError("This is an error message.");
```

### Commands

#### Static

You can register static methods and fields with `CommandAttribute`. Basically command appear in Console as `<ClassName>.<(Method/Field)Name> <arg1> <arg2> ...`. Attribute contains `Name` and `IncludeTypeName` fields:

- `Name` replaces method/field name.
- `IncludeTypeName` determines whether the class name will be included in the command (`true` - will be included, `false` - will not).

<details open>
<summary> Attribute usage example: </summary>
<br>

```csharp
using PotikotTools.Commands;

public class ExampleClass
{
    [Command(Name = "field")] // will add command 'field <int>'
    private static int exampleField;

    [Command] // will add command 'ExampleClass.ExampleMethod <int> <string>'
    public static void ExampleMethod1(int value, string message)
    {
        exampleField = value;
        Debug.Log($"{exampleField}. {message}");
    }

    [Command(IncludeTypeName = false)] // will add command 'ExampleMethod2'
    public static void ExampleMethod2() { }
}
```

</details>

#### Non Static

Also, you can register commands through this API:

```csharp
CommandHandler.Register(string commandName, Action callback);
CommandHandler.Register(ICommandInfo commandInfo);
```

All commands should implement `ICommandInfo` interface. Default command types is `MethodCommandInfo`, `FieldCommandInfo` and `ActionCommandInfo`. And also you can add your own type of command by implementing interface in your class.

<details>
<summary> Example implementation </summary>
<br>

```csharp
using System;
using System.Reflection;

public class FieldCommandInfo : ICommandInfo
{
    // 'HintText' cache
    private string _hintText;
    // 'ParameterTypes' cache
    private Type[] _parameterTypes;

    // Name. Should not contain spaces
    public string Name { get; private set; }
    // Target object
    public object Context { get; private set; }
    // Field info
    public FieldInfo FieldInfo { get; private set; }

    // Hint in console
    public string HintText
    {
        get
        {
            if (!string.IsNullOrEmpty(_hintText))
                return _hintText;

            _hintText = $"{Name} {ParameterTypes[0].Name}";
            return _hintText;
        }
    }

    // Field type
    public Type[] ParameterTypes => _parameterTypes ??= new Type[1] { FieldInfo.FieldType };

    // Validation check
    public bool IsValid => FieldInfo != null && !string.IsNullOrEmpty(Name);

    // Constructor
    public FieldCommandInfo(string name, FieldInfo fieldInfo, object context = null)
    {
        Name = name.Replace(' ', '_');
        FieldInfo = fieldInfo;
        Context = context;
    }

    // Invoke command
    public void Invoke(object[] parameters)
    {
        FieldInfo.SetValue(Context, parameters[0]);
    }
}
```

</details>

### Additional Assets

- Converter
- Optimized Scroll
- Context Menu
