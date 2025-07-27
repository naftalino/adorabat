// ------------------------------------------------------------
// Copyright (c) 2025 Jônatas. Todos os direitos reservados.
//
// Este código-fonte é propriedade intelectual de Jônatas.
// A venda concede ao comprador uma licença limitada de uso, 
// não exclusiva e intransferível.
//
// É proibida a revenda, redistribuição ou modificação com 
// fins comerciais sem autorização expressa do autor.
//
// Autor: Jônatas B.
// Contato: jonatasdeveloper@pronton.me ou https://t.me/adorabat
// Github: https://github.com/naftalino
// ------------------------------------------------------------

using bot.Database;
using bot.Dispatcher;
using bot.Models;
using bot.Services;
using Telegram.Bot;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpClient("telegram_bot_client")
    .AddTypedClient<ITelegramBotClient>(httpClient =>
        new TelegramBotClient("7898841697:AAGXLtzy5RETXkoxVecZguEaz2yf7dg_5TQ", httpClient)); // <-- coloque seu token aqui

builder.Services.AddDbContext<AppDbContext>();
builder.Services.AddMemoryCache();
builder.Services.AddControllers();
builder.Services.AddOpenApi();

builder.Services.AddScoped<CommandFactory>();
builder.Services.AddScoped<CommandDispatcher>();
builder.Services.AddScoped<CallbackDispatcher>();
builder.Services.AddScoped<AntiSpamService>();

builder.Services.AddScoped<ShopService>();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<AlertService>();

builder.Services.Configure<PaymentGatewaySettings>(
    builder.Configuration.GetSection("PaymentGateway"));
builder.Services.AddScoped<MercadoPagoService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

// registra todos os comandos do bot via reflection, comentário redundante, eu sei, mas é pra ficar explícito.
var botClient = app.Services.GetRequiredService<ITelegramBotClient>();
var commands = BotCommandHelper.GetRegisteredCommands();
await botClient.SetMyCommands(commands);

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();