using System;

namespace UrbanManagement.Domain.Entities;

/// <summary>
/// Entidade de domínio que representa um incidente urbano.
/// </summary>
public class Incident
{
    public Guid Id { get; private set; }
    public string Title { get; private set; }
    public string Description { get; private set; }
    public string AreaCode { get; private set; }
    public string Status { get; private set; }
    public Guid? AssignedToUserId { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }

    private Incident()
    {
        Title = string.Empty;
        Description = string.Empty;
        AreaCode = string.Empty;
        Status = string.Empty;
    }

    public Incident(string title, string description, string areaCode, string status = "registered", Guid? assignedToUserId = null)
    {
        if (string.IsNullOrWhiteSpace(title))
        {
            throw new ArgumentException("Título do incidente é obrigatório.", nameof(title));
        }

        if (string.IsNullOrWhiteSpace(description))
        {
            throw new ArgumentException("Descrição do incidente é obrigatória.", nameof(description));
        }

        if (string.IsNullOrWhiteSpace(areaCode))
        {
            throw new ArgumentException("Código de área é obrigatório.", nameof(areaCode));
        }

        if (string.IsNullOrWhiteSpace(status))
        {
            throw new ArgumentException("Status do incidente é obrigatório.", nameof(status));
        }

        if (assignedToUserId.HasValue && assignedToUserId.Value == Guid.Empty)
        {
            throw new ArgumentException("Identificador do usuário atribuído é inválido.", nameof(assignedToUserId));
        }

        Id = Guid.NewGuid();
        Title = title;
        Description = description;
        AreaCode = areaCode;
        Status = status;
        AssignedToUserId = assignedToUserId;
        CreatedAt = DateTime.UtcNow;
    }

    public Incident(Guid id, string title, string description, string areaCode, string status, DateTime createdAt, DateTime? updatedAt = null, Guid? assignedToUserId = null)
        : this(title, description, areaCode, status, assignedToUserId)
    {
        Id = id == default ? Guid.NewGuid() : id;
        CreatedAt = createdAt == default ? DateTime.UtcNow : createdAt;
        UpdatedAt = updatedAt;
    }

    public void UpdateDetails(string title, string description, string areaCode)
    {
        if (string.IsNullOrWhiteSpace(title))
        {
            throw new ArgumentException("Título do incidente é obrigatório.", nameof(title));
        }

        if (string.IsNullOrWhiteSpace(description))
        {
            throw new ArgumentException("Descrição do incidente é obrigatória.", nameof(description));
        }

        if (string.IsNullOrWhiteSpace(areaCode))
        {
            throw new ArgumentException("Código de área é obrigatório.", nameof(areaCode));
        }

        Title = title;
        Description = description;
        AreaCode = areaCode;
        Touch();
    }

    public void ChangeStatus(string status)
    {
        if (string.IsNullOrWhiteSpace(status))
        {
            throw new ArgumentException("Status do incidente é obrigatório.", nameof(status));
        }

        Status = status;
        Touch();
    }

    public void AssignTo(Guid userId)
    {
        if (userId == Guid.Empty)
        {
            throw new ArgumentException("Usuário atribuído é obrigatório.", nameof(userId));
        }

        AssignedToUserId = userId;
        Touch();
    }

    public void RemoveAssignment()
    {
        if (AssignedToUserId.HasValue)
        {
            AssignedToUserId = null;
            Touch();
        }
    }

    private void Touch()
    {
        UpdatedAt = DateTime.UtcNow;
    }
}
