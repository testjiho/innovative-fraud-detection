using System;
using System.Collections.Generic;
using System.Text;
using Bogus;
using Bogus.DataSets;
using CustomerProfileJsonDataGenerator.Models;
using Newtonsoft.Json;

namespace CustomerProfileJsonDataGenerator
{
    public sealed class User : IHasRandomizer, IHasContext
    {
        internal Dictionary<string, object> context = new Dictionary<string, object>();
        private SeedNotifier Notifier = new SeedNotifier();
        private Randomizer randomizer;
        private AccountData accountData;

        public User(Randomizer randomizer, AccountData ad, string locale = "en")
        {
            GetDataSources(locale);
            accountData = ad;
            Random = randomizer;
            Populate();
        }

        [JsonProperty("accountID")]
        public string AccountID { get; set; }
        [JsonProperty("firstName")]
        public string FirstName { get; set; }
        [JsonProperty("lastName")]
        public string LastName { get; set; }
        //public string Avatar { get; set; }
        [JsonProperty("userName")]
        public string UserName { get; set; }
        [JsonProperty("email")]
        public string Email { get; set; }
        [JsonProperty("gender")]
        public Name.Gender Gender { get; set; }
        [JsonProperty("cartId")]
        public Guid CartId { get; set; }
        [JsonProperty("fullName")]
        public string FullName { get; set; }
        [JsonProperty("dateOfBirth")]
        public DateTime DateOfBirth { get; set; }
        [JsonProperty("address")]
        public CardAddress Address { get; set; }
        [JsonProperty("phone")]
        public string Phone { get; set; }
        [JsonProperty("website")]
        public string Website { get; set; }
        [JsonProperty("accountAge")]
        public int AccountAge { get; set; }
        [JsonProperty("isUserRegistered")]
        public bool IsUserRegistered { get; set; }

        private Name DsName { get; set; }
        private Internet DsInternet { get; set; }
        private Date DsDate { get; set; }
        private PhoneNumbers DsPhoneNumbers { get; set; }
        private Bogus.DataSets.Address DsAddress { get; set; }
        private Bogus.DataSets.Company DsCompany { get; set; }

        private void GetDataSources(string locale)
        {
            this.DsName = this.Notifier.Flow<Name>(new Name(locale));
            this.DsInternet = this.Notifier.Flow<Internet>(new Internet(locale));
            SeedNotifier notifier = this.Notifier;
            Date date = new Date("en");
            date.Locale = locale;
            this.DsDate = notifier.Flow<Date>(date);
            this.DsPhoneNumbers = this.Notifier.Flow<PhoneNumbers>(new PhoneNumbers(locale));
            this.DsAddress = this.Notifier.Flow<Bogus.DataSets.Address>(new Bogus.DataSets.Address(locale));
            this.DsCompany = this.Notifier.Flow<Bogus.DataSets.Company>(new Bogus.DataSets.Company(locale));
        }

        internal void Populate()
        {
            this.AccountID = accountData.AccountID;
            this.AccountAge = accountData.AccountAge;
            this.IsUserRegistered = accountData.IsUserRegistered;
            this.Gender = this.Random.Enum<Name.Gender>(Array.Empty<Name.Gender>());
            this.FirstName = this.DsName.FirstName(new Name.Gender?(this.Gender));
            this.LastName = this.DsName.LastName(new Name.Gender?(this.Gender));
            this.FullName = this.FirstName + " " + this.LastName;
            this.UserName = this.DsInternet.UserName(this.FirstName, this.LastName);
            this.Email = this.DsInternet.Email(this.FirstName, this.LastName, (string)null, (string)null);
            this.Website = this.DsInternet.DomainName();
            //this.Avatar = this.DsInternet.Avatar();
            this.CartId = Guid.NewGuid();
            this.DateOfBirth = this.DsDate.Past(50, new DateTime?(Date.SystemClock().AddYears(-20)));
            this.Phone = this.DsPhoneNumbers.PhoneNumber((string)null);
            this.Address = new CardAddress()
            {
                Street = this.DsAddress.StreetAddress(false),
                Suite = this.DsAddress.SecondaryAddress(),
                City = this.DsAddress.City(),
                State = this.accountData.AccountState,
                PostalCode = this.accountData.AccountPostalCode,
                Country = this.accountData.AccountCountry,
                Geo = new CardAddress.CardGeo()
                {
                    Lat = this.DsAddress.Latitude(-90.0, 90.0),
                    Lng = this.DsAddress.Longitude(-180.0, 180.0)
                }
            };
        }

        [Newtonsoft.Json.JsonIgnore]
        public Randomizer Random
        {
            get
            {
                return this.randomizer ?? (this.Random = new Randomizer());
            }
            set
            {
                this.randomizer = value;
                this.Notifier.Notify(value);
            }
        }

        SeedNotifier IHasRandomizer.GetNotifier()
        {
            return this.Notifier;
        }

        Dictionary<string, object> IHasContext.Context
        {
            get
            {
                return this.context;
            }
        }
    }

    public class CardAddress
    {
        [JsonProperty("street")]
        public string Street { get; set; }
        [JsonProperty("suite")]
        public string Suite { get; set; }
        [JsonProperty("city")]
        public string City { get; set; }
        [JsonProperty("state")]
        public string State { get; set; }
        [JsonProperty("postalCode")]
        public string PostalCode { get; set; }
        [JsonProperty("country")]
        public string Country { get; set; }
        [JsonProperty("geo")]
        public CardGeo Geo { get; set; }

