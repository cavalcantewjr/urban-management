using System;
using UrbanManagement.Domain.Enums;

namespace UrbanManagement.Domain.Entities;

/// <summary>
/// Entidade de domínio que representa um usuário da plataforma.
/// </summary>
public class User
{
    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public string Email { get; private set; }
    public UserRole Role { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }

    private User()
    {
        Name = string.Empty;
        Email = string.Empty;
    }

    public User(string name, string email, UserRole role)
    {
        ValidateName(name);
        ValidateEmail(email);

        Id = Guid.NewGuid();
        Name = name;
        Email = email.ToLowerInvariant();
        Role = role;
        CreatedAt = DateTime.UtcNow;
    }

    public User(Guid id, string name, string email, UserRole role, DateTime createdAt, DateTime? updatedAt = null)
        : this(name, email, role)
    {
        Id = id == default ? Guid.NewGuid() : id;
        CreatedAt = createdAt == default ? DateTime.UtcNow : createdAt;
        UpdatedAt = updatedAt;
    }

    public void Update(string name, string email, UserRole role)
    {
        ValidateName(name);
        ValidateEmail(email);

        Name = name;
        Email = email.ToLowerInvariant();
        Role = role;
        Touch();
    }

    private void Touch()
    {
        UpdatedAt = DateTime.UtcNow;
    }

    private static void ValidateName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Nome é obrigatório.", nameof(name));
        }
    }

    private static void ValidateEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
        {
            throw new ArgumentException("E-mail é obrigatório.", nameof(email));
        }

        if (!email.Contains('@', StringComparison.Ordinal))
        {
            throw new ArgumentException("E-mail inválido.", nameof(email));
        }
    }
}

