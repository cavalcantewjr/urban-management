using System;
using UrbanManagement.Domain.Entities;

namespace UrbanManagement.Application.DTOs;

public record CreateIncidentRequest(string Title, string Description, string AreaCode, Guid? AssignedToUserId = null)
{
    public Incident ToEntity()
    {
        return new Incident(
            title: Title ?? throw new ArgumentNullException(nameof(Title)),
            description: Description ?? throw new ArgumentNullException(nameof(Description)),
            areaCode: AreaCode ?? throw new ArgumentNullException(nameof(AreaCode)),
            assignedToUserId: AssignedToUserId);
    }
}

