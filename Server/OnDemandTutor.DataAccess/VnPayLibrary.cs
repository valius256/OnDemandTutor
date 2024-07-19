using Microsoft.AspNetCore.Http;
using OnDemandTutor.Models.Dtos.Payment;
using System.Globalization;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;

namespace OnDemandTutor.DataAccess.Repository;


public class VnPayLibrary
{
    private readonly SortedList<string, string> _requestData = new SortedList<string, string>(new VnPayCompare());
    private readonly SortedList<string, string> _responseData = new SortedList<string, string>(new VnPayCompare());

    public PaymentSlotResponseModel GetFullResponseData(IQueryCollection collection, string hashSecret)
    {
        var vnPay = new VnPayLibrary();

        foreach (var (key, value) in collection)
        {
            if (!string.IsNullOrEmpty(key) && key.StartsWith("vnp_"))
            {
                vnPay.AddResponseData(key, value);
            }
        }

        var orderId = Convert.ToInt64(vnPay.GetResponseData("vnp_TxnRef"));
        var vnPayTranId = Convert.ToInt64(vnPay.GetResponseData("vnp_TransactionNo"));
        var vnpResponseCode = vnPay.GetResponseData("vnp_ResponseCode");
        var vnpSecureHash = collection.FirstOrDefault(k => k.Key == "vnp_SecureHash").Value;
        var money = vnPay.GetResponseData("vnp_Amount");
        var orderDescription = vnPay.GetResponseData("vnp_OrderDescription");
        var orderInfo = vnPay.GetResponseData("vnp_OrderInfo");
        // Split the orderInfo string
        var orderInfoParts = orderInfo.Split('|');

        bool isRecharge = false;
        if (orderInfoParts.Length > 0)
        {
            bool.TryParse(orderInfoParts[0], out isRecharge);
        }

        var description = orderInfoParts.Length > 1 ? orderInfoParts[1] : "";
        var userId = orderInfoParts.Length > 2 ? Convert.ToInt32(orderInfoParts[2]) : 0;

        int? classId = null;
        if (orderInfoParts.Length > 3 && !string.IsNullOrEmpty(orderInfoParts[3]))
        {
            if (int.TryParse(orderInfoParts[3], out int tempClassId))
            {
                classId = tempClassId;
            }
        }

        var returnUrl = orderInfoParts.Length > 4 ? orderInfoParts[4] : "";

        // Parse slotIds
        List<int> slotIds = new List<int>();
        if (orderInfoParts.Length > 5)
        {
            var slotIdParts = orderInfoParts[5].Split(' '); // Corrected to index 5

            foreach (var slotIdPart in slotIdParts)
            {
                if (!string.IsNullOrEmpty(slotIdPart) && int.TryParse(slotIdPart, out int tempSlotId))
                {
                    slotIds.Add(tempSlotId);
                }
            }
        }

        var checkSignature = vnPay.ValidateSignature(vnpSecureHash, hashSecret);

        if (!checkSignature)
        {
            return new PaymentSlotResponseModel()
            {
                Success = false
            };
        }

        var rs1 = new PaymentSlotResponseModel()
        {
            Success = true,
            PaymentMethod = "VnPay",
            OrderDescription = orderDescription,
            TransactionCode = orderId.ToString(),
            Token = vnpSecureHash,
            VnPayResponseCode = vnpResponseCode,
            SlotId = slotIds,
            UserId = userId,
            ClassId = classId,
            Money = (decimal.Parse(money) / 100),
            IsRechargePayment = isRecharge,
            RedirectResult = returnUrl
        };
        return rs1;
    }

    public string GetIpAddress(HttpContext context)
    {
        var ipAddress = string.Empty;
        try
        {
            var remoteIpAddress = context.Connection.RemoteIpAddress;

            if (remoteIpAddress != null)
            {
                if (remoteIpAddress.AddressFamily == AddressFamily.InterNetworkV6)
                {
                    remoteIpAddress = Dns.GetHostEntry(remoteIpAddress).AddressList
                        .FirstOrDefault(x => x.AddressFamily == AddressFamily.InterNetwork);
                }

                if (remoteIpAddress != null) ipAddress = remoteIpAddress.ToString();

                return ipAddress;
            }
        }
        catch (Exception ex)
        {
            return ex.Message;
        }

        return "127.0.0.1";
    }
    public void AddRequestData(string key, string value)
    {
        if (!string.IsNullOrEmpty(value))
        {
            _requestData.Add(key, value);
        }
    }

    public void AddResponseData(string key, string value)
    {
        if (!string.IsNullOrEmpty(value))
        {
            _responseData.Add(key, value);
        }
    }

    public string GetResponseData(string key)
    {
        return _responseData.TryGetValue(key, out var retValue) ? retValue : string.Empty;
    }

    public string CreateRequestUrl(string baseUrl, string vnpHashSecret)
    {
        var data = new StringBuilder();

        foreach (var (key, value) in _requestData.Where(kv => !string.IsNullOrEmpty(kv.Value)))
        {
            data.Append(WebUtility.UrlEncode(key) + "=" + WebUtility.UrlEncode(value) + "&");
        }

        var querystring = data.ToString();

        baseUrl += "?" + querystring;
        var signData = querystring;
        if (signData.Length > 0)
        {
            signData = signData.Remove(data.Length - 1, 1);
        }

        var vnpSecureHash = HmacSha512(vnpHashSecret, signData);
        baseUrl += "vnp_SecureHash=" + vnpSecureHash;

        return baseUrl;
    }

    public bool ValidateSignature(string inputHash, string secretKey)
    {
        var rspRaw = GetResponseData();
        var myChecksum = HmacSha512(secretKey, rspRaw);
        return myChecksum.Equals(inputHash, StringComparison.InvariantCultureIgnoreCase);
    }

    private string HmacSha512(string key, string inputData)
    {
        var hash = new StringBuilder();
        var keyBytes = Encoding.UTF8.GetBytes(key);
        var inputBytes = Encoding.UTF8.GetBytes(inputData);
        using (var hmac = new HMACSHA512(keyBytes))
        {
            var hashValue = hmac.ComputeHash(inputBytes);
            foreach (var theByte in hashValue)
            {
                hash.Append(theByte.ToString("x2"));
            }
        }

        return hash.ToString();
    }

    private string GetResponseData()
    {
        var data = new StringBuilder();
        if (_responseData.ContainsKey("vnp_SecureHashType"))
        {
            _responseData.Remove("vnp_SecureHashType");
        }

        if (_responseData.ContainsKey("vnp_SecureHash"))
        {
            _responseData.Remove("vnp_SecureHash");
        }

        foreach (var (key, value) in _responseData.Where(kv => !string.IsNullOrEmpty(kv.Value)))
        {
            data.Append(WebUtility.UrlEncode(key) + "=" + WebUtility.UrlEncode(value) + "&");
        }

        //remove last '&'
        if (data.Length > 0)
        {
            data.Remove(data.Length - 1, 1);
        }

        return data.ToString();
    }
}

public class VnPayCompare : IComparer<string>
{
    public int Compare(string x, string y)
    {
        if (x == y) return 0;
        if (x == null) return -1;
        if (y == null) return 1;
        var vnpCompare = CompareInfo.GetCompareInfo("en-US");
        return vnpCompare.Compare(x, y, CompareOptions.Ordinal);
    }
}