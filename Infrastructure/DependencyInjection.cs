using Application.Common.Interfaces;
using Infrastructure.Authentication;
using Infrastructure.Cache;
using Infrastructure.Persistence;
using Infrastructure.Persistence.Repositories;
using Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

namespace Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureServices(
            this IServiceCollection services,
            IConfiguration configuration)
        {

            var connectionString = configuration.GetConnectionString("DefaultConnection");

            var redisHost = configuration["RedisSettings:Host"];
            var redisUser = configuration["RedisSettings:User"];
            var redisPassword = configuration["RedisSettings:Password"];
            var redisSsl = configuration.GetValue<bool>("RedisSettings:Ssl", false);

            services.Configure<JwtSettings>(configuration.GetSection("JwtSettings"));

            services.AddSingleton<IJwtTokenGenerator, JwtTokenGenerator>();

            services.AddDbContext<ApplicationDbContext>(options => 
                options.UseNpgsql(connectionString));

            services.AddStackExchangeRedisCache(options =>
            {
                var configOptions = new ConfigurationOptions
                {
                    EndPoints = { redisHost! },
                    Ssl = redisSsl,
                    User = redisUser,
                    Password = redisPassword,
                    AbortOnConnectFail = false,
                    ConnectTimeout = 5000
                };

                options.ConfigurationOptions = configOptions;
            });

            services.AddScoped<ICacheService, RedisCacheService>();
            services.AddScoped<ILinkRepository, LinkRepository>();
            services.AddScoped<IStatsRepository, StatsRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddSingleton<IPasswordHasher, PasswordHasherService>();

            return services;

        }
    }
}
