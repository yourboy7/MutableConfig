using Microsoft.Extensions.DependencyInjection;
using MutableConfig;
using MutableConfig.Sample.Models;
using Newtonsoft.Json;

/*************************************** Configuration Data Preparation ****************************************************/

const string configFolderName = "Config";
var configFolderPath = Path.Join(AppContext.BaseDirectory, configFolderName);

var defaultAppSettingsConfig = new AppSettingsConfig {
    AppName = "MutableConfig Sample App", Version = "1.0.0", EnableDebug = true
};
var defaultDatabaseConfig = new DatabaseConfig {
    Host = "localhost", Port = 5432
};

/***************************************************************************************************************************/

/*************************************************** Cleanup ***************************************************************/
// Warning: The cleanup is done to ensure a consistent environment for each run of the example program, but you don’t need to do this when using ConfigContext.

var appSettingsConfigFilePath =
    Path.Combine(configFolderPath, $"{nameof(AppSettingsConfig)}.json");
var databaseConfigFilePath =
    Path.Combine(configFolderPath, $"{nameof(DatabaseConfig)}.json");
if (File.Exists(appSettingsConfigFilePath))
    File.Delete(appSettingsConfigFilePath);
if (File.Exists(databaseConfigFilePath))
    File.Delete(databaseConfigFilePath);

/***************************************************************************************************************************/

/******************************************** Dependency Injection *********************************************************/

IServiceCollection services = new ServiceCollection();

services.AddConfigContext<AppSettingsConfig>(opt =>
    opt.SetupDefaultConfigIfNotExists(defaultAppSettingsConfig, configFolderPath));
services.AddConfigContext<DatabaseConfig>(opt =>
    opt.SetupDefaultConfigIfNotExists(defaultDatabaseConfig, configFolderPath));

IServiceProvider serviceProvider = services.BuildServiceProvider();

var appSettingsConfigContext = serviceProvider
    .GetRequiredService<ConfigContext<AppSettingsConfig>>();
var databaseConfigConfigContext =
    serviceProvider.GetRequiredService<ConfigContext<DatabaseConfig>>();

/***************************************************************************************************************************/

/*************************************************** Read ******************************************************************/

var appSettingsConfig = appSettingsConfigContext.Value;
Console.WriteLine("Read at runtime...");
Console.WriteLine(
    $"appSettingsConfig.AppName: {appSettingsConfig.AppName}, defaultAppSettingsConfig.AppName: {defaultAppSettingsConfig.AppName}");
Console.WriteLine(
    $"appSettingsConfig.Version: {appSettingsConfig.Version}, defaultAppSettingsConfig.Version: {defaultAppSettingsConfig.Version}");
Console.WriteLine(
    $"appSettingsConfig.EnableDebug: {appSettingsConfig.EnableDebug}, defaultAppSettingsConfig.EnableDebug: {defaultAppSettingsConfig.EnableDebug}");
Console.WriteLine();

/***************************************************************************************************************************/

/*************************************************** Save ******************************************************************/

var databaseConfig = databaseConfigConfigContext.Value;
Console.WriteLine("Modify at runtime....");
Console.WriteLine("Before modification...");
Console.WriteLine(
    $"databaseConfig.Host == \"localhost\": {databaseConfig.Host == "localhost"}");
Console.WriteLine(
    $"databaseConfig.Port == 5432: {databaseConfig.Port == 5432}");
databaseConfig.Host = "changedHost";
databaseConfig.Port = 1234;
databaseConfigConfigContext.SaveChanges();
Console.WriteLine("After modification...");
Console.WriteLine(
    $"databaseConfig.Host == \"changedHost\": {databaseConfig.Host == "changedHost"}");
Console.WriteLine(
    $"databaseConfig.Port == 1234: {databaseConfig.Port == 1234}");
Console.WriteLine();

var deserializedDatabaseConfig =
    JsonConvert.DeserializeObject<DatabaseConfig>(
        File.ReadAllText(databaseConfigFilePath))!;
Console.WriteLine("Verify the saved results from the disk file...");
Console.WriteLine(
    $"databaseConfig.Host: \"{databaseConfig.Host}\", deserializedDatabaseConfig.Host: \"{deserializedDatabaseConfig.Host}\"");
Console.WriteLine(
    $"databaseConfig.Port: {databaseConfig.Port}, deserializedDatabaseConfig.Port: {deserializedDatabaseConfig.Port}");
Console.WriteLine(
    $"deserializedDatabaseConfig.Host == \"changedHost\": {deserializedDatabaseConfig.Host == "changedHost"}");
Console.WriteLine(
    $"deserializedDatabaseConfig.Port == 1234: {deserializedDatabaseConfig.Port == 1234}");

/***************************************************************************************************************************/