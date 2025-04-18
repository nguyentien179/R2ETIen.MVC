using System;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using R2ETien.MVC.Entities;
using R2ETien.MVC.Enum;
using R2ETien.MVC.Interface;

namespace R2ETien.MVC.Controllers;

[Route("NashTech")]
public class RookiesController : Controller
{
    private IPersonService _personService;

    public RookiesController(IPersonService personService)
    {
        _personService = personService;
    }

    [HttpGet]
    public IActionResult Members([FromQuery] Filter filter)
    {
        IEnumerable<Person> members = _personService.GetAll();

        members = filter switch
        {
            Filter.All => _personService.GetAll(),
            Filter.Male => members.Where(m => m.Gender == Gender.Male),
            Filter.Oldest => members.OrderBy(m => m.DateOfBirth).Take(1),
            Filter.Equals2000 => members.Where(m => m.DateOfBirth.Year == 2000),
            Filter.Greaterthan2000 => members.Where(m => m.DateOfBirth.Year > 2000),
            Filter.Lessthan2000 => members.Where(m => m.DateOfBirth.Year < 2000),
            _ => members,
        };

        return View(members.ToList());
    }

    [HttpGet("{id:guid}")]
    public IActionResult MemberDetails(Guid id)
    {
        var person = _personService.GetById(id);
        return person != null ? View(person) : NotFound("Member not found");
    }

    [HttpGet("AddMember")]
    public IActionResult AddMemberView()
    {
        return View();
    }

    [HttpPost("AddMember")]
    public IActionResult AddMember(Person person)
    {
        if (!ModelState.IsValid)
        {
            return View(person);
        }

        try
        {
            _personService.Create(person);
            return RedirectToAction("Members");
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", ex.Message);
            return View();
        }
    }

    [HttpGet("EditMember/{id:guid}")]
    public IActionResult EditMember(Guid id)
    {
        var person = _personService.GetById(id);
        if (person == null)
        {
            return NotFound();
        }
        return View(person);
    }

    [HttpPost("EditMember/{id}")]
    public IActionResult EditMember(Person person)
    {
        if (!ModelState.IsValid)
        {
            foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
            {
                Console.WriteLine($"Validation Error: {error.ErrorMessage}");
            }
            return View(person);
        }

        try
        {
            Console.WriteLine($"Updating person with ID: {person.Id}");
            _personService.Update(person);
            return RedirectToAction("Members");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception: {ex.Message}");
            ModelState.AddModelError("", ex.Message);
            return View(person);
        }
    }

    [HttpPost("Delete")]
    public IActionResult Delete(Guid id)
    {
        try
        {
            var person = _personService.GetById(id);
            if (person == null)
            {
                return NotFound("Person not found.");
            }

            _personService.Delete(id);
            return RedirectToAction("Members");
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }

    [Route("ExportToExcel")]
    public IActionResult ExportToExcel()
    {
        var members = _personService.GetAll().ToList();
        ExcelPackage.License.SetNonCommercialPersonal("Tien");

        using (var package = new ExcelPackage())
        {
            var worksheet = package.Workbook.Worksheets.Add("Persons");

            worksheet.Cells[1, 1].Value = "First Name";
            worksheet.Cells[1, 2].Value = "Last Name";
            worksheet.Cells[1, 3].Value = "Gender";
            worksheet.Cells[1, 4].Value = "Date of Birth";
            worksheet.Cells[1, 5].Value = "Phone Number";
            worksheet.Cells[1, 6].Value = "Birth Place";
            worksheet.Cells[1, 7].Value = "Is Graduated";

            int row = 2;
            foreach (var member in members)
            {
                worksheet.Cells[row, 1].Value = member.FirstName;
                worksheet.Cells[row, 2].Value = member.LastName;
                worksheet.Cells[row, 3].Value = member.Gender.ToString();
                worksheet.Cells[row, 4].Value = member.DateOfBirth.ToString("yyyy-MM-dd");
                worksheet.Cells[row, 5].Value = member.PhoneNumber;
                worksheet.Cells[row, 6].Value = member.BirthPlace;
                worksheet.Cells[row, 7].Value = member.IsGraduated ? "Yes" : "No";
                row++;
            }

            worksheet.Cells.AutoFitColumns();

            var excelBytes = package.GetAsByteArray();

            return File(
                excelBytes,
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                "Persons.xlsx"
            );
        }
    }
}
