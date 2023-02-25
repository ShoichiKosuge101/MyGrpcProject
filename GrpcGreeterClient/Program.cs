using System.Net.Http;
using System.Threading.Tasks;
using Grpc.Net.Client;
using GrpcGreeterClient;

// // See https://aka.ms/new-console-template for more information
// Console.WriteLine("Hello, World!");

var server = "https://172.24.128.1:7094";
var option = new GrpcChannelOptions()
{
    HttpClient = new HttpClient(new HttpClientHandler
    {
        ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator,
    })
};

var channel = GrpcChannel.ForAddress(server, option);
var client = new Greeter.GreeterClient(channel);

var reponse = await client.SayHelloAsync(new HelloRequest{Name = "World"});