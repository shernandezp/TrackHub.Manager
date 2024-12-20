﻿using Common.Infrastructure;

namespace TrackHub.Manager.Infrastructure.ManagerDB.Entities;
public sealed class Group(string name, string description, bool active, Guid accountId) : BaseAuditableEntity
{
    private Account? _account;

    public long GroupId { get; set; }
    public string Name { get; set; } = name;
    public string Description { get; set; } = description;
    public bool Active { get; set; } = active;
    public Guid AccountId { get; set; } = accountId;
    public ICollection<User> Users { get; } = [];
    public ICollection<Transporter> Transporters { get; } = [];

    public Account Account
    {
        get => _account ?? throw new InvalidOperationException("Account is not loaded");
        set => _account = value;
    }
}
