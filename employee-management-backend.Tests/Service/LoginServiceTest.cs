using employee_management_backend.Model;
using employee_management_backend.Repository.Interface;
using employee_management_backend.Service;
using employee_management_backend.Service.Utils.Passwords;
using Moq;

namespace employee_management_backend.Tests.Service;

public class LoginServiceTest
{
    private readonly Mock<IEmployeeRepository> _mockEmployeeRepository;
    private readonly LoginService _service;

    public LoginServiceTest()
    {
        _mockEmployeeRepository = new Mock<IEmployeeRepository>();
        _service = new LoginService(_mockEmployeeRepository.Object);
    }

    private readonly LoginDetails _testLogin = new()
    {
        UserId = "7734021",
        Password = "testpassword",
    };

    private readonly Employee _testEmployee = new()
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
        IsAdmin = false,
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

    [Fact]
    public async Task ValidateLogin_ShouldReturnTrue_WhenUserIdAndPasswordAreCorrect()
    {
        var hashedPassword = HashPasswordUtil.PerformPasswordHash("testpassword");

        _testEmployee.Password = hashedPassword;

        _mockEmployeeRepository.Setup(repository => repository.GetEmployeeById(_testEmployee.EmployeeId))
            .ReturnsAsync(_testEmployee);

        var result = await _service.ValidateLogin(_testLogin);

        Assert.True(result);
    }

    [Fact]
    public async Task ValidateLogin_ShouldReturnFalse_WhenUserDoesNotExist()
    {
        _mockEmployeeRepository.Setup(repository => repository.GetEmployeeById("12345678"))
            .ReturnsAsync((Employee)null!);

        var result = await _service.ValidateLogin(_testLogin);

        Assert.False(result);
    }

    [Fact]
    public async Task ValidateLogin_ShouldReturnFalse_WhenPasswordIsIncorrect()
    {
        var hashedPassword = HashPasswordUtil.PerformPasswordHash("correctpassword");

        _testEmployee.Password = hashedPassword;

        _mockEmployeeRepository.Setup(repository => repository.GetEmployeeById(_testEmployee.EmployeeId))
            .ReturnsAsync(_testEmployee);

        var result = await _service.ValidateLogin(_testLogin);

        Assert.False(result);
    }

    [Fact]
    public async Task ValidateLogin_ShouldReturnFalse_WhenEmployeePasswordIsNullOrEmpty()
    {
        _testEmployee.Password = null;

        _mockEmployeeRepository.Setup(repository => repository.GetEmployeeById(_testEmployee.EmployeeId))
            .ReturnsAsync(_testEmployee);
        
        var result = await _service.ValidateLogin(_testLogin);
        
        Assert.False(result);
    }

    [Fact]
    public async Task ValidateLogin_ShouldThrowArgumentNullException_WhenLoginDetailsAreNull()
    {
        await Assert.ThrowsAsync<ArgumentNullException>(() => _service.ValidateLogin(null!));
    }
}
