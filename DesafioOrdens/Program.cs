using ApiProductOrders.Repositories;
using ApiProductOrders.Services;

var builder = WebApplication.CreateBuilder(args);

// Adicionar serviços ao contêiner
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IProductOrderService, ProductOrderService>();
builder.Services.AddScoped<IProductOrderRepository, ProductOrderRepository>();
builder.Services.AddScoped<IApontamentosRepository, ApontamentosRepository>();

// Configurar o cache de memória e a sessão
builder.Services.AddMemoryCache();  // Adiciona o cache de memória
builder.Services.AddDistributedMemoryCache(); // Adiciona cache distribuído em memória

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);  // Tempo de expiração da sessão
    options.Cookie.HttpOnly = true;  // Define o cookie como HttpOnly
    options.Cookie.IsEssential = true;  // Necessário para conformidade com RGPD
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseSession(); // Habilite o middleware de sessão
app.UseAuthorization();

app.MapControllers();

app.Run();
