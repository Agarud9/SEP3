using Application.LogicImplementations;
using Application.LogicInterfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IFarmLogic, FarmLogic>();
builder.Services.AddScoped<IOfferLogic,OfferLogic>();
builder.Services.AddGrpcClient<FarmService.FarmServiceClient>(o =>
{
    o.Address = new Uri("http://localhost:8084");
});
builder.Services.AddGrpcClient<OfferService.OfferServiceClient>(o =>
{
    o.Address = new Uri("http://localhost:8084");
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
app.UseCors(x => x
    .AllowAnyMethod()
    .AllowAnyHeader()
    .SetIsOriginAllowed(origin => true) // allow any origin
    .AllowCredentials());

app.Run();