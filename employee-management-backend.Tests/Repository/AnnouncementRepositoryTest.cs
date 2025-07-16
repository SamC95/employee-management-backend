using employee_management_backend.Model;
using employee_management_backend.Repository;
using employee_management_backend.Repository.Database;
using static employee_management_backend.Tests.MockObjects.MockAnnouncements;
using Microsoft.EntityFrameworkCore;

namespace employee_management_backend.Tests.Repository;

public class AnnouncementRepositoryTest
{
    private readonly AnnouncementDbContext _context;
    private readonly AnnouncementRepository _repository;
    private readonly Announcement _testAnnouncement;
    private readonly Announcement _testManagerAnnouncement;
    private readonly Announcement _testAdminAnnouncement;

    public AnnouncementRepositoryTest()
    {
        var dbOptions = new DbContextOptionsBuilder<AnnouncementDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new AnnouncementDbContext(dbOptions);
        _repository = new AnnouncementRepository(_context);

        _testAnnouncement = NewAnnouncementPost;
        _testManagerAnnouncement = NewManagerPost;
        _testAdminAnnouncement = NewAdminPost;
    }
    
    private async Task SetupDatabaseData()
    {
        var announcements = new List<Announcement>
        {
            _testAnnouncement,
            _testManagerAnnouncement,
            _testAdminAnnouncement
        };

        _context.Announcements.AddRange(announcements);
        await _context.SaveChangesAsync();
    }

    [Fact]
    public async Task CreateAnnouncementPost_SavesSuccessfully()
    {
        await _repository.CreateAnnouncementPost(_testAnnouncement);

        var savedEvent =
            await _context.Announcements.FirstOrDefaultAsync(storedEvent =>
                storedEvent.EventId == _testAnnouncement.EventId);
        
        Assert.NotNull(savedEvent);
        Assert.Equal(_testAnnouncement.EventId, savedEvent.EventId);
        Assert.Equal(_testAnnouncement.Title, savedEvent.Title);
    }

    [Fact]
    public async Task GetRecentAnnouncements_FiltersByAudienceAndOrdersByDate()
    {
        await SetupDatabaseData();
        
        var allowedAudiences = new List<string> { "All-Staff", "Management" };
        const int amountToRetrieve = 2;
        
        var result = await _repository.GetRecentAnnouncements(allowedAudiences, amountToRetrieve);
        
        Assert.Equal(2, result.Count);
        Assert.Equal("Management", result[1].Audience);
        Assert.Equal("All-Staff", result[0].Audience);
    }

    [Fact]
    public async Task GetRecentAnnouncements_IgnoresNullAudience()
    {
        var allowedAudiences = new List<string> { "All-Staff", "Management", "Admin" };
        
        var result = await _repository.GetRecentAnnouncements(allowedAudiences, 5);
        
        Assert.DoesNotContain(result, announcement => announcement.Audience == null);
    }

    [Fact]
    public async Task GetRecentAnnouncements_ReturnsEmptyList_IfNoMatchingAudience()
    {
        var allowedAudiences = new List<string> { "Executive" };
        
        var result = await _repository.GetRecentAnnouncements(allowedAudiences, 5);
        
        Assert.Empty(result);
    }
}
