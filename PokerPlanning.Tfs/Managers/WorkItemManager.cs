namespace PokerPlanning.Tfs.Managers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.TeamFoundation.WorkItemTracking.Client;
    using PokerPlanning.Tfs.Domain;
    using PokerPlanning.Tfs.Managers.Query;
    
    public class WorkItemManager
    {
        private QueryManager _queryManager;

        public WorkItemManager()
        {
            _queryManager = new QueryManager();
        }

        public void Estimate(int id, int effortInHours)
        {
            QueryManager queryManager = new QueryManager();

            WorkItem item = queryManager.ItemById(id);

            item["Original Estimate"] = effortInHours;
            item["Remaining Work"] = effortInHours;
            item["Complited Work"] = 0;

            item.Save();
        }

        public IEnumerable<WorkItemSummary> GetCurrentSprintWorkItems()
        {            
            // Note: Assume that query with name 'Current Sprint Backlog' exists

            IList<StoredQuerySummary> queries = UserContext.Current.CurrentProject.StoredQueries;

            Guid queryId = Guid.Parse("{6505adcc-9976-4ebe-b083-20f46c08c003}");
            var query = queries.Where(x => x.Id == queryId).Single();

            return _queryManager.ExecuteStoredQuery(query.Id);
        }
    }
}
