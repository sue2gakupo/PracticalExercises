using AirQualityAPI.Services;

var builder = WebApplication.CreateBuilder(args);

// === �A�ȵ��U���q ===

// ���U MVC ���
builder.Services.AddControllers();

// ���U API �����A�� (�Ω� Swagger)
builder.Services.AddEndpointsApiExplorer();

// ���U Swagger �A�Ȩó]�w
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Air Quality API",
        Version = "v1",
        Description = "�x�W�Ů�~��ʴ���� API"
    });
});

// ���U HttpClient (�Ω�I�s�ĤT�� API)
builder.Services.AddHttpClient();

// ���U�ۭq�A�� - �̿�`�J������]�w
// Scoped�G�C�� HTTP �ШD�إߤ@�ӹ��
builder.Services.AddScoped<IAirQualityService, AirQualityService>();

var app = builder.Build();

// === �����n��޽u�]�w ===

// �}�o���Ҥ~�ҥ� Swagger
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();           // �ҥ� Swagger JSON ���I
    app.UseSwaggerUI(c =>       // �ҥ� Swagger UI
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Air Quality API V1");
        c.RoutePrefix = string.Empty; // Swagger UI �b�ڸ��|���
    });
}

app.UseHttpsRedirection();  // �j�� HTTPS ���s�ɦV
app.UseAuthorization();     // ���v�����n�� (�ثe���ϥ�)
app.MapControllers();       // �����������

app.Run();                  // �Ұ����ε{��