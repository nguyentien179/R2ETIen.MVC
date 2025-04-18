using System;
using Microsoft.AspNetCore.Mvc;
using Moq;
using R2ETien.MVC.Controllers;
using R2ETien.MVC.Entities;
using R2ETien.MVC.Enum;
using R2ETien.MVC.Interface;

namespace R2ETien.MVC.Tests.Controllers;

public class RookiesControllerTests
{
    private readonly Mock<IPersonService> _mockPersonService;
    private readonly RookiesController _controller;

    public RookiesControllerTests()
    {
        _mockPersonService = new Mock<IPersonService>();
        _controller = new RookiesController(_mockPersonService.Object);
    }

    [Fact]
    public void Members_WithAllFilter_ReturnsAllMembers()
    {
        var mockMembers = new List<Person>
        {
            new Person
            {
                FirstName = "John",
                LastName = "Doe",
                Gender = Gender.Male,
                DateOfBirth = new DateTime(1995, 5, 1),
                PhoneNumber = "123-456-7890",
                BirthPlace = "City A",
            },
            new Person
            {
                FirstName = "Jane",
                LastName = "Smith",
                Gender = Gender.Female,
                DateOfBirth = new DateTime(2000, 1, 1),
                PhoneNumber = "987-654-3210",
                BirthPlace = "City B",
            },
        };

        _mockPersonService.Setup(service => service.GetAll()).Returns(mockMembers);

        var filter = Filter.All;

        var result = _controller.Members(filter) as ViewResult;
        var model = result?.Model as List<Person>;

        Assert.NotNull(result);
        Assert.Equal(2, model?.Count);
    }

    [Fact]
    public void Members_WithMaleFilter_ReturnsMaleMembers()
    {
        var mockMembers = new List<Person>
        {
            new Person
            {
                FirstName = "John",
                LastName = "Doe",
                Gender = Gender.Male,
                DateOfBirth = new DateTime(1995, 5, 1),
                PhoneNumber = "123-456-7890",
                BirthPlace = "City A",
            },
            new Person
            {
                FirstName = "Jane",
                LastName = "Smith",
                Gender = Gender.Female,
                DateOfBirth = new DateTime(2000, 1, 1),
                PhoneNumber = "987-654-3210",
                BirthPlace = "City B",
            },
        };

        _mockPersonService.Setup(service => service.GetAll()).Returns(mockMembers);

        var filter = Filter.Male;

        // Act
        var result = _controller.Members(filter) as ViewResult;
        var model = result?.Model as List<Person>;

        // Assert
        Assert.NotNull(result);
        Assert.NotNull(model);
        Assert.Single(model);
        Assert.Equal("John", model?.First().FirstName);
    }

    [Fact]
    public void Members_WithOldestFilter_ReturnsOldestMember()
    {
        var mockMembers = new List<Person>
        {
            new Person
            {
                FirstName = "John",
                LastName = "Doe",
                Gender = Gender.Male,
                DateOfBirth = new DateTime(1995, 5, 1),
                PhoneNumber = "123-456-7890",
                BirthPlace = "City A",
            },
            new Person
            {
                FirstName = "Jane",
                LastName = "Smith",
                Gender = Gender.Female,
                DateOfBirth = new DateTime(2000, 1, 1),
                PhoneNumber = "987-654-3210",
                BirthPlace = "City B",
            },
        };

        _mockPersonService.Setup(service => service.GetAll()).Returns(mockMembers);

        var filter = Filter.Oldest;

        var result = _controller.Members(filter) as ViewResult;
        var model = result?.Model as List<Person>;

        Assert.NotNull(result);
        Assert.NotNull(model);
        Assert.Single(model);
        Assert.Equal("John", model?.First().FirstName);
    }

