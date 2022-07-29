using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Configurations;
using DotNet.Testcontainers.Containers;
using Microsoft.Data.SqlClient;
using Npgsql;

namespace dockercompose.IntegerationTest;

public sealed class postgreSqlTest : IAsyncLifetime
{
    private readonly TestcontainerDatabase testcontainers = new TestcontainersBuilder<PostgreSqlTestcontainer>()
        .WithDatabase(new PostgreSqlTestcontainerConfiguration
        {
            Database = "db",
            Port = 4056,
            Username = "postgres",
            Password = "postgres",
        })
        .Build();


    //private readonly TestcontainerDatabase testcontainers2 = new TestcontainersBuilder<MsSqlTestcontainer>()
    //    .WithDatabase(new MsSqlTestcontainerConfiguration
    //    {
    //        Database = "MessagesStore",
    //        Username = "sa",
    //        Password = "@A123456789@",
    //    })
    //    .Build();

    [Fact]
    public void ExecuteCommand()
    {
        using var connection = new NpgsqlConnection(this.testcontainers.ConnectionString);
        using var command = new NpgsqlCommand();
        connection.Open();
        command.Connection = connection;
        command.CommandText = "SELECT 1";
        command.ExecuteReader();
    }

    public Task InitializeAsync()
    {
        return this.testcontainers.StartAsync();
    }

    public Task DisposeAsync()
    {
        return this.testcontainers.DisposeAsync().AsTask();
    }
}