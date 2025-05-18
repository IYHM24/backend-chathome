using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using WebSockets.Hubs;
using static System.Net.WebRequestMethods;

var builder = WebApplication.CreateBuilder(args);
// Obtener y procesar las credenciales de Firebase
string firebaseConfig;
if (builder.Environment.IsDevelopment())
{
    // En desarrollo, usar la configuración del appsettings
    firebaseConfig = System.Text.Json.JsonSerializer.Serialize(
        builder.Configuration.GetSection("firebase:config").Get<Dictionary<string, object>>());
}
else
{
    // En producción, decodificar el base64
    var base64Credentials = Environment.GetEnvironmentVariable("FIREBASE_CREDENTIALS");
    if (string.IsNullOrEmpty(base64Credentials))
        throw new InvalidOperationException("Firebase credentials not found in environment variables");

    firebaseConfig = System.Text.Encoding.UTF8.GetString(
        Convert.FromBase64String(base64Credentials));
}

if (string.IsNullOrEmpty(firebaseConfig))
    throw new InvalidOperationException("Firebase credentials could not be processed");



// Add services to the container.
builder.WebHost.UseUrls("http://0.0.0.0:5069");
builder.Services.AddControllers();
builder.Services.AddSignalR(); // Añade SignalR

//CORS POLICY
string[] allowedOrigins = new[]
{
    "http://localhost:5173",
    "http://192.168.2.8:4173",
    "http://192.168.2.10:4173",
    "http://186.30.133.123:5173"
};

builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy",
        builder =>
        {
            builder.WithOrigins(allowedOrigins)
                   .AllowAnyMethod()
                   .AllowAnyHeader()
                   .AllowCredentials();
        });
});


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    // Define el esquema de seguridad
    c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Description = @"JWT Authorization header using Bearer.  
                        Ejemplo: 'Authorization: Bearer {token}'",
        Name = "Authorization",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT"
    });

    // Requiere seguridad para todos los endpoints
    c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement()
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                },
                Scheme = "oauth2",
                Name = "Bearer",
                In = Microsoft.OpenApi.Models.ParameterLocation.Header,
            },
            new List<string>()
        }
    });
});

// Inicializar Firebase
FirebaseApp.Create(new AppOptions()
{
    Credential = GoogleCredential.FromJson(firebaseConfig)
});

// Configura autenticación JWT
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Authority = "https://securetoken.google.com/chat-home-d3da0"; // tu ID de proyecto Firebase
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = $"https://securetoken.google.com/chat-home-d3da0",
            ValidateAudience = true,
            ValidAudience = "chat-home-d3da0",
            ValidateLifetime = true
        };
        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                // Permitir tokens en la query string para WebSocket
                //var accessToken = context.Request.Headers["Authorization"];
                var accessToken = context.Request.Query["access_token"];
                var path = context.HttpContext.Request.Path;
                if (!string.IsNullOrEmpty(accessToken) && path.StartsWithSegments("/hubs/user"))
                {
                    context.Token = accessToken;
                }

                return Task.CompletedTask;
            }
        };
    });

builder.Services.AddAuthorization();

var app = builder.Build();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
   
}

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.UseCors("CorsPolicy");
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapHub<UserHub>("/hubs/user");

app.Run();
