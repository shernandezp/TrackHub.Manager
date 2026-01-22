// Copyright (c) 2025 Sergio Hernandez. All rights reserved.

using TrackHub.Manager.Application.Accounts.Queries.GetAll;
using TrackHub.Manager.Domain.Interfaces;

namespace Application.UnitTests.Accounts;

[TestFixture]
public class GetAccountsQueryTests
{
    private Mock<IAccountReader> _readerMock;
    private GetAccountsQueryHandler _handler;

    [SetUp]
    public void Setup()
    {
        _readerMock = new Mock<IAccountReader>();
        _handler = new GetAccountsQueryHandler(_readerMock.Object);
    }

    [Test]
    public async Task Handle_ReturnsAccounts()
    {
        var list = new List<AccountVm> { new(Guid.NewGuid(), "A", null, default, 1, true, DateTimeOffset.UtcNow) };
        _readerMock.Setup(r => r.GetAccountsAsync(CancellationToken.None)).ReturnsAsync(list);

        var result = await _handler.Handle(new GetAccountsQuery(), CancellationToken.None);

        Assert.That(result, Is.EqualTo(list));
    }
}
