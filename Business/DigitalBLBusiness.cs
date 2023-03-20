using Msc.Agency.DigitalBL.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using System.Web;

namespace Msc.Agency.DigitalBL.Business
{
    public class DigitalBLBusiness : BaseBusiness<DataAction>
    {
        public async static Task<bool> SaveZipFileResponse(Msc.Agency.DigitalBL.Models.Rootobject rootobject)
        {
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                var dbSave = await Resolve.SaveZipFileResponse(rootobject);
                scope.Complete();
            }
            return true;
        }

        public async static Task<bool> SaveXmlFileResponse(Rootobject rootobject)
        {
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                var dbSave = await Resolve.SaveXmlFileResponse(rootobject);
               // DigitalBLBusiness.LogUpdation(res);

                scope.Complete();
            }
            return true;
        }

        //public async static Task<bool> LogUpdation(string response, string transactionId)
        //{
        //    using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
        //    {
        //        var dbSave = await Resolve.LogUpdation(response, transactionId);
        //        scope.Complete();
        //    }
        //    return true;
        //}

        public async static Task<bool> LogUpdation(string response, string transactionId, string fileType,string ConnectionString,string pdffile)
        {
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                var dbSave = await Resolve.LogUpdation(response, transactionId, fileType,ConnectionString,pdffile);
                scope.Complete();
            }
            return true;
        }

    }
}