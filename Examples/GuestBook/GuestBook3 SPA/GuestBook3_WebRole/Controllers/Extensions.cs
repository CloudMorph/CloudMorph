using System;
using System.Collections.Generic;
using System.Net.Http;

namespace GuestBook3_WebRole.Controllers
{
    public static class Extensions
    {
        public static bool TryGetFormFieldValue(this IEnumerable<HttpContent> contents, string dispositionName,
                                                out string formFieldValue)
        {
/*
            if (contents == null)
            {
                throw new ArgumentNullException("contents");
            }

            HttpContent content = contents.FirstDispositionNameOrDefault(dispositionName);
            if (content != null)
            {
                formFieldValue = content.ReadAsStringAsync().Result;
                return true;
            }

*/
            formFieldValue = null;
            return false;
        }
    }
}