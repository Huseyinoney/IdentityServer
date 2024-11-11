using Business.Abstract;
using IdentityModel.Client;
using IdentityServer4.Models;
using IdentityServer4.Services;
using IdentityServer4.Stores;
using IdentityServer4.Validation;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Business.Concrete
{
    public class AuthManager : IAuthService
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly HttpClient _client;
        private readonly RoleManager<IdentityRole> roleManager;
        public AuthManager(UserManager<IdentityUser> userManager, HttpClient httpClient, RoleManager<IdentityRole> roleManager)
        {
            this.userManager = userManager;
            
            this._client = httpClient;
            this.roleManager = roleManager;
        }
        public async Task<string> LoginAsync(string username, string password)
        {
            try
            {
                // IdentityServer'ın Discovery dokümanına ulaş
                var disco = await _client.GetDiscoveryDocumentAsync("http://localhost:5164"); // IdentityServer URL'niz
                if (disco.IsError)
                {
                    throw new Exception($"Discovery document error: {disco.Error}");
                }

                // Kullanıcı adı ve şifre ile token almak için request oluştur
                var tokenResponse = await _client.RequestPasswordTokenAsync(new PasswordTokenRequest
                {
                    Address = disco.TokenEndpoint, // Token endpoint'i, discovery dokümanından alınır
                    ClientId = "client", // Config.cs'deki ClientId ile aynı olmalı
                    ClientSecret = "secret", // Config.cs'deki ClientSecret ile aynı olmalı
                    Scope = "api1", // Config.cs'de tanımlı olan Scope
                    UserName = username,
                    Password = password
                });

                // Eğer token alma işlemi başarısızsa hata fırlat
                if (tokenResponse.IsError)
                {
                    throw new Exception($"Token request error: {tokenResponse.Error}");
                }

                // Token başarıyla alındığında access token'ı döndür
                return tokenResponse.AccessToken;
            }
            catch (Exception ex)
            {
                // Hata oluşursa yakala ve bildir
                throw new Exception("Kullanıcı Giriş Yaparken Hata Oluştu: " + ex.Message);
            }
        }

        public async Task<bool> RegisterAsync(string Username, string Password)
        {
            try
            {
                var user = new IdentityUser { UserName = Username };
                var result = await userManager.CreateAsync(user, Password);
                if (!result.Succeeded)
                {
                    return false;
                }
               

                var roleExists = await roleManager.RoleExistsAsync("user");
                if(!roleExists)
                {
                    await roleManager.CreateAsync(new IdentityRole("user"));
                }

                await userManager.AddToRoleAsync(user, "user");
                return true;
            }
            catch (Exception ex)
            {

                throw new Exception("Kullanıcı Kayıt Edilirken Bir Hata Oluştu...", ex);
            }

        }
        public Task<bool> LogOut(string Username)
        {
            throw new NotImplementedException();
        }
    }
}
