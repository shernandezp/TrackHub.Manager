﻿using Common.Infrastructure;

namespace TrackHub.Manager.Infrastructure.ManagerDB.Entities;

public class Account(string name, string? description, short type, bool active) : BaseAuditableEntity
{
    public Guid AccountId { get; private set; } = Guid.NewGuid();

    public string Name { get; set; } = name;

    public string? Description { get; set; } = description;

    public short Type { get; set; } = type;

    public bool Active { get; set; } = active;

    public IEnumerable<User> Users { get; } = [];
    public IEnumerable<Group> Groups { get; } = [];
    public IEnumerable<Operator> Operators { get; } = [];
    public AccountSettings? AccountSettings { get; set; }

}
