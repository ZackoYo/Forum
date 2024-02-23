using System.Reflection;
using Forum.Data;
using Forum.Data.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Forum.Data.Seeding;
using Forum.Data.Contracts.Repositories;
using Forum.Data.Contracts;
using Forum.Data.Repositories;
using Forum.Domain.Application;
using Forum.Infrastructure.Contracts.ExternalServices;
using Forum.Infrastructure.Contracts.InternalServices;
using Forum.Infrastructure.ExternalServices.MessagingService;
using Forum.Infrastructure.InternalServices;
using Forum.Infrastructure.Mapping;

var builder = WebApplication.CreateBuilder(args);

//builder.Configuration
//	.SetBasePath(Directory.GetCurrentDirectory())
//	.AddJsonFile("appsettings.json", false, true)
//	.AddEnvironmentVariables();

// Add services to the container.

builder.Services.AddDbContext<ApplicationDbContext>(
	options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddDefaultIdentity<ApplicationUser>()
	.AddRoles<ApplicationRole>().AddEntityFrameworkStores<ApplicationDbContext>();

/* If decide to simplify the password rules
builder.Services.AddDefaultIdentity<ApplicationUser>(IdentityOptionsProvider.GetIdentityOptions)
	.AddRoles<ApplicationRole>().AddEntityFrameworkStores<ApplicationDbContext>(); */

builder.Services.Configure<CookiePolicyOptions>(
	options =>
	{
		options.CheckConsentNeeded = context => true;
		options.MinimumSameSitePolicy = SameSiteMode.None;
	});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton(builder.Configuration);

// Data repositories
builder.Services.AddScoped(typeof(IDeletableEntityRepository<>), typeof(EfDeletableEntityRepository<>));
builder.Services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>));
builder.Services.AddScoped<IDbQueryRunner, DbQueryRunner>();

// Application services
builder.Services.AddTransient<IEmailSender, NullMessageSender>();
builder.Services.AddTransient<ISettingsService, SettingsService>();

var app = builder.Build();

// Seed data on application startup

using (var serviceScope = app.Services.CreateScope())
{
	var dbContext = serviceScope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
	dbContext.Database.Migrate();
	new ApplicationDbContextSeeder().SeedAsync(dbContext, serviceScope.ServiceProvider).GetAwaiter().GetResult();
}

AutoMapperConfig.RegisterMappings(typeof(ErrorViewModel).GetTypeInfo().Assembly);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseAuthorization();


app.MapControllers();

app.Run();