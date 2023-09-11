using Microsoft.EntityFrameworkCore;
using projects_api.Data;
using projects_api.Services;

string MyAngularFrontend = "MyAngularFrontend";

var builder = WebApplication.CreateBuilder(args);

var databaseConnectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ProjectDbContext>(options => options.UseSqlServer(databaseConnectionString));

builder.Services.AddCors(options =>
{
    options.AddPolicy(MyAngularFrontend, policy =>
    {
        policy.AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader();
    });
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<ProjectService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors(MyAngularFrontend);

app.UseAuthorization();

app.MapControllers();

app.Run();