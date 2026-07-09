global using CartFilter =
    System.Linq.Expressions.Expression<
        System.Func<CivilSubpoenaAccess.Api.Database.Entities.Cart, bool>>;

global using CaseFilter =
    System.Linq.Expressions.Expression<
        System.Func<CivilSubpoenaAccess.Api.Database.Entities.Case, bool>>;

global using CaseCartFilter =
    System.Linq.Expressions.Expression<
        System.Func<CivilSubpoenaAccess.Api.Reports.DataModels.CaseCart, bool>>;

global using UserFilter =
    System.Linq.Expressions.Expression<
        System.Func<CivilSubpoenaAccess.Api.Database.Entities.User, bool>>;

global using ILogger = Serilog.ILogger;
