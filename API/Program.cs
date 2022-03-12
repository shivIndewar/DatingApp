var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplicationServices(builder.Configuration);
builder.Services.AddControllers();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "WebAPIv5", Version = "v1" });
});
builder.Services.AddCors();
builder.Services.AddIdentityServices(builder.Configuration);
builder.Services.AddSignalR();


// Configure the HTTP request pipeline

var app = builder.Build();

// app.UseMiddleware<ExceptionMiddleware>();
            
            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "WebAPIv5 v1"));
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseCors(x => x.AllowAnyHeader()
                              .AllowAnyMethod()
                              .AllowCredentials()
                              .WithOrigins("https://localhost:4200"));
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseDefaultFiles();
            app.UseStaticFiles();

                app.MapControllers();
                app.MapHub<PresenceHub>("hubs/presence");
                app.MapHub<MessageHub>("hubs/message");
                app.MapFallbackToController("Index", "Fallback");

            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior",true);
            using var scope = app.Services.CreateScope();
            var Services = scope.ServiceProvider;

            try
            {
                var context = Services.GetRequiredService<DataContext>();
                var userManager = Services.GetRequiredService<UserManager<AppUser>>();
                var roleManager = Services.GetRequiredService<RoleManager<AppRole>>();

                await context.Database.MigrateAsync();
                await Seed.SeedUsrs(userManager, roleManager);
            }
            catch (Exception ex)
            {
                var logger = Services.GetRequiredService<ILogger<Program>>();
                logger.LogError(ex, "An error occured during migration");           
            }

             await app.RunAsync();

