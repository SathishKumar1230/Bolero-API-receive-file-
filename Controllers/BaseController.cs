using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace Msc.Agency.DigitalBL.Controllers
{
    public class BaseApiController<T> : ApiController where T : class
    {
        private static readonly Lazy<T> businessInstance = new Lazy<T>(GetBusiness, System.Threading.LazyThreadSafetyMode.ExecutionAndPublication);
        /// <summary>
        /// Gets the reader.
        /// </summary>
        /// <returns></returns>
        private static T GetBusiness()
        {
            return Activator.CreateInstance<T>();
        }
        /// <summary>
        /// Gets the leave request reader.
        /// </summary>
        /// <value>
        /// The leave request reader.
        /// </value>
        protected static T Resolve
        {
            get
            {
                return businessInstance.Value;
            }
        }
    }
}