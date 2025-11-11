using System;
using UrbanManagement.Domain.Enums;

namespace UrbanManagement.Application.DTOs;

public record UpdateUserRequest(string Name, string Email, UserRole Role)
{
    public void EnsureIsValid()
    {
        if (string.IsNullOrWhiteSpace(Name))
        {
            throw new ArgumentException("Nome é obrigatório.", nameof(Name));
        }

        if (string.IsNullOrWhiteSpace(Email))
        {
            throw new ArgumentException("E-mail é obrigatório.", nameof(Email));
        }

        if (!Email.Contains('@', StringComparison.Ordinal))
        {
            throw new ArgumentException("E-mail inválido.", nameof(Email));
        }
    }
}

