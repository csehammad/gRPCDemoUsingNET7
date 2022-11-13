using Grpc.Core;
using gRPCUsingNET7Demo;

namespace gRPCUsingNET7Demo.Services
{
    public class SalesDataService : SalesService.SalesServiceBase

    {

        public override async Task
            GetSalesData(Request request,
            IServerStreamWriter<SalesDataModel> responseStream, ServerCallContext context)
        {

            using (var reader = new StreamReader(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", "sales_records.csv")))
            {
                string line; bool isFirstLine = true;
                while ((line = reader.ReadLine()) != null)
                {
                    var pieces = line.Split(',');

                    var _model = new SalesDataModel();

                    try
                    {
                        if (isFirstLine)
                        {
                            isFirstLine = false;
                            continue;
                        }

                        _model.Region = pieces[0];
                        _model.Country = pieces[1];

                        _model.OrderID = int.TryParse(pieces[6], out int _orderID) ? _orderID : 0;
                        _model.UnitPrice = float.TryParse(pieces[9], out float _unitPrice) ? _unitPrice : 0;

                        _model.ShipDate = Google.Protobuf.WellKnownTypes.Timestamp.FromDateTime
                             ((DateTime.TryParse(pieces[7], out DateTime _dateShip) ? _dateShip : DateTime.MinValue).ToUniversalTime());

                        _model.UnitsSold = int.TryParse(pieces[8], out int _unitsSold) ? _unitsSold : 0;

                        _model.UnitCost = float.TryParse(pieces[10], out float _unitCost) ? _unitCost : 0;

                        _model.TotalRevenue = int.TryParse(pieces[11], out int _totalRevenue) ? _totalRevenue : 0;
                        _model.TotalCost = int.TryParse(pieces[13], out int _totalCost) ? _totalCost : 0;



                        await responseStream.WriteAsync(_model);


                    }

                    catch (Exception ex)
                    {
                        throw new RpcException(new Status(StatusCode.Internal, ex.ToString()));
                    }



                }
            }

        }

    }
}