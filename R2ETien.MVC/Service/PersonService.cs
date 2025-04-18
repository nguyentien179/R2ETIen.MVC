using System;
using R2ETien.MVC.Entities;
using R2ETien.MVC.Interface;

namespace R2ETien.MVC.Service;

public class PersonService : IPersonService
{
    private readonly IPersonRepository _personRepository;

    public PersonService(IPersonRepository personRepository)
    {
        _personRepository = personRepository;
    }

    public List<Person> GetAll()
    {
        return _personRepository.GetAll();
    }

    public Person GetById(Guid id)
    {
        return PersonHelper.ValidateAndFindPerson(id, _personRepository);
    }

    public void Create(Person person)
    {
        _personRepository.Create(person);
    }

    public void Update(Person person)
    {
        PersonHelper.ValidateAndFindPerson(person.Id, _personRepository);
        _personRepository.Update(person);
    }

    public void Delete(Guid id)
    {
        PersonHelper.ValidateAndFindPerson(id, _personRepository);
        _personRepository.Delete(id);
    }
}
