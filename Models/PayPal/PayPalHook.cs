// Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse); 
    public class Amount
    {
        public string currency_code { get; set; }
        public string value { get; set; }
    }

    public class Payee
    {
        public string email_address { get; set; }
    }

    public class Address
    {
        public string address_line_1 { get; set; }
        public string address_line_2 { get; set; }
        public string admin_area_2 { get; set; }
        public string admin_area_1 { get; set; }
        public string postal_code { get; set; }
        public string country_code { get; set; }
    }

    public class Shipping
    {
        public string method { get; set; }
        public Address address { get; set; }
    }

    public class SellerProtection
    {
        public string status { get; set; }
        public List<string> dispute_categories { get; set; }
    }

    public class GrossAmount
    {
        public string currency_code { get; set; }
        public string value { get; set; }
    }

    public class PaypalFee
    {
        public string currency_code { get; set; }
        public string value { get; set; }
    }

    public class NetAmount
    {
        public string currency_code { get; set; }
        public string value { get; set; }
    }

    public class SellerReceivableBreakdown
    {
        public GrossAmount gross_amount { get; set; }
        public PaypalFee paypal_fee { get; set; }
        public NetAmount net_amount { get; set; }
    }

    public class Link
    {
        public string href { get; set; }
        public string rel { get; set; }
        public string method { get; set; }
        public string encType { get; set; }
    }

    public class Capture
    {
        public string id { get; set; }
        public string status { get; set; }
        public Amount amount { get; set; }
        public SellerProtection seller_protection { get; set; }
        public bool final_capture { get; set; }
        public SellerReceivableBreakdown seller_receivable_breakdown { get; set; }
        public DateTime create_time { get; set; }
        public DateTime update_time { get; set; }
        public List<Link> links { get; set; }
    }

    public class Payments
    {
        public List<Capture> captures { get; set; }
    }

    public class PurchaseUnit
    {
        public string reference_id { get; set; }
        public Amount amount { get; set; }
        public Payee payee { get; set; }
        public Shipping shipping { get; set; }
        public Payments payments { get; set; }

        public string description { get; set; }

        public string custom_id { get; set; }
    }

    public class Name
    {
        public string given_name { get; set; }
        public string surname { get; set; }
    }

    public class Payer
    {
        public Name name { get; set; }
        public string email_address { get; set; }
        public string payer_id { get; set; }
    }

    public class Resource
    {
        public DateTime update_time { get; set; }
        public DateTime create_time { get; set; }
        public List<PurchaseUnit> purchase_units { get; set; }
        public List<Link> links { get; set; }
        public string id { get; set; }
        public GrossAmount gross_amount { get; set; }
        public string intent { get; set; }
        public Payer payer { get; set; }
        public string status { get; set; }
    }

    public class Root
    {
        public string id { get; set; }
        public DateTime create_time { get; set; }
        public string resource_type { get; set; }
        public string event_type { get; set; }
        public string summary { get; set; }
        public Resource resource { get; set; }
        public List<Link> links { get; set; }
        public string event_version { get; set; }
        public int zts { get; set; }
        public string resource_version { get; set; }
    }

