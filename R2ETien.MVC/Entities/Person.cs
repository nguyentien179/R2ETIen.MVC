using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using R2ETien.MVC.Enum;

namespace R2ETien.MVC.Entities;

public class Person
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public Gender Gender { get; set; }
    public DateTime DateOfBirth { get; set; }
    public required string PhoneNumber { get; set; }
    public required string BirthPlace { get; set; }
    public bool IsGraduated { get; set; }
}
