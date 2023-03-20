using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Msc.Agency.DigitalBL.Business
{
    public class BaseBusiness<T> where T : class
    {
        private static readonly Lazy<T> DataInstance = new Lazy<T>(GetData, System.Threading.LazyThreadSafetyMode.ExecutionAndPublication);

        /// <summary>
        /// Gets the data reader.
        /// </summary>
        /// <value>
        /// The data reader.
        /// </value>
        protected static T Resolve
        {
            get
            {
                return DataInstance.Value;
            }
        }

        /// <summary>
        /// Gets the data reader.
        /// </summary>
        /// <returns>Generic Type.</returns>
        private static T GetData()
        {
            return Activator.CreateInstance<T>();
        }
    }
}