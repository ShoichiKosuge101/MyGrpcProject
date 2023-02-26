using System.Net.Http;
using System.Threading.Tasks;
using Grpc.Net.Client;
using GrpcGreeterClient;
using NLog;

var _logger = LogManager.GetCurrentClassLogger();

// ipconfigにてip check
// docker container ls にてport check
var server = "https://172.24.143.114:7094";
_logger.Info(server);

try
{
    var option = new GrpcChannelOptions()
    {
        HttpClient = new HttpClient(new HttpClientHandler
        {
            // SSL証明書の検証で常に True を返す
            ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator,
        })
    };

    var channel = GrpcChannel.ForAddress(server, option);
    var client = new Greeter.GreeterClient(channel);

    var response = await client.SayHelloAsync(new HelloRequest{Name = "World"});
    _logger.Error(response);
    _logger.Error(response.Message);
}
catch(Exception ex)
{
    _logger.Error(ex);
}