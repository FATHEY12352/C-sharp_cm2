using System;
using System.Collections.Generic;
using System.Linq;
using DocumentTokens = System.Collections.Generic.List<string>;

namespace Antiplagiarism
{
    public class LevenshteinCalculator
    {
        private Dictionary<(DocumentTokens, DocumentTokens)
            , double> memo = new Dictionary<(DocumentTokens
                , DocumentTokens), double>();

        public List<ComparisonResult> CompareDocumentsPairwise(List<DocumentTokens> documents)
        {
            var results = new List<ComparisonResult>();

            for (int i = 0; i < documents.Count; i++)
            {
                for (int j = i + 1; j < documents.Count; j++)
                {
                    DocumentTokens first = documents[i];
                    DocumentTokens second = documents[j];
                    double distance = GetDistance(first, second);
                    results.Add(new ComparisonResult(first, second, distance));
                }
            }

            return results.OrderBy(s => s.Distance).ToList();
        }

        private double GetDistance(DocumentTokens first, DocumentTokens second)
        {
            if (memo.TryGetValue((first, second), out double distance))
            {
                return distance;
            }

            distance = CalculateDistance(first, second);
            memo[(first, second)] = distance;
            memo[(second, first)] = distance;
            return distance;
        }

        private double CalculateDistance(DocumentTokens first, DocumentTokens second)
        {
            var opt = new double[first.Count + 1, second.Count + 1];

            for (int i = 0; i <= first.Count; i++)
                opt[i, 0] = i;

            for (int i = 0; i <= second.Count; i++)
                opt[0, i] = i;

            for (int i = 1; i <= first.Count; i++)
                for (int j = 1; j <= second.Count; j++)
                {
                    if (first[i - 1] == second[j - 1])
                        opt[i, j] = opt[i - 1, j - 1];
                    else
                    {
                        var replace = TokenDistanceCalculator.GetTokenDistance(first[i - 1], second[j - 1]);
                        if (1 + opt[i - 1, j] >= replace + opt[i - 1, j - 1])
                            opt[i, j] = replace + opt[i - 1, j - 1] <= 1 + opt
                                [i, j - 1] ? replace + opt[i - 1, j - 1] : 1 + opt[i, j - 1];
                        else
                            opt[i, j] = 1 + opt[i - 1, j] <= 1 + opt[i, j - 1]
                                ? 1 + opt[i - 1, j] : 1 + opt[i, j - 1];
                    }
                }

            return opt[first.Count, second.Count];
        }
    }
}
