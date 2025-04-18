using System;
using Microsoft.EntityFrameworkCore;
using R2ETien.MVC.Entities;
using R2ETien.MVC.Enum;

namespace R2ETien.MVC.Data;

public class Context(DbContextOptions<Context> options) : DbContext(options)
{
    public DbSet<Person> Person => Set<Person>();

    public static void SeedData(Context context)
    {
        if (!context.Person.Any())
        {
            var people = new List<Person>
            {
                new Person
                {
                    FirstName = "Alice",
                    LastName = "Smith",
                    Gender = Gender.Female,
                    DateOfBirth = new DateTime(1995, 5, 21),
                    PhoneNumber = "1234567890",
                    BirthPlace = "New York",
                    IsGraduated = true,
                },
                new Person
                {
                    FirstName = "Bob",
                    LastName = "Johnson",
                    Gender = Gender.Male,
                    DateOfBirth = new DateTime(1988, 10, 15),
                    PhoneNumber = "2345678901",
                    BirthPlace = "Los Angeles",
                    IsGraduated = true,
                },
                new Person
                {
                    FirstName = "Charlie",
                    LastName = "Brown",
                    Gender = Gender.Male,
                    DateOfBirth = new DateTime(2000, 7, 8),
                    PhoneNumber = "3456789012",
                    BirthPlace = "Chicago",
                    IsGraduated = false,
                },
                new Person
                {
                    FirstName = "Diana",
                    LastName = "Williams",
                    Gender = Gender.Female,
                    DateOfBirth = new DateTime(2000, 12, 25),
                    PhoneNumber = "4567890123",
                    BirthPlace = "Houston",
                    IsGraduated = true,
                },
                new Person
                {
                    FirstName = "Ethan",
                    LastName = "Davis",
                    Gender = Gender.Male,
                    DateOfBirth = new DateTime(2005, 3, 12),
                    PhoneNumber = "5678901234",
                    BirthPlace = "Miami",
                    IsGraduated = false,
                },
                new Person
                {
                    FirstName = "Fiona",
                    LastName = "Garcia",
                    Gender = Gender.Female,
                    DateOfBirth = new DateTime(2010, 6, 30),
                    PhoneNumber = "6789012345",
                    BirthPlace = "Seattle",
                    IsGraduated = false,
                },
            };

            context.Person.AddRange(people);
            context.SaveChanges();
        }
    }
}
