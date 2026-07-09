using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
// builder.Services.AddOpenApi();

builder.Services.AddSwaggerGen();   // Generates the Swagger document

// DbContext to facilitate connection to Postgres
builder.Services.AddDbContext<JobDbContext>(options => 
    options.UseNpgsql(builder.Configuration.GetConnectionString("JobDbContext")));

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowLocalDev", policy =>
    {
        policy.WithOrigins("http://localhost:5104", "https://localhost:7099")
        .AllowAnyHeader()
        .AllowAnyMethod();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    // app.MapOpenApi();
    app.UseHttpsRedirection(); // ONLY redirect to HTTPS when running locally
    // app.UseSwagger();   // Serves the generated JSON document
    // app.UseSwaggerUI(); // Serves the web-based Swagger UI
}

app.UseSwagger();   // Serves the generated JSON document
app.UseSwaggerUI();

// Uncheck the following line if it fails to run locally
// app.UseHttpsRedirection();

// app.UseCors("AllowLocalDev");
app.UseCors(policy => policy.AllowAnyHeader()
                        .AllowAnyMethod()
                        .SetIsOriginAllowed(origin => true)
                        .AllowCredentials());

app.UseAuthorization(); // comment out if there are errors running

app.MapControllers();

// Automatically apply database migrations on startup in Render
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<JobDbContext>();
        context.Database.Migrate();
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while migrating the database.");
    }
}

app.Run();
