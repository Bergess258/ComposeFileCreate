using System;
using System.Collections.Generic;
using System.Text;

namespace YmlCreate
{
    public class AWQ
    {
        char[] pat;
        int[] badchar = new int[256];
        int m;
        public AWQ(char[] p)
        {
            pat = p;
            m = p.Length;
            /* Fill the bad character array by calling  
                the preprocessing function badCharHeuristic()  
                for given pattern */
            badCharHeuristic(p, m, badchar);
        }

        //A utility function to get maximum of two integers  
        int max(int a, int b) { return (a > b) ? a : b; }

        //The preprocessing function for Boyer Moore's  
        //bad character heuristic  
        void badCharHeuristic(char[] str, int size, int[] badchar)
        {
            int i;

            // Initialize all occurrences as -1  
            for (i = 0; i < 256; i++)
                badchar[i] = -1;

            // Fill the actual value of last occurrence  
            // of a character  
            for (i = 0; i < size; i++)
                badchar[(int)str[i]] = i;
        }

        /* A pattern searching function that uses Bad  
        Character Heuristic of Boyer Moore Algorithm */
        public bool search(char[] txt)
        {
            int n = txt.Length;

            int s = 0; // s is shift of the pattern with  
                       // respect to text  
            while (s <= (n - m))
            {
                int j = m - 1;

                /* Keep reducing index j of pattern while  
                    characters of pattern and text are  
                    matching at this shift s */
                while (j >= 0 && pat[j] == txt[s + j])
                    j--;

                /* If the pattern is present at current  
                    shift, then index j will become -1 after  
                    the above loop */
                if (j < 0)
                {
                    return true;

                    /* Shift the pattern so that the next  
                        character in text aligns with the last  
                        occurrence of it in pattern.  
                        The condition s+m < n is necessary for  
                        the case when pattern occurs at the end  
                        of text */
                    //s += (s + m < n) ? m - badchar[txt[s + m]] : 1; There's no need to search for another so uncomment it and delete return true if you need 
                }

                else
                    /* Shift the pattern so that the bad character  
                        in text aligns with the last occurrence of  
                        it in pattern. The max function is used to  
                        make sure that we get a positive shift.  
                        We may get a negative shift if the last  
                        occurrence of bad character in pattern  
                        is on the right side of the current  
                        character. */
                    s += max(1, j - badchar[txt[s + j]]);
            }
            return false;
        }
    }
}