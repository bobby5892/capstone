using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PeerIt.Interfaces
{
    interface IGenericRepository<T,J>
    {
        /// <summary>
        /// Lookup a Model byID
        /// </summary>
        /// <param name="ID">Pass</param>
        /// <returns></returns>
        T FindByID(J ID);

        /// <summary>
        /// Get a List of Models back
        /// </summary>
        /// <returns></returns>
        List<T> GetAll();

        /// <summary>
        /// Edit a Model
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        bool Edit(T model);

        /// <summary>
        /// Delete a Model
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        bool Delete(T model);
        

        /// <summary>
        /// Add a New Model and save
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        T Add(T model);

    }
}
