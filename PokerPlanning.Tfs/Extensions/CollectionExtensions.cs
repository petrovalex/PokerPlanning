using PokerPlanning.Tfs.Domain;
using System.Collections.Generic;
using Microsoft.TeamFoundation.WorkItemTracking.Client;
namespace PokerPlanning.Tfs.Extensions
{
    public static class CollectionExtensions
    {
        public static IEnumerable<WorkItemSummary> ToSummaries(this WorkItemCollection collection)
        {
            IList<WorkItemSummary> summaries = new List<WorkItemSummary>();

            foreach (WorkItem item in collection)
            {
                WorkItemSummary summary = WorkItemSummary.FromWorkItem(item);
                summaries.Add(summary);
            }

            return summaries;
        }
    }
}
