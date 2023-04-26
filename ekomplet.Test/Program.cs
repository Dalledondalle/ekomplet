﻿
using ekomplet.Domain.Models;
using System.Linq;
using System.Text;




Console.WriteLine(" ");

static IEnumerable<Supervisor> CreateSupervisors(int count)
{
    Supervisor[] supervisors = new Supervisor[count];
    string[] usedNames = new string[count];
    for (int i = 0; i < count; i++)
    {
        string name;
        do
        {
            name = GenerateName(new Random().Next(3,15));
        } while (usedNames.Contains(name));

        usedNames[i] = name;

        Supervisor supervisor = new Supervisor()
        {
            Id = Guid.NewGuid(),
            Firstname = GenerateName(new Random().Next(3, 15)),
            Lastname = name,
            Email = $"{name}@example.com",
            Phone = $"{new Random().Next(10000000, 99999999)}",
            Role = Role.Supervisor
        };
        supervisors[i] = supervisor;
    }
    return supervisors;
}

static IEnumerable<Installer> CreateInstallers(int count)
{
    Installer[] installers = new Installer[count];
    string[] usedNames = new string[count];
    for (int i = 0; i < 15; i++)
    {
        string name;
        do
        {
            name = GenerateName(new Random().Next(3, 15));
        } while (usedNames.Contains(name));

        usedNames[i] = name;

        Installer installer = new Installer()
        {
            Id = Guid.NewGuid(),
            Firstname = GenerateName(new Random().Next(3, 15)),
            Lastname = name,
            Email = $"{name}@example.com",
            Phone = $"{new Random().Next(10000000, 99999999)}",
            Role = Role.Installer
        };

        installers[i] = installer;
    }
    return installers;
}


static string GenerateName(int length)
{
    var builder = new StringBuilder();
    builder.Append((char)('A' + new Random().Next(0, 26)));
    for (int i = 0; i < length; i++)
    {
        builder.Append((char)('a' + new Random().Next(0, 26)));
    }
    return builder.ToString();
}
