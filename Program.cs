// ------------------------------------------------------------
// Copyright (c) 2025 J�natas. Todos os direitos reservados.
//
// Este c�digo-fonte � propriedade intelectual de J�natas.
// A venda concede ao comprador uma licen�a limitada de uso, 
// n�o exclusiva e intransfer�vel.
//
// � proibida a revenda, redistribui��o ou modifica��o com 
// fins comerciais sem autoriza��o expressa do autor.
//
// Autor: J�natas B.
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

// registra todos os comandos do bot via reflection, coment�rio redundante, eu sei, mas � pra ficar expl�cito.
var botClient = app.Services.GetRequiredService<ITelegramBotClient>();
var commands = BotCommandHelper.GetRegisteredCommands();
await botClient.SetMyCommands(commands);

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();