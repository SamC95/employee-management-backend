using employee_management_backend.Model;

namespace employee_management_backend.Tests.MockObjects;

public static class MockEmployees
{
    internal static Employee CreateTestEmployee(string id)
    {
        return new Employee
        {
            EmployeeId = id,
            ClockId = "123456",
            FirstName = "Test",
            LastName = "User",
            Email = "test@example.com",
            PhoneNum = "0000000000",
            Address = "123 Street",
            City = "City",
            PostCode = "00000",
            Country = "Country",
            Gender = "Test",
            DateOfBirth = new DateOnly(1990, 1, 1),
            DateHired = new DateOnly(2020, 1, 1),
            IsAdmin = false,
            IsManager = false,
            IsActive = true,
            JobTitle = "Tester",
            NationalInsuranceNumber = "AA1234567",
            NationalInsuranceCategory = "A",
            TaxCode = "1257L",
            HasPension = false,
            EmployeePensionContributionPercentage = 0,
            EmployerPensionContributionPercentage = 0,
            HasUnion = false,
            UnionContributionPercentage = 0,
        };
    }
    
    internal static Employee TestEmployeeJohn => new()
    {
        EmployeeId = "12345678",
        ClockId = "5843292",
        FirstName = "John",
        LastName = "Doe",
        Email = "john.doe@gmail.com",
        PhoneNum = "555-555-5555",
        Address = "Address 1",
        City = "City",
        PostCode = "PostCode",
        Country = "United Kingdom",
        Gender = "Male",
        DateOfBirth = new DateOnly(1991,
            12,
            31),
        DateHired = new DateOnly(2021,
            05,
            12),
        IsAdmin = false,
        IsManager = true,
        IsActive = true,
        JobTitle = "Graphic Designer",
        NationalInsuranceNumber = "AA1234567",
        NationalInsuranceCategory = "A",
        TaxCode = "1257L",
        HasPension = false,
        EmployeePensionContributionPercentage = 0,
        EmployerPensionContributionPercentage = 0,
        HasUnion = false,
        UnionContributionPercentage = 0,
    };
    
    internal static Employee TestEmployeeHassan => new()
    {
        EmployeeId = "7734021",
        ClockId = "9988776",
        FirstName = "Hassan",
        LastName = "Patel",
        Email = "hassan.patel@vertexinnovations.co.uk",
        PhoneNum = "07700 900234",
        Address = "42 Tech Park Avenue",
        City = "Manchester",
        PostCode = "M1 4AB",
        Country = "United Kingdom",
        Gender = "Male",
        DateOfBirth = new DateOnly(1990,
            3,
            14),
        DateHired = new DateOnly(2022,
            6,
            1),
        IsAdmin = true,
        IsManager = true,
        IsActive = true,
        JobTitle = "Product Developer",
        NationalInsuranceNumber = "BB1234567",
        NationalInsuranceCategory = "A",
        TaxCode = "1257L",
        HasPension = false,
        EmployeePensionContributionPercentage = 0,
        EmployerPensionContributionPercentage = 0,
        HasUnion = false,
        UnionContributionPercentage = 0,
        PayPerHour = 23.40m
    };
}
