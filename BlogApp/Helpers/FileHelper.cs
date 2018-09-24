using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace BlogApp.Helpers
{
    public class FileHelper
    {
        public static string MD5String(HttpPostedFileBase file)
        {
            Stream stream = file.InputStream;
            stream.Position = 0;

            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();

            byte[] buffer = md5.ComputeHash(stream);

            return BitConverter.ToString(buffer).Replace("-", string.Empty);
        }
    }
}