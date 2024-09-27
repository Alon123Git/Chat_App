using Microsoft.EntityFrameworkCore;
using SERVER_SIDE.DBContext;
using SERVER_SIDE.Services;
using SERVER_SIDE.SignalRHandle_Hubs;
//using SERVER_HANDLE_LOGIC_AND_signalR.SignalRHandle_Hubs;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers(); // Add services to the container.

// Register API services
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<MembersService>();
builder.Services.AddScoped<MessagesService>();
        
builder.Services.AddSignalR(); // Register SignalR services


builder.Services.AddDbContext<DataBaseContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("AppConnection"))); // SQL connection pipeline   
//builder.Services.AddSingleton<IDatabaseService, DatabaseServiceImpl>();



// Add cors
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins", builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});

var app = builder.Build();

app.MapHub<ChatHub>("/Chat"); // Configure SignalR endpoint (   SignalR להגדיר את נקודת הקצה של)

app.UseCors("AllowAllOrigins"); // Apply the CORS policy

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseDeveloperExceptionPage(); // This will give detailed error pages
}

app.UseHttpsRedirection();
app.UseAuthorization();
//app.MapHub<ChatHub>("/chathub"); // Map the SignalR hub
app.MapControllers(); // Map API controllers
app.Run();