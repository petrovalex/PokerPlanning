namespace PokerPlanning.Tfs.Domain.User
{
    using System.Collections.Generic;
    using PokerPlanning.Tfs.Domain.Summaries;
    using Microsoft.TeamFoundation.WorkItemTracking.Client;

    public class ProjectDetails
    {
        private Project _project;
        public int Id { get; private set; }
        public string Name { get; private set; }
        public string Path { get; private set; }
        public List<WorkItemType> WorkItemTypes { get; private set; }
        public List<string> WorkItemTypesAsStrings { get; private set; }
        public List<string> Users { get; private set; }
        public IList<IterationSummary> Iterations { get; set; }
        public IList<StoredQuerySummary> StoredQueries { get; private set; }

        public ProjectDetails(Project project)
        {
            _project = project;
            Id = project.Id;
            Name = project.Name;
            Path = "$/" + Name; // TODO: Check this is correct

            WorkItemTypes = new List<WorkItemType>();
            WorkItemTypesAsStrings = new List<string>();
            Users = new List<string>();
            Iterations = new List<IterationSummary>();
            StoredQueries = new List<StoredQuerySummary>();

            AddWorkItemTypes();
            AddWorkItemTypesAsStrings();
            AddIterations();
            AddStoredQueries();
        }

        private void AddStoredQueries()
        {
            foreach (StoredQuery item in _project.StoredQueries)
            {
                StoredQueries.Add(new StoredQuerySummary { Id = item.QueryGuid, Name = item.Name });
            }
        }

        private void AddIterations()
        {
            Iterations.Add(new IterationSummary
            {
                Name = "None",
                Path = Name
            });

            foreach (Node iterationNode in _project.IterationRootNodes)
            {
                Iterations.Add(new IterationSummary
                {
                    Name = iterationNode.Name,
                    Path = iterationNode.Path
                });
            }
        }

        private void AddWorkItemTypesAsStrings()
        {
            foreach (WorkItemType workItemType in _project.WorkItemTypes)
            {
                WorkItemTypesAsStrings.Add(workItemType.Name);
            }
        }

        private void AddWorkItemTypes()
        {
            foreach (WorkItemType workItemType in _project.WorkItemTypes)
            {
                WorkItemTypes.Add(workItemType);
            }
        }
    }
}
