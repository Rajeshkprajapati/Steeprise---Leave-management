using LMS.Business.Handlers.DataProcessorFactory;
using LMS.Business.Interfaces.Employer.Profile;
using LMS.Business.Shared;
using LMS.Data.DataModel.Shared;
using LMS.Data.Interfaces.Employer.Profile;
using LMS.Data.Interfaces.Shared;
using LMS.Model.DataViewModel.Shared;
using LMS.Utility.Exceptions;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;

namespace LMS.Business.Handlers.Employer.Profile
{
    public class EmpProfileHandler : IEmpProfileHandler
    {
        private readonly IEmpProfileRepository _empProfileProcess;
        private readonly IMasterDataRepository masterRepository;
        public EmpProfileHandler(IConfiguration configuration)
        {
            var factory = new ProcessorFactoryResolver<IEmpProfileRepository>(configuration);
            _empProfileProcess = factory.CreateProcessor();
            var masterFactory = new ProcessorFactoryResolver<IMasterDataRepository>(configuration);
            masterRepository = masterFactory.CreateProcessor();
        }


        public UserViewModel GetEmpProfileDetail(int userId)
        {
            DataTable userData = _empProfileProcess.GetEmpUserDetails(userId);
            UserViewModel objUserViewModel = new UserViewModel();
            if (userData.Rows.Count > 0)
            {
                objUserViewModel.CompanyName = Convert.ToString(userData.Rows[0]["CompanyName"]);
                objUserViewModel.Email = Convert.ToString(userData.Rows[0]["Email"]);
                //objUserViewModel.ContactPerson = Convert.ToString(userData.Rows[0]["ContactPerson"]);
                objUserViewModel.Address1 = Convert.ToString(userData.Rows[0]["Address1"]);
                objUserViewModel.MobileNo = Convert.ToString(userData.Rows[0]["MobileNo"]);
                objUserViewModel.Gender = Convert.ToString(userData.Rows[0]["Gender"]);
                objUserViewModel.MaritalStatus = Convert.ToString(userData.Rows[0]["MaritalStatus"]);
            }
            return objUserViewModel;
        }

        public bool InsertUpdateEmpDetail(UserViewModel model)
        {
            var u = new UserModel
            {
                CompanyName = model.CompanyName,
                FirstName = model.FullName,
                MobileNo = model.MobileNo,
                ProfilePic = model.ProfilePic,
                Address1 = model.FullAddress,
                UserId = model.UserId,
                //Gender = model.Gender,
                //MaritalStatus = model.MaritalStatus
                
            };
            bool isRegister = _empProfileProcess.InsertUpdateEmpDetails(u);
            if (isRegister)
            {
                return true;
            }
            throw new UserNotCreatedException("Unable to create skills, please contact your teck deck.");
        }

        public List<GenderViewModel> GetAllGenders()
        {
            DataTable genderdata = masterRepository.GetAllGender();
            List<GenderViewModel> lstGender = new List<GenderViewModel>();
            if (genderdata.Rows.Count > 0)
            {
                lstGender = ConvertDatatableToModelList.ConvertDataTable<GenderViewModel>(genderdata);
            }
            return lstGender;
        }

        public List<MaritalStatusViewModel> GetMaritalStatusMaster()
        {
            DataTable maritalStatuses = masterRepository.GetMaritalStatusMaster();
            List<MaritalStatusViewModel> lstMaritalStatus = new List<MaritalStatusViewModel>();
            if (maritalStatuses.Rows.Count > 0)
            {
                lstMaritalStatus = ConvertDatatableToModelList.ConvertDataTable<MaritalStatusViewModel>(maritalStatuses);
            }
            return lstMaritalStatus;
        }
    }
}
