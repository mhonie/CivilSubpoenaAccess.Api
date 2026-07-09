namespace CivilSubpoenaAccess.Api.Database.Entities;


public class User
{
    public required string UserName { get; init; }

    public required string Password { get; init; }

    public required string Pin { get; init; }

    public required string UserType { get; init; }

    public required string UserStatus { get; init; }

    public DateTime? ExpirationDate { get; init; }

    public DateTime? EntryDate { get; init; } 

    public string? AttorneyId { get; init; }

    public string? ProSeNumber { get; init; }

    public string? CityLawIndicator { get; init; }

    public int? ParticipantId { get; init; }
}