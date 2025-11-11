using System;

namespace UrbanManagement.Application.DTOs;

public record AssignIncidentRequest(Guid AssignedUserId, Guid RequestedByUserId)
{
    public void EnsureIsValid()
    {
        if (AssignedUserId == Guid.Empty)
        {
            throw new ArgumentException("Usuário responsável é obrigatório.", nameof(AssignedUserId));
        }

        if (RequestedByUserId == Guid.Empty)
        {
            throw new ArgumentException("Usuário solicitante é obrigatório.", nameof(RequestedByUserId));
        }
    }
}

