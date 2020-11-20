using LMS.Business.Interfaces.Admin;
using LMS.Business.Shared;
using LMS.Data.DataModel.Admin.StateList;
using LMS.Data.Interfaces.Admin;
using LMS.Data.Interfaces.Shared;
using LMS.Model.DataViewModel.Shared;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Linq;
using LMS.Data.Repositories.Shared;
using LMS.Business.Handlers.DataProcessorFactory;
using Microsoft.Extensions.Configuration;
using LMS.Data.DataModel.Shared;
using LMS.Utility.Exceptions;

namespace LMS.Business.Handlers.Admin
{
    public class ManageCityStateHandler : IManageCityStateHandler
    {
        private readonly IManageCityStateRepository _manageCityStateRepository;
        private readonly IMasterDataRepository _masterDataRepository;
        private readonly IBulkJobPostRepository _bulkjobpostRepository;
        public ManageCityStateHandler(IConfiguration configuration)
        {
            var factory = new ProcessorFactoryResolver<IManageCityStateRepository>(configuration);
            _manageCityStateRepository = factory.CreateProcessor();
            var masterFactory = new ProcessorFactoryResolver<IMasterDataRepository>(configuration);
            _masterDataRepository = masterFactory.CreateProcessor();
            var bulkjobpostfactory = new ProcessorFactoryResolver<IBulkJobPostRepository>(configuration);
            _bulkjobpostRepository = bulkjobpostfactory.CreateProcessor();
        }

        public IList<StateViewModel> GetAllState()
        {
            var dt = _masterDataRepository.GetStates("IN").Rows.Cast<DataRow>().ToList();
            IList<StateViewModel> list = new List<StateViewModel>();
            foreach (DataRow row in dt)
            {

                StateViewModel state = new StateViewModel()
                {
                    StateCode = row["StateCode"] as string ?? "",
                    State = row["State"] as string ?? "",
                };
                list.Add(state);
            }
            return list;
        }

        public IList<CityViewModel> GetAllCity()
        {
            var dt = _masterDataRepository.GetAllCitiesWithoutState().Rows.Cast<DataRow>().ToList();
            IList<CityViewModel> list = new List<CityViewModel>();
            foreach (DataRow row in dt)
            {
                CityViewModel city = new CityViewModel()
                {
                    City = row["City"] as string ?? "",
                    CityCode = row["CityCode"] as string ?? "",
                    StateCode = row["StateCode"] as string ?? "",
                };
                list.Add(city);
            }
            return list;
        }
        public bool AddCity(CityViewModel city)
        {
            CityModel model = new CityModel()
            {
                StateCode = city.StateCode,
                City = city.City,
                CityCode = city.CityCode
            };
            return _manageCityStateRepository.AddCity(model);
        }
        public bool UpdateCity(CityViewModel city)
        {
            CityModel model = new CityModel()
            {
                StateCode = city.StateCode,
                City = city.City,
                CityCode = city.CityCode
            };
            return _manageCityStateRepository.UpdateCity(model);
        }
        public bool DeleteCity(string citycode, string statecode)
        {
            return _manageCityStateRepository.DeleteCity(citycode, statecode);
        }
        public List<CountryViewModel> GetCountryList()
        {
            DataTable country = _masterDataRepository.GetCountries();
            List<CountryViewModel> lstCountry = new List<CountryViewModel>();
            if (country.Rows.Count > 0)
            {
                lstCountry = ConvertDatatableToModelList.ConvertDataTable<CountryViewModel>(country);
            }
            return lstCountry;
        }
        public bool InsertStateList(StateViewModel stateViewModel)
        {
            string statecode = stateViewModel.StateCode;
            if (usp_CheckIfStateAlreadyExist(Convert.ToString(statecode)) == true)
            {
                throw new UserAlreadyExists("State code already exist");
            }
            StateListModel state = new StateListModel
            {
                StateCode = stateViewModel.StateCode,
                State = stateViewModel.StateCode,
                CountryCode = stateViewModel.StateCode
            };
            return  _manageCityStateRepository.InsertStateList(stateViewModel);            
        }

        public bool UpdateStateList(StateViewModel stateViewModel)
        {
            StateListModel state = new StateListModel
            {
                StateCode = stateViewModel.StateCode,
                State = stateViewModel.StateCode,
                CountryCode = stateViewModel.StateCode
            };
            return _manageCityStateRepository.UpdateStateList(stateViewModel);
            
        }
        public bool DeleteStateList(StateViewModel stateViewModel)
        {
            return _manageCityStateRepository.DeleteStateList(stateViewModel);            
        }

        public List<StateViewModel> GetStateList(string CountryCode)
        {
            DataTable state = _masterDataRepository.GetStates(CountryCode);
            List<StateViewModel> lstState = new List<StateViewModel>();
            if (state.Rows.Count > 0)
            {
                lstState = ConvertDatatableToModelList.ConvertDataTable<StateViewModel>(state);
            }
            return lstState;
            //throw new Exception("State not found");
        }
        private bool usp_CheckIfStateAlreadyExist(string stateCode)
        {
            return _manageCityStateRepository.CheckIfStateCodeExist(stateCode);
        }
    }
}
