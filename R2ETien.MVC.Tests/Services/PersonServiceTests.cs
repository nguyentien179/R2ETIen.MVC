using System;
using Moq;
using R2ETien.MVC.Entities;
using R2ETien.MVC.Enum;
using R2ETien.MVC.Interface;
using R2ETien.MVC.Service;

namespace R2ETien.MVC.Tests.Services;

public class PersonServiceTests
{
    private readonly Mock<IPersonRepository> _mockRepo;
    private readonly PersonService _service;

    public PersonServiceTests()
    {
        _mockRepo = new Mock<IPersonRepository>();
        _service = new PersonService(_mockRepo.Object);
    }

    [Fact]
    public void GetAll_ReturnsAllPersons()
    {
        var persons = new List<Person> { CreateFakePerson(), CreateFakePerson() };
        _mockRepo.Setup(r => r.GetAll()).Returns(persons);

        var result = _service.GetAll();

        Assert.Equal(2, result.Count);
    }

    [Fact]
    public void GetById_ExistingPerson_ReturnsPerson()
    {
        var personId = Guid.NewGuid();
        var person = CreateFakePerson();
        person.Id = personId;

        _mockRepo.Setup(r => r.GetById(personId)).Returns(person);

        var result = _service.GetById(personId);

        Assert.NotNull(result);
        Assert.Equal(personId, result.Id);
        Assert.Equal("Test", result.FirstName);
    }

    [Fact]
    public void GetById_PersonNotFound_ThrowsException()
    {
        var personId = Guid.NewGuid();
        _mockRepo.Setup(r => r.GetById(personId)).Returns((Person?)null);

        var exception = Assert.Throws<Exception>(() => _service.GetById(personId));
        Assert.Equal("Person not found", exception.Message);
    }

    [Fact]
    public void Update_ExistingPerson_UpdatesPerson()
    {
        var personId = Guid.NewGuid();
        var person = CreateFakePerson();
        person.Id = personId;
        var updatedPerson = CreateFakePerson();
        updatedPerson.Id = personId;

        _mockRepo.Setup(r => r.GetById(personId)).Returns(person);

        _service.Update(updatedPerson);

        _mockRepo.Verify(r => r.Update(updatedPerson), Times.Once);
    }

    [Fact]
    public void Update_PersonNotFound_ThrowsException()
    {
        var personId = Guid.NewGuid();
        var updatedPerson = CreateFakePerson();
        updatedPerson.Id = personId;

        _mockRepo.Setup(r => r.GetById(personId)).Returns((Person?)null);

        var exception = Assert.Throws<Exception>(() => _service.Update(updatedPerson));
        Assert.Equal("Person not found", exception.Message);
    }

    [Fact]
    public void Delete_ExistingPerson_DeletesPerson()
    {
        var personId = Guid.NewGuid();
        var person = CreateFakePerson();
        person.Id = personId;

        _mockRepo.Setup(r => r.GetById(personId)).Returns(person);

        _service.Delete(personId);

        _mockRepo.Verify(r => r.Delete(personId), Times.Once);
    }

    [Fact]
    public void Delete_PersonNotFound_ThrowsException()
    {
        var personId = Guid.NewGuid();
        _mockRepo.Setup(r => r.GetById(personId)).Returns((Person?)null);

        var exception = Assert.Throws<Exception>(() => _service.Delete(personId));
        Assert.Equal("Person not found", exception.Message);
    }

    private Person CreateFakePerson()
    {
        return new Person
        {
            Id = Guid.NewGuid(),
            FirstName = "Test",
            LastName = "User",
            Gender = Gender.Male,
            DateOfBirth = new DateTime(1990, 1, 1),
            PhoneNumber = "123456789",
            BirthPlace = "Test City",
            IsGraduated = false,
        };
    }
}
