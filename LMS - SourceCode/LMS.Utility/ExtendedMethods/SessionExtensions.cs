using LMS.Utility.Helpers;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace LMS.Utility.ExtendedMethods
{
    public static class SessionExtensions
    {
        public static void Set<T>(this ISession session,string key,T data){
            var bFormatter = new BinaryFormatter();
            using (var mStream=new MemoryStream())
            {
                bFormatter.Serialize(mStream, data);
                session.Set(key, mStream.ToArray());
            }            
        }

        public static T Get<T>(this ISession session, string key)
        {
            byte[] arr = null;
            if(session.TryGetValue(key, out arr))
            {
                var bFormatter = new BinaryFormatter();
                using (var mStream = new MemoryStream(arr))
                {
                    return (T)bFormatter.Deserialize(mStream);
                }
            }
            return default(T);
        }

        public static bool IsSessionExpired(this ISession session)
        {
            bool isExpired = false;
            byte[] arr = null;
            if (!session.TryGetValue(Constants.SessionKeyUserInfo, out arr))
            {
                isExpired = true;
            }
            return isExpired;
        }
    }
}
