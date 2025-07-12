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

    public AnnouncementRepositoryTest()
    {
        var dbOptions = new DbContextOptionsBuilder<AnnouncementDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new AnnouncementDbContext(dbOptions);
        _repository = new AnnouncementRepository(_context);

        _testAnnouncement = NewAnnouncementPost;
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
}
