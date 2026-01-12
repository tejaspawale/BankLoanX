// using Swashbuckle.AspNetCore.SwaggerGen;

// var builder = WebApplication.CreateBuilder(args);

// // Add services
// builder.Services.AddControllers();
// builder.Services.AddEndpointsApiExplorer(); // Needed for Swagger
// builder.Services.AddSwaggerGen();            // Needed for Swagger

// var app = builder.Build();

// // Enable Swagger
// app.UseSwagger();
// app.UseSwaggerUI();

// // Middleware
// app.UseAuthorization();

// // Map controllers
// app.MapControllers();

// app.Run();


var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// 👉 ADD CORS HERE
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReact",
        policy =>
        {
            policy
                .WithOrigins("http://localhost:5173")
                .AllowAnyHeader()
                .AllowAnyMethod();
        });
});

var app = builder.Build();

// Enable Swagger
app.UseSwagger();
app.UseSwaggerUI();

// 👉 USE CORS HERE (IMPORTANT ORDER)
app.UseCors("AllowReact");

app.UseAuthorization();
app.MapControllers();

app.Run();
