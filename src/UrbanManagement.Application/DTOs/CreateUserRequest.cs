using System;
using UrbanManagement.Domain.Entities;
using UrbanManagement.Domain.Enums;

namespace UrbanManagement.Application.DTOs;

public record CreateUserRequest(string Name, string Email, UserRole Role)
{
    public User ToEntity()
    {
        return new User(
            name: Name ?? throw new ArgumentNullException(nameof(Name)),
            email: Email ?? throw new ArgumentNullException(nameof(Email)),
            role: Role);
    }
}

