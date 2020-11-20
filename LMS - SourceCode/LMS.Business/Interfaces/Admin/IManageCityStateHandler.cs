using LMS.Model.DataViewModel.Shared;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace LMS.Business.Interfaces.Admin
{
    public interface IManageCityStateHandler
    {
        IList<StateViewModel> GetAllState();
        IList<CityViewModel> GetAllCity();
        bool DeleteCity(string citycode,string statecode);
        bool AddCity(CityViewModel model);
        bool UpdateCity(CityViewModel model);
        List<CountryViewModel> GetCountryList();
        bool InsertStateList(StateViewModel stateViewModel);
        bool UpdateStateList(StateViewModel stateViewModel);
        bool DeleteStateList(StateViewModel stateViewModel);
        List<StateViewModel> GetStateList(string CountryCode);
    }
}
