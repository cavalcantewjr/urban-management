using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using UrbanManagement.Application.DTOs;
using UrbanManagement.Application.Interfaces;

namespace UrbanManagement.Application.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
    }

    public async Task<IReadOnlyCollection<UserDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var users = await _userRepository.GetAllAsync(cancellationToken);
        return users.Select(UserDto.FromEntity).ToList();
    }

    public async Task<UserDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        if (id == Guid.Empty)
        {
            throw new ArgumentException("Id do usuário é obrigatório.", nameof(id));
        }

        var user = await _userRepository.GetByIdAsync(id, cancellationToken);
        return user is null ? null : UserDto.FromEntity(user);
    }

    public async Task<UserDto> CreateAsync(CreateUserRequest request, CancellationToken cancellationToken = default)
    {
        if (request is null)
        {
            throw new ArgumentNullException(nameof(request));
        }

        var existingUser = await _userRepository.GetByEmailAsync(request.Email, cancellationToken);
        if (existingUser is not null)
        {
            throw new InvalidOperationException($"Já existe um usuário registrado com o e-mail '{request.Email}'.");
        }

        var user = request.ToEntity();
        var created = await _userRepository.AddAsync(user, cancellationToken);
        return UserDto.FromEntity(created);
    }

    public async Task UpdateAsync(Guid id, UpdateUserRequest request, CancellationToken cancellationToken = default)
    {
        if (id == Guid.Empty)
        {
            throw new ArgumentException("Id do usuário é obrigatório.", nameof(id));
        }

        if (request is null)
        {
            throw new ArgumentNullException(nameof(request));
        }

        request.EnsureIsValid();

        var user = await _userRepository.GetByIdAsync(id, cancellationToken);
        if (user is null)
        {
            throw new KeyNotFoundException($"Usuário '{id}' não encontrado.");
        }

        var existingWithEmail = await _userRepository.GetByEmailAsync(request.Email, cancellationToken);
        if (existingWithEmail is not null && existingWithEmail.Id != id)
        {
            throw new InvalidOperationException($"Já existe um usuário registrado com o e-mail '{request.Email}'.");
        }

        user.Update(request.Name, request.Email, request.Role);
        await _userRepository.UpdateAsync(user, cancellationToken);
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        if (id == Guid.Empty)
        {
            throw new ArgumentException("Id do usuário é obrigatório.", nameof(id));
        }

        var user = await _userRepository.GetByIdAsync(id, cancellationToken);
        if (user is null)
        {
            throw new KeyNotFoundException($"Usuário '{id}' não encontrado.");
        }

        await _userRepository.DeleteAsync(id, cancellationToken);
    }
}

