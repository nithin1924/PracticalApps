using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore; // To use UseSqlite.
using Microsoft.Extensions.DependencyInjection; // To use IServiceCollection.

namespace Northwind.EntityModels
{
    public static class NorthwindContextExtensions
    {
        public static IServiceCollection AddNorthwindContext(this IServiceCollection services, // The type to extend.
                                                             string relativePath = "..",
                                                             string databaseName = "Northwind.db")
        {
            string path = Path.Combine(relativePath, databaseName);
            path = Path.GetFullPath(path);
            NorthwindContextLogger.WriteLine($"Database path: {path}");
            if (!File.Exists(path))
            {
                throw new FileNotFoundException(
                message: $"{path} not found.", fileName: path);
            }

            services.AddDbContext<NorthwindContext>(options =>
            {
                // Data Source is the modern equivalent of Filename.
                options.UseSqlite($"Data Source={path}");
                options.LogTo(NorthwindContextLogger.WriteLine,
                new[] { Microsoft.EntityFrameworkCore.Diagnostics.RelationalEventId.CommandExecuting });
            },
            // Register with a transient lifetime to avoid concurrency 
            // issues in Blazor server-side projects.
            contextLifetime: ServiceLifetime.Transient,
            optionsLifetime: ServiceLifetime.Transient);

            return services;
        }
    }
}
