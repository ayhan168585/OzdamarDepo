using Iyzipay;
using Iyzipay.Model;
using Iyzipay.Request;
using Microsoft.Extensions.Options;
using OzdamarDepo.Application.Abstractions.Payments;
using OzdamarDepo.Application.Payments.DTOs;
using OzdamarDepo.Infrastructure.Options;
using System.Globalization;

namespace OzdamarDepo.Infrastructure.Services.Payments
{
    public class IyzicoPaymentService : IPaymentService
    {
        private readonly Iyzipay.Options _options;

        public IyzicoPaymentService(IOptions<IyzicoSettings> iyzicoOptions)
        {
            _options = new Iyzipay.Options
            {
                ApiKey = iyzicoOptions.Value.ApiKey ?? string.Empty,
                SecretKey = iyzicoOptions.Value.SecretKey ?? string.Empty,
                BaseUrl = iyzicoOptions.Value.BaseUrl ?? string.Empty
            };
        }

        public async Task<string?> InitializePaymentAsync(CheckoutFormRequestDto dto)
        {
            var request = new CreateCheckoutFormInitializeRequest
            {
                Locale = Locale.TR.ToString(),
                Price = dto.TotalPrice.ToString("F2", CultureInfo.InvariantCulture),
                PaidPrice = dto.TotalPrice.ToString("F2", CultureInfo.InvariantCulture),
                Currency = Currency.TRY.ToString(),
                BasketId = dto.BasketId,
                CallbackUrl = dto.CallbackUrl,
                Buyer = dto.Buyer,
                BillingAddress = new Address
                {
                    ContactName = dto.BillingAddress.ContactName,
                    City = dto.BillingAddress.City,
                    Country = dto.BillingAddress.Country,
                    Description = dto.BillingAddress.Address,
                    ZipCode = dto.BillingAddress.ZipCode
                },
                ShippingAddress = new Address
                {
                    ContactName = dto.ShippingAddress.ContactName,
                    City = dto.ShippingAddress.City,
                    Country = dto.ShippingAddress.Country,
                    Description = dto.ShippingAddress.Address,
                    ZipCode = dto.ShippingAddress.ZipCode
                },
                BasketItems = dto.BasketItems
            };

            var response = await CheckoutFormInitialize.Create(request, _options);

            Console.WriteLine($"Iyzico status: {response.Status}");
            Console.WriteLine($"Iyzico error: {response.ErrorMessage}");

            return response.PaymentPageUrl;
        }
    }
}
