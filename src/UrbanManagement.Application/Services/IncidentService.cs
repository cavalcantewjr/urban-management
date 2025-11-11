using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using UrbanManagement.Application.DTOs;
using UrbanManagement.Application.Interfaces;
using UrbanManagement.Domain.Entities;
using UrbanManagement.Domain.Enums;

namespace UrbanManagement.Application.Services;

public class IncidentService : IIncidentService
{
    private readonly IIncidentRepository _incidentRepository;
    private readonly IUserRepository _userRepository;

    public IncidentService(IIncidentRepository incidentRepository, IUserRepository userRepository)
    {
        _incidentRepository = incidentRepository ?? throw new ArgumentNullException(nameof(incidentRepository));
        _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
    }

    public async Task<IReadOnlyCollection<IncidentDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var incidents = await _incidentRepository.GetAllAsync(cancellationToken);
        return incidents.Select(IncidentDto.FromEntity).ToList();
    }

    public async Task<IncidentDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        if (id == Guid.Empty)
        {
            throw new ArgumentException("Id do incidente é obrigatório.", nameof(id));
        }

        var incident = await _incidentRepository.GetByIdAsync(id, cancellationToken);
        return incident is null ? null : IncidentDto.FromEntity(incident);
    }

    public async Task<IReadOnlyCollection<IncidentDto>> GetByAreaAsync(string areaCode, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(areaCode))
        {
            throw new ArgumentException("Código de área é obrigatório.", nameof(areaCode));
        }

        var incidents = await _incidentRepository.GetByAreaAsync(areaCode, cancellationToken);
        return incidents.Select(IncidentDto.FromEntity).ToList();
    }

    public async Task<IncidentDto> CreateAsync(CreateIncidentRequest request, CancellationToken cancellationToken = default)
    {
        if (request is null)
        {
            throw new ArgumentNullException(nameof(request));
        }

        var incident = request.ToEntity();

        if (request.AssignedToUserId.HasValue)
        {
            var assignedUser = await _userRepository.GetByIdAsync(request.AssignedToUserId.Value, cancellationToken);
            ValidateAssignableUser(assignedUser, request.AssignedToUserId.Value);
        }

        var createdIncident = await _incidentRepository.AddAsync(incident, cancellationToken);
        return IncidentDto.FromEntity(createdIncident);
    }

    public async Task UpdateAsync(Guid id, UpdateIncidentRequest request, CancellationToken cancellationToken = default)
    {
        if (id == Guid.Empty)
        {
            throw new ArgumentException("Id do incidente é obrigatório.", nameof(id));
        }

        if (request is null)
        {
            throw new ArgumentNullException(nameof(request));
        }

        request.EnsureIsValid();

        var incident = await _incidentRepository.GetByIdAsync(id, cancellationToken);
        if (incident is null)
        {
            throw new KeyNotFoundException($"Incidente '{id}' não encontrado.");
        }

        incident.UpdateDetails(request.Title, request.Description, request.AreaCode);
        incident.ChangeStatus(request.Status);

        if (request.AssignedToUserId.HasValue)
        {
            var assignedUser = await _userRepository.GetByIdAsync(request.AssignedToUserId.Value, cancellationToken);
            ValidateAssignableUser(assignedUser, request.AssignedToUserId.Value);
            incident.AssignTo(request.AssignedToUserId.Value);
        }

        await _incidentRepository.UpdateAsync(incident, cancellationToken);
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        if (id == Guid.Empty)
        {
            throw new ArgumentException("Id do incidente é obrigatório.", nameof(id));
        }

        var incident = await _incidentRepository.GetByIdAsync(id, cancellationToken);
        if (incident is null)
        {
            throw new KeyNotFoundException($"Incidente '{id}' não encontrado.");
        }

        await _incidentRepository.DeleteAsync(id, cancellationToken);
    }

    public async Task AssignAsync(Guid incidentId, AssignIncidentRequest request, CancellationToken cancellationToken = default)
    {
        if (incidentId == Guid.Empty)
        {
            throw new ArgumentException("Id do incidente é obrigatório.", nameof(incidentId));
        }

        if (request is null)
        {
            throw new ArgumentNullException(nameof(request));
        }

        request.EnsureIsValid();

        var incident = await _incidentRepository.GetByIdAsync(incidentId, cancellationToken);
        if (incident is null)
        {
            throw new KeyNotFoundException($"Incidente '{incidentId}' não encontrado.");
        }

        var requester = await _userRepository.GetByIdAsync(request.RequestedByUserId, cancellationToken);
        if (requester is null)
        {
            throw new InvalidOperationException($"Usuário solicitante '{request.RequestedByUserId}' não foi encontrado.");
        }

        if (requester.Role != UserRole.Admin)
        {
            throw new UnauthorizedAccessException("Somente usuários administradores podem atribuir incidentes.");
        }

        var assignee = await _userRepository.GetByIdAsync(request.AssignedUserId, cancellationToken);
        ValidateAssignableUser(assignee, request.AssignedUserId);

        incident.AssignTo(request.AssignedUserId);

        await _incidentRepository.UpdateAsync(incident, cancellationToken);
    }

    private static void ValidateAssignableUser(User? user, Guid userId)
    {
        if (user is null)
        {
            throw new KeyNotFoundException($"Usuário '{userId}' não encontrado.");
        }

        if (user.Role != UserRole.Member)
        {
            throw new InvalidOperationException("Somente integrantes podem receber incidentes.");
        }
    }
}

