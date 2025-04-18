using System;
using Microsoft.EntityFrameworkCore;

namespace R2ETien.MVC.Data;

public static class DataExtension
{
    public static void MigrateDb(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<Context>();
        dbContext.Database.Migrate();
        Context.SeedData(dbContext);
    }
}
