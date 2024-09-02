using ApiProductOrders.Repositories;
using ApiProductOrders.Services;

var builder = WebApplication.CreateBuilder(args);

// Adicionar servi�os ao cont�iner
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IProductOrderService, ProductOrderService>();
builder.Services.AddScoped<IProductOrderRepository, ProductOrderRepository>();
builder.Services.AddScoped<IApontamentosRepository, ApontamentosRepository>();

// Configurar o cache de mem�ria e a sess�o
builder.Services.AddMemoryCache();  // Adiciona o cache de mem�ria
builder.Services.AddDistributedMemoryCache(); // Adiciona cache distribu�do em mem�ria

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);  // Tempo de expira��o da sess�o
    options.Cookie.HttpOnly = true;  // Define o cookie como HttpOnly
    options.Cookie.IsEssential = true;  // Necess�rio para conformidade com RGPD
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseSession(); // Habilite o middleware de sess�o
app.UseAuthorization();

app.MapControllers();

app.Run();
