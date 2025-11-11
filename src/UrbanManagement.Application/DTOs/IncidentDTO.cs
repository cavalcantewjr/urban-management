using System;
using UrbanManagement.Domain.Entities;

namespace UrbanManagement.Application.DTOs;

public record IncidentDto(
    Guid Id,
    string Title,
    string Description,
    string AreaCode,
    string Status,
    Guid? AssignedToUserId,
    DateTime CreatedAt,
    DateTime? UpdatedAt)
{
    public static IncidentDto FromEntity(Incident incident)
    {
        if (incident is null)
        {
            throw new ArgumentNullException(nameof(incident));
        }

        return new IncidentDto(
            incident.Id,
            incident.Title,
            incident.Description,
            incident.AreaCode,
            incident.Status,
            incident.AssignedToUserId,
            incident.CreatedAt,
            incident.UpdatedAt);
    }
}
