using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using UrbanManagement.Application.Interfaces;
using UrbanManagement.Domain.Entities;
using UrbanManagement.Domain.Enums;

namespace UrbanManagement.Infrastructure.Repositories;

public class InMemoryUserRepository : IUserRepository
{
    private readonly ConcurrentDictionary<Guid, User> _storage = new();

    public InMemoryUserRepository()
    {
        SeedAdminUser();
    }

    public Task<IReadOnlyCollection<User>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var users = _storage.Values
            .Select(Clone)
            .OrderBy(user => user.Name)
            .ToList()
            .AsReadOnly();

        return Task.FromResult<IReadOnlyCollection<User>>(users);
    }

    public Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        _storage.TryGetValue(id, out var user);
        return Task.FromResult(user is null ? null : Clone(user));
    }

    public Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(email))
        {
            return Task.FromResult<User?>(null);
        }

        var normalized = email.ToLowerInvariant();
        var user = _storage.Values.SingleOrDefault(u => string.Equals(u.Email, normalized, StringComparison.OrdinalIgnoreCase));
        return Task.FromResult(user is null ? null : Clone(user));
    }

    public Task<User> AddAsync(User user, CancellationToken cancellationToken = default)
    {
        if (user is null)
        {
            throw new ArgumentNullException(nameof(user));
        }

        var snapshot = Clone(user);
        _storage[snapshot.Id] = snapshot;
        return Task.FromResult(Clone(snapshot));
    }

    public Task UpdateAsync(User user, CancellationToken cancellationToken = default)
    {
        if (user is null)
        {
            throw new ArgumentNullException(nameof(user));
        }

        _storage[user.Id] = Clone(user);
        return Task.CompletedTask;
    }

    public Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        _storage.TryRemove(id, out _);
        return Task.CompletedTask;
    }

    private static User Clone(User user)
    {
        return new User(user.Id, user.Name, user.Email, user.Role, user.CreatedAt, user.UpdatedAt);
    }

    private void SeedAdminUser()
    {
        var admin = new User("Administrador Padrão", "admin@urban.local", UserRole.Admin);
        _storage[admin.Id] = admin;
    }
}

