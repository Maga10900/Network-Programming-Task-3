using Lisener;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Text.Json;

var ip = IPAddress.Parse("127.0.0.1");
var port = 27001;

var lisener = new TcpListener(ip, port);
lisener.Start();

List<Car> cars = new List<Car>() { new Car() { Id = 1,Name="TEST"} };


while (true)
{
    var client = lisener.AcceptTcpClient();

    var stream = client.GetStream();

    var br = new BinaryReader(stream);

    var bw = new BinaryWriter(stream);


    while (true)
    {
        var input = br.ReadString();
        var command = JsonSerializer.Deserialize<Command>(input);

        Console.WriteLine(command.Text);
        Console.WriteLine(command.Param);
        switch (command.Text)
        {
            case Command.Cars:
                var carNms = JsonSerializer.Serialize(cars);
                bw.Write(carNms);
                break;


            case Command.Post:
                cars.Add(command.Param);
                bw.Write(JsonSerializer.Serialize(command.Param));
                break;



            case Command.Put:
                Car CAR = null;
                foreach (var item in cars)
                {
                    if (item.Id == command.Param.Id) CAR = item;
                }
                if (CAR is null)
                {
                    bw.Write("EROR");
                }
                else
                {
                    foreach (var item in cars)
                    {
                        if (CAR.Id == item.Id)
                        {
                            item.Name = command.Param.Name;
                            break;
                        }
                    }
                    var editedCar = JsonSerializer.Serialize(command.Param);
                    bw.Write(editedCar);
                }
                break;

            case Command.Delete:
                Car carr = null;
                foreach (var item in cars)
                {
                    if (item.Id == command.Param.Id) carr = item;
                }
                if (carr is null)
                {
                    bw.Write("EROR");
                }
                else
                {
                    cars.Remove(carr);
                    var DeletedCar = JsonSerializer.Serialize(command.Param);
                    bw.Write(DeletedCar);
                }
                break;
            default:
                break;
        }
    }
}