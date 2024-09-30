using Domain.Service.Class;
using Domain.Service.Interfaces;
using Microsoft.EntityFrameworkCore;
using ServerSide.Database;
using ServerSide.Repository.Repositorys;
using ServerSide.Service;
using ServerSide.Service.IServices;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlite($"Data Source=C:\\Users\\pc\\Desktop\\MVC.db"));
builder.Services.AddScoped<UserRepository>();
builder.Services.AddScoped<IAuthService,AuthService>();
builder.Services.AddScoped<IEncreption, Encreption>();
builder.Services.AddScoped(typeof(IJwtService<>), typeof(JwtService<>));
builder.Services.AddHttpContextAccessor();
builder.Services.AddCors(options =>
                        options.AddDefaultPolicy(policy =>
                        {
                            
                            policy.WithOrigins("https://localhost:7080")
                                .AllowAnyMethod()
                                .AllowAnyHeader()
                                .AllowCredentials()
                                ;
                        })
                    );

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