    [Fact]
    public void Members_WithGreaterThan2000Filter_ReturnsMembersBornAfter2000()
    {
        // Arrange
        var mockMembers = new List<Person>
        {
            new Person
            {
                FirstName = "John",
                LastName = "Doe",
                Gender = Gender.Male,
                DateOfBirth = new DateTime(1995, 5, 1),
                PhoneNumber = "123-456-7890",
                BirthPlace = "City A",
            },
            new Person
            {
                FirstName = "Jane",
                LastName = "Smith",
                Gender = Gender.Female,
                DateOfBirth = new DateTime(2005, 1, 1),
                PhoneNumber = "987-654-3210",
                BirthPlace = "City B",
            },
        };

        _mockPersonService.Setup(service => service.GetAll()).Returns(mockMembers);

        var filter = Filter.Greaterthan2000;

        var result = _controller.Members(filter) as ViewResult;
        var model = result?.Model as List<Person>;

        Assert.NotNull(result);
        Assert.NotNull(model);
        Assert.Single(model);
        Assert.Equal("Jane", model?.First().FirstName);
    }

    [Fact]
    public void MemberDetails_WithValidId_ReturnsViewWithPerson()
    {
        var personId = Guid.NewGuid();
        var mockPerson = new Person
        {
            Id = personId,
            FirstName = "John",
            LastName = "Doe",
            Gender = Gender.Male,
            DateOfBirth = new DateTime(1995, 5, 1),
            PhoneNumber = "123-456-7890",
            BirthPlace = "City A",
        };

        _mockPersonService.Setup(service => service.GetById(personId)).Returns(mockPerson);

        var result = _controller.MemberDetails(personId) as ViewResult;
        var model = result?.Model as Person;

        Assert.NotNull(result);
        Assert.NotNull(model);
        Assert.Equal(personId, model?.Id);
        Assert.Equal("John", model?.FirstName);
    }

    [Fact]
    public void MemberDetails_WithInvalidId_ReturnsNotFound()
    {
        var personId = Guid.NewGuid();

        _mockPersonService.Setup(service => service.GetById(personId)).Returns((Person?)null);

        var result = _controller.MemberDetails(personId) as NotFoundObjectResult;

        Assert.NotNull(result);
        Assert.Equal(404, result?.StatusCode);
        Assert.Equal("Member not found", result?.Value);
    }

    [Fact]
    public void AddMemberView_ReturnsView()
    {
        var result = _controller.AddMemberView() as ViewResult;

        Assert.NotNull(result);
        Assert.Null(result?.Model);
    }

    [Fact]
    public void AddMember_WithValidModel_RedirectsToMembers()
    {
        var newPerson = new Person
        {
            FirstName = "John",
            LastName = "Doe",
            Gender = Gender.Male,
            DateOfBirth = new DateTime(1995, 5, 1),
            PhoneNumber = "123-456-7890",
            BirthPlace = "City A",
        };

        _mockPersonService.Setup(service => service.Create(It.IsAny<Person>())).Verifiable();

        var result = _controller.AddMember(newPerson) as RedirectToActionResult;

        Assert.NotNull(result);
        Assert.Equal("Members", result?.ActionName);
        _mockPersonService.Verify(service => service.Create(It.IsAny<Person>()), Times.Once);
    }

    [Fact]
    public void AddMember_WithInvalidModel_ReturnsViewWithPerson()
    {
        var newPerson = CreateFakePerson();
        _controller.ModelState.AddModelError("FirstName", "First name is required");

        var result = _controller.AddMember(newPerson) as ViewResult;

        Assert.NotNull(result);
        Assert.Equal(newPerson, result?.Model);
    }

    [Fact]
    public void EditMember_WithValidId_ReturnsViewWithPerson()
    {
        var personId = Guid.NewGuid();
        var mockPerson = new Person
        {
            Id = personId,
            FirstName = "John",
            LastName = "Doe",
            Gender = Gender.Male,
            DateOfBirth = new DateTime(1995, 5, 1),
            PhoneNumber = "123-456-7890",
            BirthPlace = "City A",
        };

        _mockPersonService.Setup(service => service.GetById(personId)).Returns(mockPerson);

        var result = _controller.EditMember(personId) as ViewResult;
        var model = result?.Model as Person;

        Assert.NotNull(result);
        Assert.NotNull(model);
        Assert.Equal(personId, model?.Id);
        Assert.Equal("John", model?.FirstName);
    }

