using System;
using CloudAbstractions.Security;

namespace CloudAbstractions
{
    public static class TableStorageClientUtilities
    {
        /// <summary>
        /// Creates the tables needed for the specified service context.
        /// 
        /// </summary>
        /// <param name="serviceContextType">The type of service context.</param><param name="baseAddress">The Table service endpoint to use to create the client.</param><param name="credentials">The account credentials.</param>
        public static void CreateTablesFromModel(this IKvStorageProvider storage, Type serviceContextType, string baseAddress, ICredentials credentials)
        {
/*
            foreach (string tableName in storage.EnumerateEntitySetNames(serviceContextType))
                storage.CreateTableIfNotExist(tableName);
*/
        }
    }
}