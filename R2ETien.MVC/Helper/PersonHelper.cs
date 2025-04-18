using System;
using R2ETien.MVC.Entities;
using R2ETien.MVC.Interface;

namespace R2ETien.MVC.Service;

public static class PersonHelper
{
    public static Person ValidateAndFindPerson(Guid id, IPersonRepository personRepository)
    {
        if (id == Guid.Empty)
        {
            throw new ArgumentException("Invalid ID: ID cannot be empty.");
        }

        var person = personRepository.GetById(id);
        if (person == null)
        {
            throw new KeyNotFoundException($"Person with ID {id} not found.");
        }

        return person;
    }
}
