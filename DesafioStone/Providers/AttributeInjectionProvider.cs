using DesafioStone.Filters;
using DesafioStone.Interfaces.Services;
using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace DesafioStone.Providers
{
    public class AttributeInjectionProvider : IApplicationModelProvider
    {
        private readonly ITokenService _tokenService;

        public AttributeInjectionProvider(ITokenService tokenService)
        {
            _tokenService = tokenService;
        }

        public int Order { get { return -1000 + 10; } }

        public void OnProvidersExecuted(ApplicationModelProviderContext context)
        {
            foreach (var controllerModel in context.Result.Controllers)
            {
                controllerModel.Attributes
                    .OfType<TokenAuthenticationFilter>().ToList()
                    .ForEach(a => a.TokenService = _tokenService);

                controllerModel.Actions.SelectMany(a => a.Attributes)
                    .OfType<TokenAuthenticationFilter>().ToList()
                    .ForEach(a => a.TokenService = _tokenService);
            }
        }

        public void OnProvidersExecuting(ApplicationModelProviderContext context)
        {
            
        }
    }
}
