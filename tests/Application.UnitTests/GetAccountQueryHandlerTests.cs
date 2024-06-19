using TrackHub.Manager.Application.Accounts.Queries.Get;
using TrackHub.Manager.Domain.Interfaces;

namespace Application.UnitTests;

[TestFixture]
public class GetAccountQueryHandlerTests
{
    private Mock<IAccountReader> _mockReader;
    private GetAccountQueryHandler _handler;

    [SetUp]
    public void Setup()
    {
        _mockReader = new Mock<IAccountReader>();
        _handler = new GetAccountQueryHandler(_mockReader.Object);
    }

    [Test]
    public async Task Handle_WhenAccountExists_ReturnsAccountVm()
    {
        // Arrange
        var accountId = Guid.NewGuid();
        var accountVm = new AccountVm { AccountId = accountId, Name = "Customer" };
        _mockReader.Setup(reader => reader.GetAccountAsync(accountId, CancellationToken.None))
            .ReturnsAsync(accountVm);

        var query = new GetAccountQuery(accountId);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        accountVm.Should().BeEquivalentTo(result);
    }
}
