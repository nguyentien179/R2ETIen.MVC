using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using R2ETien.MVC.Data;
using R2ETien.MVC.Entities;
using R2ETien.MVC.Interface;

namespace R2ETien.MVC.Repositories;

public class PersonRepository : IPersonRepository
{
    private readonly Context _context;

    public PersonRepository(Context context)
    {
        _context = context;
    }

    public List<Person> GetAll()
    {
        return _context.Person.ToList();
    }

    public Person GetById(Guid id)
    {
        return _context.Person.AsNoTracking().FirstOrDefault(p => p.Id == id)
            ?? throw new KeyNotFoundException($"Person with ID {id} not found.");
    }

    public void Create(Person person)
    {
        ValidatePerson(person);
        _context.Person.Add(person);
        _context.SaveChanges();
    }

    public void Update(Person person)
    {
        ValidatePerson(person);
        _context.Person.Update(person);
        _context.SaveChanges();
    }

    public void Delete(Guid id)
    {
        var person = _context.Person.Find(id);
        if (person == null)
        {
            throw new KeyNotFoundException($"Person with ID {id} not found.");
        }
        _context.Person.Remove(person);
        _context.SaveChanges();
    }

    private void ValidatePerson(Person person)
    {
        var validationResults = new List<ValidationResult>();
        var validationContext = new ValidationContext(person);

        if (!Validator.TryValidateObject(person, validationContext, validationResults, true))
        {
            var errors = string.Join("; ", validationResults.Select(vr => vr.ErrorMessage));
            throw new ValidationException($"Validation failed: {errors}");
        }
    }
}
