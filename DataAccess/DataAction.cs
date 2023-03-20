//using Msc.Framework.Service.DataAccess.OracleClient;
//using Msc.Framework.Service.DataAccess.Contracts;
using Msc.Framework.Service.DataAccess.Contracts;
using Msc.Framework.Service.DataAccess.OracleClient;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Msc.Agency.DigitalBL.DataAccess
{
    public class DataAction : DbAccess
    {
        /// <summary>
        /// Gets the default name of the connection string.
        /// </summary>
        /// <value>
        /// The default name of the connection string.
        /// </value>
        protected override string DefaultConnectionStringName
        {
            get
            {
                return "Bolero";
            }
        }

        public async Task<bool> SaveZipFileResponse(Msc.Agency.DigitalBL.Models.Rootobject rootobject)
        {
            using (IDbAccess context = this.CreateContext())
            {
                DbParameter[] parameters =
                   {
                          this.CreateParameter(name : "p_in_refno",value : rootobject?.ConnectionModuleMessageHeader?.ConnectionModuleTransaction?.ConnectionModuleTransactionId),
                          this.CreateParameter(name : "p_envelope_id",value : rootobject?.ConnectionModuleMessageHeader?.ConnectionModuleTransaction?.ConnectionModuleTransactionId),
                          this.CreateParameter(name : "p_documentcorrelationid",value : rootobject?.TraInstructionResponses?.TraInstructionResponse?.TraInstructionId),
                          this.CreateParameter(name : "p_accepted_status",value : rootobject?.TraInstructionResponses?.TraInstructionResponse?.Result),
                          this.CreateParameter(name : "p_accepted_date",value : FormatDate(rootobject?.TraInstructionResponses?.TraInstructionResponse?.BblCreationTime)),
                          this.CreateParameter(name : "p_error_code",value : DBNull.Value),
                          this.CreateParameter(name : "p_error_text",value : DBNull.Value),
                          };
                 await Task.Run(() => context.ExecuteNonQuery(storedProcedure: "ibs_bolero_callback.ibs_bolero_success_envelop", parameters: parameters));
            }
            return true;
        }

        public async Task<bool> SaveXmlFileResponse(Rootobject rootobject)
        {
            using (IDbAccess context = this.CreateContext())
            {
                DbParameter[] parameters =
                   {
                          this.CreateParameter(name : "p_in_refno",value : rootobject?.ConnectionModuleMessageHeader?.ConnectionModuleTransaction?.ConnectionModuleTransactionId),
                          this.CreateParameter(name : "p_envelope_id",value : rootobject?.ConnectionModuleMessageHeader?.ConnectionModuleTransaction?.ConnectionModuleTransactionId),
                          this.CreateParameter(name : "p_documentcorrelationid",value : rootobject?.TraInstructionResponses?.TraInstructionResponse?.TraInstructionId),
                          this.CreateParameter(name : "p_accepted_status",value : rootobject?.TraInstructionResponses?.TraInstructionResponse?.Result),
                          this.CreateParameter(name : "p_accepted_date",value : rootobject?.TraInstructionResponses?.TraInstructionResponse?.Rejections?.ConnectionModuleRejection?.ConnectionModuleRejectionTime),
                          this.CreateParameter(name : "p_error_code",value : rootobject?.TraInstructionResponses?.TraInstructionResponse?.Rejections?.ConnectionModuleRejection?.ConnectionModuleRejectionCode),
                          this.CreateParameter(name : "p_error_text",value : rootobject?.TraInstructionResponses?.TraInstructionResponse?.Rejections?.ConnectionModuleRejection?.ConnectionModuleRejectionDescription),
                          };
                await Task.Run(() => context.ExecuteNonQuery(storedProcedure: "ibs_bolero_callback.ibs_bolero_error_envelop", parameters: parameters));
            }
            return true;
        }

        //public async Task<bool> LogUpdation(string response, string transactionId)
        //{
        //    using (IDbAccess context = this.CreateContext())
        //    {
        //        DbParameter[] parameters =
        //           {
        //                  this.CreateClobParameter(name : "p_req",value : response, direction: ParameterDirection.Input),
        //                  this.CreateClobParameter(name : "p_res",value : response, direction: ParameterDirection.Input),
        //                  this.CreateParameter(name : "p_session",value : transactionId),
        //                  };
        //        await Task.Run(() => context.ExecuteNonQuery(storedProcedure: "ibs_bolero_callback.ibs_bolero_wblog_updation", parameters: parameters));
        //    }
        //        //await this.LogUpdation(context, response);

        //    return true;
        //}

        public async Task<bool> LogUpdation(string response, string transactionId, string fileType,string ConnectionString,string pdffile)
        {
            using (IDbAccess context = this.CreateContext(ConnectionString))
            {
                DbParameter[] parameters =
                   {
                          this.CreateClobParameter(name : "p_req",value : response, direction: ParameterDirection.Input),
                          this.CreateClobParameter(name : "p_res",value : response, direction: ParameterDirection.Input), 
                          this.CreateClobParameter(name : "p_pdffile",value : pdffile, direction: ParameterDirection.Input),
                          this.CreateParameter(name : "p_session",value : transactionId),
                          this.CreateParameter(name : "p_fileType",value : fileType),
                          };
                await Task.Run(() => context.ExecuteNonQuery(storedProcedure: "ibs_bolero_callback.ibs_bolero_wblog_updation", parameters: parameters));
            }
            //await this.LogUpdation(context, response);

            return true;
        }

        private string FormatDate(string d)
        {
            DateTime converted = Convert.ToDateTime(d);
            DateTime thisDate;
            string dateFormat;
            if (converted.Date == default(DateTime))
            {
                return null;
            }
            else if (converted.Date.Kind != DateTimeKind.Utc)
            {
                thisDate = converted.Date;
                dateFormat = thisDate.ToString("dd/MMM/yyyy");
                return dateFormat;
            };
            return null;
        }
    }
}