using Microsoft.Extensions.Configuration;
using MySqlConnector;
using Renci.SshNet;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ConnectionInfo = Renci.SshNet.ConnectionInfo;
using MySql.Data;


public class MySqlConnectionManager
{
    private readonly string connectionString;
    private readonly string sshHost;
    private readonly int sshPort;
    private readonly string sshUsername;
    private readonly string sshPassword;
    private readonly int localPort;

    private SshClient sshClient;
    private ForwardedPortLocal sshTunnel;

    public MySqlConnectionManager(IConfiguration configuration)
    {
        this.connectionString = configuration.GetConnectionString("UpdatedConnection");
        this.sshHost = configuration["SSH:Host"];
        this.sshPort = int.Parse(configuration["SSH:Port"]);
        this.sshUsername = configuration["SSH:Username"];
        this.sshPassword = configuration["SSH:Password"];
        this.localPort = int.Parse(configuration["SSH:LocalPort"]);
    }

    public MySqlConnection GetConnection()
    {
        sshClient = new SshClient(new ConnectionInfo(
            sshHost, sshPort, sshUsername, new PasswordAuthenticationMethod(sshUsername, sshPassword)));
        sshClient.Connect();

        sshTunnel = new ForwardedPortLocal("127.0.0.1", (uint)localPort, "127.0.0.1", 3306);
        sshClient.AddForwardedPort(sshTunnel);
        sshTunnel.Start();

        var connection = new MySqlConnection(connectionString);
        connection.Open();

        return connection;
    }

    public void CloseConnection()
    {
        sshTunnel?.Stop();
        sshClient?.Disconnect();
        sshClient?.Dispose();
    }
}
