namespace DomainModel.Services
{
    using System;
    using DomainModel.Entities;
    using System.Text;
    using System.Net.Mail;

    public class EmailOrderSubmitter : IOrderSubmitter
    {
        private string smtpServerName;
        private string mailFrom;
        private string mailTo;
        private const string mailSubject = "SportStore > New order has been submitted!";
        
        public EmailOrderSubmitter(string smtpServerName, string mailFrom, string mailTo)
        {
            this.smtpServerName = smtpServerName;
            this.mailFrom = mailFrom;
            this.mailTo = mailTo;
        }
        
        public void SubmitOrder(Cart cart)
        {
            StringBuilder messageBody = new StringBuilder();
            messageBody.AppendLine("A new order has been submitted!");
            messageBody.AppendLine("---");
            messageBody.AppendLine("Items:");

            foreach (var line in cart.Lines)
            {
                var subtotal = line.Product.Price * line.Quanity;
                messageBody.AppendFormat("{0} x {1} (subtotal: {2:c}", line.Quanity, 
                    line.Product.Name, subtotal);
            }

            messageBody.AppendLine();
            messageBody.AppendFormat("Total order value: {0:c}", cart.ComputeTotalValue());
            messageBody.AppendLine();
            messageBody.AppendLine("---");
            messageBody.AppendLine("Ship to:");
            messageBody.AppendLine(cart.ShippingDetails.Name);
            messageBody.AppendLine(cart.ShippingDetails.Line1);
            messageBody.AppendLine(cart.ShippingDetails.Line2 ?? string.Empty);
            messageBody.AppendLine(cart.ShippingDetails.Line3 ?? string.Empty);
            messageBody.AppendLine(cart.ShippingDetails.City);
            messageBody.AppendLine(cart.ShippingDetails.State ?? string.Empty);
            messageBody.AppendLine(cart.ShippingDetails.Country);
            messageBody.AppendLine(cart.ShippingDetails.Zip);
            messageBody.AppendLine("---");

            messageBody.AppendFormat("Gift wrap: {0}", cart.ShippingDetails.GiftWrap.ToString());

            SmtpClient smtpClient = new SmtpClient(smtpServerName);
            smtpClient.Send(new MailMessage(mailFrom, mailTo, mailSubject,
                messageBody.ToString()));

        }
    }
}
