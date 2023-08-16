namespace EasyNetQ.Console.Entities;

public record Person(
    string FullName,
    string Document,
    DateTime birthDate);