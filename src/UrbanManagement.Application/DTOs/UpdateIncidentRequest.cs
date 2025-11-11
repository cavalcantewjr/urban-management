using System;

namespace UrbanManagement.Application.DTOs;

public record UpdateIncidentRequest(string Title, string Description, string AreaCode, string Status)
{
    public void EnsureIsValid()
    {
        if (string.IsNullOrWhiteSpace(Title))
        {
            throw new ArgumentException("Título é obrigatório.", nameof(Title));
        }

        if (string.IsNullOrWhiteSpace(Description))
        {
            throw new ArgumentException("Descrição é obrigatória.", nameof(Description));
        }

        if (string.IsNullOrWhiteSpace(AreaCode))
        {
            throw new ArgumentException("Código de área é obrigatório.", nameof(AreaCode));
        }

        if (string.IsNullOrWhiteSpace(Status))
        {
            throw new ArgumentException("Status é obrigatório.", nameof(Status));
        }
    }
}

