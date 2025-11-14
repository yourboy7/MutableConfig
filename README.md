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

Installation

```bash
dotnet add package MutableConfig
```

Add to Program.cs:

```c#
using MutableConfig;
```

```c#
// Configuration Data Preparation
const string configFolderName = "Config";
var basePath = Path.Join(AppContext.BaseDirectory, configFolderName);

var defaultAppSettingsConfig = new AppSettingsConfig {
    AppName = "MutableConfig Sample App", Version = "1.0.0", EnableDebug = true
};
var defaultDatabaseConfig = new DatabaseConfig {
    Host = "localhost", Port = 5432
};

/*
 * Dependency Injection Examples
 *
 * AppSettingsConfig is a custom configuration class.
 * Once registered through dependency injection, MutableConfig will ensure that
 * an "AppSettingsConfig.json" or "AppSettingsConfig.xml" file exists under the specified basePath.
 *
 * On the first run, if the configuration file does not exist, it will be created
 * using the default values provided through SetupDefaultConfigIfNotExists().
 *
 * Configuration objects are fully mutable at runtime, and changes made to them
 * are tracked within their corresponding ConfigContext<T>. To persist updates
 * to the underlying JSON or XML file, you explicitly call SaveChanges() on the context.
 */

// Default Generate JSON Configuration Files
builder.Services.AddConfigContext<AppSettingsConfig>(opt =>
    opt.SetBasePath(basePath)
        .SetupDefaultConfigIfNotExists(defaultAppSettingsConfig));

// Generate JSON Configuration Files
builder.Services.AddConfigContext<AppSettingsConfig>(opt =>
    opt.SetBasePath(basePath)
        .UseJson()
        .SetupDefaultConfigIfNotExists(defaultAppSettingsConfig));

// Generate XML Configuration Files
builder.Services.AddConfigContext<AppSettingsConfig>(opt =>
    opt.SetBasePath(basePath)
        .UseXml()
        .SetupDefaultConfigIfNotExists(defaultAppSettingsConfig));
```

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
