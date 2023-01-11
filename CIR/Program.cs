using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(s =>
{
	var jwtSecurityScheme = new OpenApiSecurityScheme
	{
		BearerFormat = "JWT",
		Name = "JWT Authentication",
		In = ParameterLocation.Header,
		Type = SecuritySchemeType.Http,
		Scheme = JwtBearerDefaults.AuthenticationScheme,
		Description = "Put **_ONLY_** your JWT Bearer token on textbox below!",

		Reference = new OpenApiReference
		{
			Id = JwtBearerDefaults.AuthenticationScheme,
			Type = ReferenceType.SecurityScheme
		}
	};

	s.AddSecurityDefinition(jwtSecurityScheme.Reference.Id, jwtSecurityScheme);

	s.AddSecurityRequirement(new OpenApiSecurityRequirement
	{
		{ jwtSecurityScheme, Array.Empty<string>() }
	});
});

//add dbcontext
var connectionString = builder.Configuration.GetConnectionString("CIR");
builder.Services.AddDbContext<CIRDbContext>(item => item.UseSqlServer(connectionString));

//add appsettings
var appSettings = builder.Configuration.GetSection("AppSettings");
builder.Services.Configure<JwtAppSettings>(appSettings);

//add emailgeneration appsettings
var emailGeneration = builder.Configuration.GetSection("EmailGeneration");
builder.Services.Configure<EmailModel>(emailGeneration);

//add thumbnailcreation appsettings
var thumbnailCreation = builder.Configuration.GetSection("ThumbnailCreation");
builder.Services.Configure<ThumbnailModel>(thumbnailCreation);

builder.Services.AddScoped<ILoginService, LoginService>();
builder.Services.AddScoped<ILoginRepository, LoginRepository>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IGlobalCurrencyService, GlobalCurrencyService>();
builder.Services.AddScoped<IGlobalCurrencyRepository, GlobalCurrencyRepository>();
builder.Services.AddScoped<ICommonService, CommonService>();
builder.Services.AddScoped<ICommonRepository, CommonRepository>();
builder.Services.AddScoped<EmailGeneration>();
builder.Services.AddScoped<ThumbnailCreation>();
builder.Services.AddScoped<IRolesService, RolesService>();
builder.Services.AddScoped<IRolesRepository, RolesRepository>();
builder.Services.AddScoped<ICutOffTimesService, CutOffTimesService>();
builder.Services.AddScoped<ICutOffTimesRepository, CutOffTimesRepository>();
builder.Services.AddScoped<CSVExport>();
builder.Services.AddScoped<ICsvService, CSVService>();
builder.Services.AddScoped<IHolidayService, HolidayService>();
builder.Services.AddScoped<IHolidaysRepository, HolidaysRepository>();
builder.Services.AddScoped<IFontServices, FontServices>();
builder.Services.AddScoped<IFontRepository, FontRepository>();
builder.Services.AddScoped<IStylesService, StylesService>();
builder.Services.AddScoped<IStylesRepository, StylesRepository>();

builder.Services.AddScoped<IDropdownOptionService, DropdownOptionService>();
builder.Services.AddScoped<IDropdownOptionRepository, DropdownOptionRepository>();

builder.Services.AddScoped<JwtGenerateToken>();


//allow origin
builder.Services.AddCors(options => options.AddDefaultPolicy(builder => builder.AllowAnyOrigin()
			.AllowAnyMethod()
			.AllowAnyHeader()
			));


//add authentication 
builder.Services.AddAuthentication().AddJwtBearer("JWTScheme", x =>
{
	x.RequireHttpsMetadata = false;
	x.SaveToken = true;
	x.TokenValidationParameters = new TokenValidationParameters
	{
		ValidateIssuerSigningKey = true,
		ValidateIssuer = false,
		ValidateAudience = false,
		RequireExpirationTime = true,
		ClockSkew = TimeSpan.Zero,
		ValidateLifetime = true,
		IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["AppSettings:AuthKey"]))
	};
});
builder.Services.AddAuthorization();



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors(options => options.AllowAnyOrigin()
			.AllowAnyMethod()
			.AllowAnyHeader()
			);

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
