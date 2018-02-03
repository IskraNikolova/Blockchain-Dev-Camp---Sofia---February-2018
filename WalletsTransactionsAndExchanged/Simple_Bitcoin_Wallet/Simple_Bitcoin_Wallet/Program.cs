namespace Simple_Bitcoin_Wallet
{
    using HBitcoin.KeyManagement;
    using NBitcoin;
    using System;
    using System.Collections;
    using System.Globalization;

    public class Program
    {
        public static void Main(string[] args)
        {
            string[] valiableOperations =
            {
                "create",
                "recover",
                "balance",
                "history",
                "receive",
                "send",
                "exit"
            };
            
            string input = string.Empty;
            do
            {
                Write("Enter operation [\"Create\", \"Recover\", \"Balance\", \"History\", \"Receive\", \"Send\", \"Exit\"]");
                input = ReadLine().ToLower().Trim();
            } while (!((IList)valiableOperations).Contains(input));

            while(!input.ToLower().Equals("exit"))
            {
                switch(input)
                {
                    case "create":
                        CreateWallet();
                        break;
                    case "recover":
                        WriteLine("Please note the wallet cannot check if your password is correct or not." +
                                   "If you provide a wrong password a wallet will be recovered with your " +
                                    "provided mnemonic AND password pair: ");
                        Write("Enter password: ");
                        string password = ReadLine();
                        Write("Enter mnemonic pharse: ");
                        string mnemonic = ReadLine();
                        Write("Enter date (yyyy-MM-dd): ");
                        string date = ReadLine();
                        Mnemonic mnem = new Mnemonic(mnemonic);
                        RecoverWallet(password, mnem, date);
                        break;
                    case "balance":
                        //todo
                        break;
                    case "history":
                       //todo
                        break;
                    case "receive":
                        Write("Enter wallet's name: ");
                        string walletName = ReadLine();
                        Write("Enter password: ");
                        password = ReadLine();
                        Receive(password, walletName);
                        break;
                    case "send":
                        //todo
                        break;
                }
            }
        }

        private static void Receive(string password, string walletName)
        {
            string walletFilePath = @"Wallets\";
            try
            {
                Safe loadedWallet = Safe.Load(password, walletFilePath + walletName + ".json");
                for (int i = 0; i < 10; i++)
                {
                    WriteLine(loadedWallet.GetAddress(i));
                }
            }
            catch (Exception)
            {
                WriteLine("Wallet with such name does not exist!");
                throw;
            }
        }

        private static void RecoverWallet(string password, 
            Mnemonic mnem, 
            string date)
        {
            Network currentNetwork = Network.TestNet;
            string walletFilePath = @"Wallets\";
            Random random = new Random();
            Safe wallet = Safe.Recover(mnem, 
                password, 
                walletFilePath + "RecoveredWalletNum" + random.Next() + ".json", 
                currentNetwork,
                creationTime: DateTimeOffset.ParseExact(date, "yyyy-MM-dd", CultureInfo.InvariantCulture));
            WriteLine("Wallet successfully recovered");
        }

        private static void CreateWallet()
        {
            Network currentNetwork = Network.TestNet;
            string walletFilePath = @"Wallets\";
            string password;
            string passwordConfirm;

            do
            {
                Write("Enter password: ");
                password = ReadLine();
                Write("Confirm password: ");
                passwordConfirm = ReadLine();
                if(password != passwordConfirm)
                {
                    WriteLine("Passwords did not match!");
                    WriteLine("Try again.");
                }
            } while (password != passwordConfirm);

            bool failure = true;
            while(failure)
            {
                try
                {
                    Write("Enter wallet name:");
                    string walletName = ReadLine();
                    Mnemonic mnemonic;
                    Safe wallet = Safe.Create(out mnemonic, 
                        password, 
                        walletFilePath + walletName + ".json", 
                        currentNetwork);

                    WriteLine("Wallet created successfully");
                    WriteLine("Write down the following mnemonic words.");
                    WriteLine("With the mnemonic words AND the password you can recover this wallet.");
                    WriteLine();
                    WriteLine("--------------");
                    WriteLine(mnemonic);
                    WriteLine("--------------");
                    WriteLine(
                        "Write down and keep in SECURE palce your private keys. Only through them you can access your coins!"
                        );
                    for (int i = 0; i < 10; i++)
                    {
                        WriteLine($"Address: {wallet.GetAddress(i)} -> Private key: {wallet.FindPrivateKey(wallet.GetAddress(i))}");
                    }
                    failure = false;
                }
                catch
                {

                    WriteLine("Wallet alreday exists");
                }
            }
        }

        private static string ReadLine()
        {
            return Console.ReadLine();
        }

        private static void Write(string message)
        {
            Console.Write(message);
        }

        private static void WriteLine(Object item)
        {
            Console.WriteLine(item);
        }

        private static void WriteLine()
        {
            Console.WriteLine();
        }
    }
}
