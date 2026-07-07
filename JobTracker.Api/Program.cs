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
    app.UseSwagger();   // Serves the generated JSON document
    app.UseSwaggerUI(); // Serves the web-based Swagger UI
}

app.UseHttpsRedirection();

// app.UseCors("AllowLocalDev");
app.UseCors(policy => policy.AllowAnyHeader()
                        .AllowAnyMethod()
                        .SetIsOriginAllowed(origin => true)
                        .AllowCredentials());

// app.UseAuthorization();

app.MapControllers();

app.Run();
