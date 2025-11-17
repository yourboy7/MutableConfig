# MutableConfig

[![NuGet version](https://img.shields.io/nuget/v/MutableConfig.svg)](https://www.nuget.org/packages/MutableConfig)[![License](https://img.shields.io/badge/license-Apache%202.0-blue.svg)](https://opensource.org/licenses/Apache-2.0)[![NuGet downloads](https://img.shields.io/nuget/dt/MutableConfig.svg)](https://www.nuget.org/packages/MutableConfig)[![GitHub stars](https://img.shields.io/github/stars/yourboy7/MutableConfig?style=social)](https://github.com/yourboy7/MutableConfig)

A lightweight and extensible .NET/C# library for runtime configuration management, supporting type-safe mapping of C# classes to JSON or XML files.

# About

MutableConfig is a configuration management library for the .NET/C# environment. It is designed to allow applications to easily load, modify, and persist configuration items at runtime without needing to restart or redeploy. It supports organizing configurations into a hierarchical structure, dynamic updates, and is designed to be lightweight and extensible.

Common use cases include:

- 🔄 **Runtime Configuration Updates:** When you have an application and need to change certain configuration items during runtime without stopping the application.

- 🧩 **C# to JSON or XML Mapping:** Map C# types to JSON or XML files one by one, enabling strong-typed configuration access.

- 🌐 **Centralized Multi-Module Management:** Want to centrally manage configurations across multiple modules/environments and support features like hot updates.

# Key Features

Here are some of the key features of this library:

- ⚙️ **Runtime Mutable Configuration:** Supports easily modifying configuration items with code during the application's runtime without needing to restart the application.

- 🧩 **Configuration File Hierarchy and Typed Access:**

  - Maps C# classes to JSON or XML files one-to-one, providing strong-typed access.
  - Configuration fields can be read and modified like regular C# objects.
  - Changes are persisted to the JSON or XML file explicitly via the `SaveChanges()` method, keeping the file hierarchy consistent with the type definitions.

- 💾 **Persistence and Recovery Mechanism:** Configuration changes can be saved persistently, allowing the previous state to be restored on the next startup.

- 🪶 **Lightweight:** Uses only core .NET features without relying on many external libraries, making it easy to embed and extend.

# How to Use

## Installation

```bash
dotnet add package MutableConfig
```

## Step 1 — Prepare Your Configuration Data

You can choose to prepare configuration data in two ways：

**Case 1：Generate configuration files using C# objects**

```c#
const string configFolderName = "Config";
var configFolderPath = Path.Join(AppContext.BaseDirectory, configFolderName);

var defaultAppSettingsConfig = new AppSettingsConfig {
    AppName = "MutableConfig Sample App",
    Version = "1.0.0",
    EnableDebug = true
};

var defaultDatabaseConfig = new DatabaseConfig {
    Host = "localhost",
    Port = 5432
};

```

**Case 2：Load from existing JSON/XML file**

```c#
const string configFilePath = @"C:\Users\Username\myapp\config.json";
```

## Step 2 — Register `ConfigContext<T>` in Dependency Injection

`MutableConfig` allows you to map **one C# type → one JSON/XML config file**.

When running for the first time, if the configuration file does not exist, it will be created using the value from `SetupDefaultConfigIfNotExists()`。

**Case 1：C# Object → Auto-Generate Configuration File**

Default Generate JSON Configuration File

```c#
builder.Services.AddConfigContext<AppSettingsConfig>(opt =>
    opt.SetupDefaultConfigIfNotExists(defaultAppSettingsConfig, configFolderPath));
```

Generate JSON Configuration File

```c#
builder.Services.AddConfigContext<AppSettingsConfig>(opt =>
    opt.UseJson()
        .SetupDefaultConfigIfNotExists(defaultAppSettingsConfig, configFolderPath));
```

Generate XML Configuration File

```c#
builder.Services.AddConfigContext<AppSettingsConfig>(opt =>
    opt.UseXml()
        .SetupDefaultConfigIfNotExists(defaultAppSettingsConfig, configFolderPath));
```

**Case 2：Existing configuration file → Bind to C# type**

```c#
builder.Services.AddConfigContext<AppSettingsConfig>(opt =>
    opt.LoadConfigFromFile(configFilePath));
```

## Step 3 — Accessing and Modifying Configuration via Dependency Injection

Once `ConfigContext<T>` is registered, you can inject it _anywhere_ in your application.

Below is a complete example of how to:

- Retrieve `ConfigContext<T>` from DI
- Read configuration values
- Modify configuration values
- Persist changes to the JSON/XML file

### Example: Reading and Writing Configuration

```c#
using Microsoft.Extensions.DependencyInjection;
using MutableConfig;

var builder = WebApplication.CreateBuilder(args);

/* ------------------------------------------------------------
 * Step 1: Register ConfigContext<AppSettingsConfig> in the DI container
 * ------------------------------------------------------------ */
builder.Services.AddConfigContext<AppSettingsConfig>(opt =>
    opt.UseJson()
        .SetupDefaultConfigIfNotExists(
            new AppSettingsConfig {
                AppName = "MutableConfig Sample App",
                Version = "1.0.0",
                EnableDebug = true
            },
            Path.Combine(AppContext.BaseDirectory, "Config")
        )
);

var app = builder.Build();

/* ------------------------------------------------------------
 * Step 2: Resolve the ConfigContext<AppSettingsConfig>
 * ------------------------------------------------------------ */
var configContext = app.Services.GetRequiredService<ConfigContext<AppSettingsConfig>>();

//
// ---------------------- Reading Config ----------------------
//
Console.WriteLine("App Name:       " + configContext.Value.AppName);
Console.WriteLine("Version:        " + configContext.Value.Version);
Console.WriteLine("Debug Enabled:  " + configContext.Value.EnableDebug);

//
// ---------------------- Updating Config ----------------------
//
configContext.Value.AppName = "My Updated App";
configContext.Value.EnableDebug = false;

// Persist changes to AppSettingsConfig.json
configContext.SaveChanges();

Console.WriteLine("Configuration updated and saved successfully.");



/* ------------------------------------------------------------
 * OPTIONAL: Inject into any service or controller
 * ------------------------------------------------------------

public class MyService
{
    private readonly ConfigContext<AppSettingsConfig> _context;

    public MyService(ConfigContext<AppSettingsConfig> context)
        => _context = context;

    public void PrintConfig()
        => Console.WriteLine(_context.Value.AppName);

    public void Modify()
    {
        _context.Value.EnableDebug = true;
        _context.SaveChanges();
    }
}

------------------------------------------------------------- */

app.Run();
```

### Summary

With `ConfigContext<T>` you gain:

- **Typed configuration access** — No manual JSON parsing needed
- **Runtime mutability** — Modify config values while the app is running
- **Controlled persistence** — Explicit `SaveChanges()` design
- **Automatic JSON/XML creation** — If missing, files are generated automatically
- **One C# class = one JSON/XML file** — Clean, maintainable, scalable design

# Example

You can find a complete working example here:

👉 [Sample Code](https://github.com/yourboy7/MutableConfig/blob/main/examples/MutableConfig.Sample/MutableConfig.Sample/Program.cs)

This sample demonstrates how to define a configuration class, bind it to a JSON file, and modify it at runtime while keeping both sides synchronized.

# Main Types

The main types provided by this library are:

- `ConfigContext<T>` — A configuration context for a specific type. Use `ConfigContext<T>` for a particular configuration class.
- `ConfigContextOptions<T>` — Options for configuring a `ConfigContext<T>`.

# Feedback & Contributing

**[MutableConfig](https://github.com/yourboy7/MutableConfig)** is released as open source under the [Apache-2.0 license](https://licenses.nuget.org/Apache-2.0). Bug reports and contributions are welcome at [the GitHub repository](https://github.com/yourboy7/MutableConfig).
