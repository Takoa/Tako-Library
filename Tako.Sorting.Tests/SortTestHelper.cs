namespace Tako.Sorting.Tests
{
    public static class SortTestHelper
    {
        private static readonly int[] testInts = new int[]
        {
             36, 938,  81, 935, 524, 765, 873,  77, 666, 341,
             71, 521, 610, 514, 297, 591, 977, 725, 965, 428,
            675, 521, 617, 759, 577, 276,  51, 934, 865,  77,
            497, 525, 998, 229, 745, 426, 342, 527, 637,  29,
            377, 194, 839, 983,   5, 760, 226, 544, 547, 845,
             15, 432, 171, 664, 914, 637, 718, 342, 227, 796,
            197, 776, 591,  59, 680, 574, 521, 829, 879, 355,
            250, 516, 182, 667, 424,  57,  30, 575, 449, 197,
            686, 577, 604, 457, 895, 374, 768, 217,  52, 281,
            781, 400, 246, 810, 838, 886, 579, 644, 842, 764
        };

        public static bool SortTest(ISort<int> sort)
        {
            int[] copy = new int[SortTestHelper.testInts.Length];

            SortTestHelper.testInts.CopyTo(copy, 0);
            sort.Sort(copy);

            for (int i = 0; i < copy.Length - 1;)
            {
                if (copy[++i] < copy[i])
                {
                    return false;
                }
            }

            return true;
        }
    }
}
