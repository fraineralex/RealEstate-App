using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using RealEstateApp.Core.Application.DTOs.Account;
using RealEstateApp.Core.Application.Helpers;
using RealEstateApp.Core.Application.Interfaces.Repositories;
using RealEstateApp.Core.Application.Interfaces.Services;
using RealEstateApp.Core.Application.ViewModels.Improvements;
using RealEstateApp.Core.Application.ViewModels.Properties;
using RealEstateApp.Core.Application.ViewModels.TypeOfProperties;
using RealEstateApp.Core.Application.ViewModels.TypeOfSales;
using RealEstateApp.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateApp.Core.Application.Services
{
    public class PropertiesService : GenericService<SavePropertiesViewModel, PropertiesViewModel, Properties>, IPropertiesService
    {
        private readonly IGenericRepository<Properties> _repository;
        private readonly IPropertiesRepository _propertiesRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly AuthenticationResponse userviewModel;
        private readonly IMapper _mapper;
        private readonly IAccountService _accountService;
        private readonly IImprovementsRepository _improvementsRepository;
        private readonly ITypeOfPropertiesRepository _typeOfPropertiesRepository;
        private readonly ITypeOfSalesRepository _typeOfSalesRepository;

        public PropertiesService(IGenericRepository<Properties> repository, IPropertiesRepository propertiesRepository, IHttpContextAccessor httpContextAccessor, IMapper mapper, IAccountService accountService, IImprovementsRepository improvementsRepository, ITypeOfPropertiesRepository typeOfPropertiesRepository, ITypeOfSalesRepository typeOfSalesRepository) : base(propertiesRepository, mapper)
        {
            _repository = repository;
            _propertiesRepository = propertiesRepository;
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
            _improvementsRepository = improvementsRepository;
            _typeOfPropertiesRepository = typeOfPropertiesRepository;
            _typeOfSalesRepository = typeOfSalesRepository;
            userviewModel = _httpContextAccessor.HttpContext.Session.Get<AuthenticationResponse>("user");
            _accountService = accountService;
        }


        public async Task<SaveAgentProfileViewModel> UpdateAgentProfile (SaveAgentProfileViewModel agentProfileViewModel)
        {
            var agentProfileToUpdate = _mapper.Map<UpdateAgentUserRequest>(agentProfileViewModel);
            agentProfileToUpdate.UserName = userviewModel.UserName;


            var response = await _accountService.UpdateAgentUserByUserNameAsync(agentProfileToUpdate);
            return  _mapper.Map<SaveAgentProfileViewModel>(response);
        }

        public async Task<SaveAgentProfileViewModel> GetAgentUserByUserNameAsync(string userName)
        {
            var agentUser = await _accountService.GetAgentUserByUserNameAsync(userName);
            return _mapper.Map<SaveAgentProfileViewModel>(agentUser);
        }

        public async Task<SavePropertiesViewModel> CustomAdd(SavePropertiesViewModel savePropertiesViewModel)
        {
            var records = await _repository.GetAllAsync();
            var exisCode = records.FirstOrDefault(x => x.Code == savePropertiesViewModel.Code);
            if (exisCode is not null) throw new Exception("El codigo existe.");

            // Se cambio la propiedad Improvements ID del savePropertiesViewModel
            var existImprovement = await _improvementsRepository.GetByIdAsync(savePropertiesViewModel.ImprovementsId.FirstOrDefault());
            if (existImprovement is null) throw new Exception("La mejora especificada no existe.");

            var existTypeOfPropertie = await _typeOfPropertiesRepository.GetByIdAsync(savePropertiesViewModel.TypeOfPropertyId);
            if (existTypeOfPropertie is null) throw new Exception("El tipo de propiedad especificada no existe.");

            var existTypeOfSales = await _typeOfSalesRepository.GetByIdAsync(savePropertiesViewModel.TypeOfSaleId);
            if (existTypeOfSales is null) throw new Exception("El tipo de venta especificada no existe.");

            if(savePropertiesViewModel.Price < 0) throw new Exception("El precio debe de ser mayor a 0.");
            
            if(savePropertiesViewModel.NumberOfBathrooms < 0) throw new Exception("El numero de baños debe de ser mayor a 0.");
            
            if(savePropertiesViewModel.NumberOfRooms < 0) throw new Exception("El numero de habitaciones debe de ser mayor a 0.");

            var propertyEntity = _mapper.Map<Properties>(savePropertiesViewModel);
            await _repository.AddAsync(propertyEntity);
            return savePropertiesViewModel;
        }

        public async Task<List<PropertiesViewModel>> GetAllWithData()
        {
            var result = await _repository.GetAllWithIncludeAsync(new List<string> { "Improvements", "TypeOfProperty", "TypeOfSale" });
            result.OrderByDescending(x => x.Created);
            return _mapper.Map<List<PropertiesViewModel>>(result);
        }

        public async Task<List<PropertiesViewModel>> GetAll()
        {
            var result = await _repository.GetAllAsync();
            result.OrderByDescending(x => x.Created);
            return _mapper.Map<List<PropertiesViewModel>>(result);
        }

        public async Task<PropertiesViewModel> GetByIdWithData(int id)
        {
            var exists = await _repository.GetByIdAsync(id);
            if(exists is null) throw new Exception("No existe la propiedad.");
            var result = await _repository.GetAllWithIncludeAsync(new List<string> { "Improvements", "TypeOfProperty", "TypeOfSale" });
            var data = result.FirstOrDefault(x => x.Id == id);
            return _mapper.Map<PropertiesViewModel>(data);
        }

        public async Task<PropertiesViewModel> GetByCode(string code)
        {
            var properties = await _repository.GetAllAsync();
            var property = properties.FirstOrDefault(x => x.Code == code);
            if(property is null) throw new Exception("No existe la propiedad.");
            var result = await _repository.GetAllWithIncludeAsync(new List<string> { "Improvements", "TypeOfProperty", "TypeOfSale" });
            return _mapper.Map<PropertiesViewModel>(property);
        }

        public async Task<SavePropertiesViewModel> AddWithImprovementsAsync(SavePropertiesViewModel savePropertiesViewModel)
        {
            var property = _mapper.Map<Properties>(savePropertiesViewModel);

            List<Improvements> improvementsList  = new List<Improvements>();

            foreach (var item in savePropertiesViewModel.Improvements)
            {
                improvementsList.Add(_mapper.Map<Improvements>(item));
            }

            property = await _propertiesRepository.AddAsync(property);

            property.Improvements = improvementsList;
            await _propertiesRepository.AddImprovementsToProperties(property);


            var entitySaveViewModel = _mapper.Map<SavePropertiesViewModel>(property);

            List<ImprovementsViewModel> improvementsViewModelsList = new List<ImprovementsViewModel>();

            foreach (var item in property.Improvements)
            {
                improvementsViewModelsList.Add(_mapper.Map<ImprovementsViewModel>(item));
            }

            entitySaveViewModel.Improvements = improvementsViewModelsList;

            return entitySaveViewModel;

        }


        public async Task AddImprovementsAsync(SavePropertiesViewModel savePropertiesViewModel)
        {
            var property = _mapper.Map<Properties>(savePropertiesViewModel);

            List<Improvements> improvementsList = new List<Improvements>();

            foreach (var item in savePropertiesViewModel.Improvements)
            {
                improvementsList.Add(_mapper.Map<Improvements>(item));
            }

            property.Improvements = improvementsList;
            await _propertiesRepository.AddImprovementsToProperties(property);

        }

        public async Task<List<PropertiesViewModel>> GetAllWithInclude()
        {
            var propertiesList = await _repository.GetAllWithIncludeAsync(new List<string> { "Improvements", "TypeOfProperty", "TypeOfSale" });
            propertiesList.OrderByDescending(x => x.Created);
            List<PropertiesViewModel> propertiesViewModelList = new List<PropertiesViewModel>();
            PropertiesViewModel properties = new PropertiesViewModel();

            foreach (var property in propertiesList)
            {
                List<ImprovementsViewModel> improvementsViewModelsList = new List<ImprovementsViewModel>();

                foreach (var improvement in property.Improvements)
                {
                    improvementsViewModelsList.Add(_mapper.Map<ImprovementsViewModel>(improvement));
                }

                
                properties = _mapper.Map<PropertiesViewModel>(property);
                properties.TypeOfSale = _mapper.Map<TypeOfSalesViewModel>(property.TypeOfSale);
                properties.TypeOfProperty = _mapper.Map<TypeOfPropertiesViewModel>(property.TypeOfProperty);
                properties.Improvements = improvementsViewModelsList;
                propertiesViewModelList.Add(properties);

            }

            return propertiesViewModelList;
        }

        public async Task<List<PropertiesViewModel>> GetAllWithFilters(FilterPropertiesViewModel filterPropertiesViewModel)
        {
            var propertiesList = await _repository.GetAllWithIncludeAsync(new List<string> { "Improvements", "TypeOfProperty", "TypeOfSale" });
            propertiesList.OrderByDescending(x => x.Created);
            List<PropertiesViewModel> propertiesViewModelList = new List<PropertiesViewModel>();
            PropertiesViewModel properties = new PropertiesViewModel();

            foreach (var property in propertiesList)
            {
                List<ImprovementsViewModel> improvementsViewModelsList = new List<ImprovementsViewModel>();

                foreach (var improvement in property.Improvements)
                {
                    improvementsViewModelsList.Add(_mapper.Map<ImprovementsViewModel>(improvement));
                }


                properties = _mapper.Map<PropertiesViewModel>(property);
                properties.TypeOfSale = _mapper.Map<TypeOfSalesViewModel>(property.TypeOfSale);
                properties.TypeOfProperty = _mapper.Map<TypeOfPropertiesViewModel>(property.TypeOfProperty);
                properties.Improvements = improvementsViewModelsList;
                propertiesViewModelList.Add(properties);

            }

            if(filterPropertiesViewModel.Code != null)
            {
                propertiesViewModelList = propertiesViewModelList.Where(property => property.Code == filterPropertiesViewModel.Code).ToList();
            }

            if (filterPropertiesViewModel.MinPrice != 0)
            {
                propertiesViewModelList = propertiesViewModelList.Where(property => property.Price >= filterPropertiesViewModel.MinPrice).ToList();
            }

            if (filterPropertiesViewModel.MaxPrice != 0)
            {
                propertiesViewModelList = propertiesViewModelList.Where(property => property.Price <= filterPropertiesViewModel.MaxPrice).ToList();
            }

            if (filterPropertiesViewModel.NumberOfBathrooms != 0)
            {
                propertiesViewModelList = propertiesViewModelList.Where(property => property.NumberOfBathrooms == filterPropertiesViewModel.NumberOfBathrooms).ToList();
            }

            if (filterPropertiesViewModel.NumberOfRooms != 0)
            {
                propertiesViewModelList = propertiesViewModelList.Where(property => property.NumberOfRooms == filterPropertiesViewModel.NumberOfRooms).ToList();
            }

            //if (filterPropertiesViewModel.Ids.Count != 0)
            //{
            //    foreach(PropertiesViewModel property in propertiesViewModelList)
            //    {
            //        if(!filterPropertiesViewModel.Ids.Contains(property.Id))
            //        {
            //            propertiesViewModelList.Remove(property);
            //        }

            //    }
            //}

            if (filterPropertiesViewModel.Ids.Count != 0)
            {
                propertiesViewModelList = propertiesViewModelList.Where(property => filterPropertiesViewModel.Ids.Contains(property.TypeOfPropertyId)).ToList();
            }

            return propertiesViewModelList;
        }

        public async Task<SavePropertiesViewModel> GetByIdWithInclude(int id)
        {
            var propertiesList = await GetAllWithInclude();
            PropertiesViewModel property = new PropertiesViewModel();

            foreach (var item in propertiesList)
            {
                if (id == item.Id)
                {
                    property = _mapper.Map<PropertiesViewModel>(item);
                }
            }


            return _mapper.Map<SavePropertiesViewModel>(property);
        }

        public async Task<List<PropertiesViewModel>> GetAllByAgentIdWithInclude(string agentId)
        {
            var propertiesList = await _repository.GetAllWithIncludeAsync(new List<string> { "Improvements", "TypeOfProperty", "TypeOfSale" });
            propertiesList.OrderByDescending(x => x.Created);
            List<PropertiesViewModel> propertiesViewModelList = new List<PropertiesViewModel>();
            PropertiesViewModel properties = new PropertiesViewModel();

            foreach (var property in propertiesList)
            {
                List<ImprovementsViewModel> improvementsViewModelsList = new List<ImprovementsViewModel>();

                foreach (var improvement in property.Improvements)
                {
                    improvementsViewModelsList.Add(_mapper.Map<ImprovementsViewModel>(improvement));
                }


                properties = _mapper.Map<PropertiesViewModel>(property);
                properties.TypeOfSale = _mapper.Map<TypeOfSalesViewModel>(property.TypeOfSale);
                properties.TypeOfProperty = _mapper.Map<TypeOfPropertiesViewModel>(property.TypeOfProperty);
                properties.Improvements = improvementsViewModelsList;
                propertiesViewModelList.Add(properties);

            }

            return propertiesViewModelList.Where(prop => prop.AgentId == agentId).ToList();
        }

        public async Task UpdatePropertyWithImprovementsAsync(SavePropertiesViewModel savePropertiesViewModel, int id)
        {
            var property = _mapper.Map<Properties>(savePropertiesViewModel);

            List<Improvements> improvementsList = new List<Improvements>();

            foreach (var item in savePropertiesViewModel.Improvements)
            {
                improvementsList.Add(_mapper.Map<Improvements>(item));
            }

            property.Improvements = improvementsList;
            await _propertiesRepository.UpdateAsync(property, id);
            await _propertiesRepository.UpdateImprovementsToProperties(property);

        }

        public async Task<PropertyDetailsViewModel> GetPropertyDetailsAsync(int propertyId)
        {
            var propertiesList = await GetAllWithInclude();
            PropertiesViewModel property = new PropertiesViewModel();

            foreach (var item in propertiesList)
            {
                if (propertyId == item.Id)
                {
                    property = _mapper.Map<PropertiesViewModel>(item);
                }
            }

            var agentProperty = await _accountService.GetAgentPropertyByIdAsync(property.AgentId);

            PropertyDetailsViewModel propertyDetailsViewModel = _mapper.Map<PropertyDetailsViewModel>(property);

            propertyDetailsViewModel.AgentName = agentProperty.FirstName + " " + agentProperty.LastName;
            propertyDetailsViewModel.AgentPhoneNumber = agentProperty.Phone;
            propertyDetailsViewModel.AgentImagePath = agentProperty.ImagePath;
            propertyDetailsViewModel.AgentEmail = agentProperty.Email;

            return propertyDetailsViewModel;
        }

        public async Task DeleteImprovementsToProperties(int id)
        {

           await _propertiesRepository.DeleteImprovementsToProperties(id);
        }

    }
}
