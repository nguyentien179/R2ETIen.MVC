using System;
using R2ETien.MVC.Entities;

namespace R2ETien.MVC.Interface;

public interface IPersonService
{
    void Create(Person person);
    void Update(Person person);
    void Delete(Guid id);
    List<Person> GetAll();
    Person? GetById(Guid id);
}
