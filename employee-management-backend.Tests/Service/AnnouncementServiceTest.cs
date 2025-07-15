using employee_management_backend.Model;
using employee_management_backend.Repository.Interface;
using employee_management_backend.Service;
using static employee_management_backend.Tests.MockObjects.MockAnnouncements;
using Moq;

namespace employee_management_backend.Tests.Service;

public class AnnouncementServiceTest
{
    private readonly Mock<IAnnouncementRepository> _mockRepository;
    private readonly Mock<IEmployeeRepository> _mockEmployeeRepository;
    private readonly AnnouncementService _service;
    private readonly Announcement _testAnnouncement;

    public AnnouncementServiceTest()
    {
        _mockRepository = new Mock<IAnnouncementRepository>();
        _mockEmployeeRepository = new Mock<IEmployeeRepository>();
        _service = new AnnouncementService(_mockRepository.Object, _mockEmployeeRepository.Object);

        _testAnnouncement = NewAnnouncementPost;
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
}