    [Fact]
    public void EditMember_WithInvalidId_ReturnsNotFound()
    {
        var personId = Guid.NewGuid();

        _mockPersonService.Setup(service => service.GetById(personId)).Returns((Person?)null);

        var result = _controller.EditMember(personId) as NotFoundResult;

        Assert.NotNull(result);
    }

    [Fact]
    public void EditMember_WithValidModel_RedirectsToMembers()
    {
        var personId = Guid.NewGuid();
        var updatedPerson = new Person
        {
            Id = personId,
            FirstName = "John",
            LastName = "Doe",
            Gender = Gender.Male,
            DateOfBirth = new DateTime(1995, 5, 1),
            PhoneNumber = "123-456-7890",
            BirthPlace = "City A",
        };

        _mockPersonService.Setup(service => service.Update(It.IsAny<Person>())).Verifiable();

        var result = _controller.EditMember(updatedPerson) as RedirectToActionResult;

        Assert.NotNull(result);
        Assert.Equal("Members", result?.ActionName);
        _mockPersonService.Verify(service => service.Update(It.IsAny<Person>()), Times.Once);
    }

    [Fact]
    public void EditMember_WithInvalidModel_ReturnsViewWithPerson()
    {
        var updatedPerson = CreateFakePerson();
        _controller.ModelState.AddModelError("FirstName", "First name is required");

        var result = _controller.EditMember(updatedPerson) as ViewResult;

        Assert.NotNull(result);
        Assert.Equal(updatedPerson, result?.Model);
    }

    [Fact]
    public void Delete_WithValidId_RedirectsToMembers()
    {
        // Arrange
        var personId = Guid.NewGuid();
        var mockPerson = new Person
        {
            Id = personId,
            FirstName = "John",
            LastName = "Doe",
            Gender = Gender.Male,
            DateOfBirth = new DateTime(1995, 5, 1),
            PhoneNumber = "123-456-7890",
            BirthPlace = "City A",
        };

        _mockPersonService.Setup(service => service.GetById(personId)).Returns(mockPerson);
        _mockPersonService.Setup(service => service.Delete(personId)).Verifiable();

        // Act
        var result = _controller.Delete(personId) as RedirectToActionResult;

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Members", result?.ActionName);
        _mockPersonService.Verify(service => service.Delete(personId), Times.Once);
    }

    [Fact]
    public void Delete_WithInvalidId_ReturnsNotFound()
    {
        var personId = Guid.NewGuid();

        _mockPersonService.Setup(service => service.GetById(personId)).Returns((Person?)null);

        var result = _controller.Delete(personId) as NotFoundObjectResult;

        Assert.NotNull(result);
        Assert.Equal(404, result?.StatusCode);
        Assert.Equal("Person not found.", result?.Value);
    }

    [Fact]
    public void Delete_WithException_ReturnsNotFoundWithMessage()
    {
        // Arrange
        var personId = Guid.NewGuid();
        var exceptionMessage = "Some error occurred during deletion";

        _mockPersonService
            .Setup(service => service.GetById(personId))
            .Throws(new Exception(exceptionMessage));

        // Act
        var result = _controller.Delete(personId) as NotFoundObjectResult;

        // Assert
        Assert.NotNull(result);
        Assert.Equal(404, result?.StatusCode);
        Assert.Equal(exceptionMessage, result?.Value);
    }

    private Person CreateFakePerson(
        Guid? id = null,
        string? firstName = null,
        string? lastName = null,
        Gender gender = Gender.Male,
        DateTime? dob = null,
        string? phone = null,
        string? birthPlace = null,
        bool isGraduated = true
    )
    {
        return new Person
        {
            Id = id ?? Guid.NewGuid(),
            FirstName = firstName ?? "John",
            LastName = lastName ?? "Doe",
            Gender = gender,
            DateOfBirth = dob ?? new DateTime(1990, 1, 1),
            PhoneNumber = phone ?? "123-456-7890",
            BirthPlace = birthPlace ?? "Default City",
            IsGraduated = isGraduated,
        };
    }
}
