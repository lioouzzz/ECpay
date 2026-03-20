using Ecpay.Models;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;

namespace Ecpay.Services
{
    public class CreditCardService
    {
        private readonly EcpaySettingModel _ecpaySettings;

        public CreditCardService(EcpaySettingModel ecpaySettings)
        {
            _ecpaySettings = ecpaySettings;
        }

        public CreditCardModel CreateCardOrder(OrderModel orderRequest)
        {
            var creditCardModel = new CreditCardModel
            {
                MerchantID = _ecpaySettings.MerchantID ,
                MerchantTradeNo = orderRequest.MerchantTradeNo ,
                MerchantTradeDate = DateTime.Now.ToString( "yyyy/MM/dd HH:mm:ss" ) ,
                TotalAmount = orderRequest.TotalAmount ,
                TradeDesc = orderRequest.TradeDesc ,
                ItemName = orderRequest.ItemName ,
                ReturnURL = _ecpaySettings.ReturnURL ,
                OrderResultURL = _ecpaySettings.OrderResultURL ,
                ClientBackURL = _ecpaySettings.ClientBackURL
            };



            if ( creditCardModel.MerchantTradeNo.Length > 20 )

                //只取20個字元
                creditCardModel.MerchantTradeNo = creditCardModel.MerchantTradeNo.Substring( 0 , 20 );

            creditCardModel.CheckMacValue = GenerateCheckMacValue( creditCardModel.ToDictionary() );
            return creditCardModel;
        }

        public bool ValidateCallback(IFormCollection form)
        {
            var data = new Dictionary<string , string>();

            foreach ( var item in form )
            {
                data [item.Key] = item.Value.ToString();
            }

            if ( !data.ContainsKey( "CheckMacValue" ) ) 
            {
                return false;
            }
            string checkMacValue = data ["CheckMacValue"];

            data.Remove( "CheckMacValue" );
            string newCheckMacValue= GenerateCheckMacValue( data );
            return string.Equals( checkMacValue , newCheckMacValue , StringComparison.OrdinalIgnoreCase );

        }

        public string GenerateCheckMacValue(Dictionary<string , string> data)
        {
            var hashKey = _ecpaySettings.HashKey;
            var hashIV = _ecpaySettings.HashIV;

            var sortedData = data
                                       .OrderBy( x => x.Key )
                                        .Select( x => $"{x.Key}={x.Value}" );


            var rawString = $"HashKey={hashKey}&{string.Join( "&" , sortedData )}&HashIV={hashIV}";

            // 字串進行 URL 編碼
            var encoded = Uri.EscapeDataString( rawString )
                .Replace( "%20" , "+" )
                .Replace( "%21" , "!" )
                .Replace( "%28" , "(" )
                .Replace( "%29" , ")" )
                .Replace( "%2A" , "*" )
                .Replace( "%2D" , "-" )
                .Replace( "%2E" , "." )
                .Replace( "%5F" , "_" )
                .ToLower();


            using var sha256 = SHA256.Create();
            var bytes = sha256.ComputeHash( Encoding.UTF8.GetBytes( encoded ) );
            return BitConverter.ToString( bytes ).Replace( "-" , "" ).ToUpper();
        }
    }
}

