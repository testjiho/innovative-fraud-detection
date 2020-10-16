using System;
using System.Collections.Generic;
using System.Text;

namespace CustomerProfileJsonDataGenerator.Models
{
    public class AccountData
    {
        public string AccountID { get; set; }
        public string AccountPostalCode { get; set; }
        public string AccountState { get; set; }
        public string AccountCountry { get; set; }
        public int AccountAge { get; set; }
        public bool IsUserRegistered { get; set; }

        protected string CsvHeader { get; set; }

        protected string CsvString { get; set; }

        public static AccountData FromString(string line, string header)
        {
            if (string.IsNullOrWhiteSpace(line))
            {
                throw new ArgumentException($"{nameof(line)} cannot be null, empty, or only whitespace");
            }

            var tokens = line.Split(',');
            if (tokens.Length != 6)
            {
                throw new ArgumentException($"Invalid record: {line}");
            }

            var accountData = new AccountData
            {
                CsvString = line,
                CsvHeader = header
            };
            try
            {
                accountData.AccountID = tokens[0];
                accountData.AccountPostalCode = tokens[1];
                accountData.AccountState = tokens[2];
                accountData.AccountCountry = tokens[3];
                accountData.AccountAge = int.TryParse(tokens[4], out var iresult) ? iresult : 0;
                accountData.IsUserRegistered = bool.TryParse(tokens[5], out var bresult) && bresult;

                return accountData;
            }
            catch (Exception ex)
            {
                throw new ArgumentException($"Invalid record: {line}", ex);
            }
        }
    }
}
