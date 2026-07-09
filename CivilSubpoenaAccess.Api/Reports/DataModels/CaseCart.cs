using CivilSubpoenaAccess.Api.Database.Entities;

namespace CivilSubpoenaAccess.Api.Reports.DataModels;


public class CaseCart
{
    public required Case Case { get; init; }

    public required Cart Cart { get; init; }
}