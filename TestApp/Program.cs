using System;
using System.Net.Http;
using Ekin.Clarizen;

namespace TestApp
{
    public class Program
    {
        static void Main(string[] args)
        {
            Call<dynamic> call = new Call<dynamic>()
            {
                _url = "https://timeapi.roar.pub/utils/tests/timeout/5000",
                _method = HttpMethod.Get
            };
            bool result = call.Execute().Result;
            
        }
    }
}
