using API_TEST.Data;
using API_TEST.Mapping;
using API_TEST.Repositories;
using API_TEST.Services;
using Microsoft.EntityFrameworkCore;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Fluent Validation
//builder.Services.AddValidatorsFromAssemblyContaining<RecordingValidator>();
//builder.Services.AddValidatorsFromAssemblyContaining<ScheduleValidator>();

//Handle CORS
builder.Services.AddCors(option => option.AddDefaultPolicy(policy =>
    policy.AllowAnyOrigin()
    .AllowAnyHeader()
    .AllowAnyMethod()));

//Connect DB
IConfigurationRoot cf = new ConfigurationBuilder().SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true).Build();

builder.Services.AddDbContext<MyDBContext>(opt => opt.UseSqlServer(cf.GetConnectionString("cnn")));

//Mapping
builder.Services.AddAutoMapper(typeof(Mappers));

//DI Container
builder.Services.AddScoped<IRepositoryWrapper, RepositoryWrapper>();
builder.Services.AddScoped<IMediaServices, MediaServices>();
builder.Services.AddScoped<ITimeSlotServices, TimeSlotServices>();
builder.Services.AddScoped<IScheduleServices, ScheduleServices>();


WebApplication app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();


//Static File
app.UseStaticFiles();

app.MapControllers();

app.Run();
