using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using UrbanManagement.Application.Interfaces;
using UrbanManagement.Domain.Entities;

namespace UrbanManagement.Infrastructure.Repositories;

public class InMemoryIncidentRepository : IIncidentRepository
{
    private readonly ConcurrentDictionary<Guid, Incident> _storage = new();

    public Task<IReadOnlyCollection<Incident>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var incidents = _storage.Values
            .Select(Clone)
            .OrderByDescending(incident => incident.CreatedAt)
            .ToList()
            .AsReadOnly();

        return Task.FromResult<IReadOnlyCollection<Incident>>(incidents);
    }

    public Task<Incident?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        _storage.TryGetValue(id, out var incident);
        return Task.FromResult(incident is null ? null : Clone(incident));
    }

    public Task<IReadOnlyCollection<Incident>> GetByAreaAsync(string areaCode, CancellationToken cancellationToken = default)
    {
        var incidents = _storage.Values
            .Where(incident => string.Equals(incident.AreaCode, areaCode, StringComparison.OrdinalIgnoreCase))
            .Select(Clone)
            .OrderByDescending(incident => incident.CreatedAt)
            .ToList()
            .AsReadOnly();

        return Task.FromResult<IReadOnlyCollection<Incident>>(incidents);
    }

    public Task<Incident> AddAsync(Incident incident, CancellationToken cancellationToken = default)
    {
        if (incident is null)
        {
            throw new ArgumentNullException(nameof(incident));
        }

        var snapshot = Clone(incident);
        _storage[snapshot.Id] = snapshot;
        return Task.FromResult(Clone(snapshot));
    }

    public Task UpdateAsync(Incident incident, CancellationToken cancellationToken = default)
    {
        if (incident is null)
        {
            throw new ArgumentNullException(nameof(incident));
        }

        _storage[incident.Id] = Clone(incident);
        return Task.CompletedTask;
    }

    public Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        _storage.TryRemove(id, out _);
        return Task.CompletedTask;
    }

    private static Incident Clone(Incident incident)
    {
        return new Incident(
            incident.Id,
            incident.Title,
            incident.Description,
            incident.AreaCode,
            incident.Status,
            incident.CreatedAt,
            incident.UpdatedAt);
    }
}

