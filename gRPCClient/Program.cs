using Grpc.Core;
using Grpc.Net.Client;
using gRPCUsingNET7Demo;

namespace gRPCClient
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var channel = GrpcChannel.ForAddress("https://localhost:7211");
            int Count = 0;
            var watch = System.Diagnostics.Stopwatch.StartNew();
            try
            {
                var client = new SalesService.SalesServiceClient(channel);

                using var call = client.GetSalesData(new Request { Filters = "" }
                  , deadline: DateTime.UtcNow.AddMinutes(10)
                );



                await foreach (var each in call.ResponseStream.ReadAllAsync())
                {


                    Console.WriteLine(String.Format("New Order Receieved from {0}-{1},Order ID = {2}, Unit Price ={3}, Ship Date={4}", each.Country, each.Region, each.OrderID, each.UnitPrice, each.ShipDate));
                    Count++;




                }
            }
            catch (RpcException ex) when (ex.StatusCode == StatusCode.DeadlineExceeded)
            {
                Console.WriteLine("Service timeout.");
            }

            watch.Stop();
            var elapsedMs = watch.Elapsed.TotalMinutes;

            Console.WriteLine($"Stream ended: Total Records:{ Count.ToString()} in {watch.Elapsed.TotalMinutes} minutes and {watch.Elapsed.TotalSeconds} seconds.");
            Console.Read();

        }
    }
}