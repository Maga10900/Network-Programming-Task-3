namespace ConsoleApp1;

public class Command
{
    public const string Cars = "CARS";
    public const string Put = "PUT";
    public const string Delete = "DELETE";
    public const string Post = "POST";

    public string? Text { get; set; }
    public Car? Param { get; set; }
}
