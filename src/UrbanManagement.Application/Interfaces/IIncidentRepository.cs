using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UrbanManagement.Domain.Entities;

namespace UrbanManagement.Application.Interfaces;

public interface IIncidentRepository
{
    Task<IReadOnlyCollection<Incident>> GetAllAsync(CancellationToken cancellationToken = default);

    Task<Incident?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<IReadOnlyCollection<Incident>> GetByAreaAsync(string areaCode, CancellationToken cancellationToken = default);

    Task<Incident> AddAsync(Incident incident, CancellationToken cancellationToken = default);

    Task UpdateAsync(Incident incident, CancellationToken cancellationToken = default);

    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}
