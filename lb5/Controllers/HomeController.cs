using lb5.Models;
using lb5.MyShop;
using LiqPay.SDK;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using XSystem.Security.Cryptography;
using static System.Collections.Specialized.BitVector32;

namespace lb5.Controllers
{
    public class HomeController : Controller
    {
        private readonly LiqPayKeysModel liqpayDataOptions;

        public HomeController(IOptions<LiqPayKeysModel> liqpayDataOptions)
        {
            this.liqpayDataOptions = liqpayDataOptions.Value;
        }

        public IActionResult Index()
        {
            Shop shop = new Shop();
            return View(Shop.Order);
        }

        public IActionResult Privacy()
        {
            double sum = Shop.sum();
            var liqPayClient = new LiqPayClient(liqpayDataOptions.PublicKey, liqpayDataOptions.PrivateKey);
            var data = liqPayClient.GenerateDataAndSignature(new LiqPay.SDK.Dto.LiqPayRequest()
            {
                Version = 3,
                ActionPayment = LiqPay.SDK.Dto.Enums.LiqPayRequestActionPayment.Pay,
                Action = LiqPay.SDK.Dto.Enums.LiqPayRequestAction.Pay,
                Amount = sum,
                Currency = "USD",
                Description = "Buy products",
                OrderId = Guid.NewGuid().ToString()
            });

            return View(new HomePaymentInfoModel()
            {
                Data = data.Key.ToBase64String(),
                Signature = data.Value.ToBase64String()
            });
        }

        public void Test()
        {
            Debug.WriteLine("test success");
            Shop.dicreaseAmount();
        }

        public IActionResult MinusP(string id)
        {
            int idi = Convert.ToInt32(id);
            if (Shop.Order[idi].AmountInOrder > 0)
            {
                Shop.Order[idi].AmountInOrder--;
            }
            return View("Index", Shop.Order);
        }

        public IActionResult PlusP(string id)
        {
            int idi = Convert.ToInt32(id);
            if (Shop.Order[idi].AmountInOrder < Shop.Order[idi].CurProduct.Amount)
            {
                Shop.Order[idi].AmountInOrder++;
            }
            return View("Index", Shop.Order);
        }

        //// GET: Hostels/Details/5
        //public async Task<IActionResult> Plus(string title)
        //{
        //    Debug.WriteLine("title ==> " + title);
        //    for (int i = 0; i < Shop.Order.Count; i++)
        //    {
        //        if (Shop.Order[i].CurProduct.Title == title)
        //        {
        //            Shop.Order[i].AmountInOrder++;
        //        }
        //    }

        //  //  return RedirectPermanent("/Home/Index");
        //  return View();
        //}

        public string Hash(byte[] temp)
        {
            using (SHA1Managed sha1 = new SHA1Managed())
            {
                var hash = sha1.ComputeHash(temp);
                return Convert.ToBase64String(hash);
            }
        }

        //[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        //public IActionResult Error()
        //{
        //    return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        //}
    }
}
