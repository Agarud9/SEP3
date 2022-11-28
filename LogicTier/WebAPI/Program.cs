using System.Text;
using Application.DAOInterfaces;
using Application.LogicImplementations;
using Application.LogicInterfaces;
using GprcClients.DAOImplementations;
using WebAPI.Utils;
using Grpc.Net.ClientFactory;
using WebAPI.Utils;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Shared.Auth;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
Action<GrpcClientFactoryOptions> grpcOptions = options =>
{
    options.Address = new Uri("http://localhost:6565");
};

// add here the dependency injections
builder.Services.AddGrpcClient<FarmService.FarmServiceClient>(grpcOptions);
builder.Services.AddScoped<IFarmDao, FarmDaoImpl>();
builder.Services.AddScoped<IFarmLogic, FarmLogic>();

builder.Services.AddGrpcClient<OfferService.OfferServiceClient>(grpcOptions);
builder.Services.AddScoped<IOfferDao, OfferDaoImpl>();
builder.Services.AddScoped<IOfferLogic,OfferLogic>();
builder.Services.AddScoped<IOrderLogic, OrderLogic>();
builder.Services.AddScoped<IOrderDao, OrderDaoImpl>();

builder.Services.AddGrpcClient<UserService.UserServiceClient>(grpcOptions);
builder.Services.AddScoped<IAuthDao, AuthDaoImpl>();
builder.Services.AddScoped<IAuthLogic, AuthLogic>();
builder.Services.AddScoped<IUserLogic, UserLogic>();
builder.Services.AddScoped<IUserDao, UserDaoImpl>();

builder.Services.AddGrpcClient<OrderService.OrderServiceClient>(grpcOptions);

builder.Services.AddGrpcClient<CartOfferService.CartOfferServiceClient>(grpcOptions);
builder.Services.AddScoped<ICartDao, CartDaoImpl>();
builder.Services.AddScoped<ICartLogic, CartLogic>();

builder.Services.AddTransient<IImageDao, ImageResource>();
builder.Services.AddTransient<ImageResource>();
builder.Services.AddTransient<IFarmIconDao, FarmIconResource>();

builder.Services.AddHttpContextAccessor();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidAudience = builder.Configuration["Jwt:Audience"],
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
    };
});

AuthorizationPolicies.AddPolicies(builder.Services);




var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options => options.EnableTryItOutByDefault());
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.UseCors(x => x
    .AllowAnyMethod()
    .AllowAnyHeader()
    .SetIsOriginAllowed(origin => true) // allow any origin
    .AllowCredentials());

app.Run();