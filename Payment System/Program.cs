using CSharp2nem.Connectivity;
using CSharp2nem.CryptographicFunctions;
using CSharp2nem.Model.AccountSetup;
using CSharp2nem.Model.DataModels;
using CSharp2nem.Model.Mosaics;
using CSharp2nem.Model.MultiSig;
using CSharp2nem.Model.ProvisionNamespace;
using CSharp2nem.Model.Transfer;
using CSharp2nem.PrepareHttpRequests;
using CSharp2nem.ResponseObjects;
using CSharp2nem.Utils;
using CSharp2nem.RequestClients;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace Payment_System
{
    class Program
    {
        public string Address
        {
            get { return address; }
            set { address = value; }
        }
        public string Balance
        {
            get { return balance; }
            set { balance = value; }
        }
        public string PublicKey
        {
            get { return publicKey; }
            set { publicKey = value; }
        }
        public int HarvestedBlocks
        {
            get { return harvestedBlocks; }
            set { harvestedBlocks = value; }
        }
        public string VestedBalance
        {
            get { return vestedBalance; }
            set { vestedBalance = value; }
        }
        public double Importance
        {
            get { return importance; }
            set { importance = value; }
        }
        public ObservableCollection<Mosaic> Mosaics
        {
            get { return mosaics; }
            set { mosaics = value; }
        }

        public List<CSharp2nem.Model.Transfer.Mosaics.Mosaic> MosaicList
        {
            get { return mosaicList; }
            set { mosaicList = value; }
        }

        string address;
        string balance;
        string publicKey;
        string vestedBalance;
        int harvestedBlocks;
        double importance;
        ObservableCollection<Mosaic> mosaics;
        List<CSharp2nem.Model.Transfer.Mosaics.Mosaic> mosaicList;
        Connection connection;

        public Program()
        {
            Mosaics = new ObservableCollection<Mosaic>();
            MosaicList = new List<CSharp2nem.Model.Transfer.Mosaics.Mosaic>();

            connection = new Connection();
            connection.SetTestnet();
            Address = "TCHWU4CEDKLIODYK2L2ECQKSEY5LWNFIP7UMDGXB";
        }

        public void Start() {

            try
            {
                // To get an account information from the address.
                var accountClient = new AccountClient(connection);
                var accountResult = accountClient.BeginGetAccountInfoFromAddress(Address);
                var accountResponse = accountClient.EndGetAccountInfo(accountResult);

                Balance = ((double)accountResponse.Account.Balance / 1000000).ToString("N6");
                PublicKey = accountResponse.Account.PublicKey.Substring(0, 10) + "*************";
                HarvestedBlocks = accountResponse.Account.HarvestedBlocks;
                Importance = accountResponse.Account.Importance;
                VestedBalance = ((double)accountResponse.Account.VestedBalance / 1000000).ToString("N6");

                Console.WriteLine("Account Info of: " + Address);
                Console.WriteLine("Balance: " + Balance);
                Console.WriteLine("Public Key: " + PublicKey);

                // To get mosaic information of the account from the address.
                var mosaicClient = new NamespaceMosaicClient(connection);
                var mosaicResult = mosaicClient.BeginGetMosaicsOwned(Address);
                var mosaicResponse = mosaicClient.EndGetMosaicsOwned(mosaicResult);

                foreach (var data in mosaicResponse.Data)
                {
                    // XEM is mosaic, but it does not shown because it has already been shown on the above.
                    if (data.MosaicId.Name != "xem")
                    {
                        Mosaics.Add(new Mosaic
                        {
                            Name = data.MosaicId.Name,
                            Amount = data.Quantity.ToString("N6")

                        });

                        //experiment mosaic
                        MosaicList.Add(new CSharp2nem.Model.Transfer.Mosaics.Mosaic(data.MosaicId.NamespaceId, data.MosaicId.Name, 200000));
                        Console.WriteLine("Mosaic: " + data.MosaicId.Name);
                        Console.WriteLine("Amount: " + ((double)data.Quantity / 10000).ToString("N6"));
                    }
                }

                //transfer transaction
                var accountFactory = new PrivateKeyAccountClientFactory(connection);
                var accClient = accountFactory.FromPrivateKey("7921112a8aea456fad3b260b97fa61aa77a9df362dfacec2e748ae8977ea5d7d");

                var transData = new TransferTransactionData()
                {
                    Amount = 1000000, // amount should always be 1000000 micro xem when attaching mosaics as it acts as a multiplier.
                    Message = "GG work part 2",
                    RecipientAddress = "TDRQ6B5K6434DU3QURHZP4PSIIN4LOP7G5O4T73G",
                    ListOfMosaics = MosaicList
                };

                accClient.BeginSendTransaction(body =>
                {
                    try
                    {
                        if (body.Ex != null) throw body.Ex;

                        Console.WriteLine("Message: " + body.Content.Message);
                    }
                    catch (Exception e)
                    {
                        Debug.WriteLine("Error: " + e.Message);
                    }
                }, transData);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
            Console.ReadLine();
        }


        static void Main(string[] args)
        {
            Program program = new Program();
            program.Start();
        }
    }
}
