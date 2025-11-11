using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UrbanManagement.Application.DTOs;

namespace UrbanManagement.Application.Interfaces;

public interface IIncidentService
{
    Task<IReadOnlyCollection<IncidentDto>> GetAllAsync(CancellationToken cancellationToken = default);

    Task<IncidentDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<IReadOnlyCollection<IncidentDto>> GetByAreaAsync(string areaCode, CancellationToken cancellationToken = default);

    Task<IncidentDto> CreateAsync(CreateIncidentRequest request, CancellationToken cancellationToken = default);

    Task UpdateAsync(Guid id, UpdateIncidentRequest request, CancellationToken cancellationToken = default);

    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}

