namespace PairUpApi.Configuration.Services;

public static class AuthenticationConfiguration
    {
        public static IServiceCollection ConfigureAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateIssuerSigningKey = true,
                    ValidateLifetime = true,
                    ValidIssuer = configuration["Jwt:Issuer"],
                    ValidAudience = configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:SecretKey"]!))
                };
            });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("UserOnly", policy =>
                    policy.Requirements.Add(new RoleRequirement("User"))
                );

                options.AddPolicy("AdminOnly", policy =>
                    policy.Requirements.Add(new RoleRequirement("Admin"))
                );

                options.AddPolicy("UserOrAdmin", policy =>
                    policy.Requirements.Add(new RoleRequirement("User", "Admin"))
                );
            });

            services.AddScoped<IAuthorizationHandler, RoleAuthorizationHandler>();

            return services;
        }
    }
