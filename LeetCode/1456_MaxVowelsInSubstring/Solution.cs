public class Solution
{
    public int MaxVowels(string s, int k)
    {
        int current = 0;
        for (int index = 0; index < k; index++) if (IsVowel(s[index])) current++;
        int maximum = current;
        for (int index = k; index < s.Length; index++)
        {
            if (IsVowel(s[index - k])) current--;
            if (IsVowel(s[index])) current++;
            if (current > maximum) maximum = current;
        }
        return maximum;
    }

    private bool IsVowel(char letter)
    {
        return letter == 'a' || letter == 'e' || letter == 'i' || letter == 'o' || letter == 'u';
    }
}
