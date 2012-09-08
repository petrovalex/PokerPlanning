namespace PokerPlanning.Tfs.Managers.Query
{
    using System;
    using System.Collections.Generic;
    using Microsoft.TeamFoundation.WorkItemTracking.Client;
    using PokerPlanning.Tfs.Domain;
    using PokerPlanning.Tfs.Extensions;

    public class QueryManager
    {
        public WorkItem ItemById(int id)
        {
            WorkItem item = UserContext.Current.WorkItemStore.GetWorkItem(id);

            return item;
        }

        public IEnumerable<WorkItemSummary> ExecuteStoredQuery(Guid queryId)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("project", UserContext.Current.CurrentProject.Name);

            Project project = UserContext.Current.WorkItemStore.Projects[UserContext.Current.CurrentProject.Name];
            QueryItem item = project.QueryHierarchy.Find(queryId);

            WorkItemCollection collection = UserContext.Current.WorkItemStore.Query(project.StoredQueries[queryId].QueryText, parameters);
            return collection.ToSummaries();
        }
    }
}
