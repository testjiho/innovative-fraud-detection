using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using TransactionGenerator;

namespace TransactionGenerator
{
    [JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy))]
    public class Transaction
    {
        [JsonProperty] public string TransactionID { get; set; }

        [JsonProperty] public string AccountID { get; set; }

        [JsonProperty] public double TransactionAmountUSD { get; set; }

        [JsonProperty] public double TransactionAmount { get; set; }

        [JsonProperty] public string TransactionCurrencyCode { get; set; }

        [JsonProperty] public string TransactionCurrencyConversionRate { get; set; }

        [JsonProperty] public int TransactionDate { get; set; }

        [JsonProperty] public int TransactionTime { get; set; }

        [JsonProperty] public int LocalHour { get; set; }

        [JsonProperty] public string TransactionScenario { get; set; }

        [JsonProperty] public string TransactionType { get; set; }

        [JsonProperty] public string TransactionMethod { get; set; }

        [JsonProperty] public string TransactionDeviceType { get; set; }

        [JsonProperty] public string TransactionDeviceId { get; set; }

        [JsonProperty] public string TransactionIPaddress { get; set; }

        [JsonProperty] public string IpState { get; set; }

        [JsonProperty] public string IpPostcode { get; set; }

        [JsonProperty] public string IpCountryCode { get; set; }

        [JsonProperty] public bool? IsProxyIP { get; set; }

        [JsonProperty] public string BrowserType { get; set; }

        [JsonProperty] public string BrowserLanguage { get; set; }

        [JsonProperty] public string PaymentInstrumentType { get; set; }

        [JsonProperty] public string CardType { get; set; }

        [JsonProperty] public string CardNumberInputMethod { get; set; }

        [JsonProperty] public string PaymentInstrumentID { get; set; }

        [JsonProperty] public string PaymentBillingAddress { get; set; }

        [JsonProperty] public string PaymentBillingPostalCode { get; set; }

        [JsonProperty] public string PaymentBillingState { get; set; }

        [JsonProperty] public string PaymentBillingCountryCode { get; set; }

        [JsonProperty] public string PaymentBillingName { get; set; }

        [JsonProperty] public string ShippingAddress { get; set; }

        [JsonProperty] public string ShippingPostalCode { get; set; }

        [JsonProperty] public string ShippingCity { get; set; }

        [JsonProperty] public string ShippingState { get; set; }

        [JsonProperty] public string ShippingCountry { get; set; }

        [JsonProperty] public string CvvVerifyResult { get; set; }

        [JsonProperty] public string ResponseCode { get; set; }

        [JsonProperty] public int DigitalItemCount { get; set; }

        [JsonProperty] public int PhysicalItemCount { get; set; }

        [JsonProperty] public string PurchaseProductType { get; set; }

        // Used to set expiration policy
        [JsonProperty(PropertyName = "ttl", NullValueHandling = NullValueHandling.Ignore)]
        public int? TimeToLive { get; set; }

        [JsonIgnore]
        public string PartitionKey => IpCountryCode;

        [JsonIgnore]
        protected string CsvHeader { get; set; }

        [JsonIgnore]
        protected string CsvString { get; set; }

        public string GetData()
        {
            return JsonConvert.SerializeObject(this);
        }

        public static Transaction FromString(string line, string header)
        {
            if (string.IsNullOrWhiteSpace(line))
            {
                throw new ArgumentException($"{nameof(line)} cannot be null, empty, or only whitespace");
            }

            var tokens = line.Split(',');
            if (tokens.Length != 40)
            {
                throw new ArgumentException($"Invalid record: {line}");
            }

            var tx = new Transaction
            {
                CsvString = line,
                CsvHeader = header
            };
            try
            {
                tx.TransactionID = tokens[0];
                tx.AccountID = tokens[1];
                tx.TransactionAmountUSD = double.TryParse(tokens[2], out var dresult) ? dresult : 0.0;
                tx.TransactionAmount = double.TryParse(tokens[3], out dresult) ? dresult : 0.0;
                tx.TransactionCurrencyCode = tokens[4];
                tx.TransactionCurrencyConversionRate = tokens[5];
                tx.TransactionDate = int.TryParse(tokens[6], out var iresult) ? iresult : 0;
                tx.TransactionTime = int.TryParse(tokens[7], out iresult) ? iresult : 0;
                tx.LocalHour = int.TryParse(tokens[8], out iresult) ? iresult : 0;
                tx.TransactionScenario = tokens[9];
                tx.TransactionType = tokens[10];
                tx.TransactionMethod = tokens[11];
                tx.TransactionDeviceType = tokens[12];
                tx.TransactionDeviceId = tokens[13];
                tx.TransactionIPaddress = tokens[14];
                tx.IpState = tokens[15];
                tx.IpPostcode = tokens[16];
                tx.IpCountryCode = string.IsNullOrWhiteSpace(tokens[17]) ? "unk" : tokens[17];
                tx.IsProxyIP = bool.TryParse(tokens[18], out var bresult) && bresult;
                tx.BrowserType = tokens[19];
                tx.BrowserLanguage = tokens[20];
                tx.PaymentInstrumentType = tokens[21];
                tx.CardType = tokens[22];
                tx.CardNumberInputMethod = tokens[23];
                tx.PaymentInstrumentID = tokens[24];
                tx.PaymentBillingAddress = tokens[25];
                tx.PaymentBillingPostalCode = tokens[26];
                tx.PaymentBillingState = tokens[27];
                tx.PaymentBillingCountryCode = tokens[28];
                tx.PaymentBillingName = tokens[29];
                tx.ShippingAddress = tokens[30];
                tx.ShippingPostalCode = tokens[31];
                tx.ShippingCity = tokens[32];
                tx.ShippingState = tokens[33];
                tx.ShippingCountry = tokens[34];
                tx.CvvVerifyResult = tokens[35];
                tx.ResponseCode = tokens[36];
                tx.DigitalItemCount = int.TryParse(tokens[37], out iresult) ? iresult : 0;
                tx.PhysicalItemCount = int.TryParse(tokens[38], out iresult) ? iresult : 0;
                tx.PurchaseProductType = tokens[39];

                // Set the TTL value to 60 days, which deletes the document in Cosmos DB after around two months, saving
                // storage costs while meeting Woodgrove Bank's requirement to keep the streaming data available for
                // that amount of time so they can reprocess or query the raw data within the collection as needed.
                // TODO 7: Complete the code to set the time to live (TTL) value to 60 days (in seconds).
                // COMPLETE THIS CODE ... tx.TimeToLive = ...

                return tx;
            }
            catch (Exception ex)
            {
                throw new ArgumentException($"Invalid record: {line}", ex);
            }
        }
    }
}
