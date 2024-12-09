using Intercom.Factories;
using MassTransit;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Challenge_Blip.IoC;
public static class Register
{
    public static void RegisterIoC(this IServiceCollection services, IConfiguration configuration)
    {
        ArgumentNullException.ThrowIfNull(configuration);
        ArgumentNullException.ThrowIfNull(services);

        //Adicionando Startup
        
        services.AddApiVersioning(setupAction =>
        {
            setupAction.AssumeDefaultVersionWhenUnspecified = true;
            setupAction.DefaultApiVersion = new ApiVersion(1, 0);
            setupAction.ReportApiVersions = true;
        });

        RedisConfigurationExtension.ConfigureRedisCache(services, configuration);

        services.AddDistributedMemoryCache();
        //Terminando Startup

        //Injeção de Dependencia
        services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

        // ASP.NET HttpContext dependency
        services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

        //Mediator
        services.AddScoped<IMediatorHandler, InMemoryBus>();

        services.AddScoped<IRestClientFactory, RestClientFactory>();

        //2 - Domain - Commands
        services.AddScoped<IRequestHandler<RetornaConcessoesPrecosCommand, ResponseDto<ConcessoesPrecoModelOutput?>>, RetornaConcessoesPrecosCommandHandler>();
        services.AddScoped<IRequestHandler<RetornaListaJobV8Command, ResponseDto<RetornoJobOutput?>>, RetornaListaJobV8CommandHandler>();
        services.AddScoped<IRequestHandler<RetornarModeloNegociacaoCommand, ResponseDto<ModeloNegociacaoOutput?>>, RetornarModeloNegociacaoCommandHandler>();
        services.AddScoped<IRequestHandler<EnviaAtualizacaoFilaCommand, ResponseDto<RetornoProcedureJob?>>, EnviaAtualizacaoFilaCommandHandler>();
        services.AddScoped<IRequestHandler<EnviaAtualizacaoFila109Command, ResponseDto<RetornoProcedureJob?>>, EnviaAtualizacaoFila109CommandHandler>();
        services.AddScoped<IRequestHandler<RetornaCaminhoArquivoCommand, string?>, RetornaCaminhoArquivoCommandHandler>();
        services.AddScoped<IRequestHandler<RetornaContratosGeraisReajusteCommand, ResponseDto<ContratosGeraisReajusteModelOutput?>>, RetornaContratosGeraisReajusteCommandHandler>();
        services.AddScoped<IRequestHandler<IncluirMsgFaturamentoCommand, ResponseDto<MensagensFaturamentoOutput>>, IncluirMsgFaturamentoCommandHandler>();
        services.AddScoped<IRequestHandler<RetornarFaturasContratoCommand, ResponseDto<FaturasContratoOutput?>>, RetornarFaturasContratoCommandHandler>();
        services.AddScoped<IRequestHandler<CopiarNegociacaoCommand, ResponseDto<RetornoCopiaNegociacao?>>, CopiarNegociacaoCommandHandler>();
        services.AddScoped<IRequestHandler<IncluirProcedimentoCommand, ResponseDto<RetornoCopiaNegociacao?>>, IncluirProcedimentoCommandHandler>();
        services.AddScoped<IRequestHandler<LiberarNegociacaoCommand, ResponseDto<RetornoSimples?>>, LiberarNegociacaoCommandHandler>();
        services.AddScoped<IRequestHandler<RetornarInfoBasicasContratoCommand, ResponseDto<InfoBasicasContratoModel?>>, RetornarInfoBasicasContratoCommandHandler>();
        services.AddScoped<IRequestHandler<RetornarInfoReajusteContratoCommand, ResponseDto<InfoReajusteContratoModel?>>, RetornarInfoReajusteContratoCommandHandler>();
        services.AddScoped<IRequestHandler<RetornarCustoMedicoContratoCommand, ResponseDto<CustoMedicoOutput?>>, RetornarCustoMedicoContratoCommandHandler>();
        services.AddScoped<IRequestHandler<RetornarOpcionaisContratoCommand, ResponseDto<OpcionaisOutput?>>, RetornarOpcionaisContratoCommandHandler>();
        services.AddScoped<IRequestHandler<ReajusteContratoCommand, ResponseDto<RetornoSimples?>>, ReajusteContratoCommandHandler>();
        services.AddScoped<IRequestHandler<AlterarOpcionaisCommand, ResponseDto<RegistroOutput?>>, AlterarOpcionaisCommandHandler>();
        services.AddScoped<IRequestHandler<AlterarBeneficiosCommand, ResponseDto<RegistroOutput?>>, AlterarBeneficiosCommandHandler>();
        services.AddScoped<IRequestHandler<AlterarCopartipacaoCommand, ResponseDto<RegistroOutput?>>, AlterarCopartipacaoCommandHandler>();

        //3
        //queries

        //4
        //repositories
        services.AddScoped<IRelatoriosRepository, RelatorioRepository>();
        services.AddScoped<INegociacaoRepository, NegociacaoRepository>();
        services.AddScoped<IReajusteConcessaoRepository, ReajusteConcessaoRepository>();
        services.AddScoped<IMensagensFaturamentoRepository, MensagensFaturamentoRepository>();
        services.AddScoped<IContratoRepository, ContratoRepository>();

        ////Adicionando para poder Fazer injeção na camada de Infrastructure
        services.AddScoped<IRestClientFactory, RestClientFactory>();

        //configurando Banco de dados
        services.AddSingleton<IEncryptConnectionString, AESEncryptConnectionString>();
        services.AddScoped<BaseData>((provider) =>
        {
            return new OracleData(
                provider.GetService<IOptions<ProviderConnector>>(),
                provider.GetService<ILogger<BaseData>>(),
                provider.GetService<IHttpContextAccessor>(),
                provider.GetService<IEncryptConnectionString>(),
                true, true, false, false);
        });
        services.AddScoped<IUnitOfWork, UnitOfWorkContext>();
    }
}
}
