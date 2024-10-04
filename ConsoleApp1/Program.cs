using ConsoleApp1;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Text.Json;

var ip = IPAddress.Loopback;
var port = 27001;
var clinet = new TcpClient();
clinet.Connect(ip, port);

var stream = clinet.GetStream();

var br = new BinaryReader(stream);
var bw = new BinaryWriter(stream);

Command command = null;

string responce = null;
string str = null;

while (true)
{
    Console.WriteLine("Write any command or HELP");
    str = Console.ReadLine()!.ToUpper();
    if (str == "HELP")
    {
        Console.WriteLine();
        Console.WriteLine("Command List");
        Console.WriteLine(Command.Cars);
        Console.WriteLine($"{Command.Put} <Car Id> <Car Name>");
        Console.WriteLine($"{Command.Post} <Car Id> <Car Name>");
        Console.WriteLine($"{Command.Delete} <Car Id>");
        Console.WriteLine($"HELP");
        Console.ReadLine();
        Console.Clear();
    }
    var input = str.Split(' ');
    switch (input[0])
    {
        case Command.Cars:
            command = new Command() { Text = input[0] };
            bw.Write(JsonSerializer.Serialize(command));
            responce = br.ReadString();
            var Cars = JsonSerializer.Deserialize < List<Car>>(responce);
            foreach (var _car in Cars)
            {
                Console.WriteLine($"            {_car}");
            }
            break;

        case Command.Post:
            Car car = new Car() { Id = int.Parse(input[1]),Name = input[2] };
            command = new Command() { Text = input[0],Param = car};
            bw.Write(JsonSerializer.Serialize(command));
            responce = br.ReadString();
            var _new = JsonSerializer.Deserialize<Car>(responce);
            Console.WriteLine($"{_new.Name} Is added");
            break;

        case Command.Delete:
            Car __car = new Car() { Id = int.Parse(input[1])};
            command = new Command() { Text = input[0], Param = __car };
            bw.Write(JsonSerializer.Serialize(command));
            responce = br.ReadString();
            if (responce.Contains("EROR"))
            {
                Console.WriteLine(responce);
            }
            else
            {
                var DeletedCar = JsonSerializer.Deserialize<Car>(responce);
                Console.WriteLine($"{DeletedCar.Id} Deleted");
            }
            break;

        case Command.Put:
            Car ___car = new Car() { Id = int.Parse(input[1]), Name = input[2] };
            command = new Command() { Text = input[0], Param = ___car };
            bw.Write(JsonSerializer.Serialize(command));
            responce = br.ReadString();
            if (responce.Contains("EROR"))
            {
                Console.WriteLine(responce);

            }
            else
            {
                var EditedCar = JsonSerializer.Deserialize<Car>(responce);
                Console.WriteLine($"{EditedCar.Id} Edited");
            }
            break;
        
        default:
            break;
    }
}