        public class CardGeo
        {
            [JsonProperty("lat")]
            public double Lat { get; set; }
            [JsonProperty("lng")]
            public double Lng { get; set; }
        }
    }

    public class ProductReview
    {
        [JsonProperty("productId")]
        public int ProductId { get; set; }
        [JsonProperty("reviewText")]
        public string ReviewText { get; set; }
        [JsonProperty("reviewDate")]
        public DateTime ReviewDate { get; set; }
    }

    public class ProductPurchase
    {
        [JsonProperty("productId")]
        public int ProductId { get; set; }
        [JsonProperty("itemsPurchasedLast12Months")]
        public int ItemsPurchasedLast12Months { get; set; }
    }

    public sealed class UserProfile : IHasRandomizer, IHasContext
    {
        internal Dictionary<string, object> Context = new Dictionary<string, object>();
        private readonly SeedNotifier _notifier = new SeedNotifier();
        private Randomizer _randomizer;

        public UserProfile(Randomizer randomizer, string accountId, string locale = "en")
        {
            GetDataSources(locale);
            AccountID = accountId;
            Random = randomizer;
            Populate();
        }

        [JsonProperty("accountID")]
        public string AccountID { get; set; }
        [JsonProperty("cartId")]
        public Guid CartId { get; set; }
        [JsonProperty("preferredProducts")]
        public List<int> PreferredProducts { get; set; } = new List<int>();
        [JsonProperty("productReviews")]
        public List<ProductReview> ProductReviews { get; set; }

        private Name DsName { get; set; }
        private Date DsDate { get; set; }
        private Rant DsRant { get; set; }

        private void GetDataSources(string locale)
        {
            DsName = this._notifier.Flow<Name>(new Name(locale));
            var notifier = this._notifier;
            var date = new Date("en") {Locale = locale};
            DsDate = notifier.Flow<Date>(date);
            DsRant = notifier.Flow<Rant>(new Rant());
        }

        internal void Populate()
        {
            var howManyFavoriteProducts = _randomizer.Number(10);
            var howManyReviews = _randomizer.Number(0, 5);
            var reviews = new List<ProductReview>();
            for (var i = 0; i < howManyReviews; i++)
            {
                reviews.Add(new ProductReview
                {
                    ProductId = _randomizer.Number(1, 5000),
                    ReviewDate = this.DsDate.Past(4, DateTime.Now),
                    ReviewText = DsRant.Review(_randomizer.Word())
                });
            }

            for (var i = 0; i < howManyFavoriteProducts; i++)
            {
                PreferredProducts.Add(_randomizer.Number(1, 5000));
            }
            CartId = Guid.NewGuid();
            ProductReviews = reviews;
        }

        [Newtonsoft.Json.JsonIgnore]
        public Randomizer Random
        {
            get => this._randomizer ?? (this.Random = new Randomizer());
            set
            {
                this._randomizer = value;
                this._notifier.Notify(value);
            }
        }

        SeedNotifier IHasRandomizer.GetNotifier()
        {
            return this._notifier;
        }

        Dictionary<string, object> IHasContext.Context => this.Context;
    }

    public sealed class ProductPurchases : IHasRandomizer, IHasContext
    {
        internal Dictionary<string, object> Context = new Dictionary<string, object>();
        private readonly SeedNotifier _notifier = new SeedNotifier();
        private Randomizer _randomizer;

        public ProductPurchases(Randomizer randomizer, string accountId, string locale = "en")
        {
            GetDataSources(locale);
            VisitorId = accountId;
            Random = randomizer;
            Populate();
        }

        [JsonProperty("visitorId")]
        public string VisitorId { get; set; }
        [JsonProperty("topProductPurchases")]
        public List<ProductPurchase> TopProductPurchases { get; set; }

        private Name DsName { get; set; }
        private Date DsDate { get; set; }
        private Rant DsRant { get; set; }

        private void GetDataSources(string locale)
        {
            DsName = this._notifier.Flow<Name>(new Name(locale));
            var notifier = this._notifier;
            var date = new Date("en") { Locale = locale };
            DsDate = notifier.Flow<Date>(date);
            DsRant = notifier.Flow<Rant>(new Rant());
        }

        internal void Populate()
        {
            var howManyProducts = _randomizer.Number(20);
            var purchases = new List<ProductPurchase>();
            for (var i = 0; i < howManyProducts; i++)
            {
                purchases.Add(new ProductPurchase
                {
                    ProductId = _randomizer.Number(1, 5000),
                    ItemsPurchasedLast12Months = _randomizer.Number(1, 99)
                });
            }

            TopProductPurchases = purchases;
        }

        [Newtonsoft.Json.JsonIgnore]
        public Randomizer Random
        {
            get => this._randomizer ?? (this.Random = new Randomizer());
            set
            {
                this._randomizer = value;
                this._notifier.Notify(value);
            }
        }

        SeedNotifier IHasRandomizer.GetNotifier()
        {
            return this._notifier;
        }

        Dictionary<string, object> IHasContext.Context => this.Context;
    }
}
