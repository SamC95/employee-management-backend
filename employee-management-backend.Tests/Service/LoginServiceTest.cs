using employee_management_backend.Model;
using employee_management_backend.Repository.Interface;
using employee_management_backend.Service;
using employee_management_backend.Service.Utils.Passwords;
using static employee_management_backend.Tests.MockObjects.MockEmployees;
using static employee_management_backend.Tests.MockObjects.MockLogins;
using Moq;

namespace employee_management_backend.Tests.Service;

public class LoginServiceTest
{
    private readonly Mock<IEmployeeRepository> _mockEmployeeRepository;
    private readonly LoginService _service;
    private readonly Employee _testEmployee;
    private readonly LoginDetails _testLogin;

    public LoginServiceTest()
    {
        _mockEmployeeRepository = new Mock<IEmployeeRepository>();
        _service = new LoginService(_mockEmployeeRepository.Object);
        _testEmployee = TestEmployeeHassan;
        _testLogin = TestLogin;
    }

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
