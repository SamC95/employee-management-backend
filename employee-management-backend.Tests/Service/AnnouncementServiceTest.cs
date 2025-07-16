using employee_management_backend.Model;
using employee_management_backend.Repository.Interface;
using employee_management_backend.Service;
using static employee_management_backend.Tests.MockObjects.MockAnnouncements;
using static employee_management_backend.Tests.MockObjects.MockEmployees;
using Moq;

namespace employee_management_backend.Tests.Service;

public class AnnouncementServiceTest
{
    private readonly Mock<IAnnouncementRepository> _mockRepository;
    private readonly Mock<IEmployeeRepository> _mockEmployeeRepository;
    private readonly AnnouncementService _service;
    private readonly Announcement _testAnnouncement;
    private readonly Announcement _testManagerAnnouncement;
    private readonly Announcement _testAdminAnnouncement;
    private readonly Employee _testAdmin;
    private readonly Employee _testManager;
    private readonly Employee _testEmployee;

    public AnnouncementServiceTest()
    {
        _mockRepository = new Mock<IAnnouncementRepository>();
        _mockEmployeeRepository = new Mock<IEmployeeRepository>();
        _service = new AnnouncementService(_mockRepository.Object, _mockEmployeeRepository.Object);

        _testAnnouncement = NewAnnouncementPost;
        _testManagerAnnouncement = NewManagerPost;
        _testAdminAnnouncement = NewAdminPost;
        _testAdmin = TestEmployeeHassan;
        _testManager = TestEmployeeJohn;
        _testEmployee = CreateTestEmployee("12345678");
    }

    [Fact]
    public async Task CreateAnnouncementPost_WhenSuccessful_CallsRepository()
    {
        await _service.CreateAnnouncementPost(_testAnnouncement);

        _mockRepository.Verify(repo => repo.CreateAnnouncementPost(_testAnnouncement), Times.Once());
    }

    [Fact]
    public async Task CreateAnnouncementPost_WhenAnnouncementIsNull_ThrowsArgumentException()
    {
        await Assert.ThrowsAsync<ArgumentException>(() => _service.CreateAnnouncementPost(null!));
    }

    [Fact]
    public async Task GetRecentAnnouncements_Admin_ReturnsAllAnnouncements()
    {
        _mockEmployeeRepository.Setup(repo => repo.GetEmployeeById(_testAdmin.EmployeeId)).ReturnsAsync(_testAdmin);

        var expectedAudiences = new List<string> { "All-Staff", "Management", "Admin" };
        var expectedAnnouncements = new List<Announcement>
        {
            _testAnnouncement,
            _testManagerAnnouncement,
            _testAdminAnnouncement
        };

        _mockRepository.Setup(repo => repo.GetRecentAnnouncements(expectedAudiences, 5))
            .ReturnsAsync(expectedAnnouncements);

        var result = await _service.GetRecentAnnouncements(_testAdmin.EmployeeId);

        Assert.Equal(expectedAnnouncements, result);
    }

    [Fact]
    public async Task GetRecentAnnouncements_Manager_ReturnsManagerAndStaffAnnouncements()
    {
        _mockEmployeeRepository.Setup(repo => repo.GetEmployeeById(_testManager.EmployeeId)).ReturnsAsync(_testManager);

        var expectedAudiences = new List<string> { "All-Staff", "Management" };
        var expectedAnnouncements = new List<Announcement>
        {
            _testAnnouncement,
            _testManagerAnnouncement,
        };

        _mockRepository.Setup(repo => repo.GetRecentAnnouncements(expectedAudiences, 5))
            .ReturnsAsync(expectedAnnouncements);

        var result = await _service.GetRecentAnnouncements(_testManager.EmployeeId);

        Assert.Equal(expectedAnnouncements, result);
    }

    [Fact]
    public async Task GetRecentAnnouncements_Employee_ReturnsStaffAnnouncements()
    {
        _mockEmployeeRepository.Setup(repo => repo.GetEmployeeById(_testEmployee.EmployeeId))
            .ReturnsAsync(_testEmployee);

        var expectedAudiences = new List<string> { "All-Staff" };
        var expectedAnnouncements = new List<Announcement>
        {
            _testAnnouncement,
        };

        _mockRepository.Setup(repo => repo.GetRecentAnnouncements(expectedAudiences, 5))
            .ReturnsAsync(expectedAnnouncements);
        
        var result = await _service.GetRecentAnnouncements(_testEmployee.EmployeeId);
        
        Assert.Equal(expectedAnnouncements, result);
    }

    [Fact]
    public async Task GetRecentAnnouncements_EmployeeNotFound_ThrowsArgumentException()
    {
        const string userId = "missing-id";
        _mockEmployeeRepository.Setup(repo => repo.GetEmployeeById(userId)).ReturnsAsync((Employee)null);
        
        var exception = await Assert.ThrowsAsync<ArgumentException>(() => _service.GetRecentAnnouncements(userId));
        
        Assert.Equal("Employee not found", exception.Message);
    }
}
