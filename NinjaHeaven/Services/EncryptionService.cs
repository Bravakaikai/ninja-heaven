using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NinjaHeaven.Models;

namespace NinjaHeaven.Services
{
    public static class EncryptionService
    {
        public static string Encrypt(String password)
        {
            var encrypt = ExpressEncription.RSAEncription.EncryptString(password, @"Server/Keys/Encryption/public.key");
            return encrypt;
        }

        public static string Decrypt(String encryptString)
        {
            var decrypt = ExpressEncription.RSAEncription.DecryptString(encryptString, @"Server/Keys/Encryption/private.key");
            return decrypt;
        }
    }
}
