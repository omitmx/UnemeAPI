using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using UnemeAPI.Models.DBs;
using UnemeAPI.Utils.Jwt;
using UnemeAPI.Utils.Servicios;

string MiCors = "MiCors";
var builder = WebApplication.CreateBuilder(args);
//cors
builder.Services.AddCors(op =>
{
    op.AddPolicy(name: MiCors, builder =>
    {
        builder.WithOrigins("*");
    });
});

// Add services to the container.

builder.Services.AddControllers();

builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
    options.ForwardedHeaders =
    ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
});
//JWT secreto
var appSettSection = builder.Configuration.GetSection("AppSettings");
builder.Services.Configure<AppSettings>(appSettSection);
//JWT
var appSettings = appSettSection.Get<AppSettings>();
var llave = Encoding.ASCII.GetBytes(appSettings.Secreto??"");

//conexion DB PostGres
var pgCnn = builder.Configuration.GetSection("PGCnn");
builder.Services.Configure<PGCnn>(pgCnn);



// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddHttpsRedirection(options =>
{
    options.RedirectStatusCode = StatusCodes.Status307TemporaryRedirect;
    options.HttpsPort = 5001;
});
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "WsUneme", Version = "v1" });
    // Include 'SecurityScheme' to use JWT Authentication
    var jwtSecurityScheme = new OpenApiSecurityScheme
    {
        Scheme = "bearer",
        BearerFormat = "JWT",
        Name = "JWT Authentication",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Description = "Ingresar token de sesion!",

        Reference = new OpenApiReference
        {
            Id = JwtBearerDefaults.AuthenticationScheme,
            Type = ReferenceType.SecurityScheme
        }
    };

    c.AddSecurityDefinition(jwtSecurityScheme.Reference.Id, jwtSecurityScheme);

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
                                            {
                                                { jwtSecurityScheme, Array.Empty<string>() }
                                            });
});

builder.Services.AddAuthentication(d =>
{
    d.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    d.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
             .AddJwtBearer(d =>
             {
                 d.RequireHttpsMetadata = false;
                 d.SaveToken = true;
                 d.TokenValidationParameters = new TokenValidationParameters
                 {
                     ValidateIssuerSigningKey = true,
                     IssuerSigningKey = new SymmetricSecurityKey(llave),
                     ValidateIssuer = false,
                     ValidateAudience = false
                 };
             });

//singletons
//servicios
builder.Services.AddScoped<IUsuServicio, cUsuServicio>(); 


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors(MiCors);
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
