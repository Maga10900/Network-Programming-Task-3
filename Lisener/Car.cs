﻿namespace Lisener;

public class Car
{
    public int Id { get; set; }
    public string Name { get; set; }
    public override string ToString()
    {
        return $"{Id} {Name}";
    }
}
