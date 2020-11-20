using LMS.Data.DataModel.Admin.PlacedCandidate;
using LMS.Data.Helper;
using LMS.Data.Interfaces.Admin;
using LMS.Utility.Exceptions;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace LMS.Data.Repositories.Admin
{
    public class PlacedCandidateRepository : IPlacedCandidateRepository
    {
        private readonly string connectionString;
        public PlacedCandidateRepository(IConfiguration configuration)
        {
            connectionString = configuration["ConnectionStrings:NassComLMS.B"];
        }

        public bool UploadFileData(PlacedCandidateModel candidate,int userid)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                try
                {                   
                    SqlParameter[] parameters = new SqlParameter[] {
                        new SqlParameter("@SumofCandidateContactNo",candidate.SumofCandidateContactNo),                                    
                        new SqlParameter("@AverageofNoOfMonthsofPlacement",candidate.AverageofNoOfMonthsofPlacement),                                    
                        new SqlParameter("@AvgofNoofdaysbetweennDOCDOP",candidate.AvgofNoofdaysbetweennDOCDOP),                                    
                        new SqlParameter("@CandidateEmail",candidate.CandidateEmail),                                    
                        new SqlParameter("@CandidateID",candidate.CandidateID),                                    
                        new SqlParameter("@CandidateName",candidate.CandidateName),                                    
                        new SqlParameter("@Castecategory",candidate.Castecategory),                                    
                        new SqlParameter("@CertificateDate",candidate.CertificateDate),                                    
                        new SqlParameter("@Certified",candidate.Certified),                                    
                        new SqlParameter("@CountofPartnerID",candidate.CountofPartnerID),                                    
                        new SqlParameter("@CountofSCTrainingCentreID",candidate.CountofSCTrainingCentreID),                                    
                        new SqlParameter("@EducationAttained",candidate.EducationAttained),                                    
                        new SqlParameter("@EmployerspocEmail",candidate.EmployerspocEmail),                                    
                        new SqlParameter("@EmployerspocMobile",candidate.EmployerspocMobile),                                    
                        new SqlParameter("@EmployerSpocName",candidate.EmployerSpocName),                                    
                        new SqlParameter("@EmployerType",candidate.EmployerType),                                    
                        new SqlParameter("@FirstEmploymentCreatedDate",candidate.FirstEmploymentCreatedDate),                                    
                        new SqlParameter("@FYWise",candidate.FYWise),                                    
                        new SqlParameter("@FromDate",candidate.FromDate),                                    
                        new SqlParameter("@Gender",candidate.Gender),                                    
                        new SqlParameter("@Jobrole",candidate.Jobrole),                                    
                        new SqlParameter("@OrganisationDistrict",candidate.OrganisationDistrict),                                    
                        new SqlParameter("@OrganisationState",candidate.OrganisationState),                                    
                        new SqlParameter("@OrganizationAddress",candidate.OrganizationAddress),                                    
                        new SqlParameter("@OrganizationName",candidate.OrganizationName),                                    
                        new SqlParameter("@PartnerName",candidate.PartnerName),                                    
                        new SqlParameter("@PartnerSPOCEmail",candidate.PartnerSPOCEmail),                                    
                        new SqlParameter("@PartnerSPOCMobile",candidate.PartnerSPOCMobile),                                    
                        new SqlParameter("@PartnerSPOCName",candidate.PartnerSPOCName),                                    
                        new SqlParameter("@SectorName",candidate.SectorName),                                    
                        new SqlParameter("@SelfEmployedDistrict",candidate.SelfEmployedDistrict),                                    
                        new SqlParameter("@SelfEmployedState",candidate.SelfEmployedState),                                    
                        new SqlParameter("@SumofSalleryPerMonth",candidate.SumofSalleryPerMonth),                                    
                        new SqlParameter("@SumofTCSPOCMobile",candidate.SumofTCSPOCMobile),                                    
                        new SqlParameter("@TCDistrict",candidate.TCDistrict),                                    
                        new SqlParameter("@TCSPOCEmail",candidate.TCSPOCEmail),                                    
                        new SqlParameter("@TCSPOCName",candidate.TCSPOCName),                                    
                        new SqlParameter("@TCState",candidate.TCState),                                    
                        new SqlParameter("@ToDate",candidate.ToDate),                                    
                        new SqlParameter("@TrainingCentreName",candidate.TrainingCentreName),                                    
                        new SqlParameter("@TrainingType",candidate.TrainingType),                                    
                        new SqlParameter("@CreatedBy",userid),                                    
                    };
                    var result =
                        SqlHelper.ExecuteNonQuery
                        (
                            connection,
                            CommandType.StoredProcedure,
                            "usp_InsertPlacedCandidate",
                            parameters
                            );
                    if (result > 0)
                    {
                        return true;
                    }
                }
                finally
                {
                    SqlHelper.CloseConnection(connection);
                }
            }
            throw new RecordNotAddedException("Unable to Add Records");
        }

        public DataTable GetAllCandidate()
        {
            using (var connection = new SqlConnection(connectionString))
            {
                try
                {
                    var result =
                        SqlHelper.ExecuteReader
                        (
                            connection,
                            CommandType.StoredProcedure,
                            "usp_GetAllPlacedCandidate"
                            );
                    if (result != null && result.HasRows)
                    {
                        var dt = new DataTable();
                        dt.Load(result);
                        return dt;
                    }
                }
                finally
                {
                    SqlHelper.CloseConnection(connection);
                }
            }
            throw new DataNotFound("Data Not Found");
        }
    }
}
