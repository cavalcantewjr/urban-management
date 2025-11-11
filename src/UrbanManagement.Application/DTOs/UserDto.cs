using System;
using UrbanManagement.Domain.Entities;
using UrbanManagement.Domain.Enums;

namespace UrbanManagement.Application.DTOs;

public record UserDto(Guid Id, string Name, string Email, UserRole Role, DateTime CreatedAt, DateTime? UpdatedAt)
{
    public static UserDto FromEntity(User user)
    {
        if (user is null)
        {
            throw new ArgumentNullException(nameof(user));
        }

        return new UserDto(user.Id, user.Name, user.Email, user.Role, user.CreatedAt, user.UpdatedAt);
    }
}

