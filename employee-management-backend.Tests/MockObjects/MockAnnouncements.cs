﻿using employee_management_backend.Model;

namespace employee_management_backend.Tests.MockObjects;

public static class MockAnnouncements
{
    internal static Announcement NewAnnouncementPost => new()
    {
        EventId = Guid.NewGuid(),
        CreatedByEmployeeId = "12345678",
        CreationDate = new DateTime(2025, 7, 3),
        Title = "Quarterly Strategy Update",
        Description = "All teams are invited to attend the Q3 strategy meeting. We'll review progress and set priorities for the next quarter.",
        Audience = "All-Staff",
        ReadCount = 0
    };
    
    internal static Announcement NewManagerPost => new()
    {
        EventId = Guid.NewGuid(),
        CreatedByEmployeeId = "12345678",
        CreationDate = new DateTime(2025, 7, 1),
        Title = "Quarterly Strategy Update",
        Description = "Management are invited to attend the Q3 strategy meeting. We'll review progress and set priorities for the next quarter.",
        Audience = "Management",
        ReadCount = 0
    };
    
    internal static Announcement NewAdminPost => new()
    {
        EventId = Guid.NewGuid(),
        CreatedByEmployeeId = "12345678",
        CreationDate = new DateTime(2025, 7, 1),
        Title = "Quarterly Strategy Update",
        Description = "Admin staff are invited to attend the Q3 strategy meeting. We'll review progress and set priorities for the next quarter.",
        Audience = "Admin",
        ReadCount = 0
    };
}
