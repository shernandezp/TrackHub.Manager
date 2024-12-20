﻿using Common.Infrastructure;

namespace TrackHub.Manager.Infrastructure.ManagerDB.Entities;
public sealed class Operator(string name, string? description, string? phoneNumber, string? emailAddress, string? address, string? contactName, int protocolType, Guid accountId) : BaseAuditableEntity
{
    private Account? _account;
    public Guid OperatorId { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = name;
    public string? Description { get; set; } = description;
    public string? PhoneNumber { get; set; } = phoneNumber;
    public string? EmailAddress { get; set; } = emailAddress;
    public string? Address { get; set; } = address;
    public string? ContactName { get; set; } = contactName;
    public int ProtocolType { get; set; } = protocolType;
    public Guid AccountId { get; set; } = accountId;
    public Credential? Credential { get; set; }
    public ICollection<Device> Devices { get; } = [];

    public Account Account
    {
        get => _account ?? throw new InvalidOperationException("Account is not loaded");
        set => _account = value;
    }
}
