namespace PotikotTools
{
    public static class StringExtensions
    {
        public static string RemoveLeftSpaces(this string str)
        {
            if (str == "" || str[0] != ' ')
                return str;

            int firstCharacterIndex = 0, limit = str.Length;
            for (; firstCharacterIndex < limit; firstCharacterIndex++)
                if (str[firstCharacterIndex] != ' ')
                    break;

            return str[firstCharacterIndex..];
        }

        public static string RemoveRightSpaces(this string str)
        {
            if (str == "" || str[^1] != ' ')
                return str;

            int length = str.Length - 1;
            for (; length > 0; length--)
                if (str[length] != ' ')
                    break;

            return str[..++length];
        }

        public static string RemoveSideSpaces(this string str)
        {
            return str.RemoveLeftSpaces().RemoveRightSpaces();
        }
    }
}