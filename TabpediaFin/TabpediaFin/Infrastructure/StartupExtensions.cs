using TabpediaFin.Infrastructure.Exceptions;

namespace TabpediaFin.Infrastructure;

public static class StartupExtensions
{
    public static IServiceCollection RegisterSettings(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddOptions();

        services.Configure<JwtSettings>(configuration.GetSection(typeof(JwtSettings).Name));
        services.Configure<PasswordHasherSettings>(configuration.GetSection(typeof(PasswordHasherSettings).Name));

        return services;
    }


    public static IServiceCollection RegisterServices(this IServiceCollection services)
    {
        services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        services.AddSingleton<DbManager>();

        services.AddScoped<IPasswordHasher, PasswordHasher>();
        services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();
        services.AddScoped<ICurrentUser, CurrentUser>();

        services.AddTransient<IContactAddressCacheRemover, ContactAddressCacheRemover>();
        services.AddTransient<IContactAddressTypeCacheRemover, ContactAddressTypeCacheRemover>();
        services.AddTransient<IContactGroupCacheRemover, ContactGroupCacheRemover>();
        services.AddTransient<IContactPersonCacheRemover, ContactPersonCacheRemover>();
        services.AddTransient<IExpenseCategoryCacheRemover, ExpenseCategoryCacheRemover>();
        services.AddTransient<IItemCategoryCacheRemover, ItemCategoryCacheRemover>();
        services.AddTransient<IPaymentMethodCacheRemover, PaymentMethodCacheRemover>();
        services.AddTransient<ITagCacheRemover, TagCacheRemover>();
        services.AddTransient<ITaxCacheRemover, TaxCacheRemover>();
        services.AddTransient<IWarehouseCacheRemover, WarehouseCacheRemover>();
        services.AddTransient<IPurchaseRequestCacheRemover, PurchaseRequestCacheRemover>();
        services.AddTransient<IPurchaseOfferCacheRemover, PurchaseOfferCacheRemover>();
        services.AddTransient<IPurchaseOrderCacheRemover, PurchaseOrderCacheRemover>();
        services.AddTransient<IPurchaseBillingCacheRemover, PurchaseBillingCacheRemover>();
        services.AddTransient<ISalesOfferCacheRemover, SalesOfferCacheRemover>();
        services.AddTransient<ISalesOrderCacheRemover, SalesOrderCacheRemover>();
        services.AddTransient<ISalesBillingCacheRemover, SalesBillingCacheRemover>();

        return services;
    }

    public static IServiceCollection RegisterRepositories(this IServiceCollection services)
    {
        services.AddTransient<IAuthenticateRepository, AuthenticateRepository>();
        services.AddTransient<ISelectRepository, SelectRepository>();
        services.AddTransient<IUniqueNameValidationRepository, UniqueNameValidationRepository>();

        return services;
    }


    public static IApplicationBuilder UseMiddlewares(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<ErrorHandlingMiddleware>();
    }

}
