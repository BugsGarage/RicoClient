namespace RicoClient.Scripts.User.Storage
{
    public struct UserStorage 
    {
        public string Username { get; set; }

        public int MoneyValue { get; set; }

        public TokenInfo Tokens { get; set; }
    }
}
