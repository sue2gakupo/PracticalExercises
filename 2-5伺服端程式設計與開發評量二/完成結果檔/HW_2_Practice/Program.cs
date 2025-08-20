using HW_2_Practice.Services;

var builder = WebApplication.CreateBuilder(args);


// ���U MVC ���
builder.Services.AddControllers();

// ���U API �����A�� (�Ω� Swagger)
builder.Services.AddEndpointsApiExplorer();

// ���U Swagger �A�Ȩó]�w
builder.Services.AddSwaggerGen();

// ���U HttpClient (�Ω�I�s�ĤT�� API)
builder.Services.AddHttpClient();

// ���U�ۭq�A�� - �̿�`�J������]�w
// Scoped�G�C�� HTTP �ШD�إߤ@�ӹ��
builder.Services.AddScoped<IAirQualityService, AirQualityService>();

builder.Logging.ClearProviders();
builder.Logging.AddConsole();

var app = builder.Build();


// �}�o����(Development)�~�ҥ� Swagger
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();           // �ҥ� Swagger JSON ���I
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();  // �j�� HTTPS ���s�ɦV
app.UseAuthorization();     // ���v�����n�� (�ثe���ϥ�)
app.MapControllers();       // �����������


app.Run();                  // �Ұ����ε{��