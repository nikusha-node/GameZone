var builder = WebApplication.CreateBuilder(args);

// Register controllers (our API endpoints live in the Controllers folder)
builder.Services.AddControllers();

// Swagger: generates API documentation and a test page
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Show Swagger only while developing
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();