using CsvHelper;
using CsvHelper.TypeConversion;
using D5.Models;
using Microsoft.AspNetCore.Mvc;

namespace D5.Controllers;

public class RookiesController : Controller
{
    static List<Person> persons = new List<Person>
        {
            new Person
            {
                FirstName = "Phuong",
                LastName = "Nguyen Nam",
                Gender = "Male",
                DateOfBirth = new DateTime(2001, 1, 22),
                PhoneNumber = "",
                BirthPlace = "Phu Tho",
                IsGraduated = false
            },
            new Person
            {
                FirstName = "Nam",
                LastName = "Nguyen Thanh",
                Gender = "Male",
                DateOfBirth = new DateTime(2001, 1, 20),
                PhoneNumber = "",
                BirthPlace = "Ha Noi",
                IsGraduated = false
            },
            new Person
            {
                FirstName = "Son",
                LastName = "Do Hong",
                Gender = "Male",
                DateOfBirth = new DateTime(2000, 11, 6),
                PhoneNumber = "",
                BirthPlace = "Ha Noi",
                IsGraduated = false
            },
            new Person
            {
                FirstName = "Huy",
                LastName = "Nguyen Duc",
                Gender = "Male",
                DateOfBirth = new DateTime(1996, 1, 26),
                PhoneNumber = "",
                BirthPlace = "Ha Noi",
                IsGraduated = false
            },
            new Person
            {
                FirstName = "Hoang",
                LastName = "Phuong Viet",
                Gender = "Male",
                DateOfBirth = new DateTime(1999, 2, 5),
                PhoneNumber = "",
                BirthPlace = "Ha Noi",
                IsGraduated = false
            },
            new Person
            {
                FirstName = "Long",
                LastName = "Lai Quoc",
                Gender = "Male",
                DateOfBirth = new DateTime(1997, 5, 30),
                PhoneNumber = "",
                BirthPlace = "Bac Giang",
                IsGraduated = false
            },
            new Person
            {
                FirstName = "Thanh",
                LastName = "Tran Chi",
                Gender = "Male",
                DateOfBirth = new DateTime(2000, 9, 18),
                PhoneNumber = "",
                BirthPlace = "Ha Noi",
                IsGraduated = false
            },
            new Person
            {
                FirstName = "Miss",
                LastName = "Nguyen Thanh",
                Gender = "Female",
                DateOfBirth = new DateTime(1996, 1, 20),
                PhoneNumber = "",
                BirthPlace = "Ha Noi",
                IsGraduated = false
            }
        };

    [Route("NashTech/Rookies/Get-Male-Person")]
    [Route("rookies/Get-Male-Person")]
    public IActionResult GetMalePerson()
    {
        var results = (from person in persons
                       where person.Gender == "Male"
                       select person).ToList();
        return Json(results);
    }

    [Route("NashTech/Rookies/Get-Oldest-Member")]
    [Route("rookies/Get-Oldest-Member")]
    public IActionResult GetOldestMember()
    {
        var maxAge = persons.Max(m => m.TotalDays);
        var oldest = persons.First(m => m.TotalDays >= maxAge);
        return Json(oldest);
    }

    [Route("NashTech/Rookies/Get-Full-Name")]
    [Route("rookies/Get-Full-Name")]
    public IActionResult GetFullNames()
    {
        var FullName = persons.Select(m => m.FullName).ToList();
        return Json(FullName);
    }

    [Route("NashTech/Rookies/split-members-by-birth-year/{year:int}")]
    [Route("rookies/split-members-by-birth-year/{year:int}")]
    public IActionResult SplitMembersByBirthYear(int year)
    {
        var results = from person in persons
                      group person by person.DateOfBirth.Year.CompareTo(year) into grp
                      select new
                      {
                          Key = grp.Key switch
                          {
                              -1 => $"Birth Year Less Than {year}",
                              0 => $"Birth Year Equals To {year}",
                              1 => $"Birth Year Greater Than {year}",
                              _ => string.Empty
                          },
                          Data = grp.ToList()
                      };
        return Json(results);
    }

    [Route("NashTech/Rookies/get-first-member-by-birth-place")]
    [Route("rookies/get-first-member-by-birth-place")]
    public IActionResult GetFirstMembersByBirthPlace()
    {
        var member = persons.FirstOrDefault(p => p.BirthPlace == "Ha Noi");
        return Json(member);
    }

    [Route("NashTech/Rookies/Export")]
    [Route("rookies/export")]
    public IActionResult Export()
    {
        var buffer = WriteCsvToMemory(persons);
        var memoryStream = new MemoryStream(buffer);
        return new FileStreamResult(memoryStream, "text/csv") { FileDownloadName = "people.csv" };
    }
    public byte[] WriteCsvToMemory(List<Person> data)
    {
        using (var stream = new MemoryStream())
        using (var writer = new StreamWriter(stream))
        using (var csvWriter = new CsvWriter(writer, System.Globalization.CultureInfo.InvariantCulture))
        {
            var options = new TypeConverterOptions { Formats = new[] { "dd MMM yyyy" } };
            csvWriter.Context.TypeConverterOptionsCache.AddOptions<DateTime>(options); 

            csvWriter.WriteRecords(data);
            writer.Flush();
            return stream.ToArray();
        }
    }
}
