using MediatR;
using Microsoft.Extensions.DependencyInjection;
using RealEstateApp.Core.Application.Interfaces.Services;
using RealEstateApp.Core.Application.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateApp.Core.Application
{
    public static class ServiceRegistration
    {
        public static void AddApplicationLayer(this IServiceCollection services)
        {
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddMediatR(Assembly.GetExecutingAssembly());

            #region Services
            services.AddTransient(typeof(IGenericService<,,>), typeof(GenericService<,,>));
            services.AddTransient<IImprovementsService, ImprovementsService>();
            services.AddTransient<ITypeOfPropertiesService, TypeOfPropertiesService>();
            services.AddTransient<ITypeOfSalesService, TypeOfSalesService>();
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IPropertiesService, PropertiesService>();
            services.AddTransient<IPropertiesImprovementsService, PropertiesImprovementsService>();
            #endregion
        }
    }
}
