using Ecpay.Models;
using System.Data;
using System.Data.SqlClient;

namespace Ecpay.Services
{
    public class OrderService
    {
        public OrderModel? GetOrderById(int orderId)
        {
            using ( SqlConnection sqlCon = new SqlConnection( ConnectionStrings.Ecpay ) )
            {
                sqlCon.Open();

                string queryOrderSql =
                    """
                      SELECT 
                            Id,
                            OrderNo,
                            TotalAmount,
                            TradeDesc,
                            ItemName,
                            Status,
                            MerchantTradeNo,
                            TradeNo,
                            PaymentType,
                            PaymentDate
                    FROM EcpayOrders
                    WHERE Id = @Id; 
                    """;

                using ( SqlCommand sqlCmd = new SqlCommand( queryOrderSql , sqlCon ) )
                {
                    sqlCmd.Parameters.AddWithValue( "@Id" , orderId );
                    using ( SqlDataReader reader = sqlCmd.ExecuteReader() )
                    {
                        if ( reader.Read() )
                        {
                            return new OrderModel
                            {
                                Id = reader.GetInt32( 0 ) ,
                                OrderNo = reader.GetString( 1 ) ,
                                TotalAmount = reader.GetInt32( 2 ) ,
                                TradeDesc = reader.GetString( 3 ) ,
                                ItemName = reader.GetString( 4 ) ,
                                Status = reader.GetString( 5 ) ,
                                MerchantTradeNo = reader.GetString( 6 ) ,
                                TradeNo = reader.GetString( 7 ) ,
                                PaymentType = reader.GetString( 8 ) ,
                                PaymentDate = reader.GetString( 9 )
                            };
                        }
                        else
                        {
                            return null;
                        }

                    }
                }
            }
        }

        public OrderModel? GetMerchantTradeNo(string merchantTradeNo)
        {


            using ( SqlConnection sqlCon = new SqlConnection( ConnectionStrings.Ecpay ) )
            {
                sqlCon.Open();

                string queryOrderSql =
                   """
                      SELECT 
                            Id,
                            OrderNo,
                            TotalAmount,
                            TradeDesc,
                            ItemName,
                            Status,
                            MerchantTradeNo,
                            TradeNo,
                            PaymentType,
                            PaymentDate
                    FROM EcpayOrders
                    WHERE MerchantTradeNo = @MerchantTradeNo; 
                    """;

                using ( SqlCommand sqlCmd = new SqlCommand( queryOrderSql , sqlCon ) )
                {
                    sqlCmd.Parameters.AddWithValue( "@MerchantTradeNo" , merchantTradeNo );
                    using var reader = sqlCmd.ExecuteReader();

                    if ( !reader.Read() )
                        return null;

                    return new OrderModel
                    {
                        Id = reader.GetInt32( 0 ) ,
                        OrderNo = reader.GetString( 1 ) ,
                        TotalAmount = reader.GetInt32( 2 ) ,
                        TradeDesc = reader.GetString( 3 ) ,
                        ItemName = reader.GetString( 4 ) ,
                        Status = reader.GetString( 5 ) ,
                        MerchantTradeNo = reader.GetString( 6 ) ,
                        TradeNo = reader.GetString( 7 ) ,
                        PaymentType = reader.GetString( 8 ) ,
                        PaymentDate = reader.GetString( 9 )
                    };
                }
            }
        }


        public void UpdateMerchantTradeNo(int orderId , string merchantTradeNo)
        {
            using ( SqlConnection sqlCon = new SqlConnection( ConnectionStrings.Ecpay ) )
            {
                sqlCon.Open();
                string updateSql =
                    """
                    UPDATE EcpayOrders
                    SET MerchantTradeNo = @MerchantTradeNo
                    WHERE Id = @Id
                    """;
                using ( SqlCommand sqlCmd = new SqlCommand( updateSql , sqlCon ) )
                {
                    sqlCmd.Parameters.AddWithValue( "@MerchantTradeNo" , merchantTradeNo );
                    sqlCmd.Parameters.AddWithValue( "@Id" , orderId );
                    sqlCmd.ExecuteNonQuery();
                }
            }
        }

        public void UpdatePaid(int orderId)
        {
            using ( SqlConnection sqlCon = new SqlConnection( ConnectionStrings.Ecpay ) )
            {
                sqlCon.Open();
                string updateSql =
                    """
                    UPDATE EcpayOrders
                    SET Status = @Status
                    WHERE Id = @Id
                    """;
                using ( SqlCommand sqlCmd = new SqlCommand( updateSql , sqlCon ) )
                {
                    sqlCmd.Parameters.AddWithValue( "@Status" , "Paid" );
                    sqlCmd.Parameters.AddWithValue( "@Id" , orderId );
                    sqlCmd.ExecuteNonQuery();
                }
            }

        }

    }
}

