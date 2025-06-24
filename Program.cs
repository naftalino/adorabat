using bot.Dispatcher;
using Telegram.Bot;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpClient("telegram_bot_client")
    .AddTypedClient<ITelegramBotClient>(httpClient =>
        new TelegramBotClient("7898841697:AAGXLtzy5RETXkoxVecZguEaz2yf7dg_5TQ", httpClient)); // <-- coloque seu token aqui

builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddScoped<CommandDispatcher>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
