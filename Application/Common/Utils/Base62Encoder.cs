namespace Application.Common.Utils
{
    public static class Base62Encoder
    {
        private const string Alphabet = "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
        private const long BaseOffset = 125000000;

        public static string Encode(long id)
        {
            long adjustedId = id + BaseOffset;

            if (adjustedId == 0) return Alphabet[0].ToString();

            var result = new System.Text.StringBuilder();
            while (adjustedId > 0)
            {
                result.Insert(0, Alphabet[(int)(adjustedId % 62)]);
                adjustedId /= 62;
            }

            return result.ToString();
        }

    }
}
