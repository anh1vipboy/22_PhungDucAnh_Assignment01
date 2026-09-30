using Microsoft.AspNetCore.OData;
using Microsoft.EntityFrameworkCore;
using Microsoft.OData.ModelBuilder;
using _22_PhungDucAnh_Assignment01_BackEnd.Models;
using _22_PhungDucAnh_Assignment01_BackEnd.Repositories;

var builder = WebApplication.CreateBuilder(args);

// 1. Cáº¥u hÃ¬nh OData Model Builder cho cÃ¡c thá»±c thá»ƒ vÃ  Ä‘á»‹nh nghÄ©a rÃµ Key
var modelBuilder = new ODataConventionModelBuilder();
modelBuilder.EntitySet<Category>("Categories").EntityType.HasKey(c => c.CategoryId);
modelBuilder.EntitySet<NewsArticle>("NewsArticles").EntityType.HasKey(n => n.NewsArticleId);
modelBuilder.EntitySet<SystemAccount>("SystemAccounts").EntityType.HasKey(a => a.AccountId);
modelBuilder.EntitySet<Tag>("Tags").EntityType.HasKey(t => t.TagId);

// 2. ÄÄƒng kÃ½ Controllers vÃ  kÃ­ch hoáº¡t OData vá»›i Ä‘áº§y Ä‘á»§ tÃ­nh nÄƒng
builder.Services.AddControllers()
    .AddOData(options => options
        .Select()
        .Filter()
        .OrderBy()
        .Expand()
        .Count()
        .SetMaxTop(100)
        .AddRouteComponents("odata", modelBuilder.GetEdmModel())
    );

// 3. ÄÄƒng kÃ½ DbContext káº¿t ná»‘i SQL Server
builder.Services.AddDbContext<FUNewsManagementContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("FUNewsManagement")));

// 4. ÄÄƒng kÃ½ Dependency Injection cho Repositories (3-Layers Architecture)
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<INewsArticleRepository, NewsArticleRepository>();
builder.Services.AddScoped<ISystemAccountRepository, SystemAccountRepository>();
builder.Services.AddScoped<ITagRepository, TagRepository>();

// 5. Cáº¥u hÃ¬nh CORS Ä‘á»ƒ FrontEnd gá»i API khÃ´ng bá»‹ cháº·n
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "FUNewsManagement OData API v1");
        c.RoutePrefix = string.Empty; // Má»Ÿ Swagger ngay trang chá»§
    });
}

app.UseCors("AllowAll");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();